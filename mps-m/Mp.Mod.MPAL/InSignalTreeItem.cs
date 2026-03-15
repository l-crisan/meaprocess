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

using System;
using System.Xml;
using System.Drawing;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.MPAL
{
    internal enum PropertyType
    {
        SampleRate,
        Minimum,
        Maximum,
        Factor,
        Offset
    }

    internal class MpalInSignalTreeItem
    {
        private XmlElement _xmlSignal;
        private string _name;
        private string _dataType;
        private bool _isPort;
        private bool _isSignalProp;
        private Port _port;
        private PropertyType _propType;

        public MpalInSignalTreeItem(string name, Port port)
        {
            _isPort = true;
            _name = name;
            _port = port;
        }

        public MpalInSignalTreeItem(XmlElement signal, PropertyType prop, Document doc)
        {
            if (XmlHelper.GetObjectID(signal) == 0)
            {
                _xmlSignal = doc.GetXmlObjectById(Convert.ToUInt32(signal.InnerText));
            }
            else
            {
                _xmlSignal = signal;
            }
            _propType = prop;

            _isSignalProp = true;
            _isPort = false;

            if(prop == PropertyType.SampleRate)
                _name = "Sample rate (Hz)";
            else
                _name = prop.ToString();

            _dataType = "LREAL";
        }

        public MpalInSignalTreeItem(XmlElement signal,Document doc)
        {
            if (XmlHelper.GetObjectID(signal) == 0)
            {
                _xmlSignal = doc.GetXmlObjectById(Convert.ToUInt32(signal.InnerText));
            }
            else
            {
                _xmlSignal = signal;
            }
            _isPort = false;
            _isSignalProp = false;
            
            _name = XmlHelper.GetParam(_xmlSignal, "name");

            switch (XmlHelper.GetParamNumber(_xmlSignal, "valueDataType"))
            {
	            case 1:  //< Boolean 8 bit type.
                    _dataType = "BOOL";
                break;
		        case  2:  //< Real 64 bit type.
                    _dataType = "LREAL";
                break;
		        case  3:  //< Real 32 bit type.
                    _dataType = "REAL";
                break;
		        case  4:  //< Unsigned integer 8 bit type.
                    _dataType = "USINT";
                break;
		        case  5:  //< Signed integer 8 bit type.
                    _dataType = "SINT";
                break;
		        case  6:  //< Unsigned integer 16 bit type.
                    _dataType = "UINT";
                break;
		        case  7:  //< Signed integer 16 bit type.
                    _dataType = "INT";
                break;
		        case  8:  //< Unsigned integer 32 bit type.
                    _dataType = "UDINT";
                break;
		        case  9:  //< Signed integer 32 bit type.
                    _dataType = "DINT";
                break;
		        case 10: //< Unsigned integer 64 bit type.
                    _dataType = "ULINT";
                break;
		        case  11: //< Signed integer 64 bit type.
                    _dataType = "LINT";
                break;
		        case 12: //< String type.
                    _dataType = "STRING";
                break;
		        case 14: //< Object type.
                break;
            }
        }

        public PropertyType PropType
        {
            get { return _propType; }
        }

        public bool IsPort
        {
            get { return _isPort; }
        }

        public bool IsSignalProp
        {
            get { return _isSignalProp; }
        }

        public XmlElement Signal
        {
            get { return _xmlSignal; }
        }

        public Port PortObj
        {
            get { return _port; }
        }

        public Bitmap Icon
        {
            get
            {
                if(_isSignalProp)
                    return Images.Property.ToBitmap();

                if (_isPort)
                    return Images.Port.ToBitmap();
                else
                    return Images.Signal.ToBitmap();
            }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }
    }
}
