using System;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Mp.XmlRpc;

namespace Mp.Runtime.Scripting
{
    [Guid("F29F9F58-0D56-437B-B34B-B0300E5F1D5A")]
    [ProgId("MeaProcessApp.MeaProcessEm")]
    public class MeaProcessEm : IMeaProcessEm
    {
        private string _serverIP ="127.0.0.1";
        private int _port = 5050;

        public MeaProcessEm()
        {
        }

        public string ServerIP 
        {
            get { return _serverIP; }
            set { _serverIP = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        private XmlDocument LoadDocument(string runtimeFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
  
            try
            {
                using (FileStream fs = new FileStream(runtimeFile, FileMode.Open))
                {
                    ZipInputStream istream = new ZipInputStream(fs);
                    istream.Password = "52gr2541g4211";
                    ZipEntry entry = istream.GetNextEntry();
                    byte[] bytes = new byte[istream.Length];

                    istream.Read(bytes, 0, (int)istream.Length);
                    MemoryStream mm = new MemoryStream(bytes);
                    xmlDoc.Load(mm);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Console.WriteLine(ex.Message);
                    xmlDoc.Load(runtimeFile);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }

            return xmlDoc ;
        }

        public bool OpenScheme(string schemeFile)
        {
            try
            {
                string schemeID = Path.GetFileName(schemeFile);
                DateTime dt = File.GetLastWriteTime(schemeFile);

                XmlDocument xmlDoc = LoadDocument(schemeFile);

                if (xmlDoc == null)
                    return false;
                
                XmlRpcRequest request = new XmlRpcRequest();
                request.MethodName = "LoadScheme";
                request.Params = new System.Collections.ArrayList();

                for( int i = 0; i < xmlDoc.DocumentElement.ChildNodes.Count; ++i)
                {
                    XmlElement xmlChild = (XmlElement) xmlDoc.DocumentElement.ChildNodes[i];


                    XmlAttribute xmlAtt  = xmlChild.Attributes["type"];

                    if (xmlAtt == null)
                        continue;

                    if (xmlAtt.Value == "Connections" || xmlAtt.Value == "GUI")
                    {
                        xmlDoc.DocumentElement.RemoveChild(xmlChild);
                        --i;
                    }
                }
              
                request.Params.Add(schemeID);
                request.Params.Add(dt.Ticks.ToString());
                request.Params.Add(xmlDoc.OuterXml);
                
                XmlRpcResponse responce = request.Send("http://" + _serverIP + ":" + _port.ToString() + "/MeaProcess");

                return Convert.ToBoolean(responce.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
               return false;
            }
        }

        public bool Start()
        {
            try
            {
                XmlRpcRequest request = new XmlRpcRequest();
                request.MethodName = "Start";

                request.Send("http://" + _serverIP + ":" + _port.ToString() + "/MeaProcess");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        public bool Stop()
        {
            try
            {
                XmlRpcRequest request = new XmlRpcRequest();
                request.MethodName = "Stop";

                request.Send("http://" + _serverIP + ":" + _port.ToString() + "/MeaProcess");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        public bool Reinitialize()
        {
            try
            {
                XmlRpcRequest request = new XmlRpcRequest();
                request.MethodName = "Reinitialize";

                request.Send("http://" + _serverIP + ":" + _port.ToString() + "/MeaProcess");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        public string Messages
        {
            get
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "GetMessages";

                    XmlRpcResponse responce = request.Send("http://" + _serverIP + ":" + _port.ToString() + "/MeaProcess");
                    return Convert.ToString(responce.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "";
                }
            }
        }

        public int InputSignals
        {
            get
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "GetNoOfInputSignals";

                    XmlRpcResponse responce = request.Send("http://" + _serverIP + ":" + _port.ToString() + "/MeaProcess");
                    return Convert.ToInt32(responce.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return 0;
                }
            }
        }

        public int OutputSignals
        {
            get
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "GetNoOfOutputSignals";

                    XmlRpcResponse responce = request.Send("http://" + _serverIP + ":" + _port.ToString() + "/MeaProcess");
                    return Convert.ToInt32(responce.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return 0;
                }
            }
        }

        public ISignal GetSignal(int index, bool input)
        {
            Signal signal = new Signal("http://" + _serverIP + ":" + _port.ToString() + "/MeaProcess", index, input);
            return signal;
        }
    }
}
