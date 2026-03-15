using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mp.Scheme.Sdk;

namespace Mp.Streaming
{
    internal class StreamingPS : ProcessStation
    {
        private static Hashtable _connection = new Hashtable();

        public StreamingPS()
        {
            
        }

        public static Hashtable Connections
        {
            get { return _connection;  }
        }

    }
}
