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
using Mp.Scheme.Sdk;
using Mp.Utils;
using System.Xml;

namespace Mp.Mod.Controller
{
    public partial class TwoPointPSDlg : Form
    {
        private XmlElement _xmlSignalList;
        private XmlElement _xmlPS;
        private Document _doc;

        public TwoPointPSDlg(Document doc, XmlElement xmlPS, XmlElement xmlSignalList)
        {
            _doc = doc;
            _xmlPS = xmlPS;
            _xmlSignalList = xmlSignalList;
            
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            psName.Text = XmlHelper.GetParam(_xmlPS, "name");
            ctrlUpperValue.Text = XmlHelper.GetParamDouble(_xmlPS, "upperLimit").ToString();
            ctrlLowerValue.Text = XmlHelper.GetParamDouble(_xmlPS, "lowerLimit").ToString();
            setPoint.Text = XmlHelper.GetParamDouble(_xmlPS, "setPoint").ToString();
        }
      

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();            
            XmlHelper.SetParamDouble(_xmlPS, "upperLimit", "double", Convert.ToDouble(ctrlUpperValue.Text));
            XmlHelper.SetParamDouble(_xmlPS, "lowerLimit", "double", Convert.ToDouble(ctrlLowerValue.Text));
            XmlHelper.SetParamDouble(_xmlPS, "setPoint", "double", Convert.ToDouble(setPoint.Text));

            DialogResult = System.Windows.Forms.DialogResult.OK;
            _doc.Modified = true;
            Close();
        }

        private void Value_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            Control ctrl = (Control)sender;
            try
            {
                Convert.ToDouble(ctrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ctrl, ex.Message);
                e.Cancel = true;
            }
        }
    

        private void cancel_Click(object sender, EventArgs e)
        {

            Close();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1570);
        }
    }
}
