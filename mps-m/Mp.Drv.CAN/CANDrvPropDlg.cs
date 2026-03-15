using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Drv.CAN
{
    public partial class CANDrvPropDlg : Form
    {
        private XmlElement _xmlPS;
        private Document _doc;


        public CANDrvPropDlg(XmlElement xmlPS, Document doc)
        {
            _xmlPS = xmlPS;
            _doc = doc;            
            InitializeComponent();

            CANDriverHelper.SetupDriver(driver, _doc.RuntimeEngine);

            //Load Data
            name.Text = XmlHelper.GetParam(_xmlPS, "name");

            //Load the driver type
            string driverType = XmlHelper.GetParam(_xmlPS, "driver");

            CANDriverHelper.SetupDevices(device, driverType, _doc.RuntimeEngine);
            CANDriverHelper.SelectDriver(driver, driverType);

            //Load the device
            string devID = XmlHelper.GetParam(_xmlPS, "device");
            CANDriverHelper.SelectDevice(device, devID);

            //Load the device no.
            deviceNo.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlPS, "deviceNo");
        }

        private void OK_Click(object sender, EventArgs e)
        {

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);

            DriverItem item = (DriverItem) driver.Items[driver.SelectedIndex];

            XmlHelper.SetParam(_xmlPS, "driver","string", item.Lib);

            DeviceItem devItem = (DeviceItem)device.Items[device.SelectedIndex];

            XmlHelper.SetParam(_xmlPS, "device", "string", devItem.ID);

            XmlHelper.SetParamNumber(_xmlPS, "deviceNo","uint8_t", deviceNo.SelectedIndex);

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            _doc.Modified = true;
            Close();
        }

        private void driver_SelectedIndexChanged(object sender, EventArgs e)
        {
            DriverItem item = (DriverItem) driver.Items[driver.SelectedIndex];
            CANDriverHelper.SetupDevices(device, item.Lib, _doc.RuntimeEngine);
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1710);
        }
    }
}
