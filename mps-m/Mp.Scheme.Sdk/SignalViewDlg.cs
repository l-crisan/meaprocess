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
    public partial class SignalViewDlg : Form
    {
        private Document _doc;

        public SignalViewDlg(XmlElement xmlSigList, Document doc)
        {
            _doc = doc;

            InitializeComponent();
            this.Icon = Document.AppIcon;

            foreach (XmlElement xmlSignal in xmlSigList.ChildNodes)
            {
                if (XmlHelper.GetObjectID(xmlSignal) == 0)
                    continue;

                int i = signalGrid.Rows.Add();
                DataGridViewRow row = signalGrid.Rows[i];

                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[1].Value = XmlHelper.GetParam(xmlSignal, "unit");
                row.Cells[2].Value = XmlHelper.GetParam(xmlSignal, "comment");
                row.Cells[3].Value = XmlHelper.GetParamDouble(xmlSignal, "samplerate").ToString() + " Hz";
                row.Cells[4].Value = GetDataTypeString((int)XmlHelper.GetParamNumber(xmlSignal, "valueDataType"));
                row.Cells[5].Value = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                row.Cells[6].Value = XmlHelper.GetParamDouble(xmlSignal, "physMax");

                XmlElement xmlSource = _doc.GetXmlObjectById((uint)XmlHelper.GetParamNumber(xmlSignal, "sourceNumber"));

                if (xmlSource == null)
                    row.Cells[7].Value = "MeaProcess";
                else
                    row.Cells[7].Value = XmlHelper.GetParam(xmlSource, "name");

                row.Tag = xmlSignal;
            }

            FormStateHandler.Restore(this, Document.RegistryKey + "SignalViewDlg");
        }


        private string GetDataTypeString(int type)
        {
            SignalDataType sigType = (SignalDataType)type;
            return sigType.ToString();
        }


        private void OnOKClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in signalGrid.Rows)
            {
                XmlElement xmlSignal = (XmlElement)row.Tag;

                XmlHelper.SetParam(xmlSignal, "name", "string", (string)row.Cells[0].Value);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[1].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[2].Value);
            }
            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            FormStateHandler.Save(this, Document.RegistryKey + "SignalViewDlg");
        }
    }
}
