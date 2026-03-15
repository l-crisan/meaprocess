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
using System.Xml;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    public partial class StopPSPropDlg : Form
    {
        private XmlElement _xmlPs;

        public StopPSPropDlg(XmlElement xmlPs)
        {
            _xmlPs = xmlPs;
            InitializeComponent();
            this.Icon = Document.AppIcon;
            name.Text = XmlHelper.GetParam(_xmlPs, "name");
            delay.Text = XmlHelper.GetParamNumber(_xmlPs, "delay").ToString();
        }

        private void OnDelayValidating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(delay.Text);
            }
            catch(Exception ex)
            {
                errorProvider.SetError(delay,ex.Message);
                e.Cancel = true;
            }

        }

        private void OnOKClick(object sender, EventArgs e)
        {
            XmlHelper.SetParamNumber(_xmlPs, "delay", "uint32_t", Convert.ToUInt32(delay.Text));
            XmlHelper.SetParam(_xmlPs, "name","string", name.Text);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
