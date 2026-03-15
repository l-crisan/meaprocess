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
    public partial class ScalingPSDlg : Form
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

        public ScalingPSDlg(XmlElement xmlPS, Document doc, XmlElement xmlInSignalList, XmlElement xmlOutSignalList)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            _xmlInSignalList = xmlInSignalList;
            _xmlOutSignalList = xmlOutSignalList;
            _signals = new SignalInputView(doc, xmlInSignalList);
            _signals.Dock = DockStyle.Fill;
            InitializeComponent();
            this.Icon = Document.AppIcon;
            DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn)scalingGrid.Columns[1];
            col.Items.Clear();
            col.Items.Add(StringResource.LinearScaling);
            col.Items.Add(StringResource.TwoPointScaling);
            col.Items.Add(StringResource.TableScaling);

            splitContainer.Panel1.Controls.Add(_signals);
            LoadData();
            FormStateHandler.Restore(this, "Mp.Calculation.ScalingDlg");
        }

        private string GetScalingName(int scaling)
        {
            switch (scaling)
            {
                case 0:
                    return StringResource.LinearScaling;
                case 1:
                    return StringResource.TwoPointScaling;
                case 2:
                    return StringResource.TableScaling;
            }

            return "";
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

            int index = scalingGrid.Rows.Add();
            DataGridViewRow row = scalingGrid.Rows[index];
            row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            row.Cells[1].Value = GetScalingName(0);
            row.Cells[2].Value = "...";
            Scaling scaling = new Scaling();
            scaling.InSignal = xmlSignal;
            scaling.SigName = "Scaling" + (row.Index + 1).ToString();
            scaling.SigComment = "";
            scaling.SigUnit = "";
            row.Tag = scaling;
            scaling.OutSignal = null;
        }

        private void LoadData()
        {
            name.Text = XmlHelper.GetParam(_xmlPS, "name");

            XmlElement xmlScalings = XmlHelper.GetChildByType(_xmlPS, "Mp.Calculation.Scalings");

            if (xmlScalings == null)
                return;

            foreach (XmlElement xmlScaling in xmlScalings.ChildNodes)
            {
                Scaling scaling = new Scaling();
                int index = scalingGrid.Rows.Add();
                DataGridViewRow row = scalingGrid.Rows[index];

                uint sigId = (uint)XmlHelper.GetParamNumber(xmlScaling, "signal");
                XmlElement xmlSignal = _doc.GetXmlObjectById(sigId);
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                scaling.ScalingType = (int)XmlHelper.GetParamNumber(xmlScaling, "type");
                row.Cells[1].Value = GetScalingName(scaling.ScalingType);
                row.Cells[2].Value = "...";
                scaling.InSignal = xmlSignal;

                sigId = (uint)XmlHelper.GetParamNumber(xmlScaling, "outSignal");
                scaling.OutSignal = _doc.GetXmlObjectById(sigId);
                scaling.SigName = XmlHelper.GetParam(scaling.OutSignal, "name");
                scaling.SigUnit = XmlHelper.GetParam(scaling.OutSignal, "unit");
                scaling.SigComment = XmlHelper.GetParam(scaling.OutSignal, "comment");

                switch (scaling.ScalingType)
                {
                    case 0://Liniear
                        scaling.Factor = XmlHelper.GetParamDouble(xmlScaling, "factor");
                        scaling.Offset = XmlHelper.GetParamDouble(xmlScaling, "offset");
                    break;
                    
                    case 1: //2 Point
                        scaling.P1.X = (float) XmlHelper.GetParamDouble(xmlScaling, "p1x");
                        scaling.P1.Y = (float) XmlHelper.GetParamDouble(xmlScaling, "p1y");
                        scaling.P2.X = (float) XmlHelper.GetParamDouble(xmlScaling, "p2x");
                        scaling.P2.Y = (float) XmlHelper.GetParamDouble(xmlScaling, "p2y");
                    break;

                    case 2: //Table
                    {
                        string table = XmlHelper.GetParam(xmlScaling, "table");
                        string[] pairs = table.Split(';');

                        foreach (string pair in pairs)
                        {
                            string[] points = pair.Split(',');
                            PointF point = new PointF();
                            point.X = Convert.ToSingle(points[0]);
                            point.Y = Convert.ToSingle(points[1]);
                            scaling.Table.Add(point);
                        }
                    }
                    break;
                }
                
                row.Tag = scaling;
            }
        }

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (scalingGrid.SelectedCells.Count == 0)
                return;

            int index = scalingGrid.SelectedCells[0].RowIndex;
            DataGridViewRow row = scalingGrid.Rows[index];
            scalingGrid.Rows.RemoveAt(index);
        }

        private void scalingGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2 || e.RowIndex == -1)
                return;
            
            DataGridViewRow row = scalingGrid.Rows[e.RowIndex];
            Scaling scaling = (Scaling)row.Tag;

            string scalingType = row.Cells[1].Value.ToString();
            
            if (scalingType == StringResource.LinearScaling)
            {
                LinearScalingDlg dlg = new LinearScalingDlg(scaling);
                dlg.ShowDialog();
            }
            else if (scalingType == StringResource.TwoPointScaling)
            {
                TwoPointScalingDlg dlg = new TwoPointScalingDlg(scaling);
                dlg.ShowDialog();
            }

            else if (scalingType == StringResource.TableScaling)
            {
                TableScalingDlg dlg = new TableScalingDlg(scaling);
                dlg.ShowDialog();
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (scalingGrid.Rows.Count == 0)
            {
                errorProvider.SetError(scalingGrid, StringResource.NoScalingDefinedErr);
                return;
            }

            foreach (DataGridViewRow row in scalingGrid.Rows)
            {
                Scaling cls = (Scaling)row.Tag;
                if (cls.Table.Count == 0 && cls.ScalingType == 2)
                {
                    string message = String.Format(StringResource.TableScalingErr, cls.SigName);
                    errorProvider.SetError(scalingGrid, message);
                    return;
                }
            }

            _doc.UpdateSource(_xmlPS);

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);

            XmlElement xmlScalings = XmlHelper.GetChildByType(_xmlPS, "Mp.Calculation.Scalings");

            if (xmlScalings != null)
                _doc.RemoveXmlObject(xmlScalings);

            xmlScalings = _doc.CreateXmlObject(_xmlPS, "Mp.Calculation.Scalings", "");

            foreach (DataGridViewRow row in scalingGrid.Rows)
            {
                Scaling cls = (Scaling)row.Tag;
                XmlElement xmlScaling = _doc.CreateXmlObject(xmlScalings, "Mp.Calculation.Scaling", "");

                uint sigId = XmlHelper.GetObjectID(cls.InSignal);
                XmlHelper.SetParamNumber(xmlScaling, "signal", "uint32_t", sigId);

                if (cls.OutSignal == null)
                    cls.OutSignal = _doc.CreateXmlObject(_xmlOutSignalList, "Mp.Sig", "Mp.Sig");

                XmlHelper.SetParam(cls.OutSignal, "name", "string", cls.SigName);
                XmlHelper.SetParam(cls.OutSignal, "unit", "string", cls.SigUnit);
                XmlHelper.SetParam(cls.OutSignal, "comment", "string", cls.SigComment);

                XmlHelper.SetParamNumber(cls.OutSignal, "valueDataType", "uint8_t", (int)SignalDataType.LREAL);
//                XmlHelper.SetParamDouble(cls.OutSignal, "physMin", "double", cls.InSignal);
//                XmlHelper.SetParamDouble(cls.OutSignal, "physMax", "double", UInt32.MaxValue);

                uint srcID = (uint)XmlHelper.GetParamNumber(_xmlPS, "sourceId");
                XmlHelper.SetParamNumber(cls.OutSignal, "sourceNumber", "uint32_t", (long)srcID);
                sigId = XmlHelper.GetObjectID(cls.OutSignal);
                XmlHelper.SetParamNumber(xmlScaling, "outSignal", "uint32_t", sigId);
                XmlHelper.SetParamNumber(xmlScaling, "type", "uint8_t", cls.ScalingType);

                switch (cls.ScalingType)
                {
                    case 0: //Liniar
                        XmlHelper.SetParamDouble(xmlScaling, "factor", "double", cls.Factor);
                        XmlHelper.SetParamDouble(xmlScaling, "offset", "double", cls.Offset);
                    break;

                    case 1: //2 Point
                        XmlHelper.SetParamDouble(xmlScaling, "p1x", "double", cls.P1.X);
                        XmlHelper.SetParamDouble(xmlScaling, "p1y", "double", cls.P1.Y);
                        XmlHelper.SetParamDouble(xmlScaling, "p2x", "double", cls.P2.X);
                        XmlHelper.SetParamDouble(xmlScaling, "p2y", "double", cls.P2.Y);
                    break;

                    case 2: //Table
                    {
                        StringBuilder builder = new StringBuilder();


                        foreach (PointF point in cls.Table)
                        {
                            builder.Append(point.X.ToString());
                            builder.Append(",");
                            builder.Append(point.Y.ToString());
                            builder.Append(";");
                        }

                        string table = builder.ToString().TrimEnd(';');
                        XmlHelper.SetParam(xmlScaling, "table", "string", table);
                    }
                    break;
                }
            }
            RemoveUnusedSignals();

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void RemoveUnusedSignals()
        {
            for(int i = 0; i < _xmlOutSignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement) _xmlOutSignalList.ChildNodes[i];
                uint id = XmlHelper.GetObjectID(xmlSignal);
                bool found = false;
                foreach (DataGridViewRow row in scalingGrid.Rows)
                {
                    Scaling cls = (Scaling)row.Tag;
                    if( cls.OutSignal == xmlSignal)
                    {
                        found = true;
                        break;
                    }
                }

                if(!found)
                {
                    _doc.RemoveXmlObject(xmlSignal);
                    --i;
                }
            }
        }

        private void scalingGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;

            DataGridViewRow row = scalingGrid.Rows[e.RowIndex];
            string scalingType = row.Cells[e.ColumnIndex].Value.ToString();

            Scaling scaling = (Scaling)row.Tag;

            if (scalingType == StringResource.LinearScaling)
                scaling.ScalingType = 0;
            else if(scalingType == StringResource.TwoPointScaling)
                scaling.ScalingType = 1;
            else if (scalingType == StringResource.TableScaling)
                scaling.ScalingType = 2;
        }

        private void ScalingPSDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, "Mp.Calculation.ScalingDlg");
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1400);
        }
    }
}
