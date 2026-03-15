using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using System.Media;
using Mp.Utils;
using Mp.Scheme.Sdk;
using Mp.Drv.CAN;

namespace Mp.Mod.CAN
{
    public partial class CANEventPSDlg : Form
    {
        private XmlElement _xmlSignalList;
        private XmlElement _xmlPS;
        private Document _doc;
        private ImageList _imgList = new ImageList();
        
        public CANEventPSDlg(XmlElement xmlSignalList, XmlElement xmlPS, Document doc)
        {
            _xmlSignalList = xmlSignalList;
            _xmlPS = xmlPS;
            _doc = doc;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            _imgList.Images.Add(Resource.Signal);
            signals.SmallImageList = _imgList;
            signals.LargeImageList = _imgList;
            driver.Items.Clear();
            
            string driverId = XmlHelper.GetParam(_xmlPS, "driver");
            CANDriverHelper.SetupDriver(driver, doc.RuntimeEngine);
            CANDriverHelper.SetupDevices(device, driverId, doc.RuntimeEngine);

            CANDriverHelper.SelectDriver(driver, driverId);

            string deviceId =  XmlHelper.GetParam(_xmlPS, "device");
            CANDriverHelper.SelectDevice(device, deviceId);

            deviceNo.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlPS, "deviecNo");

            name.Text = XmlHelper.GetParam(xmlPS, "name");
        
            port.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlPS, "port");
            string ratestr = XmlHelper.GetParam(_xmlPS, "bitrate");

            if (ratestr != "")
                bitrate.Text = ratestr;
            else
                bitrate.SelectedIndex = 0;

            adrMode.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlPS, "adrMode");

            FormStateHandler.Restore(this, Document.RegistryKey + "EventPSDlg");

            LoadSignals();
         
        }

        private void LoadSignals()
        {
            if(_xmlSignalList == null)
                return;

            foreach (XmlElement xmlElement in _xmlSignalList.ChildNodes)
            {
                XmlElement xmlSignal = xmlElement;

                if (XmlHelper.GetObjectID(xmlSignal) == 0)
                    xmlSignal = _doc.GetXmlObjectById(Convert.ToUInt32(xmlSignal.InnerText));

                string[] items = new string[1];
                items[0] = XmlHelper.GetParam(xmlSignal,"name");

                ListViewItem item = new ListViewItem(items,0);
                item.Tag = xmlSignal;
                signals.Items.Add(item);
            }
        }

        private string GetOperation(int op)
        {
            switch (op)
            {
                case 0:
                    return "<>";
                case 1:
                    return "=";
                case 2:
                    return "<";
                case 3:
                    return "<=";
                case 4:
                    return ">";
                case 5:
                    return ">=";
            }
            return "<>";
        }

        private int GetOperation(string op)
        {
            switch (op)
            {
                case "<>":
                    return 0;
                
                case "=":
                    return 1;
                
                case "<":
                    return 2;
                
                case "<=":
                    return 3;
                
                case ">":
                    return 4;

                 case ">=":
                    return 5;
            }

            return 0;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            DriverItem drvItem = (DriverItem)driver.Items[driver.SelectedIndex];
            XmlHelper.SetParam(_xmlPS, "driver", "string", drvItem.Lib);
       
            DeviceItem item = (DeviceItem)device.Items[device.SelectedIndex];
            XmlHelper.SetParam(_xmlPS, "device", "string", item.ID.ToString());

            XmlHelper.SetParamNumber(_xmlPS, "deviceNo", "uint8_t", deviceNo.SelectedIndex);

            XmlHelper.SetParamNumber(_xmlPS, "port", "uint8_t", port.SelectedIndex);

            XmlHelper.SetParam(_xmlPS, "bitrate", "uint32_t", bitrate.Text);

            XmlHelper.SetParamNumber(_xmlPS, "adrMode", "uint8_t", adrMode.SelectedIndex);

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);

            XmlElement xmlEvents = XmlHelper.GetChildByType(_xmlPS, "Mp.CAN.Events");

            if (xmlEvents != null)
                _doc.RemoveXmlObject(xmlEvents);

            //Create the new devices.

            xmlEvents = _doc.CreateXmlObject(_xmlPS, "Mp.CAN.Events", "");

            foreach (DataGridViewRow row in events.Rows)
            {
                XmlElement xmlEvent = _doc.CreateXmlObject(xmlEvents, "Mp.CAN.Event", "");
                XmlElement xmlSignal = (XmlElement) row.Tag;

                uint id = XmlHelper.GetObjectID(xmlSignal);
                XmlHelper.SetParamNumber(xmlEvent, "signal", "uint32_t", (int)id);
                XmlHelper.SetParamNumber(xmlEvent, "operation","uint8_t", GetOperation((string) row.Cells[1].Value));
                XmlHelper.SetParamDouble(xmlEvent, "limit", "double", Convert.ToDouble(row.Cells[2].Value));
                XmlHelper.SetParamNumber(xmlEvent, "id", "uint32_t", Convert.ToUInt32(row.Cells[3].Value));
                XmlHelper.SetParam(xmlEvent, "data", "string", GetMsgData((string)row.Cells[4].Value));
            }

            DialogResult = DialogResult.OK;
            _doc.Modified = true;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void signals_ItemDrag(object sender, ItemDragEventArgs e)
        {            
            DoDragDrop(signals.SelectedItems, DragDropEffects.Move);
        }

        private void events_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                e.Effect = DragDropEffects.Move;
        }

        private void events_DragDrop(object sender, DragEventArgs e)
        {
            ListView.SelectedListViewItemCollection selected = (ListView.SelectedListViewItemCollection) e.Data.GetData("System.Windows.Forms.ListView+SelectedListViewItemCollection");

            foreach (ListViewItem item in selected)
            {
                XmlElement xmlSignal = (XmlElement) item.Tag;

                int index = events.Rows.Add();
                DataGridViewRow row = events.Rows[index];
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[1].Value = "<>";
                row.Cells[2].Value = "0";
                row.Cells[3].Value = 1;
                row.Cells[4].Value = "FF FF FF FF FF FF FF FF";
                row.Tag = xmlSignal;
            }

        }

        private void events_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 3)
                return;
            
            if(e.RowIndex == -1)
                return;

            DataGridViewRow row = events.Rows[e.RowIndex];
        }

        private void EventPSDlg_Load(object sender, EventArgs e)
        {
            XmlElement xmlEvents = XmlHelper.GetChildByType(_xmlPS, "Mp.CAN.Events");

            if( xmlEvents == null)
                return;

            foreach (XmlElement xmlEvent in xmlEvents.ChildNodes)
            {
                int index = events.Rows.Add();
                DataGridViewRow row = events.Rows[index];
                uint sigId = (uint)XmlHelper.GetParamNumber(xmlEvent, "signal");
                row.Cells[1].Value = GetOperation((int)XmlHelper.GetParamNumber(xmlEvent, "operation"));
                row.Cells[2].Value = XmlHelper.GetParamDouble(xmlEvent, "limit");

                row.Cells[3].Value = (uint) XmlHelper.GetParamNumber(xmlEvent, "id");

                
                
                string strdata = XmlHelper.GetParam(xmlEvent, "data");
                string s = "";

                int i = 0;
                foreach (char ch in strdata)
                {
                    s += ch;
                    i++;
                    if ((i % 2) == 0)
                        s += " ";
                }

                row.Cells[4].Value = s;  
                                
                XmlElement  xmlSignal = _doc.GetXmlObjectById(sigId);

                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Tag = xmlSignal;


            }
        }

        private void removeEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if( events.SelectedCells.Count == 0)
                return;

            DataGridViewRow row  = events.Rows[events.SelectedCells[0].RowIndex];
            events.Rows.Remove(row);
        }

        private void events_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            switch (e.ColumnIndex)
            {
                case 2:
                {
                    try
                    {
                        Convert.ToDouble(e.FormattedValue);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(events, ex.Message);
                        e.Cancel = true;
                    }
                }
                break;
                case 3:
                {
                    try
                    {
                        uint id = Convert.ToUInt32(e.FormattedValue);

                        if (id == 0)
                            throw new Exception("The CAN ID must be greater as 0.");
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(events, ex.Message);
                        e.Cancel = true;
                    }
                }
                break;
                case 4:
                {
                    try
                    {
                        string data = GetMsgData((string) e.FormattedValue);
                        ulong.Parse(data, System.Globalization.NumberStyles.HexNumber);
                    }
                    catch(Exception ex)
                    {
                        errorProvider.SetError(events, ex.Message);
                        e.Cancel = true;
                    }                                        
                }
                break;
            }
            
        }

        private static string GetMsgData(string data)
        {
            return data.Replace(" ", "");
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1120);
        }

        private void EventPSDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
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
