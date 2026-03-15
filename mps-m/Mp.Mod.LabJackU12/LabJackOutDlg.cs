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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.LabJackU12
{
    public partial class LabJackOutDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlPs;
        private XmlElement _xmlSignalList;
        private ImageList _imgList = new ImageList();

        public LabJackOutDlg(Document doc, XmlElement xmlPs, XmlElement xmlSignalList)
        {
            _doc = doc;
            _xmlPs = xmlPs;
            _xmlSignalList = xmlSignalList;
            InitializeComponent();
            _imgList.Images.Add(Resource.Signal);
            signals.SmallImageList = _imgList;
            signals.LargeImageList = _imgList;

            FormStateHandler.Restore(this,Document.RegistryKey + "LabJackOutDlg");
        }

        public string PsName
        {
            get { return psName.Text; }
            set { psName.Text = value; }
        }

        private void LabJackOutDlg_Load(object sender, EventArgs e)
        {
            Cursor cursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            LoadSignals();
            LoadChannels();
            LoadData();
            this.Cursor = cursor;
        }

        private void LoadSignals()
        {
            if (_xmlSignalList == null)
                return;

            XmlElement xmlSignal;

            foreach (XmlElement xmlElement in _xmlSignalList.ChildNodes)
            {
                if (XmlHelper.GetObjectID(xmlElement) == 0)
                    xmlSignal = _doc.GetXmlObjectById(Convert.ToUInt32(xmlElement.InnerText));
                else
                    xmlSignal = xmlElement;

                string[] data = new string[4];
                data[0] = XmlHelper.GetParam(xmlSignal, "name");
                data[1] = XmlHelper.GetParam(xmlSignal, "samplerate") + " (Hz)";
                data[2] = XmlHelper.GetParamDouble(xmlSignal, "physMin").ToString();
                data[3] = XmlHelper.GetParamDouble(xmlSignal, "physMax").ToString();

                ListViewItem item = new ListViewItem(data, 0);
                item.Tag = xmlSignal;
                signals.Items.Add(item);
            }
        }

        private void InsertAnalogChannels(int serial)
        {
            for (int i = 0; i < 2; ++i)
            {
                int index = analogChns.Rows.Add();
                DataGridViewRow row = analogChns.Rows[index];
                row.Cells[1].Value = "AO" + i;
                row.Cells[1].Tag = i;
                row.Cells[2].Value = serial;
                row.Cells[3].Value = "10 Hz";
                row.Cells[4].Value = 0.0;
                row.Cells[5].Value = 5.0;
            }
        }

        private void InsertDigitalChannels(int serial)
        {
            for (int i = 0; i < 20; ++i)
            {
                int index = digitalChns.Rows.Add();
                DataGridViewRow row = digitalChns.Rows[index];
                if( i < 4)
                    row.Cells[1].Value = "IO" + i;
                else
                    row.Cells[1].Value = "D" + (i - 4);

                row.Cells[1].Tag = i;

                row.Cells[2].Value = serial;
                row.Cells[3].Value = "10 Hz";
            }
        }

        private void LoadChannels()
        {
            InsertAnalogChannels(-1);
            InsertDigitalChannels(-1);

            int[] productIDList = new int[127];
            int[] serialnumList = new int[127];
            int[] localIDList = new int[127];
            int[] powerList = new int[127];
            int[] calMatrix = new int[127 * 20];

            int numberFound = 0;
            int reserved1 = 0;
            int reserved2 = 0;
            try
            {
                int ret = DriverWrapper.ListAll(productIDList, serialnumList, localIDList, powerList, calMatrix, ref numberFound, ref reserved1, ref reserved2);

                for (int i = 0; i < numberFound; ++i)
                {
                    InsertAnalogChannels(serialnumList[i]);
                    InsertDigitalChannels(serialnumList[i]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private DataGridViewRow GetRowByData(int board, int channel, int type)
        {
            DataGridView view = analogChns;
            if (type == 1)
                view = digitalChns;

            foreach (DataGridViewRow row in view.Rows)
            {
                int curBoard = Convert.ToInt32(row.Cells[2].Value);
                int curChn = Convert.ToInt32(row.Cells[1].Tag);
                if( curChn == channel && curBoard == board)
                    return row;
            }

            return null;
        }

        private void LoadData()
        {
            XmlElement xmlOutSignals = XmlHelper.GetChildByType(_xmlPs, "Mp.LabJackU12.Output");

            if (xmlOutSignals == null)
                return;

            foreach (XmlElement xmlULOutSignal in xmlOutSignals.ChildNodes)
            {
                int boardNo = (int)XmlHelper.GetParamNumber(xmlULOutSignal, "board");
                int channel = (int)XmlHelper.GetParamNumber(xmlULOutSignal, "channel");
                int channelType = (int)XmlHelper.GetParamNumber(xmlULOutSignal, "channelType");

                DataGridViewRow row = GetRowByData(boardNo, channel, channelType);

                if (row == null)
                    continue;

                uint rate = (uint)XmlHelper.GetParamNumber(xmlULOutSignal, "rate");
                string strrate = GetRate(rate);
                row.Cells[3].Value = strrate;

                if (channelType == 0)
                {
                    double min = XmlHelper.GetParamDouble(xmlULOutSignal, "min");
                    double max = XmlHelper.GetParamDouble(xmlULOutSignal, "max");

                    row.Cells[4].Value = min;
                    row.Cells[5].Value = max;
                }

                UpdateSampleRate(boardNo, strrate);

                uint sigId = (uint)XmlHelper.GetParamNumber(xmlULOutSignal, "signalID");
                XmlElement xmlSignal = _doc.GetXmlObjectById(sigId);
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Tag = xmlSignal;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            //Remove the old boards
            XmlElement xmlOutSignals = XmlHelper.GetChildByType(_xmlPs, "Mp.LabJackU12.Output");

            if (xmlOutSignals != null)
                _doc.RemoveXmlObject(xmlOutSignals);

            //Create the new devices.
            xmlOutSignals = _doc.CreateXmlObject(_xmlPs, "Mp.LabJackU12.Output", "");
            
            //Save analog signals.
            foreach (DataGridViewRow row in analogChns.Rows)
            {
                if (row.Tag == null)
                    continue;

                XmlElement xmlSignal = (XmlElement)row.Tag;

                double min = Convert.ToDouble(row.Cells[4].Value);
                double max = Convert.ToDouble(row.Cells[5].Value);

                XmlElement xmlULOutSignal = _doc.CreateXmlObject(xmlOutSignals, "Mp.LabJackU12.OutChn", "");

                int channel = Convert.ToInt32(row.Cells[1].Tag);
                int board = Convert.ToInt32(row.Cells[2].Value);
                XmlHelper.SetParamNumber(xmlULOutSignal, "board", "int32_t", board);
                XmlHelper.SetParamNumber(xmlULOutSignal, "channel", "uint8_t", channel);
                uint rate = GetRate((string)row.Cells[3].Value);
                XmlHelper.SetParamNumber(xmlULOutSignal, "rate", "uint32_t", rate);
                XmlHelper.SetParamNumber(xmlULOutSignal, "channelType", "uint8_t", 0);
                XmlHelper.SetParamDouble(xmlULOutSignal, "min", "double", min);
                XmlHelper.SetParamDouble(xmlULOutSignal, "max", "double", max);
                uint id = XmlHelper.GetObjectID(xmlSignal);
                XmlHelper.SetParamNumber(xmlULOutSignal, "signalID", "uint32_t", id);
            }

            //Save digital signals.
            foreach (DataGridViewRow row in digitalChns.Rows)
            {
                if (row.Tag == null)
                    continue;

                XmlElement xmlSignal = (XmlElement)row.Tag;
                XmlElement xmlULOutSignal = _doc.CreateXmlObject(xmlOutSignals, "Mp.LabJackU12.OutChn", "");

                int board = Convert.ToInt32(row.Cells[2].Value);
                uint rate = GetRate((string)row.Cells[3].Value);
                XmlHelper.SetParamNumber(xmlULOutSignal, "rate", "uint32_t", rate);
                XmlHelper.SetParamNumber(xmlULOutSignal, "board", "int32_t", board);

                int channel = Convert.ToInt32(row.Cells[1].Tag);
                XmlHelper.SetParamNumber(xmlULOutSignal, "channel", "uint8_t", channel);
                XmlHelper.SetParamNumber(xmlULOutSignal, "channelType", "uint8_t", 1);
                uint id = XmlHelper.GetObjectID(xmlSignal);
                XmlHelper.SetParamNumber(xmlULOutSignal, "signalID", "uint32_t", id);
            }

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private uint GetRate(string rate)
        {
            return Convert.ToUInt32(rate.Replace(" Hz", ""));
        }

        private string GetRate(uint rate)
        {
            return rate.ToString() + " Hz";
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void LabJackOutDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, Document.RegistryKey + "LabJackOutDlg");
        }

        private void signals_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop((ListViewItem)e.Item, DragDropEffects.Move);
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

        private void analogChns_DragDrop(object sender, DragEventArgs e)
        {
            ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");
            Point p = analogChns.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo info = analogChns.HitTest(p.X, p.Y);
            if (info.RowIndex != -1)
            {
                DataGridViewRow row = analogChns.Rows[info.RowIndex];
                XmlElement xmlSignal = (XmlElement)item.Tag;
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[4].Value = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                row.Cells[5].Value = XmlHelper.GetParamDouble(xmlSignal, "physMax");
                row.Tag = xmlSignal;
            }
        }

        private void removeAnalogSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSignalFromChn(analogChns);
        }

        private void RemoveSignalFromChn(DataGridView view)
        {
            if (view.SelectedCells.Count == 0)
                return;

            DataGridViewRow row = view.Rows[view.SelectedCells[0].RowIndex];
            row.Tag = null;
            row.Cells[0].Value = "";
        }

        private void digitalChns_DragDrop(object sender, DragEventArgs e)
        {
            ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");
            Point p = digitalChns.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo info = digitalChns.HitTest(p.X, p.Y);
            if (info.RowIndex != -1)
            {
                DataGridViewRow row = digitalChns.Rows[info.RowIndex];
                XmlElement xmlSignal = (XmlElement)item.Tag;
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Tag = xmlSignal;
            }
        }

        private void removeSignalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RemoveSignalFromChn(digitalChns);
        }

        private void UpdateSampleRate(int serial, string rate)
        {
            foreach (DataGridViewRow row in analogChns.Rows)
            {
                int curSer = Convert.ToInt32(row.Cells[2].Value);

                if (curSer == serial)
                    row.Cells[3].Value = rate;
            }

            foreach (DataGridViewRow row in digitalChns.Rows)
            {
                int curSer = Convert.ToInt32(row.Cells[2].Value);

                if (curSer == serial)
                    row.Cells[3].Value = rate;
            }
        }

        private void digitalChns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateViewData(e.RowIndex, e.ColumnIndex, digitalChns);
        }
        
        private void analogChns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateViewData(e.RowIndex, e.ColumnIndex, analogChns);
        }


        bool _noChangeValueEvent = false;
        private void UpdateViewData(int rowIndex, int colIndex, DataGridView view)
        {
            if (_noChangeValueEvent || rowIndex == -1 || colIndex != 3)
                return;

            DataGridViewRow row = view.Rows[rowIndex];
            string rate = (string)row.Cells[3].Value;
            int serial = Convert.ToInt32(row.Cells[2].Value);

            _noChangeValueEvent = true;
            UpdateSampleRate(serial, rate);
            _noChangeValueEvent = false;
        }

        private void analogChns_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            if (e.ColumnIndex == 4 || e.ColumnIndex == 5)
            {
                try
                {
                    Convert.ToDouble(e.FormattedValue);
                }
                catch (Exception ex)
                {
                    errorProvider.SetError(analogChns, ex.Message);
                    e.Cancel = true;
                }
            }
        }

        private void analogChns_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            errorProvider.Clear();
            
            if (e.RowIndex == -1)
                return;

            DataGridViewRow row = analogChns.Rows[e.RowIndex];

            double min = Convert.ToDouble(row.Cells[4].Value);
            double max = Convert.ToDouble(row.Cells[5].Value);

            if( min >= max)
            {
                errorProvider.SetError(analogChns, StringResource.MinMaxError);
                e.Cancel = true;
            }
        }
    }
}
