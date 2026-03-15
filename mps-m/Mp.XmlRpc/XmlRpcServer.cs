namespace Mp.XmlRpc
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using System.Xml;

    /// <summary>A simple HTTP server.</summary>
    public class XmlRpcServer : IEnumerable
    {
        private TcpListener _myListener;
        private int _port;
        private Hashtable _handlers;
        private XmlRpcSystemObject _system;
        private string _ip;
        private  bool _running = false;
        private Thread _listenThread = new Thread(new ParameterizedThreadStart(Run));

        //The constructor which make the TcpListener start listening on the
        //given port. It also calls a Thread on the method StartListen(). 
        public XmlRpcServer(string ip, int port)
        {
            _ip = ip;
            _port = port;
            _handlers = new Hashtable();
            _system = new XmlRpcSystemObject(this);
            _myListener = new TcpListener(IPAddress.Parse(_ip), _port);            
        }

        // Process the client connection.
        public static void Run(object obj)
        {
            // Get the listener that handles the client request.
            XmlRpcServer server = (XmlRpcServer)obj;

            while (server._running)
            {
                TcpClient client = server._myListener.AcceptTcpClient();

                if (!server._running)
                    return;

                SimpleHttpRequest httpReq = new SimpleHttpRequest(client);

                if (httpReq.HttpMethod == "POST")
                {
                    try
                    {
                        server.HttpPost(httpReq);
                    }
                    catch (Exception e)
                    {
                        Logger.WriteEntry("Failed on post: " + e, EventLogEntryType.Error);
                    }
                }
                else
                {
                    Logger.WriteEntry("Only POST methods are supported: " + httpReq.HttpMethod + " ignored", EventLogEntryType.FailureAudit);
                }

                httpReq.Close();
            }
        }

        public void Start()
        {
            _running = true;
            _myListener.Start();
            _listenThread.Start(this);
        }        

        public void Stop()
        {
            if (!_running)
                return;

            _running = false;  

            //Connect to my self for wake up
            string debuggerAddress = "http://" +_ip + ":" + _port.ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(debuggerAddress);
            request.KeepAlive = false;

            request.Method = "POST";
            request.ContentType = "text/xml";

            Stream stream = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(stream);
            sw.Write("Cancel");
            stream.Flush();
            stream.Close();

            //Wait for termination of the listener thread
            _listenThread.Join();

            //Stop the listener
            _myListener.Stop();            
        }

        public IEnumerator GetEnumerator()
        {
            return _handlers.GetEnumerator();
        }

        public Object this[String name]
        {
            get { return _handlers[name]; }
        }

        /// <summary>
        /// This function send the Header Information to the client (Browser)
        /// </summary>
        /// <param name="sHttpVersion">HTTP Version</param>
        /// <param name="sMIMEHeader">Mime Type</param>
        /// <param name="iTotBytes">Total Bytes to be sent in the body</param>
        /// <param name="sStatusCode"></param>
        /// <param name="output">Socket reference</param>
        public void SendHeader(string sHttpVersion, string sMIMEHeader, long iTotBytes, string sStatusCode, TextWriter output)
        {
            String sBuffer = "";

            // if Mime type is not provided set default to text/html
            if (sMIMEHeader.Length == 0)
            {
                sMIMEHeader = "text/html";  // Default Mime Type is text/html
            }

            sBuffer = sBuffer + sHttpVersion + sStatusCode + "\r\n";
            sBuffer = sBuffer + "Connection: close\r\n";
            if (iTotBytes > 0)
                sBuffer = sBuffer + "Content-Length: " + iTotBytes + "\r\n";
            sBuffer = sBuffer + "Server: XmlRpcServer \r\n";
            sBuffer = sBuffer + "Content-Type: " + sMIMEHeader + "\r\n";

            sBuffer = sBuffer += "\r\n";

            output.Write(sBuffer);
        }     

        public void HttpPost(SimpleHttpRequest req)
        {
            XmlRpcRequest rpc = XmlRpcRequestDeserializer.Parse(req.Input);

            XmlRpcResponse resp = new XmlRpcResponse();
            Object target = _handlers[rpc.MethodNameObject];

            if (target == null)
            {
                resp.SetFault(-1, "Object " + rpc.MethodNameObject + " not registered.");
            }
            else
            {
                try
                {
                    resp.Value = rpc.Invoke(target);
                }
                catch (XmlRpcException e)
                {
                    resp.SetFault(e.Code, e.Message);
                }
                catch (Exception e2)
                {
                    resp.SetFault(-1, e2.Message);
                }
            }

            Logger.WriteEntry(resp.ToString(), EventLogEntryType.Information);

            MemoryStream mm = new MemoryStream();
            StreamWriter sw = new StreamWriter(mm);

            XmlTextWriter xml = new XmlTextWriter(sw);

            XmlRpcResponseSerializer.Serialize(xml, resp);
            xml.Flush();
            mm.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(mm);
            string body = sr.ReadToEnd();

            SendHeader(req.Protocol, "text/xml", body.Length, " 200 OK", req.Output);

//            req.Output.Flush();

            req.Output.Write(body);
            req.Output.Flush();
        }

        ///<summary>
        ///Add an XML-RPC handler object by name.
        ///</summary>
        public void Add(String name, Object obj)
        {
            _handlers.Add(name, obj);
        }
    }
}
