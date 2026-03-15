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
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Event
{
    public partial class SignalControlDlg : Form
    {
        private SignalInputView _signals;
        private XmlElement _xmlPS;
        private Document _doc;
        private XmlElement _xmlInSignalList;
        private XmlElement _xmlOutSignalList;
        
        public class ControlInfo
        {
            public ControlInfo()
            {
                Lower = 5;
                Upper = 7;
                Alteration = 2;
            }

            public XmlElement InSignal;
            public XmlElement OutSignal;
            
            public int WindowType;
            public double Lower;
            public double Upper;

            public double SlopeValue;
            public int SlopeType;
            
            public double Alteration;
            public int SignalIf;
            public int Absolut;
            
        }

        public SignalControlDlg(XmlElement xmlPS, Document doc, XmlElement xmlInSignalList, XmlElement xmlOutSignalList)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            _xmlInSignalList = xmlInSignalList;
            _xmlOutSignalList = xmlOutSignalList;
            _signals = new SignalInputView(doc, xmlInSignalList);
            _signals.Dock = DockStyle.Fill;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn)outputGrid.Columns[2];
            col.Items.Clear();
            col.Items.Add(StringResource.Window);
            col.Items.Add(StringResource.Slope);
            col.Items.Add(StringResource.Alteration);

            _signals.TabIndex = 2;
            splitContainer.Panel1.Controls.Add(_signals);
            LoadData();

            FormStateHandler.Restore(this, "Mp.Event.SignalControlDlg");
        }


        private void LoadData()
        {
            name.Text = XmlHelper.GetParam(_xmlPS, "name");

            foreach( XmlElement xmlSignal in _xmlOutSignalList.ChildNodes)
            {
                ControlInfo ctrlInfo = new ControlInfo();
                int index = outputGrid.Rows.Add();
                DataGridViewRow row = outputGrid.Rows[index];
                ctrlInfo.OutSignal = xmlSignal;
                row.Tag = ctrlInfo;
                
                ctrlInfo.InSignal = _doc.GetXmlObjectById((uint) XmlHelper.GetParamNumber(ctrlInfo.OutSignal, "inSignal"));

                row.Cells[0].Value = XmlHelper.GetParam(ctrlInfo.InSignal, "name");
                row.Cells[1].Value = XmlHelper.GetParam(ctrlInfo.OutSignal, "name");
                row.Cells[3].Value = XmlHelper.GetParam(ctrlInfo.OutSignal, "outValOnTrue");
                row.Cells[4].Value = XmlHelper.GetParam(ctrlInfo.OutSignal, "outValOnFalse");
                row.Cells[5].Value = "...";
                row.Cells[6].Value = XmlHelper.GetParam(ctrlInfo.OutSignal, "unit");
                row.Cells[7].Value = XmlHelper.GetParam(ctrlInfo.OutSignal, "comment");
                int ctrlType = (int) XmlHelper.GetParamNumber(ctrlInfo.OutSignal, "ctrlType");

                switch( ctrlType)
                {
                    case 0:
                        row.Cells[2].Value = StringResource.Window;
                    break;
                    case 1:
                        row.Cells[2].Value = StringResource.Slope;
                    break;
                    case 2:
                        row.Cells[2].Value = StringResource.Alteration;
                    break;
                }
                

                //Window
                ctrlInfo.WindowType = (int) XmlHelper.GetParamNumber(ctrlInfo.OutSignal, "windowType");
                ctrlInfo.Lower = XmlHelper.GetParamDouble(ctrlInfo.OutSignal, "lower");
                ctrlInfo.Upper = XmlHelper.GetParamDouble(ctrlInfo.OutSignal, "upper");

                //Slope
                ctrlInfo.SlopeType = (int) XmlHelper.GetParamNumber(ctrlInfo.OutSignal, "slopeType");
                ctrlInfo.SlopeValue = XmlHelper.GetParamDouble(ctrlInfo.OutSignal, "slopeValue");

                //Alteration
                ctrlInfo.Alteration=  XmlHelper.GetParamDouble(ctrlInfo.OutSignal, "alteration");
                ctrlInfo.Absolut = (int) XmlHelper.GetParamNumber(ctrlInfo.OutSignal, "absolut");
                ctrlInfo.SignalIf = (int) XmlHelper.GetParamNumber(ctrlInfo.OutSignal, "signalIf");
            }
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

            System.Windows.Forms.ListViewItem item = (System.Windows.Forms.ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

            XmlElement xmlSignal = item.Tag as XmlElement;

            if (xmlSignal == null)
                return;

            int index = outputGrid.Rows.Add();
            ControlInfo ctrlInfo = new ControlInfo();

            DataGridViewRow row = outputGrid.Rows[index];
            row.Tag = ctrlInfo;
            row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            row.Cells[1].Value = "";
            row.Cells[2].Value = StringResource.Window;
            row.Cells[3].Value = 1;
            row.Cells[4].Value = 0;
            row.Cells[5].Value = "...";
            row.Cells[6].Value = "";
            row.Cells[7].Value = "";
            ctrlInfo.InSignal = xmlSignal;
        }
     
        private void OK_Click(object sender, EventArgs e)
        {

            errorProvider.Clear();
            
            foreach (DataGridViewRow row in outputGrid.Rows)
            {
                string sigName = row.Cells[1].Value.ToString();
                if (sigName == null || sigName == "")
                {
                    errorProvider.SetError(outputGrid, StringResource.NotAllSigNameErr);
                    return;
                }
            }

            _doc.UpdateSource(_xmlPS);

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);

            foreach (DataGridViewRow row in outputGrid.Rows)
            {
                ControlInfo ctrlInfo = (ControlInfo) row.Tag;

                if(ctrlInfo.OutSignal == null)
                    ctrlInfo.OutSignal = _doc.CreateXmlObject(_xmlOutSignalList, "Mp.Sig", "Mp.Event.Sig.Ctrl");

                XmlHelper.SetParam(ctrlInfo.OutSignal, "name", "string", row.Cells[1].Value.ToString());
                XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "outValOnTrue", "int32_t", Convert.ToInt32(row.Cells[3].Value));
                XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "outValOnFalse", "int32_t", Convert.ToInt32(row.Cells[4].Value));
                
                XmlHelper.SetParam(ctrlInfo.OutSignal, "unit", "string", row.Cells[6].Value.ToString());
                XmlHelper.SetParam(ctrlInfo.OutSignal, "comment", "string", row.Cells[7].Value.ToString());

                XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "valueDataType", "uint8_t", (int)SignalDataType.DINT);
                XmlHelper.SetParamDouble(ctrlInfo.OutSignal, "physMin", "double", -255);
                XmlHelper.SetParamDouble(ctrlInfo.OutSignal, "physMax", "double", 255);

                uint srcID = (uint)XmlHelper.GetParamNumber(_xmlPS, "sourceId");
                XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "sourceNumber", "uint32_t", (long)srcID);
                string type = row.Cells[2].Value.ToString();

                
                if( type == StringResource.Window)
                    XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "ctrlType", "uint8_t", 0);
                    
                if( type == StringResource.Slope)
                    XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "ctrlType", "uint8_t", 1);

                if( type == StringResource.Alteration)
                    XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "ctrlType", "uint8_t", 2);

                XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "inSignal", "uint32_t", XmlHelper.GetObjectID(ctrlInfo.InSignal));

                //Window
                XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "windowType", "uint8_t", ctrlInfo.WindowType);
                XmlHelper.SetParamDouble(ctrlInfo.OutSignal, "lower", "double", ctrlInfo.Lower);
                XmlHelper.SetParamDouble(ctrlInfo.OutSignal, "upper", "double", ctrlInfo.Upper);

                //Slope
                XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "slopeType", "uint8_t", ctrlInfo.SlopeType);
                XmlHelper.SetParamDouble(ctrlInfo.OutSignal, "slopeValue", "double", ctrlInfo.SlopeValue);
                
                //Alteration
                XmlHelper.SetParamDouble(ctrlInfo.OutSignal, "alteration", "double", ctrlInfo.Alteration);
                XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "absolut", "uint8_t", ctrlInfo.Absolut);
                XmlHelper.SetParamNumber(ctrlInfo.OutSignal, "signalIf", "uint8_t", ctrlInfo.SignalIf);
            }

            RemoveUnusedSignals();
            _doc.Modified = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void RemoveUnusedSignals()
        {
            for (int i = 0; i < _xmlOutSignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement)_xmlOutSignalList.ChildNodes[i];
                uint id = XmlHelper.GetObjectID(xmlSignal);
                bool found = false;
                foreach (DataGridViewRow row in outputGrid.Rows)
                {
                    ControlInfo cls = (ControlInfo)row.Tag;
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

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void outputGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex != 5)
                return;

            DataGridViewRow row = outputGrid.Rows[e.RowIndex];

            string type = row.Cells[2].Value.ToString();

            if (type == StringResource.Window)
            {
                WindowControlDlg dlg = new WindowControlDlg((ControlInfo)row.Tag);
                dlg.ShowDialog();
            }

            if (type == StringResource.Slope)
            {
                SlopeDlg dlg = new SlopeDlg((ControlInfo)row.Tag);
                dlg.ShowDialog();
            }

            if (type == StringResource.Alteration)
            {
                AlterationDlg dlg = new AlterationDlg((ControlInfo)row.Tag);
                dlg.ShowDialog();
            }
        }

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (outputGrid.SelectedCells.Count == 0)
                return;

            int index = outputGrid.SelectedCells[0].RowIndex;
            DataGridViewRow row = outputGrid.Rows[index];
            outputGrid.Rows.RemoveAt(index);
        }

        private void SignalControlDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, "Mp.Event.SignalControlDlg");
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1350);
        }

        private void OnOutputGridCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();
            
            if(e.RowIndex  < 0 )
                return;

            if(e.ColumnIndex != 3 && e.ColumnIndex != 4)
                return;

            try
            {
                Convert.ToInt32(outputGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            }
            catch(Exception ex)
            {
                errorProvider.SetError(outputGrid, ex.Message);
            }
        }
    }
}
