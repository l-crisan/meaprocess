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
using System.Globalization;
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Statistics
{
    public partial class ClassingPSDlg : Form
    {
        private SignalInputView _signals;
        private XmlElement _xmlPS;
        private Document _doc;
        private XmlElement _xmlInSignalList;
        private XmlElement _xmlOutSignalList;
        private NumberFormatInfo _ninfo = new NumberFormatInfo();

        public ClassingPSDlg(XmlElement xmlPS, Document doc, XmlElement xmlInSignalList, XmlElement xmlOutSignalList)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            _xmlInSignalList = xmlInSignalList;
            _xmlOutSignalList = xmlOutSignalList;
            _signals = new SignalInputView(doc, xmlInSignalList);
            _signals.Dock = DockStyle.Fill;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            _ninfo.NumberDecimalSeparator = ".";

            splitContainer.Panel1.Controls.Add(_signals);
            _doc.UpdateSource(_xmlPS);
            LoadData();
        }

        private void LoadData()
        {
            name.Text = XmlHelper.GetParam(_xmlPS, "name");

            XmlElement xmlClassings = XmlHelper.GetChildByType(_xmlPS, "Mp.Stat.Classings");

            if (xmlClassings == null)
                return;

            foreach (XmlElement xmlClassing in xmlClassings.ChildNodes)
            {
                Classing cls = new Classing();
                int index = classing.Rows.Add();
                DataGridViewRow row = classing.Rows[index];

                uint sigId = (uint) XmlHelper.GetParamNumber(xmlClassing,"signal");
                XmlElement xmlSignal = _doc.GetXmlObjectById(sigId);
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                cls.ClassingType = (int)XmlHelper.GetParamNumber(xmlClassing, "type");
                row.Cells[1].Value = GetClassingName(cls.ClassingType);
                row.Cells[2].Value = "...";
                cls.InSignal = xmlSignal;

                sigId = (uint)XmlHelper.GetParamNumber(xmlClassing, "outSignal");
                cls.OutSignal = _doc.GetXmlObjectById(sigId);
                cls.SigName = XmlHelper.GetParam(cls.OutSignal, "name");
                cls.SigUnit = XmlHelper.GetParam(cls.OutSignal, "unit");
                cls.SigComment = XmlHelper.GetParam(cls.OutSignal, "comment");
                cls.SigRate = XmlHelper.GetParamDouble(cls.OutSignal, "samplerate");

                cls.Hysteresis = XmlHelper.GetParamDouble(xmlClassing, "hysteresis");
                cls.LowerLimit = XmlHelper.GetParamDouble(xmlClassing, "lowerLimit");
                cls.UpperLimit = XmlHelper.GetParamDouble(xmlClassing, "upperLimit");
                cls.NoOfClasses = (int) XmlHelper.GetParamNumber(xmlClassing, "count");
                cls.ReferenceValue = XmlHelper.GetParamDouble(xmlClassing, "refValue");
                cls.ResetNValues = (uint) XmlHelper.GetParamNumber(xmlClassing, "resetNValues");
                row.Tag = cls;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();


            if (classing.Rows.Count == 0)
            {
                errorProvider.SetError(classing, StringResource.NoClassingDefinedErr);
                return;
            }

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);

            XmlElement xmlClassings = XmlHelper.GetChildByType(_xmlPS, "Mp.Stat.Classings");

            if (xmlClassings != null)
                _doc.RemoveXmlObject(xmlClassings);

            xmlClassings = _doc.CreateXmlObject(_xmlPS, "Mp.Stat.Classings", "");

            foreach (DataGridViewRow row in classing.Rows)
            {
                Classing cls = (Classing) row.Tag;
                XmlElement xmlClassing = _doc.CreateXmlObject(xmlClassings, "Mp.Stat.Classing", "");

                uint sigId = XmlHelper.GetObjectID(cls.InSignal);
                XmlHelper.SetParamNumber(xmlClassing, "signal","uint32_t",sigId);

                if(cls.OutSignal == null)
                    cls.OutSignal = _doc.CreateXmlObject(_xmlOutSignalList, "Mp.Sig", "Mp.Stat.Sig.Class");

                XmlHelper.SetParam(cls.OutSignal, "name", "string",cls.SigName);
                XmlHelper.SetParam(cls.OutSignal, "unit", "string", cls.SigUnit);
                XmlHelper.SetParam(cls.OutSignal, "comment", "string", cls.SigComment);

                XmlHelper.SetParamDouble(cls.OutSignal, "samplerate", "double", cls.SigRate);
                XmlHelper.SetParamNumber(cls.OutSignal, "valueDataType", "uint8_t", (int)SignalDataType.UDINT);
                XmlHelper.SetParamDouble(cls.OutSignal, "physMin", "double", UInt32.MinValue);
                XmlHelper.SetParamDouble(cls.OutSignal, "physMax", "double", UInt32.MaxValue);

                uint srcID = (uint)XmlHelper.GetParamNumber(_xmlPS, "sourceId");
                XmlHelper.SetParamNumber(cls.OutSignal, "sourceNumber", "uint32_t", (long)srcID);
                string sigParams = "classingType= " + cls.ClassingType + ";";
                sigParams += "classes=" + cls.NoOfClasses.ToString() + ";";
                sigParams += "hysteresis=" + cls.Hysteresis.ToString(_ninfo) + ";";
                sigParams += "lower=" + cls.LowerLimit.ToString(_ninfo) + ";";
                sigParams += "upper=" + cls.UpperLimit.ToString(_ninfo) + ";";
                XmlHelper.SetParam(cls.OutSignal, "parameters", "string", sigParams);

                sigId = XmlHelper.GetObjectID(cls.OutSignal);
                XmlHelper.SetParamNumber(xmlClassing, "outSignal", "uint32_t", sigId);
                XmlHelper.SetParamNumber(xmlClassing, "type", "uint8_t", cls.ClassingType);
                XmlHelper.SetParamDouble(xmlClassing, "hysteresis","double", cls.Hysteresis);
                XmlHelper.SetParamDouble(xmlClassing, "lowerLimit", "double", cls.LowerLimit);
                XmlHelper.SetParamDouble(xmlClassing, "upperLimit", "double", cls.UpperLimit);
                XmlHelper.SetParamNumber(xmlClassing, "count", "uint32_t", cls.NoOfClasses);
                XmlHelper.SetParamDouble(xmlClassing, "refValue", "double", cls.ReferenceValue);
                XmlHelper.SetParamNumber(xmlClassing, "resetNValues", "uint32_t", cls.ResetNValues);
            }

            RemoveUnusedSignals();

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void channels_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
            {
                ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

                if (item.Tag is XmlElement)
                    e.Effect = DragDropEffects.Move;
            }

        }

        private void channels_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
                return;

            System.Windows.Forms.ListViewItem item = (System.Windows.Forms.ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

            XmlElement xmlSignal = item.Tag as XmlElement;

            if (xmlSignal == null)
                return;

            int index = classing.Rows.Add();
            DataGridViewRow row = classing.Rows[index];
            row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            row.Cells[1].Value = GetClassingName(0);
            row.Cells[2].Value = "...";
            Classing cls = new Classing();
            cls.InSignal = xmlSignal;
            cls.LowerLimit = XmlHelper.GetParamDouble(xmlSignal, "physMin");
            cls.UpperLimit = XmlHelper.GetParamDouble(xmlSignal, "physMax");
            cls.Hysteresis = 0;
            cls.ClassingType = 0;
            cls.NoOfClasses = 10;
            cls.ReferenceValue = 0;
            cls.SigName = "Classing" + (row.Index + 1).ToString();
            cls.SigComment = "";
            cls.SigUnit = "";
            cls.SigRate = 5.0;

            row.Tag = cls;
            cls.OutSignal = null;
        }

        private void channels_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2 || e.RowIndex == -1)
                return;

            DataGridViewRow row = classing.Rows[e.RowIndex];
            ClassingDlg dlg = new ClassingDlg((Classing) row.Tag,_doc);
            dlg.ShowDialog();
        }

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (classing.SelectedCells.Count == 0)
                return;

            int index = classing.SelectedCells[0].RowIndex;
            DataGridViewRow row = classing.Rows[index];
            classing.Rows.RemoveAt(index);
        }

        private void RemoveUnusedSignals()
        {
            for (int i = 0; i < _xmlOutSignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement)_xmlOutSignalList.ChildNodes[i];
                uint id = XmlHelper.GetObjectID(xmlSignal);
                bool found = false;
                foreach (DataGridViewRow row in classing.Rows)
                {
                    Classing cls = (Classing)row.Tag;
                    if (cls.OutSignal == xmlSignal)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _doc.RemoveXmlObject(xmlSignal);
                    --i;
                }
            }
        }


        private string GetClassingName(int type)
        {
            switch (type)
            {
                case 0:
                    return StringResource.Sampling;
                case 1:
                    return StringResource.ZeroCrossingPeak;
                case 2:
                    return StringResource.PeakCounting1;
                case 3:
                    return StringResource.PeakCounting2;
                case 4:
                    return StringResource.LevelCrossingCounting;
            }
            return "";
        }

        private void help_Click(object sender, EventArgs e)
        {

        }

        private void ClassingPSDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {

        }
    }
}
