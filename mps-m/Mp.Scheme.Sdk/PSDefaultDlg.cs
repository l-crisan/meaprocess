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
using System.Xml;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    internal partial class PSDefaultDlg : Form
    {
        private XmlElement _xmlPS;
        private Document _doc;

        public PSDefaultDlg(XmlElement xmlPS, Document doc)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            InitializeComponent();
            name.Text = XmlHelper.GetParam(xmlPS, "name");
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);
            _doc.Modified = true;
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