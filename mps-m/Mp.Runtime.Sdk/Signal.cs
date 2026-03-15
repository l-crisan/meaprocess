//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2015  Laurentiu-Gheorghe Crisan
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System.Xml;
using System.IO;
using Mp.Utils;

namespace Mp.Runtime.Sdk
{
    /// <summary>
    /// The representation of a visual process station.
    /// </summary>
    public class Signal
    {
        private XmlElement _xmlRep;

        /// <summary>
        /// The signal data type.
        /// </summary>
        public enum DataTypes
        {
            BOOL  = 1,
            LREAL = 2,
            REAL = 3,
            USINT = 4,
            SINT = 5,
            UINT = 6,
            INT = 7,
            UDINT = 8,
            DINT = 9,
            ULINT = 10,
            LINT = 11,
            StringType = 12,
            ObjectType = 14
        };

        private DataTypes _dataType;
        private string _name;
        private string _unit;
        private string _comment;
        private double _minimum;
        private double _maximum;
        private double _sampleRate;
        private uint _dataTypeSize;
        private ushort _physSourceId;
        private uint _signalId;
        private ISignalScaling _scaling;
        private int _signalIndex;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Signal(XmlElement xmlRep)
        {
            _xmlRep = xmlRep;
            _scaling = new FactorOffsetSignalScaling(1, 0);
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public Signal(Signal toCopy)
        {
            this._dataType = toCopy._dataType;
            this._name = toCopy._name;
            this._unit = toCopy._unit;
            this._comment = toCopy._comment;
            this._minimum = toCopy._minimum;
            this._maximum = toCopy._maximum;
            this._sampleRate = toCopy._sampleRate;
            this._dataTypeSize = toCopy._dataTypeSize;
            this._physSourceId = toCopy._physSourceId;
            this._signalId = toCopy._signalId;
            this._scaling = toCopy._scaling;
            this._signalIndex = toCopy._signalIndex;
            this._xmlRep = toCopy._xmlRep;
            _scaling = new FactorOffsetSignalScaling(1, 0);
        }

        public XmlElement XmlRep
        {
            get { return _xmlRep; }
        }
        /// <summary>
        /// Gets or sets the signal id.
        /// </summary>
        public uint SignalID
        {
            get { return _signalId; }
            set { _signalId = value; }
        }

        /// <summary>
        /// Gets or sets the signal index in the port signal list.
        /// </summary>
        public int SignalIndex
        {
            get { return _signalIndex; }
            set { _signalIndex = value; }
        }

        /// <summary>
        /// Gets or sets the signal name.
        /// </summary>
        public string Name
        {
            get{ return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the signal unit.
        /// </summary>
        public string Unit
        {
            get{ return _unit;}
            set { _unit = value; }
        }

        /// <summary>
        /// Gets or sets the signal comment.
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        /// <summary>
        /// Gets or sets the signal minimum value.
        /// </summary>
        public double Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        /// <summary>
        /// Gets or sets the signal maximum value.
        /// </summary>
        public double Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        /// <summary>
        /// Gets or sets the signal sample rate in Hz.
        /// </summary>
        public double SampleRate
        {
            get{ return _sampleRate;}
            set { _sampleRate = value; }
            
        }
        /// <summary>
        /// Gets the data type size.
        /// </summary>
        public uint DataTypeSize
        {
            get 
            
            {
                return _dataTypeSize; 
            }

            set { _dataTypeSize = value; }
        }

        /// <summary>
        /// Gest or sets the physical source identifier.
        /// </summary>
        public ushort PhysSourceId
        {
            get { return _physSourceId; }
            set { _physSourceId = value; }
        }

        /// <summary>
        /// Gets or sets the signal data type.
        /// </summary>
        public DataTypes DataType
        {
            set
            {
                _dataType = value;
                switch (_dataType)
                {
                    case DataTypes.REAL:  //Real 32 bit type.
                    case DataTypes.UDINT:  //Unsigned integer 32 bit type.
                    case DataTypes.DINT:  //Signed integer 32 bit type.
                    case DataTypes.StringType: // String type.
                    case DataTypes.ObjectType: // Object type.
                        _dataTypeSize = 4;
                        break;
                    case DataTypes.USINT:  // Unsigned integer 8 bit type.
                    case DataTypes.SINT:  // Signed integer 8 bit type.
                    case DataTypes.BOOL:
                        _dataTypeSize = 1;
                        break;
                    case DataTypes.INT:  // Unsigned integer 16 bit type.
                    case DataTypes.UINT:  // Signed integer 16 bit type.
                        _dataTypeSize = 2;
                        break;
                    case DataTypes.ULINT: // Unsigned integer 64 bit type.
                    case DataTypes.LINT: // Signed integer 64 bit type.
                    case DataTypes.LREAL:  // Real 64 bit type.
                        _dataTypeSize = 8;
                        break;
                }
            }

            get { return _dataType; }
        }

        /// <summary>
        /// Ovveride this to load your scaling object from xml.
        /// </summary>
        /// <param name="xmlScaling">The scaling object xml representation.</param>
        public virtual void OnLoadScalingObject(XmlElement xmlScaling)
        {
            if( xmlScaling == null)
                return;

            string scalingType = xmlScaling.GetAttribute("subType");          

            if (scalingType == "Mp.Scaling.FactorOffset")
            {
                _scaling = new FactorOffsetSignalScaling(XmlHelper.GetParamDouble(xmlScaling, "factor"),
                                                         XmlHelper.GetParamDouble(xmlScaling, "offset"));
            }
            else
            {
                _scaling = new FactorOffsetSignalScaling(1,0);
            }
        }

        public double ReadValue(byte[] data, int offset)
        {
            double value = 0;
            MemoryStream ms = new MemoryStream(data);
            ms.Seek(offset, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(ms);
            
            switch (_dataType)
            {
                case DataTypes.REAL:
                {
                    float v =  br.ReadSingle();
                    value = _scaling.ScaleValue(v);
                }
                break;

                case DataTypes.UDINT:
                {
                    uint v =  br.ReadUInt32();
                    value = _scaling.ScaleValue(v);
                }
                break;

                case DataTypes.DINT:
                {
                    int v =  br.ReadInt32();
                    value = _scaling.ScaleValue(v);
                }
                break;

                case DataTypes.USINT:
                {
                    byte v = br.ReadByte();
                    value = _scaling.ScaleValue(v);
                }
                break;

                case DataTypes.SINT:
                {
                    sbyte v =  br.ReadSByte();
                    value = _scaling.ScaleValue(v);
                }
                break;

                case DataTypes.BOOL:
                    value = br.ReadByte();
                break;

                case DataTypes.INT: 
                {
                    short v =  br.ReadInt16();
                    value = _scaling.ScaleValue(v);
                }
                break;

                case DataTypes.UINT:
                {
                    ushort v =  br.ReadUInt16();
                    value = _scaling.ScaleValue(v);
                }
                break;

                case DataTypes.ULINT:
                {
                    ulong v =  br.ReadUInt64();
                    value = _scaling.ScaleValue(v);
                }
                break;

                case DataTypes.LINT:
                {
                    long v =  br.ReadInt64();
                    value = _scaling.ScaleValue(v);
                }
                break;

                case DataTypes.LREAL:
                {
                    double v =  br.ReadDouble();
                    value = _scaling.ScaleValue(v);
                }
                break;
            }

            return value;
        }
        
        /// <summary>
        /// Gets or sets the signal scaling.
        /// </summary>
        public ISignalScaling Scaling
        {
            set { _scaling = value; }
            get { return _scaling; }
        }
    }
}
