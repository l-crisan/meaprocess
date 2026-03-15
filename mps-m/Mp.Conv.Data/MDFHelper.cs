using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Conv.Data
{
    internal class MDFHelper
    {
        public static void StringToByte(string str, byte[] data)
        {
            char [] chars  = str.ToCharArray();

            for (int i = 0; i < Math.Min(data.Length,str.Length); ++i)                
                data[i] = (byte)chars[i];
        }
    }
}
