using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;

using Mp.Utils;
using Mp.Scheme.Sdk;
using Mp.Visual.Diagram;


namespace Mp.Mod.CAN
{
    public partial class CANLoggerPortDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlSignalList;
        private Port _port;

        public CANLoggerPortDlg(Document doc, Port port, int portIdx)
        {
            _doc = doc;
            _port = port;
            _xmlSignalList = port.SignalList;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            rate.SelectedIndex = 0;
            adrMode.SelectedIndex = 0;
            bitRate.SelectedIndex = 0;
            portNo.Text = (portIdx + 1).ToString();
            LoadChannels();
            LoadData();
        }

        private void LoadChannels()
        {
            int index = signals.Rows.Add();
            DataGridViewRow row = signals.Rows[index];

            row.Cells[0].Value = StringResource.TypeTimeStamp;
            row.Cells[2].Value = "ULINT";
            row.Cells[3].Value = " 10 µs res.";

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = "Identifier";
            row.Cells[2].Value = "UDINT";
            row.Cells[3].Value = "";

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = "DLC";
            row.Cells[2].Value = "USINT";
            row.Cells[3].Value = "";


            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Message;
            row.Cells[2].Value = "ULINT";
            row.Cells[3].Value = "";

        }

        private void LoadData()
        {
            bitRate.Text = XmlHelper.GetParamNumber(_port.XmlRep, "bitRate").ToString();
            adrMode.SelectedIndex = (int)XmlHelper.GetParamNumber(_port.XmlRep, "extendedId");

            if (adrMode.SelectedIndex == 0)
            {
                code.Text = XmlHelper.GetParamNumber(_port.XmlRep, "code").ToString("X3");
                mask.Text = XmlHelper.GetParamNumber(_port.XmlRep, "mask").ToString("X3");
            }
            else
            {
                code.Text = XmlHelper.GetParamNumber(_port.XmlRep, "code").ToString("X8");
                mask.Text = XmlHelper.GetParamNumber(_port.XmlRep, "mask").ToString("X8");
            }
            
            acceptAll.Checked = XmlHelper.GetParamNumber(_port.XmlRep, "acceptAll") == 0;
            
            foreach (XmlElement xmlSignal in _xmlSignalList.ChildNodes)
            {
                int rowIndex = (int) XmlHelper.GetParamNumber(xmlSignal, "channelType");
                DataGridViewRow row = signals.Rows[rowIndex];
                row.Cells[1].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[3].Value = XmlHelper.GetParam(xmlSignal, "unit");
                row.Cells[4].Value = XmlHelper.GetParam(xmlSignal, "comment");
                double dbrate =  XmlHelper.GetParamDouble(xmlSignal, "samplerate");
                rate.SelectedIndex = GetSamplerate(dbrate);
                row.Tag = xmlSignal;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {

            XmlHelper.SetParamNumber(_port.XmlRep, "bitRate", "uint32_t", Convert.ToUInt32(bitRate.Text));
            XmlHelper.SetParamNumber(_port.XmlRep, "extendedId", "uint8_t", adrMode.SelectedIndex);
            
            if(acceptAll.Checked)
                XmlHelper.SetParamNumber(_port.XmlRep, "acceptAll", "uint8_t", 0);
            else
                XmlHelper.SetParamNumber(_port.XmlRep, "acceptAll", "uint8_t", 1);

            XmlHelper.SetParamNumber(_port.XmlRep, "code", "uint32_t", Convert.ToUInt32(code.Text,16));
            XmlHelper.SetParamNumber(_port.XmlRep, "mask", "uint32_t", Convert.ToUInt32(mask.Text, 16));


            foreach (DataGridViewRow row in signals.Rows)
            {
                string name = (string)row.Cells[1].Value;

                XmlElement xmlSignal = null;

                if (name == null)
                    name = "";

                if (!row.Visible)
                    name = "";

                if (name == "" && row.Tag != null)
                { //Remove 
                    xmlSignal = (XmlElement)row.Tag;
                    _doc.RemoveXmlObject(xmlSignal);
                    continue;
                }
                else if (name != "" && row.Tag != null)
                {//Update
                    xmlSignal = (XmlElement)row.Tag;
                }
                else if (name != "" && row.Tag == null)
                {//Create                    
                    xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.CAN.Sig.Logger");
                }
                else
                {
                    continue;
                }

                XmlHelper.SetParamNumber(xmlSignal, "channelType", "uint8_t",row.Index);
                XmlHelper.SetParam(xmlSignal, "name", "string", (string)row.Cells[1].Value);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[3].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[4].Value);
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", GetSamplerate(rate.SelectedIndex));
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", GetValueType(row));
                XmlHelper.SetParamNumber(xmlSignal, "physMin", "double", 0);
                XmlHelper.SetParamNumber(xmlSignal, "physMax", "double", 100000);
            }
            _doc.Modified = true;
            Close();
        }

        private int GetValueType(DataGridViewRow row)
        {
            string dtype = (string) row.Cells[2].Value;
            switch (dtype)
            {
                case "ULINT":
                    return (int)SignalDataType.ULINT;
                case "USINT":
                    return (int) SignalDataType.USINT;
                case "UDINT":
                    return (int)SignalDataType.UDINT;

            }
            throw new Exception("Unknow data type");
        }


        private int GetSamplerate(double r)
        {
            int rt = (int)r;

            switch (rt)
            {
                case 1:
                    return 0;
                case 2:
                    return 1;
                case 5:
                    return 2;
                case 10:
                    return 3;
                case 20:
                    return 4;
                case 50:
                    return 5;
                case 100:
                    return 6;
                case 200:
                    return 7;
                case 500:
                    return 8;
                case 1000:
                    return 9;
                case 1200:
                    return 10;
                case 1500:
                    return 11;
                case 2000:
                    return 12;
                case 2200:
                    return 13;
                case 2500:
                    return 14;
                case 3000:
                    return 15;
                case 3200:
                    return 16;
                case 3500:
                    return 17;
                case 4000:
                    return 18;
            }
            return 0;
        }
        private double GetSamplerate(int r)
        {
            switch (r)
            {
                case 0:
                    return 1;
                case 1:
                    return 2;
                case 2:
                    return 5;
                case 3:
                    return 10;
                case 4:
                    return 20;
                case 5:
                    return 50;
                case 6:
                    return 100;
                case 7:
                    return 200;
                case 8:
                    return 500;
                case 9:
                    return 1000;
                case 10:
                    return 1200;
                case 11:
                    return 1500;
                case 12:
                    return 2000;
                case 13:
                    return 2200;
                case 14:
                    return 2500;
                case 15:
                    return 3000;
                case 16:
                    return 3200;
                case 17:
                    return 3500;
                case 18:
                    return 4000;

            }
            return 1;
        }
        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void adrMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if( !acceptAll.Checked)
                return;

            if (adrMode.SelectedIndex == 0)
            {
                mask.Text = "000";
                code.Text = "000";
            }
            else
            {
                mask.Text = "80000000";
                code.Text = "80000000";
            }
        }

        private void acceptAll_CheckedChanged(object sender, EventArgs e)
        {

            if (!acceptAll.Checked)
            {
                mask.ReadOnly = false;
                code.ReadOnly = false;
            }
            else
            {
                mask.ReadOnly = true;
                code.ReadOnly = true;

                if (adrMode.SelectedIndex == 0)
                {
                    mask.Text = "000";
                    code.Text = "000";
                }
                else
                {
                    mask.Text = "80000000";
                    code.Text = "80000000";
                }
            }

        }

        private void mask_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(mask.Text, 16);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(mask, ex.Message);
                e.Cancel = true;
            }
        }

        private void code_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(code.Text, 16);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(code, ex.Message);
            }  
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 880);
        }

        private void CANLoggerPortDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
