using System;
using System.Runtime.InteropServices;
using Mp.XmlRpc;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Mp.Runtime.Scripting
{
    [Guid("D899DC0C-E6BC-46BE-91A9-F1DDB01CEAE7")]
    [ProgId("MeaProcessApp.MeaProcess")]
    public class MeaProcess : IMeaProcess
    {
        private Process _meaProcess = null;
        private string _serverURL = "http://127.0.0.1:";
        private string _serverIPPort = "127.0.0.1:";

        public bool OpenScheme(string scheme, int  port)
        {
            _serverURL += port.ToString();
            _serverIPPort += port.ToString();

            try
            {

                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Atesion\\MeaProcess Realtime\\Runtime");

                EventWaitHandle startedEvent = NamedEvent.OpenOrCreate(_serverIPPort, false, EventResetMode.AutoReset);

                _meaProcess = new Process();
                _meaProcess.StartInfo.FileName = Path.Combine((string)key.GetValue("Path",""), "Mp.Runtime.App.exe");
                _meaProcess.StartInfo.Arguments = "\"" + scheme + "\"" + " \"" + _serverIPPort + "\"";
                key.Close();
                _meaProcess.Start();

                startedEvent.WaitOne();                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        public void Close()
        {
            try
            {
                XmlRpcRequest request = new XmlRpcRequest();
                request.MethodName = "Close";

                request.Send(_serverURL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Start()
        {
            try
            {
                XmlRpcRequest request = new XmlRpcRequest();
                request.MethodName = "Start";

                request.Send(_serverURL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Stop()
        {
            try
            {
                XmlRpcRequest request = new XmlRpcRequest();
                request.MethodName = "Stop";

                request.Send(_serverURL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public string Messages
        {
            get
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "ReadMessages";
                    XmlRpcResponse responce = request.Send(_serverURL);
                    return Convert.ToString(responce.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                return "";
            }
        }


        public void Reinitialize()
        {
            try
            {
                XmlRpcRequest request = new XmlRpcRequest();
                request.MethodName = "Reinitialize";

                request.Send(_serverURL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public bool Visible
        {
            set
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "SetVisible";
                    request.Params = new System.Collections.ArrayList();
                    request.Params.Add(value);

                    request.Send(_serverURL);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            get
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "GetVisible";
                    XmlRpcResponse responce = request.Send(_serverURL);

                    return Convert.ToBoolean(responce.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                return false;
            }
        }


        public int InputSignals
        {
            get
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "GetNumberOfInputSignals";

                    XmlRpcResponse responce = request.Send(_serverURL);

                    return Convert.ToInt32(responce.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                return 0;
            }
        }

        public int OutputSignals
        {
            get
            {

                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "GetNumberOfOutputSignals";

                    XmlRpcResponse responce = request.Send(_serverURL);

                    return Convert.ToInt32(responce.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                return 0;
            }
        }

        public ISignal GetSignal(int index, bool input)
        {
            Signal signal = new Signal(_serverURL, index, input);
            return signal;
        }
    }
}
