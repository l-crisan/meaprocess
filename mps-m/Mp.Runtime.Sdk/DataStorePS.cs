//Copyright(C)2005, Laurentiu-Gheorghe Crisan, All rights reserved.
using System;
using System.Collections;
using System.IO;
using System.Xml;

//Zip UnZip
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;


namespace Mp.Runtime.Sdk
{
	/// <summary>
	/// 
	/// </summary>
	public class DataStorePS : ProcessStation
	{
        public DataStorePS() { }

		#region API
		public override bool OnCopyDataFromXML(XmlNode Node)
		{
			if(!base.OnCopyDataFromXML(Node))
				return false;
			
			_strFileName			= PoXmlHelper.GetParam(Node,"fileName");
			_bWriteTimeSignal		= (PoXmlHelper.GetParamNumber(Node,"writeTimeSignal") > 0);
			_bOverWriteExistingFile = (PoXmlHelper.GetParamNumber(Node,"overWriteExistingFile") > 0);
			_bZipFilesAfterMea		= (PoXmlHelper.GetParamNumber(Node,"zipFilesAfterMea")>0);
			_bDeleteFilesAfterZip	= (PoXmlHelper.GetParamNumber(Node,"deleteFilesAfterZip")>0);
			_strMeaComment			= PoXmlHelper.GetParam(Node,"meaComment");
			return true;
		}

		public override bool OnInitInstance()
		{
			if(!base.OnInitInstance())
				return false;
	
			_InPort			=  (PoProcessStationPort)_InputPorts[0];
			_TriggerPort	=  (PoTriggerPort) _InputPorts[1];
			_InSignalList	=  (PoSignalList) _InPort.SignalList;

			CreateStorageGroup();
			return true;
		}

		public override void OnExitInstance()
		{
			DestroyStorageGroups();
			_CreatedFiles.Clear();
			base.OnExitInstance ();
		}

		public override bool OnUpdateDataValue(byte[] Data, bool[] Update, uint nNoOfRecords, PoSignalList SignalList, PoProcessStationPort Port)
		{
			if(!base.OnUpdateDataValue(Data,Update,nNoOfRecords,SignalList,Port))
				return false;

			if(!_bRun)
				return true;

			if(_bErrorState)
				return true;

			if(Port.ObjectType == "PORT_TYPE_INPUT")
			{
				PoSignal			Signal;
				PoStorageGroupInfo	GrpInfo;

				for(int nSig = 0; nSig < SignalList.SignalList.Count; nSig++)
				{
					Signal  = (PoSignal) SignalList.SignalList[nSig];
					GrpInfo = (PoStorageGroupInfo) _StorageGrpMap[GetSourceKey(Signal)];
					
					if(!Update[nSig+1])
						continue;
					
					GrpInfo.PutData(nSig,SignalList,Data);
					
					if(!GrpInfo.IsGroupDataFull())
						continue;
					
					//Save the data set to file
					if(!_bRun)
						break;

					if(!GrpInfo.StoreDataSet())
					{
						Console.WriteLine("Signal data coulden't be stored");
						break;
					}
					GrpInfo.ClearDataSet();		
				}
			}
			else if(Port.ObjectType ==  "PORT_TYPE_TRIGGER")
			{
				MemoryStream mr = new MemoryStream(Data);
				BinaryReader DataReader = new BinaryReader(mr);

				bool FistValue		= DataReader.ReadBoolean();
				bool SecondValue    = DataReader.ReadBoolean();

				if(Update[1])
				{
					if(_TriggerPort.TriggerType == TTriggerType.TRIGGER_START)
					{
						if(FistValue && !_bTriggerStart)
						{
							_bTriggerStart = true;
							OnStartTrigger();					
						}
					}
					else if(_TriggerPort.TriggerType == TTriggerType.TRIGGER_START_STOP)
					{
						if(_TriggerPort.IsOneStartStopSignal)
						{
							if(FistValue && !_bTriggerStart)
							{
								_bTriggerStart = true;
								OnStartTrigger();
							}
							else if(!(FistValue) && _bTriggerStart)
							{
								_bTriggerStart = false;
								OnStopTrigger();
							}
						}
						else					
						{
							if(FistValue && !_bTriggerStart)
							{
								_bTriggerStart = true;
								OnStartTrigger();
							}
						}
					}					
					else if(_TriggerPort.TriggerType == TTriggerType.TRIGGER_EVENT)
					{
						if(FistValue)
							OnEventTrigger();
					}
				}

				if(!_TriggerPort.IsOneStartStopSignal)
				{
					if(Update[2])
					{
						if(SecondValue && _bTriggerStart)
						{
							_bTriggerStart = false;
							OnStopTrigger();
						}
					}		
				}
			}
			return true;
		}

		#endregion

		#region Events
		protected override bool OnInitialize()
		{
			bool bRetVal = CreateMeaMetaFile();

			switch(_TriggerPort.TriggerType)
			{
				case TTriggerType.TRIGGER_STOP:
				case TTriggerType.TRIGGER_NO:
					SetStore(true);
				break;

				case TTriggerType.TRIGGER_EVENT:
					CreatePrePostTriggerFifo();
					SetStore(false);
				break;
				default:
					SetStore(false);
				break;
			}
			return bRetVal;
		}

		protected override bool OnDeinitialize()
		{
			if(_TriggerPort.TriggerType == TTriggerType.TRIGGER_EVENT)
				DestroyPrePostTriggerFifo();

			return true;
		}
		protected override bool OnStart()
		{
			bool				bRetVal = true;

			_bErrorState		= false;
			_bTriggerStart		= false;

			//init the storage groups 
			PoStorageGroupInfo StorageGroup;

			foreach(DictionaryEntry entry in _StorageGrpMap)
			{
				StorageGroup = (PoStorageGroupInfo) entry.Value;
				if(!StorageGroup.Init())
					bRetVal = false;
			}

			//Set run status flag 
			_bRun = bRetVal;
			return bRetVal;
		}

		protected override bool OnStop()
		{
			_bRun				= false;
			PoStorageGroupInfo StorageGroup;

			foreach(DictionaryEntry entry in _StorageGrpMap)
			{
				StorageGroup = (PoStorageGroupInfo) entry.Value;
				StorageGroup.Exit();
			}

			return CompresFilesOnStop();
		}

		protected bool CompresFilesOnStop()
		{
			if(!_bZipFilesAfterMea)
				return true;

			string		strArchiveFileName = _strFileName;
			strArchiveFileName += ".mxz"; //MeaProcess XML Zip-File  ".mxz";

			try
			{
				//Create a zip archive
				Crc32 crc = new Crc32();			
				ZipOutputStream Zip = new ZipOutputStream(File.Create(strArchiveFileName));
				Zip.SetLevel(6); // 0 - store only to 9 - means best compression
			
				foreach(string strFile in _CreatedFiles)				
				{
				
					FileInfo  FileInfo = new FileInfo(strFile);
					
					ZipEntry entry = new ZipEntry(FileInfo.Name);
					
					FileStream fs = File.OpenRead(strFile);
			
					byte[] buffer = new byte[fs.Length];
				
					fs.Read(buffer, 0, buffer.Length);
					
					entry.DateTime = DateTime.Now;
					entry.Size = fs.Length;
					
					fs.Close();
			
					crc.Reset();
					crc.Update(buffer);
			
					entry.Crc  = crc.Value;
			
					Zip.PutNextEntry(entry);
			
					Zip.Write(buffer, 0, buffer.Length);
				}
		
				Zip.Finish();
				Zip.Close();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.WriteLine("Zipping file failed!!!");
				 return false;
			}

			if(_bDeleteFilesAfterZip)
			{
				foreach(string strFile in _CreatedFiles)				
					File.Delete(strFile);	
			}

			return true;
		}

		protected virtual void OnStartTrigger()
		{
			SetStore(true);
		}

		protected virtual void OnStopTrigger()
		{
			SetStore(false);
		}

		protected virtual void OnEventTrigger()
		{
			SetStore(true);
		}
		#endregion 

		#region StorageGroupInfo
		protected class PoStorageGroupInfo
		{
			public PoStorageGroupInfo(){}

			public bool Init()
			{
				bool bRetVal = true;
				TimeIncrement = 1/ Samplerate;

				try
				{
					_FileWriter =  new FileStream(this.FileName, FileMode.OpenOrCreate);
				}
				catch(Exception e)
				{
					bRetVal = false;
					Console.WriteLine(e.ToString());
					Console.WriteLine("Mea data file creation failed");
				}
				return bRetVal;
			}

			public void PutData(int nSignal, PoSignalList SignalList,byte[] bData)
			{
				int			nSignalIndexInGroup		= (int) SignalGrpSignalIndexMap[nSignal];
				int			nOffset					= 0;
				int			nOffsetIn				= SignalList.GetSignalDataOffset(nSignal);
				PoSignal	Signal					= (PoSignal) SignalArray[nSignalIndexInGroup];
	
				if(WriteTimeStamp)
					nOffset =((int) SignalDataOffset[nSignalIndexInGroup]) + 8;
				else
					nOffset = (int) SignalDataOffset[nSignalIndexInGroup];

				Array.Copy(bData,nOffsetIn,Data,nOffset,Signal.ValueSize);
				SignalDataSet[nSignalIndexInGroup] = true;
			}
			
			public bool IsGroupDataFull()
			{
				foreach(bool bFull in  SignalDataSet)
				{
					if(bFull == false)
						return false;
				}
				return true;
			}

			public void ClearDataSet()
			{
				for(int nIndex = 0; nIndex < SignalDataSet.Count; nIndex++)
					SignalDataSet[nIndex] = false;
			}

			public bool StoreDataSet()
			{
				if(_FileWriter == null)
					return false;

				if(!Store && !EventTrigger)
				{//No Store, no event trigger 	
					TimeStamp +=  TimeIncrement;
					return true;
				}
				MemoryStream mr = new MemoryStream(Data);
				BinaryWriter DataWriter = new BinaryWriter(mr);

				//Write the time stamp
				if(this.WriteTimeStamp)
					DataWriter.Write(TimeStamp);

				if(EventTrigger && !Store)
				{//Store to Trigger Fifo
					if(TriggerFiFo.Count == (int) NoOfFiFoSamples)
					{
						TriggerFiFo.Dequeue();
						TriggerFiFo.Enqueue(Data);
					}
					else
					{
						TriggerFiFo.Enqueue(Data);
					}
					TimeStamp += TimeIncrement;
					return true;
				}

				if(EventTrigger && (TriggerFiFo.Count == (int) NoOfFiFoSamples)) 
				{//Store the event trigger Fifo, fisrt

					foreach(byte[] TriggerData  in TriggerFiFo)
					{
						_FileWriter.Write(TriggerData,0,TriggerData.Length);
					}
				}

				//Store the current value
				_FileWriter.Write(Data,0,Data.Length);

				if(EventTrigger)
				{
					CurTriggerSamples++;
					if(PostTriggerSamples == CurTriggerSamples)
					{
						Store = false;
						CurTriggerSamples = 0;
					}		
				}

				//Increment the time stamp
				TimeStamp += TimeIncrement;
				return true;
			}

			public void Exit()
			{

				if(_FileWriter == null)
					return;

				_FileWriter.Flush();
				_FileWriter.Close();
			}
			
			public Hashtable SignalGrpSignalIndexMap = new Hashtable();
			public ArrayList SignalArray			 = new ArrayList(); 
			public ArrayList SignalDataOffset		 = new ArrayList();
			public ArrayList SignalDataSet			 = new ArrayList();
			public int		 DataSize;			
			public string    FileName;
			public byte[]	 Data;
			public int		 Source;
			public double	 Samplerate;
			public double	 TimeStamp;
			public double	 TimeIncrement;
			public bool		 WriteTimeStamp;
			public bool		 Store;
			
			//Event trigger data
			public bool		EventTrigger;
			public Queue	TriggerFiFo				= new Queue();
			public ulong	PostTriggerSamples;
			public ulong	CurTriggerSamples;
			public ulong	NoOfFiFoSamples;
	
			protected FileStream _FileWriter;
		}
		#endregion 

		#region Helper
		protected string GetSourceKey(PoSignal Signal)
		{
			string	strKey;
			strKey = Signal.SourceNo.ToString();
			strKey += "_";
			strKey += Signal.SampleRate.ToString();
			return strKey;
		}

		protected void CreateStorageGroup()
		{
			PoStorageGroupInfo			StorageGrpInfo;
			PoSignal					Signal = null;
			double						dbSamplerate;

			//Create the storage data set groups
			for(int nSig = 0; nSig < _InSignalList.SignalList.Count; nSig++)
			{
				Signal			= (PoSignal) _InSignalList.SignalList[nSig];
				dbSamplerate	= Signal.SampleRate;
				StorageGrpInfo	= (PoStorageGroupInfo) _StorageGrpMap[GetSourceKey(Signal)];

				if(StorageGrpInfo == null)
				{
					StorageGrpInfo							= new PoStorageGroupInfo();
					StorageGrpInfo.Source					= (int) Signal.SourceNo;
					_StorageGrpMap[GetSourceKey(Signal)]	= StorageGrpInfo;
				}

				StorageGrpInfo.SignalArray.Add(Signal);
				StorageGrpInfo.SignalGrpSignalIndexMap[nSig] = StorageGrpInfo.SignalArray.Count - 1;
			}

			//Calculate the signal offset for data data array, and initialize the group runtime variables
			int					nSignalOffset = 0;
			int					nGrp = 0;
			PoStorageGroupInfo	StrGrpInfo;

			foreach(DictionaryEntry entry in _StorageGrpMap)
			{
				StrGrpInfo = (PoStorageGroupInfo) entry.Value;
				nSignalOffset	= 0;
				nGrp++;

				for(int nGrpSig = 0; nGrpSig < StrGrpInfo.SignalArray.Count; nGrpSig++)
				{
					Signal = (PoSignal) StrGrpInfo.SignalArray[nGrpSig];			
					StrGrpInfo.SignalDataOffset.Add(nSignalOffset);
					nSignalOffset += Signal.ValueSize;
					StrGrpInfo.SignalDataSet.Add(false);
				}

				StrGrpInfo.WriteTimeStamp       =  _bWriteTimeSignal;
				StrGrpInfo.Samplerate           =  Signal.SampleRate;
				StrGrpInfo.FileName				=  _strFileName;
				StrGrpInfo.FileName				+= ".grp.";
				StrGrpInfo.FileName				+= nGrp.ToString();
				StrGrpInfo.FileName				+= ".bin";
				_CreatedFiles.Add(StrGrpInfo.FileName);

				if(_bWriteTimeSignal)
				{
					StrGrpInfo.Data					= new byte[8+ nSignalOffset];
					StrGrpInfo.DataSize				= 8 + nSignalOffset;
				}
				else
				{
					StrGrpInfo.Data					= new byte[nSignalOffset];
					StrGrpInfo.DataSize				= nSignalOffset;
				}
			}	
		}
		
		protected bool CreateMeaMetaFile()
		{
			string						strMetaData;
			string						strText;
			ulong						ID = 1;
	
			strMetaData = "<?xml version=\"1.0\" ?>\n<MeaProcess fileVersion = \"00.00.01\" id= \"_";
			strMetaData += ID.ToString();
			ID++;
			strMetaData += "\"";
			strMetaData += ">\n";
			strMetaData += "<meaComment>";
			strMetaData += _strMeaComment;
			strMetaData += "</meaComment>";

	/*
				strMetaData += "	<fileCreationTime>";
			//	strMetaData += itoa(Time.GetHour(),szBuffer,10);
				strMetaData += ":";
			//	strMetaData += itoa(Time.GetMinute(),szBuffer,10);
				strMetaData += ":";
			//	strMetaData += itoa(Time.GetSecond(),szBuffer,10);
				strMetaData += "	</fileCreationTime>\n";
				strMetaData += "	<fileCreationDate>";
			//	strMetaData += itoa(Time.GetDay(),szBuffer,10);
				strMetaData += ".";
			//	strMetaData += itoa(Time.GetMonth(),szBuffer,10);
				strMetaData += ".";
			//	strMetaData += itoa(Time.GetYear(),szBuffer,10);
				strMetaData += "	</fileCreationDate>\n";
			*/
			//m_strFileName
			PoStorageGroupInfo StorageGrpInfo;

			foreach(DictionaryEntry entry in _StorageGrpMap)
			{
				StorageGrpInfo = (PoStorageGroupInfo) entry.Value;
				strMetaData += "		<SignalStorageGroup id=\"_";
				strMetaData += ID.ToString();
				ID++;
				strMetaData +="\">\n";
				strMetaData += "			<dataFile>";	
				strText     = StorageGrpInfo.FileName;
		
				for(int nIndex = strText.Length - 1; nIndex > 0; nIndex--)
				{
					if(strText[nIndex] == '\\')
					{
						strText = strText.Substring(nIndex+ 1,strText.Length - nIndex - 1);	
						break;
					}
				}

				strMetaData += strText;
				strMetaData += "</dataFile>\n";	
				strMetaData += "			<samplerate>";
				strMetaData += StorageGrpInfo.Samplerate.ToString();
				strMetaData += "</samplerate>\n";
				strMetaData += "			<sourceID>";
				strMetaData += StorageGrpInfo.Source.ToString();
				strMetaData += "</sourceID>\n";

				strMetaData += "			<Signals id=\"_";
				strMetaData += ID.ToString();
				ID++;
				strMetaData += "\">\n";
		
				if(this._bWriteTimeSignal)
				{
					strMetaData += "			<Signal id=\"_";
					strMetaData += ID.ToString();
					ID++;
					strMetaData +="\">\n";
					strMetaData += "				<name>";
					strMetaData += "Time";
					strMetaData += "</name>";
					strMetaData += "				<unit>";
					strMetaData += "s";
					strMetaData += "</unit>";
					strMetaData += "				<valueDataType>";
					strMetaData += "VALUE_DATA_TYPE_REAL_64";
					strMetaData += "</valueDataType>";
					strMetaData += "			</Signal>\n";
				}

				foreach(PoSignal Signal in StorageGrpInfo.SignalArray)
				{			
					strMetaData += "			<Signal id=\"_";
					strMetaData += ID.ToString();
					ID++;
					strMetaData +="\">\n";
					strMetaData += "				<name>";
					strMetaData += Signal.Name;
					strMetaData += "</name>";
					strMetaData += "				<unit>";
					strMetaData += Signal.Unit;
					strMetaData += "</unit>";
					strMetaData += "				<physMin>";
					strMetaData += Signal.PhysMin.ToString();
					strMetaData += "</physMin>";
					strMetaData += "				<physMax>";
					strMetaData += Signal.PhysMax.ToString();
					strMetaData += "</physMax>";
					strMetaData += "				<comment>";
					strMetaData += Signal.Commment;
					strMetaData += "</comment>";
					strMetaData += "				<valueDataType>";
					strMetaData += Signal.ValueDataType.ToString();
					strMetaData += "</valueDataType>";
			
					if(Signal.Scaling != null)
						strMetaData += Signal.Scaling.GetScalingObjectAsString((uint) ID++);
			
					strMetaData += "			</Signal>\n";
				}
				strMetaData += "			</Signals>\n";
				strMetaData += "		</SignalStorageGroup>\n";
			}	

			strMetaData += "</MeaProcess>";
	
			string strFile = _strFileName;
			strFile += ".manifest";
			_CreatedFiles.Add(strFile);
			StreamWriter MetaFileWriter;

			try
			{
				if(_bOverWriteExistingFile)
				{
					MetaFileWriter = new StreamWriter(strFile);
				}
				else
				{
					if(File.Exists(strFile))
					{
						Console.WriteLine("File " + strFile + "allrady existing!!!"); 
						return false;
					}
			
					MetaFileWriter = new StreamWriter(strFile);
				}
	
				MetaFileWriter.Write(strMetaData);
				MetaFileWriter.Flush();
				MetaFileWriter.Close();
				return true;
			}
			catch(Exception e)
			{
				Console.WriteLine(e.ToString());
				Console.WriteLine("Data store mata file creation failed");
				return false;
			}
		}
		protected void DestroyStorageGroups()
		{
			foreach(PoStorageGroupInfo StrGrpInfo in _StorageGrpMap )
				StrGrpInfo.Exit();

			_StorageGrpMap.Clear();
		}

		//Triger handling
		protected void SetStore(bool bStore)
		{
			PoStorageGroupInfo StrGrpInfo;
			foreach(DictionaryEntry entry in _StorageGrpMap )
			{
				StrGrpInfo = (PoStorageGroupInfo) entry.Value;
				StrGrpInfo.Store = bStore;
			}
		}

		protected void CreatePrePostTriggerFifo()
		{
			double						dbPreTime	 = _TriggerPort.PreEventTime;	
			double						dbPostTime	 = _TriggerPort.PostEventTime;
			ulong						nNoOfSamples = 0;
			double						dbSampleRate;

			foreach(PoStorageGroupInfo StrGrpInfo in _StorageGrpMap)
			{
				dbSampleRate	= StrGrpInfo.Samplerate;
				nNoOfSamples	= (ulong)(dbSampleRate * dbPreTime);

				StrGrpInfo.PostTriggerSamples = (ulong)(dbSampleRate * dbPostTime);
				StrGrpInfo.EventTrigger   = true;
				StrGrpInfo.NoOfFiFoSamples = nNoOfSamples;
				StrGrpInfo.TriggerFiFo.Clear();
			}
		}
		protected void DestroyPrePostTriggerFifo()
		{
			foreach(PoStorageGroupInfo	StrGrpInfo in _StorageGrpMap)
			{
				StrGrpInfo.PostTriggerSamples   = 0;
				StrGrpInfo.EventTrigger			= false;
				StrGrpInfo.TriggerFiFo.Clear();
			}
		}
		#endregion

		#region	Member
		protected string				_strFileName;
		protected PoSignalList			_InSignalList;
		protected PoProcessStationPort	_InPort;
		protected Hashtable				_StorageGrpMap = new Hashtable();
		protected bool					_bRun;
		protected bool					_bErrorState;
		protected bool					_bWriteTimeSignal;
		protected bool					_bOverWriteExistingFile;
		protected ArrayList				_CreatedFiles = new ArrayList();
		protected PoTriggerPort			_TriggerPort;
		protected string				_strMeaComment;
		protected bool					_bTriggerStart;
		protected bool					_bZipFilesAfterMea;
		protected bool					_bDeleteFilesAfterZip;

		#endregion
	}
}
