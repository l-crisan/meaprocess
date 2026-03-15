using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Mp.Drv.CAN;
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.OBD2
{
    internal partial class PsPropDlg : Form
    {
        private XmlElement _xmlRep;
        private Document _doc;

      

        public PsPropDlg(XmlElement xmlRep, Document doc)
        {
            _doc = doc;
            _xmlRep = xmlRep;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            driver.Items.Clear();
            string devID = XmlHelper.GetParam(_xmlRep, "device");
            deviceNo.SelectedIndex = (int) XmlHelper.GetParamNumber(_xmlRep, "deviceNo");

            CANDriverHelper.SetupDriver(driver, _doc.RuntimeEngine);
            
            if(!_doc.RuntimeEngine.SupportMCM)
            {
                driver.Items.Add(new DriverItem("ELM327", "mps-can-elm327"));
                driver.Items.Add(new DriverItem("KLine", "mps-kline-rs232"));
            }

            name.Text = XmlHelper.GetParam(_xmlRep, "name");
            string driverID = XmlHelper.GetParam(_xmlRep, "driver");
            string deviceID = XmlHelper.GetParam(_xmlRep, "device");

            CANDriverHelper.SetupDevices(device, driverID, _doc.RuntimeEngine);
            CANDriverHelper.SelectDriver(driver, driverID);
            CANDriverHelper.SelectDevice(device, deviceID);


            int portIdx = (int)  XmlHelper.GetParamNumber(_xmlRep, "port");

            serialBaudRate.Text = XmlHelper.GetParam(_xmlRep, "serRate");
            if (serialBaudRate.Text == "")
                serialBaudRate.Text = "10400";

            if(driver.SelectedIndex == 15)
                addressMode.SelectedIndex = 0;
            else
                addressMode.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlRep, "addressMode");

         
            port.SelectedIndex = portIdx;
            rate.Text = XmlHelper.GetParamNumber(_xmlRep, "canRate").ToString();
        }

      
        private void OK_Click(object sender, EventArgs e)
        {
            XmlHelper.SetParam(_xmlRep, "name", "string", name.Text);
            DriverItem item = (DriverItem)driver.Items[driver.SelectedIndex];
            XmlHelper.SetParam(_xmlRep, "driver", "string", item.Lib);

            XmlHelper.SetParamNumber(_xmlRep, "deviceNo", "uint8_t", deviceNo.SelectedIndex);

      
            DeviceItem devItem = (DeviceItem)device.Items[device.SelectedIndex];
            XmlHelper.SetParam(_xmlRep, "device", "string", devItem.ID.ToString());

            XmlHelper.SetParamNumber(_xmlRep, "port", "uint32_t", port.SelectedIndex);
            
            if(driver.SelectedIndex == 15)
                XmlHelper.SetParamNumber(_xmlRep, "addressMode", "uint8_t", 2);
            else
                XmlHelper.SetParamNumber(_xmlRep, "addressMode", "uint8_t", addressMode.SelectedIndex);

            XmlHelper.SetParamNumber(_xmlRep, "canRate","uint32_t",Convert.ToUInt32(rate.Text));
            XmlHelper.SetParamNumber(_xmlRep, "serRate", "uint32_t", Convert.ToUInt32(serialBaudRate.Text));
            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public string PsName
        {
            get { return name.Text; }
        }

        private void driver_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            DriverItem item = (DriverItem)driver.Items[driver.SelectedIndex];

            port.Enabled = true;
            if (item.Lib == "mps-can-elm327" ||
                 item.Lib == "mps-kline-rs232")
            {//ELM             
                port.Items.Clear();
                port.Enabled = false;

                serialBaudRate.Enabled = true;
                addressMode.Enabled = true;
                rate.Enabled = true;
            }
            else
            {
                serialBaudRate.Enabled = false;
                addressMode.Enabled = true;
                rate.Enabled = true;
                int idx = port.SelectedIndex;
                port.Items.Clear();

                for (int i = 1; i < 3; ++i)
                    port.Items.Add("CAN" + i.ToString());

                if( idx < port.Items.Count && idx > -1)
                    port.SelectedIndex = idx;
                else
                    port.SelectedIndex = 0;
            }

            CANDriverHelper.SetupDevices(device, item.Lib, _doc.RuntimeEngine);
        }

        private void serialBaudRate_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(serialBaudRate.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(serialBaudRate, ex.Message);
                e.Cancel = true;
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1000);
        }

        private void PsPropDlg_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            help_Click(null, null);
        }

        private void PsPropDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
