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
    public partial class CANViewDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlSinalList;
        private XmlElement _xmlPS;
        private SignalInputView _signals;

        public CANViewDlg(Document doc, XmlElement xmlPS, XmlElement xmlSignalList)
        {
            _doc = doc;
            _xmlSinalList = xmlSignalList;
            _xmlPS = xmlPS;
            InitializeComponent();

            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            _signals = new SignalInputView(doc, _xmlSinalList);
            _signals.Dock = DockStyle.Fill;
            _signals.TabIndex = 9;
            splitContainer1.Panel1.Controls.Add(_signals);
            
            
            psName.Text = XmlHelper.GetParam(_xmlPS, "name");

            InitChannels();
            LoadCANSettings();
            LoadMapping();
        }

        private void InitChannels()
        {
            int index = channels.Rows.Add();   
            DataGridViewRow row  = channels.Rows[index];
            row.Cells[0].Value = "";
            row.Cells[1].Value = StringResource.TypeTimeStamp;

            index = channels.Rows.Add();
            row = channels.Rows[index];
            row.Cells[0].Value = "";
            row.Cells[1].Value = "Identifier";

            index = channels.Rows.Add();
            row = channels.Rows[index];
            row.Cells[0].Value = "";
            row.Cells[1].Value = "DLC";

            index = channels.Rows.Add();
            row = channels.Rows[index];
            row.Cells[0].Value = "";
            row.Cells[1].Value = StringResource.Message;
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
            ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");
            Point p = channels.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo info = channels.HitTest(p.X, p.Y);

            if (info.RowIndex != -1)
            {
                DataGridViewRow row = channels.Rows[info.RowIndex];
                XmlElement xmlSignal = (XmlElement)item.Tag;
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Tag = xmlSignal;
            }

            errorProvider.Clear();
        }


        private void LoadCANSettings()
        {
            driver.Items.Clear();

            CANDriverHelper.SetupDriver(driver, _doc.RuntimeEngine);
            string driverType = XmlHelper.GetParam(_xmlPS, "driver");

            CANDriverHelper.SetupDevices(device, driverType, _doc.RuntimeEngine);
            CANDriverHelper.SelectDriver(driver, driverType);

            string devId = XmlHelper.GetParam(_xmlPS, "device");
            CANDriverHelper.SelectDevice(device, devId);

            int devNo = (int)XmlHelper.GetParamNumber(_xmlPS, "deviceNo");
            deviceNo.SelectedIndex = devNo;

            port.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlPS, "port");
            string ratestr = XmlHelper.GetParam(_xmlPS, "bitrate");

            if (ratestr != "")
                bitrate.Text = ratestr;
            else
                bitrate.SelectedIndex = 0;

            adrMode.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlPS, "adrMode");
        }


        private void LoadMapping()
        {
            string mapping = XmlHelper.GetParam(_xmlPS, "signalTypeMap");

            string[] arrayMapping = mapping.Split('#');

            foreach (string map in arrayMapping)
            {
                if( map == "")
                    continue;

                string[] array = map.Split(new char[] { '/' });
                DataGridViewRow row = channels.Rows[Convert.ToInt32(array[1])];
                
                if (row == null)
                    continue;

                uint signalID = Convert.ToUInt32(Convert.ToInt32(array[0]));
                
                foreach( XmlElement xmlSignal in _xmlSinalList.ChildNodes)
                {
                    uint current = XmlHelper.GetObjectID(xmlSignal);

                    if( signalID == current)
                    {
                        row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                        row.Tag = xmlSignal;
                        break;
                    }
                }
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            
            string sigTypeMap = "";
            
            foreach (DataGridViewRow row in channels.Rows)
            {                                
                XmlElement xmlSignal = (XmlElement)row.Tag;

                if (xmlSignal == null && (row.Index == 0 || row.Index == 1))
                {
                    errorProvider.SetError(channels, StringResource.TimeAndIDNotMapped);
                    return;
                }

                if (xmlSignal == null)
                    continue;

                string mapping = XmlHelper.GetObjectID(xmlSignal).ToString() + "/" + row.Index.ToString();
                sigTypeMap += (mapping+ "#");

                if (row.Index == 4)
                {
                    SignalDataType sigType = (SignalDataType)(int) XmlHelper.GetParamNumber(xmlSignal, "valueDataType");
                    if( sigType != SignalDataType.ULINT)
                    {
                        errorProvider.SetError(channels,StringResource.UnsupDataType);
                        return;
                    }
                }
            }
            

            XmlHelper.SetParam(_xmlPS, "name", "string", psName.Text);
            DriverItem drvItem = (DriverItem)driver.Items[driver.SelectedIndex];

            XmlHelper.SetParam(_xmlPS, "driver", "string", drvItem.Lib);

            DeviceItem devItem = (DeviceItem)device.Items[device.SelectedIndex];
            XmlHelper.SetParam(_xmlPS, "device", "string", devItem.ID);

            XmlHelper.SetParamNumber(_xmlPS, "deviceNo", "uint8_t", deviceNo.SelectedIndex);

            XmlHelper.SetParamNumber(_xmlPS, "port", "uint8_t", port.SelectedIndex);

            XmlHelper.SetParam(_xmlPS, "bitrate", "uint32_t", bitrate.Text);

            XmlHelper.SetParamNumber(_xmlPS, "adrMode", "uint8_t", adrMode.SelectedIndex);

            sigTypeMap.TrimEnd('#');
            XmlHelper.SetParam(_xmlPS, "signalTypeMap", "string", sigTypeMap);

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnHelpClick(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 890);
        }

        private void OnHelpRequested(object sender, HelpEventArgs hlpevent)
        {
            OnHelpClick(null, null);
        }

        private void OnDriverSelectedIndexChanged(object sender, EventArgs e)
        {
            DriverItem item = (DriverItem)driver.Items[driver.SelectedIndex];
            CANDriverHelper.SetupDevices(device, item.Lib, _doc.RuntimeEngine);
        }

        private void OnRemoveSignal(object sender, EventArgs e)
        {
            if (channels.SelectedCells.Count == 0)
                return;

            DataGridViewRow row = channels.Rows[channels.SelectedCells[0].RowIndex];
            row.Tag = null;
            row.Cells[0].Value = "";
        }
    }
}
