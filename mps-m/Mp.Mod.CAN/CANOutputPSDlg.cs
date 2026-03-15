using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;
using Mp.Drv.CAN;

namespace Mp.Mod.CAN
{
    public partial class CANOutputPSDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlSignalList;
        private XmlElement _xmlPS;
        private SignalInputView _signals;

        private class ChannelItem
        {
            public ChannelItem()
            {
            }
            public XmlElement XmlSignal;
            public string MsgName;
            public uint ID;
            public int  DLC;
            public string ChannelName;
            public double Rate;
            public Drv.CANdb.Signal.ByteType ByteOrder;
            public Drv.CANdb.Signal.SignalType SignalType;
            public int ModeValue;
            public int PivotBit;
            public int BitCount;
            public Drv.CANdb.Signal.ValueType DataType;
            public double Factor;
            public double Offset;
            public double Min;
            public double Max;
            public ModeSignal ModeSignal;
        }
        
        public CANOutputPSDlg(Document doc, XmlElement xmlSignalList, XmlElement xmlPS )
        {
            _signals = new SignalInputView(doc, xmlSignalList);
            _signals.Dock = DockStyle.Fill;

            _doc = doc;
            _xmlSignalList = xmlSignalList;
            _xmlPS = xmlPS;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            driver.Items.Clear();

            splitContainer.Panel1.Controls.Add(_signals);

            driver.Items.Clear();

            CANDriverHelper.SetupDriver(driver, _doc.RuntimeEngine);
            name.Text = XmlHelper.GetParam(xmlPS, "name");
            string driverType = XmlHelper.GetParam(_xmlPS, "driver");

            CANDriverHelper.SetupDevices(device, driverType, _doc.RuntimeEngine);
            CANDriverHelper.SelectDriver(driver, driverType);

            string devId = XmlHelper.GetParam(_xmlPS, "device");
            CANDriverHelper.SelectDevice(device, devId);

            int devNo = (int) XmlHelper.GetParamNumber(_xmlPS, "deviceNo");
            deviceNo.SelectedIndex = devNo;

            port.SelectedIndex = (int) XmlHelper.GetParamNumber(_xmlPS,"port");
            string ratestr = XmlHelper.GetParam(_xmlPS, "bitrate");
            
            if (ratestr != "")
                bitrate.Text = ratestr;
            else
                bitrate.SelectedIndex = 0;

            adrMode.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlPS,"adrMode");

            LoadData();
            FormStateHandler.Restore(this, Document.RegistryKey + "CANOutputPSDlg");
        }

        private void LoadData()
        {
            XmlElement xmlChannels = XmlHelper.GetChildByType(_xmlPS, "Mp.CAN.OutChannels");

            if (xmlChannels == null)
                return;

            foreach (XmlElement xmlChannel in xmlChannels.ChildNodes)
            {
                int index = channels.Rows.Add();
                DataGridViewRow row = channels.Rows[index];
                ChannelItem item = new ChannelItem();

                item.BitCount = (int) XmlHelper.GetParamNumber(xmlChannel, "bitCount");
                item.ByteOrder = (Mp.Drv.CANdb.Signal.ByteType)XmlHelper.GetParamNumber(xmlChannel, "byteOrder");
                item.ChannelName = XmlHelper.GetParam(xmlChannel, "chnName");
                item.DataType = (Mp.Drv.CANdb.Signal.ValueType)XmlHelper.GetParamNumber(xmlChannel, "dataType");
                item.DLC = (int) XmlHelper.GetParamNumber(xmlChannel, "dlc");
                item.Factor = XmlHelper.GetParamDouble(xmlChannel, "factor");
                item.ID = (uint) XmlHelper.GetParamNumber(xmlChannel, "id");
                item.Max = XmlHelper.GetParamDouble(xmlChannel, "max");
                item.Min = XmlHelper.GetParamDouble(xmlChannel, "min");
                item.ModeValue = (int) XmlHelper.GetParamNumber(xmlChannel, "modeValue");
                item.MsgName = XmlHelper.GetParam(xmlChannel, "msgName");
                item.Offset = XmlHelper.GetParamDouble(xmlChannel, "offset");
                item.PivotBit = (int) XmlHelper.GetParamNumber(xmlChannel, "pivotBit");
                item.Rate = XmlHelper.GetParamDouble(xmlChannel, "rate");
                item.SignalType = (Mp.Drv.CANdb.Signal.SignalType)XmlHelper.GetParamNumber(xmlChannel, "sigType");

                uint id = (uint) XmlHelper.GetParamNumber(xmlChannel, "signal");
                if (id != 0)
                {
                    item.XmlSignal = _doc.GetXmlObjectById(id);
                    row.Cells[0].Value = XmlHelper.GetParam(item.XmlSignal, "name");
                }

                if (item.SignalType == Mp.Drv.CANdb.Signal.SignalType.ModeDepended)
                {
                    ModeSignal msignal = new ModeSignal();
                    msignal.BitCount = (int) XmlHelper.GetParamNumber(xmlChannel, "modeBitCount");
                    msignal.ByteOrder = (Mp.Drv.CANdb.Signal.ByteType)XmlHelper.GetParamNumber(xmlChannel, "modeByteOrder");
                    msignal.DataType = (Mp.Drv.CANdb.Signal.ValueType)XmlHelper.GetParamNumber(xmlChannel, "modeDataType");
                    msignal.Factor = XmlHelper.GetParamDouble(xmlChannel, "modeFactor");
                    msignal.Offset = XmlHelper.GetParamDouble(xmlChannel, "modeOffset");
                    msignal.PivotBit = (int) XmlHelper.GetParamNumber(xmlChannel, "modePivotBit");
                    item.ModeSignal = msignal;
                }


                row.Cells[1].Value = item.ChannelName;
                row.Cells[2].Value = item.Rate.ToString() + " (Hz)";
                row.Cells[3].Value = item.MsgName;
                row.Cells[4].Value = item.ID;
                row.Tag = item;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {

            DriverItem drvItem = (DriverItem)driver.Items[driver.SelectedIndex];

            XmlHelper.SetParam(_xmlPS, "driver", "string", drvItem.Lib);
            
            DeviceItem devItem = (DeviceItem)device.Items[device.SelectedIndex];
            XmlHelper.SetParam(_xmlPS, "device", "string", devItem.ID);

            XmlHelper.SetParamNumber(_xmlPS, "deviceNo", "uint8_t", deviceNo.SelectedIndex);

            XmlHelper.SetParamNumber(_xmlPS, "port", "uint8_t", port.SelectedIndex);

            XmlHelper.SetParam(_xmlPS, "bitrate", "uint32_t", bitrate.Text);

            XmlHelper.SetParamNumber(_xmlPS, "adrMode", "uint8_t", adrMode.SelectedIndex);

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);

            //Remove the old channels
            XmlElement xmlChannels = XmlHelper.GetChildByType(_xmlPS, "Mp.CAN.OutChannels");

            if (xmlChannels != null)
                _doc.RemoveXmlObject(xmlChannels);

            //Create the new channels.

            xmlChannels = _doc.CreateXmlObject(_xmlPS, "Mp.CAN.OutChannels", "");

            foreach (DataGridViewRow row in channels.Rows)
            {
                XmlElement xmlChannel = _doc.CreateXmlObject(xmlChannels, "Mp.CAN.OutChannel", "");
                ChannelItem item = (ChannelItem)row.Tag;
                
                XmlHelper.SetParamNumber(xmlChannel, "bitCount", "uint8_t", item.BitCount);
                XmlHelper.SetParamNumber(xmlChannel, "byteOrder", "uint8_t", (int)item.ByteOrder);
                XmlHelper.SetParam(xmlChannel, "chnName", "string", item.ChannelName);
                XmlHelper.SetParamNumber(xmlChannel, "dataType", "uint8_t", (int) item.DataType);
                XmlHelper.SetParamNumber(xmlChannel, "dlc", "uint8_t", item.DLC);
                XmlHelper.SetParamDouble(xmlChannel, "factor", "double", item.Factor);
                XmlHelper.SetParamNumber(xmlChannel, "id", "uint32_t", item.ID);
                XmlHelper.SetParamDouble(xmlChannel, "max", "double", item.Max);
                XmlHelper.SetParamDouble(xmlChannel, "min", "double", item.Min);
                XmlHelper.SetParamNumber(xmlChannel, "modeValue", "int32_t", item.ModeValue);
                XmlHelper.SetParam(xmlChannel, "msgName", "string", item.MsgName);
                XmlHelper.SetParamDouble(xmlChannel, "offset", "double", item.Offset);
                XmlHelper.SetParamNumber(xmlChannel, "pivotBit", "uint8_t", item.PivotBit);
                XmlHelper.SetParamDouble(xmlChannel, "rate", "double", item.Rate);
                XmlHelper.SetParamNumber(xmlChannel, "sigType", "uint8_t", (int)item.SignalType);
                
                if(item.XmlSignal != null)
                {
                    uint id = XmlHelper.GetObjectID(item.XmlSignal);
                    XmlHelper.SetParamNumber(xmlChannel, "signal", "uint32_t", id);
                }

                if (item.SignalType == Mp.Drv.CANdb.Signal.SignalType.ModeDepended)
                {
                    ModeSignal msignal = item.ModeSignal;
                    XmlHelper.SetParamNumber(xmlChannel, "modeBitCount", "uint8_t", msignal.BitCount);
                    XmlHelper.SetParamNumber(xmlChannel, "modeByteOrder", "uint8_t", (int)msignal.ByteOrder);
                    XmlHelper.SetParamNumber(xmlChannel, "modeDataType", "uint8_t", (int)msignal.DataType);
                    XmlHelper.SetParamDouble(xmlChannel, "modeFactor", "double", msignal.Factor);
                    XmlHelper.SetParamDouble(xmlChannel, "modeOffset", "double", msignal.Offset);
                    XmlHelper.SetParamNumber(xmlChannel, "modePivotBit", "uint8_t", msignal.PivotBit);
                }
            }

            _doc.Modified = true;
            this.DialogResult = DialogResult.OK;
            Close();

        }

        private void edit_Click(object sender, EventArgs e)
        {
            EditCANOutputDlg dlg = new EditCANOutputDlg();

            //Copy data in
            foreach (DataGridViewRow row in channels.Rows)
            {
                int index = dlg.Channels.Rows.Add();
                DataGridViewRow nrow = dlg.Channels.Rows[index];
                ChannelItem item = (ChannelItem)row.Tag;
                CopyDataToRow(nrow, item);
            }


            //Show 
            if (dlg.ShowDialog() != DialogResult.OK)
                return;


            //Copy data out
            channels.Rows.Clear();

            foreach (DataGridViewRow row in dlg.Channels.Rows)
            {
                ChannelItem item = GetChannelItem(row);

                int index = channels.Rows.Add();
                DataGridViewRow nrow = channels.Rows[index];

                nrow.Tag = item;
                
                if (item.XmlSignal != null)
                    nrow.Cells[0].Value = XmlHelper.GetParam(item.XmlSignal, "name");

                nrow.Cells[1].Value = item.ChannelName;
                nrow.Cells[2].Value = item.Rate.ToString() + " (Hz)";
                nrow.Cells[3].Value = item.MsgName;
                nrow.Cells[4].Value = item.ID;
            }
        }


        private Drv.CANdb.Signal.SignalType GetSignalType(string t)
        {
            switch(t)
            {
                case "Standard":
                    return Mp.Drv.CANdb.Signal.SignalType.Standard;
                case "Mode":
                    return Mp.Drv.CANdb.Signal.SignalType.ModeSignal;
                case "Mode depended":
                    return Mp.Drv.CANdb.Signal.SignalType.ModeDepended;
            }

            return Mp.Drv.CANdb.Signal.SignalType.Standard;
         }

        private string GetSignalType(Mp.Drv.CANdb.Signal.SignalType t)
        {
            switch (t)
            {
                case Mp.Drv.CANdb.Signal.SignalType.Standard:
                    return "Standard";

                case Mp.Drv.CANdb.Signal.SignalType.ModeSignal:
                    return "Mode";

                case Mp.Drv.CANdb.Signal.SignalType.ModeDepended:
                    return "Mode depended";
            }

            return "Standard";
        }

        private ChannelItem GetChannelItem(DataGridViewRow row)
        {
            ChannelItem item = new ChannelItem();

            item.MsgName =  (string) row.Cells[(int)SignalCols.Message].Value;
            item.BitCount = Convert.ToInt32(row.Cells[(int)SignalCols.BitCount].Value);
            item.DLC = Convert.ToInt32(row.Cells[(int)SignalCols.ByteCount].Value);
            item.ByteOrder = (Mp.Drv.CANdb.Signal.ByteType)EditCANOutputDlg.GetByteOrder((string)row.Cells[(int)SignalCols.ByteOrder].Value);
            item.Factor = Convert.ToDouble(row.Cells[(int)SignalCols.Factor].Value);
            item.ID = Convert.ToUInt32(row.Cells[(int)SignalCols.ID].Value);
            item.Max = Convert.ToDouble(row.Cells[(int)SignalCols.Max].Value);
            item.Min = Convert.ToDouble(row.Cells[(int)SignalCols.Min].Value);
            item.Rate = Convert.ToDouble(row.Cells[(int)SignalCols.Rate].Value);
            item.ModeValue = Convert.ToInt32(row.Cells[(int)SignalCols.ModeValue].Value);
            item.Offset = Convert.ToDouble(row.Cells[(int)SignalCols.Offset].Value);
            item.PivotBit = Convert.ToInt32(row.Cells[(int)SignalCols.PivotBit].Value);
            item.ChannelName = (string) row.Cells[(int)SignalCols.Signal].Value;
            item.SignalType = GetSignalType((string)row.Cells[(int)SignalCols.SignalType].Value);
            item.DataType = CANPortDlg.GetCANDataType( (string) row.Cells[(int)SignalCols.DataType].Value);

            if (item.SignalType == Mp.Drv.CANdb.Signal.SignalType.ModeDepended)
            {
                ModeSignal modeSignal = (ModeSignal) row.Cells[(int)SignalCols.SignalType].Tag;
                item.ModeSignal = modeSignal;
            }

            item.XmlSignal = (XmlElement) row.Tag;
            return item;
        }

        private void CopyDataToRow(DataGridViewRow row, ChannelItem item)
        {

            row.Cells[(int)SignalCols.Message].Value = item.MsgName;
            row.Cells[(int)SignalCols.BitCount].Value = item.BitCount;
            row.Cells[(int)SignalCols.ByteCount].Value = item.DLC;
            row.Cells[(int)SignalCols.ByteOrder].Value = EditCANOutputDlg.GetByteOrder((int)item.ByteOrder);
            row.Cells[(int)SignalCols.Factor].Value = item.Factor;
            row.Cells[(int)SignalCols.ID].Value = item.ID;
            row.Cells[(int)SignalCols.Max].Value = item.Max;
            row.Cells[(int)SignalCols.Min].Value = item.Min;
            row.Cells[(int)SignalCols.Rate].Value = item.Rate;
            row.Cells[(int)SignalCols.ModeValue].Value = item.ModeValue;
            row.Cells[(int)SignalCols.Offset].Value = item.Offset;
            row.Cells[(int)SignalCols.PivotBit].Value = item.PivotBit;
            row.Cells[(int)SignalCols.Signal].Value = item.ChannelName;
            row.Cells[(int)SignalCols.SignalType].Value = GetSignalType(item.SignalType);
            row.Cells[(int)SignalCols.DataType].Value = CANPortDlg.GetCANDataType(item.DataType);

            if (item.SignalType == Mp.Drv.CANdb.Signal.SignalType.ModeDepended)
                row.Cells[(int)SignalCols.SignalType].Tag = item.ModeSignal;

            row.Tag = item.XmlSignal;
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

            Point p = new Point(e.X, e.Y);
            p = channels.PointToClient(p);
            DataGridView.HitTestInfo info = channels.HitTest(p.X, p.Y);

            if (info.RowIndex == -1)
                return;

            DataGridViewRow row = channels.Rows[info.RowIndex];
            ChannelItem chnItem = (ChannelItem)row.Tag;
            chnItem.XmlSignal = xmlSignal;
            row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
        }

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (channels.SelectedCells.Count == 0)
                return;

            int index = channels.SelectedCells[0].RowIndex;

            DataGridViewRow row = channels.Rows[index];
            ChannelItem chnItem = (ChannelItem)row.Tag;
            chnItem.XmlSignal = null;
            row.Cells[0].Value = "";

        }

        private void CANOutputPSDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, Document.RegistryKey + "CANOutputPSDlg");
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 860);
        }

        private void CANOutputPSDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }

        private void driver_SelectedIndexChanged(object sender, EventArgs e)
        {
            DriverItem item = (DriverItem)driver.Items[driver.SelectedIndex];
            CANDriverHelper.SetupDevices(device, item.Lib, _doc.RuntimeEngine);
        }
    }
}
