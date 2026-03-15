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
    public partial class ThreeStepPortDlg : Form
    {
        private XmlElement _xmlSignalList;
        private XmlElement _xmlPS;
        private Document _doc;

        public ThreeStepPortDlg(Document doc, XmlElement xmlPS, XmlElement xmlSignalList)
        {
            _xmlSignalList = xmlSignalList;
            _xmlPS = xmlPS;
            _doc = doc;

            InitializeComponent();
            this.Icon = Document.AppIcon;

            if (_xmlSignalList.ChildNodes.Count == 0)
            {
                outOffValue.Text = "0";
                outOn1Value.Text = "5";
                outOn2Value.Text = "-5";
            }
            else
            {
                XmlElement xmlSignal = (XmlElement)_xmlSignalList.ChildNodes[0];

                outOffValue.Text = XmlHelper.GetParamDouble(xmlSignal, "outOffValue").ToString();
                outOn1Value.Text = XmlHelper.GetParamDouble(xmlSignal, "outOn1Value").ToString();
                outOn2Value.Text = XmlHelper.GetParamDouble(xmlSignal, "outOn2Value").ToString();
                signalName.Text = XmlHelper.GetParam(xmlSignal, "name");
                unit.Text = XmlHelper.GetParam(xmlSignal, "unit");
                comment.Text = XmlHelper.GetParam(xmlSignal, "comment");
            }
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

        private void signalName_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            if (signalName.Text == null || signalName.Text == "")
            {

                errorProvider.SetError(signalName, StringResource.SignalNameErr);
                e.Cancel = true;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (signalName.Text == null || signalName.Text == "")
            {
                errorProvider.SetError(signalName, StringResource.SignalNameErr);
                return;
            }

            _doc.UpdateSource(_xmlPS);

            XmlElement xmlSignal = null;
            if (_xmlSignalList.ChildNodes.Count == 0)
                xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Controller.Sig.Ctrl");
            else
                xmlSignal = (XmlElement)_xmlSignalList.ChildNodes[0];

            double off = Convert.ToDouble(outOffValue.Text);
            double on1 = Convert.ToDouble(outOn1Value.Text);
            double on2 = Convert.ToDouble(outOn2Value.Text);
            XmlHelper.SetParamDouble(xmlSignal, "outOffValue", "double", off);
            XmlHelper.SetParamDouble(xmlSignal, "outOn1Value", "double", on1);
            XmlHelper.SetParamDouble(xmlSignal, "outOn2Value", "double", on2);
            XmlHelper.SetParam(xmlSignal, "name", "string", signalName.Text);
            XmlHelper.SetParam(xmlSignal, "unit", "string", unit.Text);
            XmlHelper.SetParam(xmlSignal, "comment", "string", comment.Text);
            XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)SignalDataType.LREAL);
            XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", XmlHelper.GetParamNumber(_xmlPS, "sourceId"));

            XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Math.Min(Math.Min(off, on1),on2));
            XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Math.Max(Math.Max(off, on1), on2));
            _doc.Modified = true;
            Close();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1580);
        }
    }
}
