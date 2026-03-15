using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Mp.Drv.CANdb
{
    public class Signal : DBObject
    {
        public enum ByteType
        {
            MotorolaBackward = 0,
            MotorolaForward = 2,
            Intel = 1,
        }

        public enum SignalType
        {
            Standard,
            ModeSignal,
            ModeDepended,
        }

        public enum ValueType
        {
            Signed,
            Unsigned,
            Float,
            Double,
        }

        private string _name;
        private ByteType _byteType;
        private SignalType _sigType;
        private long _sigMode;
        private int _startBit;
        private int _bitCount;
        private ValueType _valueType;
        private double _factor;
        private double _offset;
        private double _min;
        private double _max;
        private string _unit;
        private string[] _networkNames;


        public Signal()
        {
        }

        public string Name
        {
            get { return _name; }
        }

        public ByteType ByteOrder
        {
            get { return _byteType; }
        }

        public SignalType Type
        {
            get { return _sigType; }
        }

        public long ModeValue
        {
            get { return _sigMode; }
        }

        public int PivotBit
        {
            get { return _startBit; }
        }

        public int BitCount
        {
            get { return _bitCount; }
        }

        public ValueType DataType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }

        public double Factor
        {
            get { return _factor; }
        }

        public double Offset
        {
            get { return _offset; }
        }

        public double Min
        {
            get { return _min; }
        }

        public double Max
        {
            get { return _max; }
        }

        public string Unit
        {
            get { return _unit; }
        }

        public void ParseSignal(DBInfo dbInfo,Message msgInfo, string line, string orgLine, int linePos)
        {
            string str;
            List<string> sigToken = new List<string>();
            string[] dummyToken;

            //Tokenize string
            str = line;
            str = str.Replace('|',' ');
            str = str.Replace('(', ' ');
            str = str.Replace(')', ' ');
            str = str.Replace('[', ' ');
            str = str.Replace(']', ' ');
            str = str.Replace('@', ' ');
            str = str.Replace(':', ' ');
            str = str.Replace(',', ' ');

            dummyToken = str.Split(' ');
            
            for(int i = 0; i < dummyToken.Length; i++)
            {
                if(dummyToken[i] != "")
                    sigToken.Add(dummyToken[i]);
            }

            if(sigToken.Count < 10)
                throw new Exception(linePos.ToString() + " : " + StringResource.SigDefErr + "(" + orgLine + ")");

            int index = 1;

            //Signal name
            _name = sigToken[index];
            index++;

            if(sigToken[index][0] == 'M' || sigToken[index][0] == 'm')
            {
                if (sigToken[index].Length > 1)
                {
                    str = sigToken[index];
                    _sigType = SignalType.ModeDepended;

                    str = str.Substring(1,str.Length - 1);
                    try
                    {
                        _sigMode = Convert.ToInt64(str);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                        throw new Exception(linePos.ToString() + " : " + StringResource.ModeDepErr + "(" + orgLine + ")");
                    }
                }
                else
                {
                    _sigType = SignalType.ModeSignal;
                }
                index++;
            }
            else
            {
                _sigType = SignalType.Standard;
            }

            //Start bit
            try
            {
                _startBit = Convert.ToInt32(sigToken[index]);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(linePos.ToString() + " : " + StringResource.StartBitErr + "(" + orgLine + ")");
            }
            index++;

            //Bit count
            try
            {
                _bitCount = Convert.ToInt32(sigToken[index]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(linePos.ToString() + " : " + StringResource.BitCountErr + "(" + orgLine + ")");
            }
            index++;

            if (sigToken[index].Length < 2)
                throw new Exception(linePos.ToString() + " : " + StringResource.ByteOrderErr + "(" + orgLine + ")");

            //Byte type
            if(sigToken[index][0] == '1')
            {
                _byteType = ByteType.Intel;
            }
            else
            {
                _byteType = ByteType.MotorolaBackward;
            }

            //Signed
            if (sigToken[index][1] == '-')
                _valueType = ValueType.Signed;
            else
                _valueType = ValueType.Unsigned;

            index++;

            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";

            //Factor offset
            try
            {
                _factor = Convert.ToDouble(sigToken[index], info);
                index++;
                _offset = Convert.ToDouble(sigToken[index], info);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(linePos.ToString() + " : " + StringResource.FactorOffsetErr + "(" + orgLine + ")");
            }

            index++;

            //Min Max
            try
            {
                _min = Convert.ToDouble(sigToken[index], info);
                index++;
                _max = Convert.ToDouble(sigToken[index], info);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(linePos.ToString() + " : " + StringResource.SignalMinMaxErr + "(" + orgLine + ")");
            }
            index++;

            //Signal Unit
            _unit = sigToken[index].TrimStart('"');
            _unit = _unit.TrimEnd('"');

            index++;
            _networkNames = sigToken[index].Split(',');

            msgInfo.Signals.Add(this);
        }
    }
}
