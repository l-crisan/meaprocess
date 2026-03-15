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
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Calculation
{
    public partial class SignalConvertPSDlg : Form
    {
        private SignalInputView _signals;
        private XmlElement _xmlPS;
        private Document _doc;
        private XmlElement _xmlInSignalList;
        private XmlElement _xmlOutSignalList;

        public class Scaling
        {
            public int ScalingType;
            public double Factor = 1;
            public double Offset = 0;
            public PointF P1 = new PointF();
            public PointF P2 = new PointF();
            public List<PointF> Table = new List<PointF>();
            public XmlElement InSignal;
            public XmlElement OutSignal;
            public string SigName;
            public string SigUnit;
            public string SigComment;
        }

        public SignalConvertPSDlg(XmlElement xmlPS, Document doc, XmlElement xmlInSignalList, XmlElement xmlOutSignalList)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            _xmlInSignalList = xmlInSignalList;
            _xmlOutSignalList = xmlOutSignalList;
            _signals = new SignalInputView(doc, xmlInSignalList);
            _signals.Dock = DockStyle.Fill;
            InitializeComponent();

            this.Icon = Document.AppIcon;
            splitContainer.Panel1.Controls.Add(_signals);
            LoadData();
            FormStateHandler.Restore(this, "Mp.Calculation.SignalConvertDlg");
        }

        private void OnChannelsDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
            {
                ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

                if (item.Tag is XmlElement)
                    e.Effect = DragDropEffects.Move;
            }

        }

        private void OnChannelsDragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
                return;

            ListViewItem item = (System.Windows.Forms.ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

            XmlElement xmlSignal = item.Tag as XmlElement;

            if (xmlSignal == null)
                return;

            int index = convertSignalTable.Rows.Add();
            DataGridViewRow row = convertSignalTable.Rows[index];
            row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            row.Cells[0].Tag = xmlSignal;
            row.Cells[1].Value = XmlHelper.GetParam(xmlSignal, "name");
            SignalDataType type = (SignalDataType) (int) XmlHelper.GetParamNumber(xmlSignal, "valueDataType");
            row.Cells[2].Value = type.ToString();
            row.Cells[3].Value = "Cast";
            row.Cells[4].Value = XmlHelper.GetParam(xmlSignal, "unit");
            row.Cells[5].Value = XmlHelper.GetParam(xmlSignal, "comment");
        }

        private void LoadData()
        {
            name.Text = XmlHelper.GetParam(_xmlPS, "name");

            convertSignalTable.Rows.Clear();

            foreach (XmlElement xmlConvertSignal in _xmlOutSignalList.ChildNodes)
            {
                int index = convertSignalTable.Rows.Add();
                DataGridViewRow row = convertSignalTable.Rows[index];

                uint fromID = (uint)XmlHelper.GetParamNumber(xmlConvertSignal, "fromSignal");

                XmlElement fromSignal = _doc.GetXmlObjectById(fromID);

                if (fromSignal == null)
                    continue;


                row.Tag = xmlConvertSignal;

                row.Cells[0].Tag = fromSignal;
                row.Cells[0].Value = XmlHelper.GetParam(fromSignal, "name");

                row.Cells[1].Value = XmlHelper.GetParam(xmlConvertSignal, "name");

                row.Cells[2].Value = GetToType((int)XmlHelper.GetParamNumber(xmlConvertSignal, "valueDataType"));

                row.Cells[3].Value = GetConvert((int)XmlHelper.GetParamNumber(xmlConvertSignal, "convert"));

                row.Cells[4].Value = XmlHelper.GetParam(xmlConvertSignal, "unit");

                row.Cells[5].Value = XmlHelper.GetParam(xmlConvertSignal, "comment");

            }
        }

        private void OnRemoveSignalToolClick(object sender, EventArgs e)
        {
            if (convertSignalTable.SelectedCells.Count == 0)
                return;

            int index = convertSignalTable.SelectedCells[0].RowIndex;
            DataGridViewRow row = convertSignalTable.Rows[index];
            convertSignalTable.Rows.RemoveAt(index);
        }


        private static int GetToType(string str)
        {
            switch (str)
            {
                case "BOOL":
                    return (int)SignalDataType.BOOL;

                case "LREAL":
                    return (int)SignalDataType.LREAL;

                case "REAL":
                    return (int)SignalDataType.REAL;

                case "USINT":
                    return (int)SignalDataType.USINT;

                case "SINT":
                    return (int)SignalDataType.SINT;

                case "UINT":
                    return (int)SignalDataType.UINT;

                case "INT":
                    return (int)SignalDataType.INT;

                case "UDINT":
                    return (int)SignalDataType.UDINT;

                case "DINT":
                    return (int)SignalDataType.DINT;

                case "ULINT":
                    return (int)SignalDataType.ULINT;

                case "LINT":
                    return (int)SignalDataType.LINT;
            }

            return 0;
        }



        private static string GetToType(int type)
        {
            SignalDataType tt = (SignalDataType)type;
            return tt.ToString();
        }


        private static int GetConvert(string str)
        {
            switch (str)
            {
                case "Cast":
                    return 0;

                case "Round":
                    return 1;

                case "Floor":
                    return 2;

                case "Ceiling":
                    return 3;

                case "Abs":
                    return 4;
            }
            return 0;
        }

        private static string GetConvert(int c)
        {
            switch (c)
            {
                case 0:
                    return "Cast";

                case 1:
                    return "Round";

                case 2:
                    return "Floor";

                case 3:
                    return "Ceiling";

                case 4:
                    return "Abs";
            }

            return "Cast";
        }


        private void OnOKClick(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (convertSignalTable.Rows.Count == 0)
            {
                errorProvider.SetError(convertSignalTable, StringResource.NoScalingDefinedErr);
                return;
            }

            Dictionary<XmlElement, bool> currentSignals = new Dictionary<XmlElement, bool>();

            foreach (DataGridViewRow row in convertSignalTable.Rows)
            {
                XmlElement xmlConvertSignal = row.Tag as XmlElement;

                if (xmlConvertSignal == null)
                {
                    xmlConvertSignal = _doc.CreateXmlObject(_xmlOutSignalList, "Mp.Sig", "Mp.Calculation.Sig.Convert");
                    row.Tag = xmlConvertSignal;
                }

                XmlElement fromSignal = (XmlElement)row.Cells[0].Tag;

                uint fromSigId = XmlHelper.GetObjectID(fromSignal);
                double samplerate = XmlHelper.GetParamDouble(fromSignal, "samplerate");
                double min = XmlHelper.GetParamDouble(fromSignal, "physMin");
                double max = XmlHelper.GetParamDouble(fromSignal, "physMax");
                XmlHelper.SetParamNumber(xmlConvertSignal, "fromSignal", "uint32_t", fromSigId);
                XmlHelper.SetParam(xmlConvertSignal, "name", "string", row.Cells[1].Value.ToString());
                XmlHelper.SetParamNumber(xmlConvertSignal, "valueDataType", "uint8_t", GetToType(row.Cells[2].Value.ToString()));
                XmlHelper.SetParamNumber(xmlConvertSignal, "convert", "uint8_t", GetConvert(row.Cells[3].Value.ToString()));
                XmlHelper.SetParam(xmlConvertSignal, "unit", "string", row.Cells[4].Value.ToString());
                XmlHelper.SetParam(xmlConvertSignal, "comment", "string", row.Cells[5].Value.ToString());
                XmlHelper.SetParamDouble(xmlConvertSignal, "samplerate", "double", samplerate);
                XmlHelper.SetParamDouble(xmlConvertSignal, "physMin", "double", min);
                XmlHelper.SetParamDouble(xmlConvertSignal, "physMax", "double", max);

                int srcID = (int) XmlHelper.GetParamNumber(fromSignal, "sourceNumber");
                XmlHelper.SetParamNumber(xmlConvertSignal, "sourceNumber", "uint32_t", (long)srcID);
                currentSignals.Add(xmlConvertSignal, true);
            }

            //Remove unused Signals.
            List<XmlElement> signalToDelete = new List<XmlElement>();

            foreach (XmlElement sig in _xmlOutSignalList.ChildNodes)
            {
                if (!currentSignals.ContainsKey(sig))
                    signalToDelete.Add(sig);
            }

            foreach (XmlElement sig in signalToDelete)
                _doc.RemoveXmlObject(sig);

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, "Mp.Calculation.SignalConvertDlg");
            base.OnFormClosing(e);
        }
    }
}
