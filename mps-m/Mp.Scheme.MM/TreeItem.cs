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
using System.Drawing;
using System.IO;


namespace Mp.Scheme.MM
{
    internal class TreeItem
    {
        private Sdk.Module _module = null;
        private string _name;
        private string _version;
        private string _description;
        private string _file;
        private string _manufacturer;

        public TreeItem(Sdk.Module  module)
        {
            _module = module;
            _name = _module.ModuleName;
            _version = _module.ModuleVersion;
            _description = _module.ModuleDescription;
            _file = _module.ModuleFile;
            _manufacturer = _module.ModuleManufacturer;
        }


        public Sdk.Module ItemModule
        {
            get { return _module; }
        }


        public string Name
        {
            get{ return _name;}
            set{ ;}
        }


        public string Version
        {
            get{ return _version;}
            set{;}
        }


        public string Created
        {
            get
            {
                try
                {
                    DateTime dateTime = System.IO.File.GetLastWriteTime(_file);
                    return dateTime.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return "";
                }
            }

            set { ;}
        }


        public string Manufacturer
        {
            get{ return _manufacturer;}
            set{ ;}
        }


        public string Description
        {
            get{ return _description;}
            set{ ;}
        }


        public string File
        {
            get { return  Path.GetFileName(_file); }
            set{;}
        }


        public Bitmap Icon
        {
            get
            {
                if (_module == null)
                    return Resource.Module.ToBitmap();

                if (_module.ParentType != null)
                    return Resource.Module.ToBitmap();
                else
                    return Resource.Runtime.ToBitmap();
            }
            set{;}
        }
    }
}
