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
using System.ComponentModel;
using System.Windows.Forms;

namespace Mp.Scheme.Designer
{
	public class CustomTypeDescriptor : ICustomTypeDescriptor
	{
        public Control HostControl;
        public bool Visible = true;
		PropertyDescriptorCollection _props;

		public CustomTypeDescriptor(Control hostControl, PropertyDescriptorCollection props)
		{
			HostControl = hostControl;
			_props = props;
		}
        
		public AttributeCollection GetAttributes()
		{
			return new AttributeCollection(null);
		}

		public String GetClassName()
		{
			return null;
		}

		public String GetComponentName()
		{
			return null;
		}

		public TypeConverter GetConverter()
		{
			return null;
		}

		public EventDescriptor GetDefaultEvent()
		{
			return null;
		}

		public PropertyDescriptor GetDefaultProperty()
		{
			return null;
		}

		public Object GetEditor(Type editorBaseType)
		{
			return null;
		}

		public EventDescriptorCollection GetEvents()
		{
			return new EventDescriptorCollection(null);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes)
		{
			return new EventDescriptorCollection(null);
		}

		public PropertyDescriptorCollection GetProperties()
		{
			return _props;
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return _props;
		}

		public Object GetPropertyOwner (PropertyDescriptor pd)
		{
			return HostControl;
		}
	}
}
