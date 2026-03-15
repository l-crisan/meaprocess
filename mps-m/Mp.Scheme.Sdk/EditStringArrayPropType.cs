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
using System.Windows.Forms;

namespace Mp.Scheme.Sdk
{
    public partial class EditStringArrayPropType : Form
    {
        public EditStringArrayPropType(String enumText)
        {
            InitializeComponent();
            Icon = Document.AppIcon;
            enumeration.Text = enumText;
        }

        public string EnumText
        {
            get { return enumeration.Text; }
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }        
    }
}
