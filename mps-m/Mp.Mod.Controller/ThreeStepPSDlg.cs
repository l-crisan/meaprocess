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
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Controller
{
    public partial class ThreeStepPSDlg : Form
    {
        private XmlElement _xmlSignalList;
        private XmlElement _xmlPS;
        private Document _doc;

        public ThreeStepPSDlg(Document doc, XmlElement xmlPS, XmlElement xmlSignalList)
        {
            _doc = doc;
            _xmlPS = xmlPS;
            _xmlSignalList = xmlSignalList;
            
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            psName.Text = XmlHelper.GetParam(_xmlPS, "name");
            setPoint.Text = XmlHelper.GetParamDouble(_xmlPS, "setPoint").ToString();
            lowerOnLimit.Text = XmlHelper.GetParamDouble(_xmlPS, "lowerOnLimit").ToString();
            lowerOffLimit.Text = XmlHelper.GetParamDouble(_xmlPS, "lowerOffLimit").ToString();
            upperOnLimit.Text = XmlHelper.GetParamDouble(_xmlPS, "upperOnLimit").ToString();
            upperOffLimit.Text = XmlHelper.GetParamDouble(_xmlPS, "upperOffLimit").ToString();
        }

        private void value_Validating(object sender, CancelEventArgs e)
        {

            errorProvider.Clear();
            Control ctrl = (Control)sender;
            try
            {
                Convert.ToDouble(ctrl.Text);
            }
            catch(Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(ctrl, ex.Message);
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            XmlHelper.SetParam(_xmlPS, "name","string", psName.Text);
            XmlHelper.SetParamDouble(_xmlPS, "setPoint", "double", Convert.ToDouble(setPoint.Text));

            XmlHelper.SetParamDouble(_xmlPS, "lowerOnLimit", "double", Convert.ToDouble(lowerOnLimit.Text));
            XmlHelper.SetParamDouble(_xmlPS, "lowerOffLimit", "double",Convert.ToDouble(lowerOffLimit.Text));
            XmlHelper.SetParamDouble(_xmlPS, "upperOnLimit", "double", Convert.ToDouble(upperOnLimit.Text));
            XmlHelper.SetParamDouble(_xmlPS, "upperOffLimit", "double", Convert.ToDouble(upperOffLimit.Text));
            _doc.Modified = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1580);
        }
    }
}
