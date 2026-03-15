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

namespace Mp.Scheme.Info
{
    public partial class SchemeInfoDlg : Form
    {
        private XmlElement _xmlDoc;

        public SchemeInfoDlg(XmlElement xmlDoc)
        {
            _xmlDoc = xmlDoc;
            InitializeComponent();            
            textCtrl.Text = XmlHelper.GetParam(_xmlDoc, "description");
            runtime.Text = XmlHelper.GetParam(_xmlDoc, "type");
        }


        private void OnOKClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            XmlHelper.SetParam(_xmlDoc, "description", "string", textCtrl.Text);            
            Close();
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private void OnLinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch (Exception)
            {
            }
        }
    }
}
