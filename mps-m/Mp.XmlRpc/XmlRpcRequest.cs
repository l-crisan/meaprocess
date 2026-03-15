namespace Mp.XmlRpc
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Xml;
    using System.Net;
    using System.Text;
    using System.Reflection;
    using System.Threading;

    public class XmlRpcRequest
    {
        public String MethodName = null;
        public ArrayList Params = null;
        private Encoding _encoding = new UTF8Encoding();

        public XmlRpcRequest()
        {
            Params = new ArrayList();
        }

        public String MethodNameObject
        {
            get
            {
                int index = MethodName.IndexOf(".");

                if (index == -1)
                    return MethodName;

                return MethodName.Substring(0, index);
            }
        }

        public String MethodNameMethod
        {
            get
            {
                int index = MethodName.IndexOf(".");

                if (index == -1)
                    return MethodName;

                return MethodName.Substring(index + 1, MethodName.Length - index - 1);
            }
        }

        public XmlRpcResponse Send(String url)
        {
            try
            {
                XmlRpcResponse resp;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.ReadWriteTimeout = 1000000;
                request.KeepAlive = false;
                request.Timeout = 1000000;
                request.MaximumResponseHeadersLength = -1;
                request.Method = "POST";
                request.ContentType = "text/xml";

                //Request
                Stream stream = request.GetRequestStream();
                XmlTextWriter xml = new XmlTextWriter(stream, _encoding);
                XmlRpcRequestSerializer.Serialize(xml, this);
                xml.Flush();
                xml.Close();

                stream.Flush();
                stream.Close();

                //Responce
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                int lenght = (int)response.ContentLength;

                StreamReader input = new StreamReader(response.GetResponseStream());

                char[] buffer = new char[lenght];

                input.ReadBlock(buffer, 0, lenght);
                input.BaseStream.Flush();
                input.Close();
                response.Close();

                MemoryStream mm = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(mm);
                bw.Write(buffer);
                mm.Seek(0, SeekOrigin.Begin);

                resp = XmlRpcResponseDeserializer.Parse(new StreamReader(mm));

                return resp;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Object Invoke(Object target)
        {
            Type type = target.GetType();
            MethodInfo method = type.GetMethod(MethodNameMethod);

            if (method == null)
                throw new XmlRpcException(-2, "Method " + MethodNameMethod + " not found.");

            if (XmlRpcExposedAttribute.IsExposed(target.GetType()) &&
                !XmlRpcExposedAttribute.IsExposed(method))
                throw new XmlRpcException(-3, "Method " + MethodNameMethod + " is not exposed.");

            Object[] args = new Object[Params.Count];

            for (int i = 0; i < Params.Count; i++)
                args[i] = Params[i];

            return method.Invoke(target, args);
        }

        override public String ToString()
        {
            StringWriter strBuf = new StringWriter();
            XmlTextWriter xml = new XmlTextWriter(strBuf);
            xml.Formatting = Formatting.Indented;
            xml.Indentation = 4;
            XmlRpcRequestSerializer.Serialize(xml, this);
            xml.Flush();
            xml.Close();
            return strBuf.ToString();
        }
    }
}
