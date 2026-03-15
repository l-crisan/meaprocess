using System;
using System.Windows.Forms;
using Mp.XmlRpc;
using System.Globalization;
namespace Mp.Runtime.Scripting
{
    public class Signal : ISignal
    {
        private string _url;
        private int _index;
        private bool _input;

        public Signal(string url, int index, bool input)
        {
            _url = url;
            _index = index ;
            _input = input;
        }

        public string Name
        {
            get
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "GetSignalName";
                    request.Params = new System.Collections.ArrayList();
                    request.Params.Add(_index);
                    request.Params.Add(_input);
                    XmlRpcResponse responce = request.Send(_url);

                    return Convert.ToString(responce.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                return "";
            }
        }

        public string Unit
        {
            get
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "GetSignalUnit";
                    request.Params = new System.Collections.ArrayList();
                    request.Params.Add(_index);
                    request.Params.Add(_input);
                    XmlRpcResponse responce = request.Send(_url);

                    return Convert.ToString(responce.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                return "";
            }
        }

        public double Value
        {
            get
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "GetSignalValue";
                    request.Params = new System.Collections.ArrayList();
                    request.Params.Add(_index);
                    XmlRpcResponse responce = request.Send(_url);
                    NumberFormatInfo info = new NumberFormatInfo();
                    info.NumberDecimalSeparator = ".";
                    return Convert.ToDouble(responce.Value, info);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                return 0.0;
            }

            set
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "SetSignalValue";
                    request.Params = new System.Collections.ArrayList();
                    request.Params.Add(_index);
                    request.Params.Add(value);
                    request.Send(_url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public double Minimum
        {
            get
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "GetSignalMin";
                    request.Params = new System.Collections.ArrayList();
                    request.Params.Add(_index);
                    request.Params.Add(_input);
                    XmlRpcResponse responce = request.Send(_url);
                    NumberFormatInfo info = new NumberFormatInfo();
                    info.NumberDecimalSeparator = ".";
                    return Convert.ToDouble(responce.Value, info);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                return 0.0;
            }
        }

        public double Maximum
        {
            get
            {
                try
                {
                    XmlRpcRequest request = new XmlRpcRequest();
                    request.MethodName = "GetSignalMax";
                    request.Params = new System.Collections.ArrayList();
                    request.Params.Add(_index);
                    request.Params.Add(_input);
                    XmlRpcResponse responce = request.Send(_url);
                    NumberFormatInfo info = new NumberFormatInfo();
                    info.NumberDecimalSeparator = ".";
                    return Convert.ToDouble(responce.Value, info);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                return 0.0;
            }
        }
    }
}
