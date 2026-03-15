/***************************************************************************
 *   Copyright (C) 2006-2007 by Laurentiu-Gheorghe Crisan                  *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU Library General Public License as       *
 *   published by the Free Software Foundation; either version 2 of the    *
 *   License, or (at your option) any later version.                       *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU Library General Public     *
 *   License along with this program; if not, write to the                 *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/
using System;
using System.Xml;
using System.Collections;
using Mp.Components;

namespace Mp.Runtime.Sdk
{

    public class ProcessStationPort : Mp.Runtime.Sdk.Object
	{
        public ProcessStationPort() { }
		
		public override bool OnCopyDataFromXML(XmlNode node)
		{		
			if(!base.OnCopyDataFromXML(node))
				return false;
			
			_name	= PoXmlHelper.GetParam(node,"name");
			_portNo	= PoXmlHelper.GetParamNumber(node,"number");
			
			XmlNode signalListNode = node["refSignalList"];

			if(signalListNode != null)
				_sigListID = PoXmlHelper.GetObjectIDFromRef(signalListNode);
			else
				_sigListID = 0;
		
			foreach(XmlNode child in node.ChildNodes)
			{
				if(child.Name == "refLinkToPort")
					_connectedToPortID.Add(PoXmlHelper.GetObjectIDFromRef(child));
			}
			return true;
		}

		public override bool OnCopyDataToXML(XmlNode Node)
		{
			return base.OnCopyDataToXML(Node);
		}

		public override bool OnInitInstance()
		{
			if(!base.OnInitInstance())
				return false;
			
			PoObjectFactoryManager cbjFactoryMng = RuntimeEngine.GetObjectFactoryManager();
			PoProcessStationPort port;
			
            foreach( uint id in _connectedToPortID)
			{
				Port = (ProcessStationPort) objFactoryMng.GetObjectByID(id);
				_connectedToPort.Add(Port);
			}
			
			_signalList	= (SignalList) objFactoryMng.GetObjectByID(_sigListID);
			
			return true;
		}

		public override void OnExitInstance()
		{
			base.OnExitInstance();
		}

		public virtual void OnUpdateDataValue(byte []data,bool []update,uint noOfRecords)
		{
			ProcessStationPort Port;

			if(_isInputPort)
				_parentStation.OnUpdateDataValue(data,update,noOfRecords,_signalList,this);
			else
			{
				if(ObjectType == "PORT_CONTROL_OUTPUT" && Update[(int)ControlSignals.Message])
				{					
					if(_connectedToPort.Count > 0)
					{
						port =(ProcessStationPort) _connectedToPort[0];
						port.OnUpdateDataValue(data,update,noOfRecords);
					}
				}
				else
				{
					for(int index = 0; index  < _connectedToPort.Count; index++)
					{
						port = (ProcessStationPort) _connectedToPort[index];
						port.OnUpdateDataValue(data,update,noOfRecords);
					}
				}
			}
		}

		public object ParentStation
		{
			get { return _parentStation;}
			set { _parentStation = (ProcessStation)value;}
		}

		public uint PortNo
		{
			get {  return _portNo;}
			set { _portNo = value;}
		}

		public string Name
		{
			get { return _name;}
			set { _name = value;}
		}

		public bool InputPort
		{
			get { return _isInputPort;}
			set {_isInputPort = value;}
		}
		public SignalList SignalList
		{
			get { return _signalList;}
		}

		private SignalList		_signalList			= new SignalList();
		protected ArrayList		_connectedToPortID	= new ArrayList();
		protected ArrayList		_connectedToPort	= new ArrayList();
		private ProcessStation	_parentStation;
		private uint			_portNo;
		private string			_name;
		private uint			_sigListID;
		private bool			_isInputPort;
		
	}
}
