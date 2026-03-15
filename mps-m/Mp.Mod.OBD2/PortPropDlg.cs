using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.OBD2
{
    internal partial class PortPropDlg : Form
    {
        private XmlElement _xmlSignalList;
        private XmlElement _xmlPS;
        private Document _doc;
        static int _rateIndex;
        private enum SignalColumns
        {
            SidPid,
            Description,
            Name,
            Unit,
            DataType,
            Comment
        }

        private enum VehicleInfoCols
        {
            SidInfo,
            Description,
            Property,
            AttachProp
        }

        private class ChannelInfo
        {
            public ChannelInfo(int sid, int pid, int byteOffset, int totalDataSize, string defName, double factor, double offset, double min, double max)
            {
                SID = sid;
                PID = pid;
                ByteOffset = byteOffset;
                DefName = defName;
                Offset = offset;
                Factor = factor;
                Min = min;
                Max = max;
                TotalDataSize = totalDataSize;
            }

            public ChannelInfo(int sid, int pid, int sensor, int byteOffset, int totalDataSize, string defName, double factor, double offset, double min, double max)
            {
                SID = sid;
                PID = pid;
                Sensor = sensor;
                ByteOffset = byteOffset;
                DefName = defName;
                Offset = offset;
                Factor = factor;
                Min = min;
                Max = max;
                TotalDataSize = totalDataSize;
            }
            public int SID;
            public int PID;
            public int Sensor = -1;
            public int ByteOffset;
            public string DefName;
            public double Factor;
            public double Offset;
            public double Min;
            public double Max;
            public int TotalDataSize;
        }

        public PortPropDlg(XmlElement xmlPS, XmlElement xmlSignalList, Document doc)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            _xmlSignalList = xmlSignalList;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            meaRate.SelectedIndex = _rateIndex;
            statusRate.SelectedIndex = _rateIndex;
            dtcRate.SelectedIndex = _rateIndex;
            oxygenRate.SelectedIndex = _rateIndex;
            freezeFrameRate.SelectedIndex = _rateIndex;
            LoadMeaChannels(meaSignals,0x01,"");
            LoadMeaChannels(freezFrameSignals, 0x02,"FF_");
            LoadStatusChannels();
            LoadDTCChannels();
            LoadOxygenTestChannels();
            LoadVehicleInfo();
            UpdateMeaChannels(meaSignals);
            UpdateMeaChannels(freezFrameSignals);
            meaOxygenSensor.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlPS, "oxygenSensorLoc");
            meaOxygenSensor.SelectedIndex = (int) XmlHelper.GetParamNumber(_xmlPS, "oxygenSensorLoc");
            LoadData();
            LoadVehicleInfoData();
            Mp.Utils.FormStateHandler.Restore(this, Document.RegistryKey + "PortPropDlg");
        }

        private void LoadData()
        {
            double meaRateValue = -1;
            double statusRateValue = -1;
            double dtcRateValue = -1;
            double oxygenRateValue = -1;
            double freezeFrameRateValue = -1;

            foreach (XmlElement xmlSignal in _xmlSignalList.ChildNodes)
            {
                int pid = (int)XmlHelper.GetParamNumber(xmlSignal, "pid");
                int sid = (int) XmlHelper.GetParamNumber(xmlSignal, "sid");
                int sensor = (int)XmlHelper.GetParamNumber(xmlSignal, "sensor");
                int byteOffset = (int) XmlHelper.GetParamNumber(xmlSignal, "byteOffset");
                
                DataGridViewRow row = GetRowByData(sid, pid, sensor, byteOffset,meaSignals);
                if( row != null)
                    meaRateValue = XmlHelper.GetParamDouble(xmlSignal, "samplerate");

                if (row == null)
                {
                    row = GetRowByData(sid, pid, sensor, byteOffset, dtcSignals);
                    if( row != null)
                        dtcRateValue = XmlHelper.GetParamDouble(xmlSignal, "samplerate");
                }

                if (row == null)
                {
                    row = GetRowByData(sid, pid, sensor, byteOffset, statusSignals);
                    if( row != null)
                        statusRateValue = XmlHelper.GetParamDouble(xmlSignal, "samplerate");
                }

                if (row == null)
                {
                    row = GetRowByData(sid, pid, sensor, byteOffset, oxygenTestSignals);
                    if( row != null)
                        oxygenRateValue = XmlHelper.GetParamDouble(xmlSignal, "samplerate");
                }

                if (row == null)
                {
                    row = GetRowByData(sid, pid, sensor, byteOffset, freezFrameSignals);
                    if( row != null)
                        freezeFrameRateValue = XmlHelper.GetParamDouble(xmlSignal, "samplerate");                    
                }

                if (row == null)
                    continue;
            
                row.Cells[(int)SignalColumns.Name].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[(int)SignalColumns.Unit].Value = XmlHelper.GetParam(xmlSignal, "unit");
                row.Cells[(int)SignalColumns.Comment].Value = XmlHelper.GetParam(xmlSignal, "comment");
                row.Tag = xmlSignal;
            }

            if(meaRateValue != -1)
                meaRate.SelectedIndex = GetSamplerateIndex((int)meaRateValue);

            if (statusRateValue != -1)
                statusRate.SelectedIndex = GetSamplerateIndex((int)statusRateValue);

            if (dtcRateValue != -1)
                dtcRate.SelectedIndex = GetSamplerateIndex((int)dtcRateValue);

            if(oxygenRateValue != -1)
                oxygenRate.SelectedIndex = GetSamplerateIndex((int)oxygenRateValue);

            if( freezeFrameRateValue != -1)
                freezeFrameRate.SelectedIndex = GetSamplerateIndex((int)freezeFrameRateValue);
        }

        private void LoadVehicleInfo()            
        {
            int index = vehicleInfo.Rows.Add();
            DataGridViewRow row = vehicleInfo.Rows[index];
            row.Cells[(int)VehicleInfoCols.SidInfo].Value = "$09/$02";
            row.Cells[(int)VehicleInfoCols.Description].Value = StringResource.VIN;
            row.Cells[(int)VehicleInfoCols.Description].Tag = new ChannelInfo(0x09, 0x02, 1,1, "", 0, 0, 0, 0);
            row.Cells[(int)VehicleInfoCols.AttachProp].Value = "...";

            index = vehicleInfo.Rows.Add();
            row = vehicleInfo.Rows[index];
            row.Cells[(int)VehicleInfoCols.SidInfo].Value = "$09/$04";
            row.Cells[(int)VehicleInfoCols.Description].Value = StringResource.CalId;
            row.Cells[(int)VehicleInfoCols.Description].Tag = new ChannelInfo(0x09, 0x04, 1, 1, "", 0, 0, 0, 0);
            row.Cells[(int)VehicleInfoCols.AttachProp].Value = "...";

            index = vehicleInfo.Rows.Add();
            row = vehicleInfo.Rows[index];
            row.Cells[(int)VehicleInfoCols.SidInfo].Value = "$09/$06";
            row.Cells[(int)VehicleInfoCols.Description].Value = StringResource.CalVerNo;
            row.Cells[(int)VehicleInfoCols.Description].Tag = new ChannelInfo(0x09, 0x06, 1, 1, "", 0, 0, 0, 0);
            row.Cells[(int)VehicleInfoCols.AttachProp].Value = "...";
            /*
            index = vehicleInfo.Rows.Add();
            row = vehicleInfo.Rows[index];
            row.Cells[(int)VehicleInfoCols.SidInfo].Value = "$09/$08";
            row.Cells[(int)VehicleInfoCols.Description].Value = "In-use performance tracking";
            row.Cells[(int)VehicleInfoCols.Description].Tag = new ChannelInfo(0x09, 0x08, 1, "", 0, 0, 0, 0);
            row.Cells[(int)VehicleInfoCols.AttachProp].Value = "...";
            */
            index = vehicleInfo.Rows.Add();
            row = vehicleInfo.Rows[index];
            row.Cells[(int)VehicleInfoCols.SidInfo].Value = "$09/$0A";
            row.Cells[(int)VehicleInfoCols.Description].Value = StringResource.ECUnames;
            row.Cells[(int)VehicleInfoCols.Description].Tag = new ChannelInfo(0x09, 0x0A, 1, 1, "", 0, 0, 0, 0);
            row.Cells[(int)VehicleInfoCols.AttachProp].Value = "...";

        }

        private void LoadOxygenTestChannels()
        {
            int index = 0;
            DataGridViewRow row = null;

            //0x05,0x01,0x01
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$01/$01";
            row.Cells[(int)SignalColumns.Description].Value = "(1/1) Rich to lean sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x01, 0x01, 2,3, "RLSTV1", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x02,0x01
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$02/$01";
            row.Cells[(int)SignalColumns.Description].Value = "(1/1) Lean to rich sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x02, 0x01, 2,3, "LRSTV1", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x03,0x01
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$03/$01";
            row.Cells[(int)SignalColumns.Description].Value = "(1/1) Low sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x03, 0x01, 2,3, "LSVSTC1", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x04,0x01
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$04/$01";
            row.Cells[(int)SignalColumns.Description].Value = "(1/1) High sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x04, 0x01, 2, 3, "HSVSTC1", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x05,0x01
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$05/$01";
            row.Cells[(int)SignalColumns.Description].Value = "(1/1) Rich to lean sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x05, 0x01, 2, 3, "RLSST1", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x06,0x01
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$06/$01";
            row.Cells[(int)SignalColumns.Description].Value = "(1/1) Lean to rich sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x06, 0x01, 2, 3, "LRSST1", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //0x05,0x07,0x01
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$07/$01";
            row.Cells[(int)SignalColumns.Description].Value = "(1/1) Minimum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x07, 0x01, 2, 3, "MSVTC1", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x08,0x01
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$08/$01";
            row.Cells[(int)SignalColumns.Description].Value = "(1/1) Maximum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x08, 0x01, 2, 3, "MASVTC1", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x09,0x01
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$09/$01";
            row.Cells[(int)SignalColumns.Description].Value = "(1/1) Time between sensor transitions";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x09, 0x01, 2, 3, "TBST1", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x0A,0x01
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$0A/$01";
            row.Cells[(int)SignalColumns.Description].Value = "(1/1) Sensor period";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x0A, 0x01, 2, 3, "SP1", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";




            //0x05,0x01,0x02
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$01/$02";
            row.Cells[(int)SignalColumns.Description].Value = "(1/2) Rich to lean sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x01, 0x02, 2, 3, "RLSTV2", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x02,0x02
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$02/$02";
            row.Cells[(int)SignalColumns.Description].Value = "(1/2) Lean to rich sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x02, 0x02, 2, 3, "LRSTV2", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x03,0x02
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$03/$02";
            row.Cells[(int)SignalColumns.Description].Value = "(1/2) Low sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x03, 0x02, 2, 3, "LSVSTC2", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x04,0x02
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$04/$02";
            row.Cells[(int)SignalColumns.Description].Value = "(1/2) High sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x04, 0x02, 2, "HSVSTC2", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x05,0x02
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$05/$02";
            row.Cells[(int)SignalColumns.Description].Value = "(1/2) Rich to lean sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x05, 0x02, 2, 3, "RLSST2", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x06,0x02
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$06/$02";
            row.Cells[(int)SignalColumns.Description].Value = "(1/2) Lean to rich sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x06, 0x02, 2, 3, "LRSST2", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //0x05,0x07,0x02
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$07/$02";
            row.Cells[(int)SignalColumns.Description].Value = "(1/2) Minimum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x07, 0x02, 2, 3, "MSVTC2", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x08,0x02
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$08/$02";
            row.Cells[(int)SignalColumns.Description].Value = "(1/2) Maximum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x08, 0x02, 2, 3, "MASVTC2", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x09,0x02
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$09/$02";
            row.Cells[(int)SignalColumns.Description].Value = "(1/2) Time between sensor transitions";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x09, 0x02, 2, 3,"TBST2", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x0A,0x02
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$0A/$02";
            row.Cells[(int)SignalColumns.Description].Value = "(1/2) Sensor period";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x0A, 0x02, 2, 3,"SP2", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //----
            //0x05,0x01,0x04
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$01/$04";
            row.Cells[(int)SignalColumns.Description].Value = "(1/3) Rich to lean sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x01, 0x04, 2, 3,"RLSTV4", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x02,0x04
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$02/$04";
            row.Cells[(int)SignalColumns.Description].Value = "(1/3) Lean to rich sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x02, 0x04, 2, 3, "LRSTV4", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x03,0x04
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$03/$04";
            row.Cells[(int)SignalColumns.Description].Value = "(1/3) Low sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x03, 0x04, 2, 3, "LSVSTC4", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x04,0x04
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$04/$04";
            row.Cells[(int)SignalColumns.Description].Value = "(1/3) High sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x04, 0x04, 2, 3, "HSVSTC4", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x05,0x04
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$05/$04";
            row.Cells[(int)SignalColumns.Description].Value = "(1/3) Rich to lean sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x05, 0x04, 2, 3, "RLSST4", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x06,0x04
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$06/$04";
            row.Cells[(int)SignalColumns.Description].Value = "(1/3) Lean to rich sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x06, 0x04, 2, 3, "LRSST4", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //0x05,0x07,0x04
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$07/$04";
            row.Cells[(int)SignalColumns.Description].Value = "(1/3) Minimum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x07, 0x04, 2, 3, "MSVTC4", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x08,0x04
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$08/$04";
            row.Cells[(int)SignalColumns.Description].Value = "(1/3) Maximum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x08, 0x04, 2, 3, "MASVTC4", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x09,0x04
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$09/$04";
            row.Cells[(int)SignalColumns.Description].Value = "(1/3) Time between sensor transitions";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x09, 0x04, 2, 3, "TBST4", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x0A,0x04
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$0A/$04";
            row.Cells[(int)SignalColumns.Description].Value = "(1/3) Sensor period";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x0A, 0x04, 2, 3, "SP4", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //----
            //0x05,0x01,0x08
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$01/$08";
            row.Cells[(int)SignalColumns.Description].Value = "(1/4) Rich to lean sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x01, 0x08, 2, 3, "RLSTV8", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x02,0x08
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$02/$08";
            row.Cells[(int)SignalColumns.Description].Value = "(1/4) Lean to rich sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x02, 0x08, 2, 3, "LRSTV8", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x03,0x08
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$03/$08";
            row.Cells[(int)SignalColumns.Description].Value = "(1/4) Low sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x03, 0x08, 2, 3, "LSVSTC8", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x04,0x08
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$04/$08";
            row.Cells[(int)SignalColumns.Description].Value = "(1/4) High sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x04, 0x08, 2, 3, "HSVSTC8", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x05,0x08
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$05/$08";
            row.Cells[(int)SignalColumns.Description].Value = "(1/4) Rich to lean sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x05, 0x08, 2, 3, "RLSST8", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x06,0x08
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$06/$08";
            row.Cells[(int)SignalColumns.Description].Value = "(1/4) Lean to rich sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x06, 0x08, 2, 3, "LRSST8", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //0x05,0x07,0x08
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$07/$08";
            row.Cells[(int)SignalColumns.Description].Value = "(1/4) Minimum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x07, 0x08, 2, 3, "MSVTC8", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x08,0x08
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$08/$08";
            row.Cells[(int)SignalColumns.Description].Value = "(1/4) Maximum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x08, 0x08, 2, 3, "MASVTC8", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x09,0x08
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$09/$08";
            row.Cells[(int)SignalColumns.Description].Value = "(1/4) Time between sensor transitions";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x09, 0x08, 2, 3, "TBST8", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x0A,0x08
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$0A/$08";
            row.Cells[(int)SignalColumns.Description].Value = "(1/4) Sensor period";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x0A, 0x08, 2, 3, "SP8", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //----
            //0x05,0x01,0x10
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$01/$10";
            row.Cells[(int)SignalColumns.Description].Value = "(2/1) Rich to lean sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x01, 0x10, 2, 3, "RLSTV10", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x02,0x10
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$02/$10";
            row.Cells[(int)SignalColumns.Description].Value = "(2/1) Lean to rich sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x02, 0x10, 2, 3, "LRSTV10", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x03,0x10
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$03/$10";
            row.Cells[(int)SignalColumns.Description].Value = "(2/1) Low sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x03, 0x10, 2, 3, "LSVSTC10", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x04,0x10
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$04/$10";
            row.Cells[(int)SignalColumns.Description].Value = "(2/1) High sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x04, 0x10, 2, 3, "HSVSTC10", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x05,0x10
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$05/$10";
            row.Cells[(int)SignalColumns.Description].Value = "(2/1) Rich to lean sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x05, 0x10, 2, 3, "RLSST10", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x06,0x10
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$06/$10";
            row.Cells[(int)SignalColumns.Description].Value = "(2/1) Lean to rich sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x06, 0x10, 2, 3, "LRSST10", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //0x05,0x07,0x10
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$07/$10";
            row.Cells[(int)SignalColumns.Description].Value = "(2/1) Minimum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x07, 0x10, 1, 3, "MSVTC10", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x08,0x10
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$08/$10";
            row.Cells[(int)SignalColumns.Description].Value = "(2/1) Maximum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x08, 0x10, 2, 3, "MASVTC10", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x09,0x10
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$09/$10";
            row.Cells[(int)SignalColumns.Description].Value = "(2/1) Time between sensor transitions";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x09, 0x10, 2, 3, "TBST10", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x0A,0x10
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$0A/$10";
            row.Cells[(int)SignalColumns.Description].Value = "(2/1) Sensor period";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x0A, 0x10, 2, 3, "SP10", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //----
            //0x05,0x01,0x20
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$01/$20";
            row.Cells[(int)SignalColumns.Description].Value = "(2/2) Rich to lean sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x01, 0x20, 2, 3, "RLSTV20", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x02,0x20
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$02/$20";
            row.Cells[(int)SignalColumns.Description].Value = "(2/2) Lean to rich sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x02, 0x20, 2, 3, "LRSTV20", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x03,0x20
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$03/$20";
            row.Cells[(int)SignalColumns.Description].Value = "(2/2) Low sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x03, 0x20, 2, 3, "LSVSTC20", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x04,0x20
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$04/$20";
            row.Cells[(int)SignalColumns.Description].Value = "(2/2) High sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x04, 0x20, 2, 3, "HSVSTC20", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x05,0x20
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$05/$20";
            row.Cells[(int)SignalColumns.Description].Value = "(2/2) Rich to lean sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x05, 0x20, 2, 3, "RLSST20", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x06,0x20
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$06/$20";
            row.Cells[(int)SignalColumns.Description].Value = "(2/2) Lean to rich sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x06, 0x20, 2, 3, "LRSST20", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //0x05,0x07,0x20
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$07/$20";
            row.Cells[(int)SignalColumns.Description].Value = "(2/2) Minimum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x07, 0x20, 2, 3, "MSVTC20", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x08,0x20
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$08/$20";
            row.Cells[(int)SignalColumns.Description].Value = "(2/2) Maximum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x08, 0x20, 2, 3, "MASVTC20", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x09,0x20
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$09/$20";
            row.Cells[(int)SignalColumns.Description].Value = "(2/2) Time between sensor transitions";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x09, 0x20, 2, 3, "TBST20", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x0A,0x20
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$0A/$20";
            row.Cells[(int)SignalColumns.Description].Value = "(2/2) Sensor period";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x0A, 0x20, 2, 3, "SP20", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";



            //----
            //0x05,0x01,0x40
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$01/$40";
            row.Cells[(int)SignalColumns.Description].Value = "(2/3) Rich to lean sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x01, 0x40, 2, 3, "RLSTV40", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x02,0x40
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$02/$40";
            row.Cells[(int)SignalColumns.Description].Value = "(2/3) Lean to rich sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x02, 0x40, 2, 3, "LRSTV40", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x03,0x40
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$03/$40";
            row.Cells[(int)SignalColumns.Description].Value = "(2/3) Low sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x03, 0x40, 2, 3, "LSVSTC40", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x04,0x40
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$04/$40";
            row.Cells[(int)SignalColumns.Description].Value = "(2/3) High sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x04, 0x40, 2, 3, "HSVSTC40", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x05,0x40
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$05/$40";
            row.Cells[(int)SignalColumns.Description].Value = "(2/3) Rich to lean sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x05, 0x40, 2, 3, "RLSST40", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x06,0x40
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$06/$40";
            row.Cells[(int)SignalColumns.Description].Value = "(2/3) Lean to rich sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x06, 0x40, 2, 3, "LRSST40", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //0x05,0x07,0x40
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$07/$40";
            row.Cells[(int)SignalColumns.Description].Value = "(2/3) Minimum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x07, 0x40, 2, 3, "MSVTC40", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x08,0x40
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$08/$40";
            row.Cells[(int)SignalColumns.Description].Value = "(2/3) Maximum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x08, 0x40, 2, 3, "MASVTC40", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x09,0x40
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$09/$40";
            row.Cells[(int)SignalColumns.Description].Value = "(2/3) Time between sensor transitions";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x09, 0x40, 2, 3, "TBST40", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x0A,0x40
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$0A/$40";
            row.Cells[(int)SignalColumns.Description].Value = "(2/3) Sensor period";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x0A, 0x40, 2, 3, "SP40", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";



            //----
            //0x05,0x01,0x80
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$01/$80";
            row.Cells[(int)SignalColumns.Description].Value = "(2/4) Rich to lean sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x01, 0x80, 2, 3, "RLSTV80", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x02,0x80
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$02/$80";
            row.Cells[(int)SignalColumns.Description].Value = "(2/4) Lean to rich sensor threshold voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x02, 0x80, 2, 3, "LRSTV80", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x03,0x80
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$03/$80";
            row.Cells[(int)SignalColumns.Description].Value = "(2/4) Low sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x03, 0x80, 2, 3, "LSVSTC80", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x04,0x80
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$04/$80";
            row.Cells[(int)SignalColumns.Description].Value = "(2/4) High sensor voltage for switch time calculation";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x04, 0x80, 2, 3, "HSVSTC80", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x05,0x80
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$05/$80";
            row.Cells[(int)SignalColumns.Description].Value = "(2/4) Rich to lean sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x05, 0x80, 2, 3, "RLSST80", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x06,0x80
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$06/$80";
            row.Cells[(int)SignalColumns.Description].Value = "(2/4) Lean to rich sensor switch time";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x06, 0x80, 2, 3, "LRSST80", 0.004, 0, 0, 1.02);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";


            //0x05,0x07,0x80
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$07/$80";
            row.Cells[(int)SignalColumns.Description].Value = "(2/4) Minimum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x07, 0x80, 2, 3, "MSVTC80", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x08,0x80
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$08/$80";
            row.Cells[(int)SignalColumns.Description].Value = "(2/4) Maximum sensor voltage for test cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x08, 0x80, 2, 3,"MASVTC80", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x05,0x09,0x80
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$09/$80";
            row.Cells[(int)SignalColumns.Description].Value = "(2/4) Time between sensor transitions";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x09, 0x80, 2, 3, "TBST80", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";

            //0x05,0x0A,0x80
            index = oxygenTestSignals.Rows.Add();
            row = oxygenTestSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$05/$0A/$80";
            row.Cells[(int)SignalColumns.Description].Value = "(2/4) Sensor period";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x05, 0x0A, 0x80, 2, 3, "SP80", 0.04, 0, 0, 10.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "s";
        }

        private void LoadStatusChannels()
        {
            int index = 0;
            DataGridViewRow row = null;

            //0x01,0x01
            index = statusSignals.Rows.Add();
            row = statusSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$01/$01";
            row.Cells[(int)SignalColumns.Description].Value = "Malfunction indicator lamp";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x01, 0x01, 1, 1, "MIL", 1, 0, 0, 1);
            row.Cells[(int)SignalColumns.DataType].Value = "BOOL";

            //0x01,0x03
            index = statusSignals.Rows.Add();
            row = statusSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$01/$03";
            row.Cells[(int)SignalColumns.Description].Value = "Fuel system 1 status";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x01, 0x03, 1, 2, "FUELSYS1", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "BYTE";

            //0x01,0x03
            index = statusSignals.Rows.Add();
            row = statusSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$01/$03";
            row.Cells[(int)SignalColumns.Description].Value = "Fuel system 2 status";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x01, 0x03, 2, 2, "FUELSYS2", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "BYTE";

            //0x01,0x12
            index = statusSignals.Rows.Add();
            row = statusSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$01/$12";
            row.Cells[(int)SignalColumns.Description].Value = "Commanded secondary air status";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x01, 0x12, 1,1, "AIR_STAT", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "BYTE";
            row.Cells[(int)SignalColumns.Unit].Value = "";


            //0x01,0x13
            index = statusSignals.Rows.Add();
            row = statusSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$01/$13";
            row.Cells[(int)SignalColumns.Description].Value = "Location of oxygen sensors $13";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x01, 0x13, 1, 1, "O2SLOC$13", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "BYTE";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x1D
            index = statusSignals.Rows.Add();
            row = statusSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$01/$1D";
            row.Cells[(int)SignalColumns.Description].Value = "Location of oxygen sensors $1D";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x01, 0x1D, 1, 1, "O2SLOC$1D", 1, 0, 0, 8);
            row.Cells[(int)SignalColumns.DataType].Value = "BYTE";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x1E
            index = statusSignals.Rows.Add();
            row = statusSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$01/$1E";
            row.Cells[(int)SignalColumns.Description].Value = "Auxiliary input status";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x01, 0x1E, 1, 1, "PTO_STAT", 1, 0, 0, 8);
            row.Cells[(int)SignalColumns.DataType].Value = "BYTE";
            row.Cells[(int)SignalColumns.Unit].Value = "";


            //0x01,0x41
            index = statusSignals.Rows.Add();
            row = statusSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$01/$41";
            row.Cells[(int)SignalColumns.Description].Value = "Monitor status this driving cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x01, 0x41, 1, 4, "MSTDC", 1, 0, 0, UInt32.MaxValue);
            row.Cells[(int)SignalColumns.DataType].Value = "DWORD";
            row.Cells[(int)SignalColumns.Unit].Value = "";


        }

        private void LoadDTCChannels()
        {
            int index = 0;
            DataGridViewRow row = null;

            //0x01,0x02
            index = dtcSignals.Rows.Add();
            row = dtcSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$01/$02";
            row.Cells[(int)SignalColumns.Description].Value = "DTC that caused required freeze data storage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x01, 0x02, 1, 2, "DTCFRZF", 1, 0, 0, 65535);
            row.Cells[(int)SignalColumns.DataType].Value = "WORD";

            //0x03 Emission-related DTC
            index = dtcSignals.Rows.Add();
            row = dtcSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$03";
            row.Cells[(int)SignalColumns.Description].Value = "Emission-related DTC";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x03, 0x00, 1, 2, "DTC", 1, 0, 0, 65535);
            row.Cells[(int)SignalColumns.DataType].Value = "WORD";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x07 Emission-related DTC
            index = dtcSignals.Rows.Add();
            row = dtcSignals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$07";
            row.Cells[(int)SignalColumns.Description].Value = "Emission-related DTC detected during current or last completed driving cycle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(0x07, 0x00, 1, 2, "DTCDDCDC", 1, 0, 0, 65535);
            row.Cells[(int)SignalColumns.DataType].Value = "WORD";
            row.Cells[(int)SignalColumns.Unit].Value = "";
        }

        private void UpdateMeaChannels(DataGridView signals)
        {
            foreach (DataGridViewRow row in signals.Rows)
            {
                ChannelInfo info = (ChannelInfo) row.Cells[(int)SignalColumns.Description].Tag;

                if (meaOxygenSensor.SelectedIndex == 0)
                {//$13
                    switch (info.PID)
                    {
                        case 0x06:
                        case 0x07:
                        case 0x08:
                        case 0x09:
                            if (info.ByteOffset == 2)
                                row.Visible = false;

                            if (info.ByteOffset == 1)
                                row.Visible = true;
                        break;

                        case 0x14:
                            if( info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 1/1";
                            else                                
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 1/1";
                        break;
                        case 0x15:
                            if( info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 1/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 1/2";
                        break;
                        case 0x16:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 1/3";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 1/3";
                        break;
                        case 0x17:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 1/4";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 1/4";
                        break;
                        case 0x18:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 2/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 2/1";
                        break;
                        case 0x19:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 2/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 2/2";
                        break;
                        case 0x1A:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 2/3";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 2/3";
                        break;
                        case 0x1B:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 2/4";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 2/4";
                        break;
//-----
                        case 0x24:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/1";
                        break;
                        case 0x25:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/2";
                        break;

                        case 0x26:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/3";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/3";
                        break;
                        case 0x27:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/4";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/4";
                        break;
                        case 0x28:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/1";
                        break;
                        case 0x29:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/2";
                        break;
                        case 0x2A:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/3";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/3";
                        break;
                        case 0x2B:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/4";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/4";   
                        break;
//---
                        case 0x34:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 1/1";
                        break;
                        case 0x35:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 1/2";
                        break;

                        case 0x36:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/3";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 1/3";
                        break;
                        case 0x37:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/4";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 1/4";
                        break;
                        case 0x38:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 2/1";
                        break;

                        case 0x39:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 2/2";
                        break;

                        case 0x3A:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/3";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 2/3";
                        break;
                        case 0x3B:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/4";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 2/4";
                        break;

                        case 0x55:
                        case 0x56:
                        case 0x57:
                        case 0x58:
                            if (info.ByteOffset == 2)
                                row.Visible = false;

                            if (info.ByteOffset == 1)
                                row.Visible = true;
                        break;

                    }
                }
                else
                {//$1D
                    switch (info.PID)
                    {

                        case 0x06:
                        case 0x07:
                        case 0x08:
                        case 0x09:
                            if (info.ByteOffset == 2)
                                row.Visible = true;

                            if (info.ByteOffset == 1)
                                row.Visible = true;
                        break;
                        case 0x14:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 1/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 1/1";
                        break;
                        case 0x15:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 1/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 1/2";
                        break;
                        case 0x16:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 2/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 2/1";
                        break;
                        case 0x17:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 2/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 2/2";
                        break;
                        case 0x18:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 3/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 3/1";
                        break;
                        case 0x19:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 3/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 3/2";
                        break;
                        case 0x1A:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 4/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 4/1";
                            break;
                        case 0x1B:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor output voltage 4/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 4/2";
                        break;

                        case 0x24:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/1";
                        break;
                        case 0x25:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/2";
                        break;

                        case 0x26:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/1";
                        break;
                        case 0x27:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/2";
                        break;
                        case 0x28:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 3/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 3/1";
                        break;
                        case 0x29:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 3/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 3/2";
                        break;
                        case 0x2A:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 4/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 4/1";
                        break;
                        case 0x2B:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 4/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 4/2";
                        break;
//----
                        case 0x34:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 1/1";
                        break;
                        case 0x35:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 1/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 1/2";
                        break;

                        case 0x36:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 2/1";
                        break;
                        case 0x37:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 2/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 2/2";
                        break;
                        case 0x38:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 3/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 3/1";
                        break;

                        case 0x39:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 3/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 3/2";
                        break;

                        case 0x3A:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 4/1";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 4/1";
                        break;
                        case 0x3B:
                            if (info.ByteOffset == 1)
                                row.Cells[(int)SignalColumns.Description].Value = "Equivalence Ration (lambda) 4/2";
                            else
                                row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 4/2";
                        break;

                        case 0x55:
                        case 0x56:
                        case 0x57:
                        case 0x58:
                            if (info.ByteOffset == 2)
                                row.Visible = true;

                            if (info.ByteOffset == 1)
                                row.Visible = true;
                        break;
                    }
                }
            }
        }

        private void LoadMeaChannels(DataGridView signals, int sid, string prefixName)
        {
            int index = 0;
            DataGridViewRow row = null;

            
            //0x01,0x04
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$04";
            row.Cells[(int)SignalColumns.Description].Value = "Calculated engine load value";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x04, 1,1, prefixName + "LOAD_PCT", 100.0 / 255.0, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x05
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$05";
            row.Cells[(int)SignalColumns.Description].Value = "Engine coolant temperatue";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x05, 1,1, prefixName + "ECT", 1, -40, -40, 215);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "°C";

            //0x01,0x06
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$06";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim - Bank 1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x06, 1,2, prefixName + "SHRTFT1", 100.0 / 128.0, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x08
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$08";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim - Bank 2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x08, 1,2, prefixName + "SHORTFT2", 100.0 / 128.0, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x06
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$06";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim - Bank 3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x06, 2,2, prefixName + "SHRTFT3", 100.0 / 128.0, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x08
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$08";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim - Bank 4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x08, 2,2, prefixName + "SHORTFT4", 100.0 / 128.0, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x07
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$07";
            row.Cells[(int)SignalColumns.Description].Value = "Long term fuel trim - Bank 1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x07, 1,2, prefixName + "LONGFT1", 100.0 / 128.0, -100, -100, 99.2);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x09
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$09";
            row.Cells[(int)SignalColumns.Description].Value = "Long term fuel trim - Bank 2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x09, 1,2, prefixName + "LONGFT2", 100.0 / 128.0, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x07
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$07";
            row.Cells[(int)SignalColumns.Description].Value = "Long term fuel trim - Bank 3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x07, 2,2, prefixName + "LONGFT1", 100.0 / 128.0, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x09
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$09";
            row.Cells[(int)SignalColumns.Description].Value = "Long term fuel trim - Bank 4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x09, 2,2, prefixName + "LONGFT2", 100.0 / 128.0, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x0A
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$0A";
            row.Cells[(int)SignalColumns.Description].Value = "Fuel rail pressure";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x0A, 1,1, prefixName + "FRP", 3, 0, 0, 765);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "kPa";

            //0x01,0x0B
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$0B";
            row.Cells[(int)SignalColumns.Description].Value = "Intake manifold absolute pressure";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x0B, 1,1, prefixName + "MAP", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "kPa";


            //0x01,0x0C
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$0C";
            row.Cells[(int)SignalColumns.Description].Value = "Engine RPM";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x0C, 1,2, prefixName + "RPM", 1 / 4.0, 0, 0, 16383.75);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "1/min";

            //0x01,0x0D
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$0D";
            row.Cells[(int)SignalColumns.Description].Value = "Vehicle speed sensor";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x0D, 1, 1, prefixName + "VSS", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "km/h";

            //0x01,0x0E
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$0E";
            row.Cells[(int)SignalColumns.Description].Value = "Ignition timing advance";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x0E, 1,1, prefixName + "SPARKADV", 1 / 2.0, -64, -64, 63.5);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "°";

            //0x01,0x0F
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$0F";
            row.Cells[(int)SignalColumns.Description].Value = "Intake air temperature";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x0F, 1,1, prefixName + "IAT", 1, -40, -40, 215);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "°C";

            //0x01,0x10
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$10";
            row.Cells[(int)SignalColumns.Description].Value = "Air flow rate from mass air flow sensor";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x10, 1,1, prefixName + "MAF", 0.01, 0, 0, 655.35);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "g/s";

            //0x01,0x11
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$11";
            row.Cells[(int)SignalColumns.Description].Value = "Absolute throttle position";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x11, 1,1, prefixName + "TP", 100 / 255, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x14
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$14";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x14, 1,2, prefixName + "O2S11V", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x01,0x15
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$15";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x15, 1, 2, prefixName + "O2S12V", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x01,0x16
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$16";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x16, 1, 2, prefixName + "O2S13V", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x01,0x17
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$17";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x17, 1, 2, prefixName + "O2S14V", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x01,0x18
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$18";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x18, 1, 2, prefixName + "O2S21V", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x01,0x19
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$19";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x19, 1, 2, prefixName + "O2S22V", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";


            //0x01,0x1A
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$1A";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x1A, 1, 2, prefixName + "O2S23V", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";


            //0x01,0x1B
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$1B";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x1B, 1, 2, prefixName + "O2S24V", 0.005, 0, 0, 1.275);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x01,0x14
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$14";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 1/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x14, 2, 2, prefixName + "SHRTFT11", 100 / 128, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";



            //0x01,0x15
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$15";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 1/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x15, 2, 2, prefixName + "SHRTFT12", 100 / 128, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x16
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$16";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 1/3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x16, 2, 2, prefixName + "SHRTFT13", 100 / 128, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x17
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$17";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 1/4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x17, 2, 2, prefixName + "SHRTFT14", 100 / 128, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";



        
            //0x01,0x18
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$18";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 2/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x18, 2, 2, prefixName + "SHRTFT21", 100 / 128, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";



            //0x01,0x19
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$19";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 2/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x19, 2, 2, prefixName + "SHRTFT22", 100 / 128, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x1A
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$1A";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 2/3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x1A, 2, 2, prefixName + "SHRTFT23", 100 / 128, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x1B
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$1B";
            row.Cells[(int)SignalColumns.Description].Value = "Short term fuel trim 2/4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x1B, 2, 2, prefixName + "SHRTFT24", 100 / 128, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x1F
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$1F";
            row.Cells[(int)SignalColumns.Description].Value = "Time since engine start";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x1F, 1, 2, prefixName + "RUNTM", 1, 0, 0, 65535);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "sec.";


            //0x01,0x21
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$21";
            row.Cells[(int)SignalColumns.Description].Value = "Distance travelled while MIL is activated";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x21, 1, 2, prefixName + "MIL_DIST", 1, 0, 0, 65535);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "km";


            //0x01,0x22
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$22";
            row.Cells[(int)SignalColumns.Description].Value = "Fuel rail pressure relative to manifold vacuum";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x22, 1, 1, prefixName + "FRPMV", 0.079, 0, 0, 5177.27);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "kPa";

            //0x01,0x23
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$23";
            row.Cells[(int)SignalColumns.Description].Value = "Fuel rail pressure";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x23, 1, 2, prefixName + "FRP", 10, 0, 0, 655350);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "kPa";


            //0x01,0x24
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$24";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 1/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x24, 1, 4, prefixName + "EQ_RAT11", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x25
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$25";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 1/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x25, 1, 4, prefixName + "EQ_RAT12", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x26
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$26";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 1/3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x26, 1, 4, prefixName + "EQ_RAT13", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";


            //0x01,0x27
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$27";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 1/4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x27, 1, 4, prefixName + "EQ_RAT14", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x28
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$28";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 2/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x28, 1, 4, prefixName + "EQ_RAT21", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x29
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$29";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 2/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x29, 1, 4, prefixName + "EQ_RAT22", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x2A
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$2A";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 2/3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x2A, 1, 4, prefixName + "EQ_RAT23", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x2B
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$2B";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 2/4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x2B, 1, 4, prefixName + "EQ_RAT24", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x24
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$24";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x24, 3, 4, prefixName + "O2S11", 0.000122, 0, 0, 7.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x01,0x25
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$25";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x25, 3, 4, prefixName + "O2S12", 0.000122, 0, 0, 7.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x01,0x26
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$26";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x26, 3, 4, prefixName + "O2S13", 0.000122, 0, 0, 7.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";


            //0x01,0x27
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$27";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 1/4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x27, 3, 4, prefixName + "O2S14", 0.000122, 0, 0, 7.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";


            //0x01,0x28
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$28";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x28, 3, 4, prefixName + "O2S21", 0.000122, 0, 0, 7.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";



            //0x01,0x29
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$29";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x29, 3, 4, prefixName + "O2S22", 0.000122, 0, 0, 7.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";


            //0x01,0x2A
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$2A";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x2A, 3, 4, prefixName + "O2S23", 0.000122, 0, 0, 7.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x01,0x2B
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$2B";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor voltage 2/4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x2B, 3, 4, prefixName + "O2S24", 0.000122, 0, 0, 7.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

   
      

            //0x01,0x2C
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$2C";
            row.Cells[(int)SignalColumns.Description].Value = "Commanded EGR";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x2C, 1, 1, prefixName + "EGR_PCT", 100 / 255.0, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x2D
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$2D";
            row.Cells[(int)SignalColumns.Description].Value = "EGR error";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x2D, 1, 1, prefixName + "EGR_ERR", 100 / 128.0, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x2E
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$2E";
            row.Cells[(int)SignalColumns.Description].Value = "Commanded evaporative purge";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x2E, 1, 1, prefixName + "EVAP_PCT", 100 / 255.0, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x2F
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$2F";
            row.Cells[(int)SignalColumns.Description].Value = "Fuel level input";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x2F, 1, 1, prefixName + "FLI", 100 / 255.0, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x30
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$30";
            row.Cells[(int)SignalColumns.Description].Value = "No. of wurm-ups since DTC cleared";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x30, 1, 1, prefixName + "WATM_UPS", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";


            //0x01,0x31
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$31";
            row.Cells[(int)SignalColumns.Description].Value = "Distance since DTC cleared";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x31, 1, 2, prefixName + "CLR_DIST", 1, 0, 0, 65535);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "km";

            //0x01,0x32
            //TODO: check scaling
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$32";
            row.Cells[(int)SignalColumns.Description].Value = "Evap system vapour pressure";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x32, 1, 2, prefixName + "EVAP_VP", 0.25, -8192, -8192, 8191.75);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "Pa";

            //0x01,0x33            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$33";
            row.Cells[(int)SignalColumns.Description].Value = "Barometric pressure";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x33, 1, 1, prefixName + "BARO", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "kPa";



            //0x01,0x34            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$34";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 1/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x34, 3, 4, prefixName + "O2S11A", 0.0039625, -128, -128, 127.996);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "mA";


            //0x01,0x35           
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$35";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 1/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x35, 3, 4, prefixName + "O2S12A", 0.0039625, -128, -128, 127.996);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "mA";



            //0x01,0x36            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$36";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 2/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x36, 3, 4, prefixName + "O2S21A", 0.0039625, -128, -128, 127.996);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "mA";


            //0x01,0x37           
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$37";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 2/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x37, 3, 4, prefixName + "O2S22A", 0.0039625, -128, -128, 127.996);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "mA";

            //0x01,0x38           
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$38";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 3/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x38, 3, 4, prefixName + "O2S31A", 0.0039625, -128, -128, 127.996);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "mA";


            //0x01,0x39            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$39";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 3/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x39, 3, 4, prefixName + "O2S32A", 0.0039625, -128, -128, 127.996);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "mA";

            //0x01,0x3A            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$3A";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 4/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x3A, 3, 4, prefixName + "O2S41A", 0.0039625, -128, -128, 127.996);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "mA";

            //0x01,0x3B            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$3B";
            row.Cells[(int)SignalColumns.Description].Value = "Oxygen sensor current 4/3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x3B, 3, 4, prefixName + "O2S42A", 0.0039625, -128, -128, 127.996);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "mA";

            //0x01,0x34            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$34";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 1/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x34, 1, 4, prefixName + "EQ_RAT11", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x35            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$35";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 1/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x35, 1, 4, prefixName + "EQ_RAT12", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x36           
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$36";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 2/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x36, 1, 4, prefixName + "EQ_RAT21", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";



            //0x01,0x37            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$37";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 2/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x37, 1, 4, prefixName + "EQ_RAT22", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";



            //0x01,0x38            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$38";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 3/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x38, 1, 4, prefixName + "EQ_RAT31", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";




            //0x01,0x39            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$39";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 3/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x39, 1, 4, prefixName + "EQ_RAT32", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";



            //0x01,0x3A            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$3A";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 4/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x3A, 1, 4, prefixName + "EQ_RAT41", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

  


            //0x01,0x3B            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$3B";
            row.Cells[(int)SignalColumns.Description].Value = "Equivalence ratio (lambda) 4/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x3B, 1, 4, prefixName + "EQ_RAT42", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";




            //0x01,0x3C           
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$3C";
            row.Cells[(int)SignalColumns.Description].Value = "Catalyst temperature 1/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x3C, 3, 2, prefixName + "CATEMP11", 0.1, -40, -40, 6513.5);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "°C";

            //0x01,0x3D
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$3D";
            row.Cells[(int)SignalColumns.Description].Value = "Catalyst temperature 2/1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x3D, 3, 2, prefixName + "CATEMP21", 0.1, -40, -40, 6513.5);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "°C";

            //0x01,0x3E
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$3E";
            row.Cells[(int)SignalColumns.Description].Value = "Catalyst temperature 1/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x3E, 3, 2, prefixName + "CATEMP12", 0.1, -40, -40, 6513.5);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "°C";


            //0x01,0x3F
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$3F";
            row.Cells[(int)SignalColumns.Description].Value = "Catalyst temperature 2/2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x3F, 3, 2, prefixName + "CATEMP22", 0.1, -40, -40, 6513.5);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "°C";

            //0x01,0x42
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$42";
            row.Cells[(int)SignalColumns.Description].Value = "Control module voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x42, 1, 2, prefixName + "VPWR", 0.001, 0, 0, 65.535);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";

            //0x01,0x43
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$43";
            row.Cells[(int)SignalColumns.Description].Value = "Absolute load value";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x43, 1, 2, prefixName + "LOAD_ABS", 100 / 255.0, 0, 0, 25700);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x44
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$44";
            row.Cells[(int)SignalColumns.Description].Value = "Commanded equivalence ration";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x44, 1, 2, prefixName + "EQ_RAT", 0.0000305, 0, 0, 1.999);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x45
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$45";
            row.Cells[(int)SignalColumns.Description].Value = "Relative throttle position";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x45, 1, 1, prefixName + "TP_R", 100 / 255.0, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x46
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$46";
            row.Cells[(int)SignalColumns.Description].Value = "Ambient air temperature";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x46, 1, 1, prefixName + "AAT", 1, -40, -40, 215);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "°C";

            //0x01,0x47
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$47";
            row.Cells[(int)SignalColumns.Description].Value = "Absolute throttle position B";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x47, 1, 1, prefixName + "TP_B", 100 / 255, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x48
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$48";
            row.Cells[(int)SignalColumns.Description].Value = "Absolute throttle position C";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x48, 1, 1, prefixName + "TP_C", 100 / 255, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x49
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$49";
            row.Cells[(int)SignalColumns.Description].Value = "Accelerator pedal position D";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x49, 1, 1, prefixName + "APP_D", 100 / 255, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x4A
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$4A";
            row.Cells[(int)SignalColumns.Description].Value = "Accelerator pedal position E";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x4A, 1, 1, prefixName + "APP_E", 100 / 255, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x4B
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$4B";
            row.Cells[(int)SignalColumns.Description].Value = "Accelerator pedal position F";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x4B, 1, 1, prefixName + "APP_F", 100 / 255, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x4C
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$4C";
            row.Cells[(int)SignalColumns.Description].Value = "Command throttle actuator control";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x4C, 1, 1, prefixName + "TAC_PCT", 100 / 255, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x4D
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$4D";
            row.Cells[(int)SignalColumns.Description].Value = "Time run by the engine while MIL is activated";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x4D, 1, 2, prefixName + "MIL_TIME", 1, 0, 0, 65535);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "min";

            //0x01,0x4E
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$4E";
            row.Cells[(int)SignalColumns.Description].Value = "Time since DTC cleared";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x4E, 1, 2, prefixName + "CLR_TIME", 1, 0, 0, 65535);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "min";


            //0x01,0x4F
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$4F";
            row.Cells[(int)SignalColumns.Description].Value = "Maximum value for equivalence ratio";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x4F, 1, 4, prefixName + "MVFER", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x4F
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$4F";
            row.Cells[(int)SignalColumns.Description].Value = "Maximum value for oxygen sensor voltage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x4F, 2, 4, prefixName + "MVOSV", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "V";


            //0x01,0x4F
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$4F";
            row.Cells[(int)SignalColumns.Description].Value = "Maximum value for Oxygen Sensor Current";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x4F, 3, 4, prefixName + "MVOSA", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "mA";


            //0x01,0x4F
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$4F";
            row.Cells[(int)SignalColumns.Description].Value = "Maximum value for Intake Manifold Absolute Pressure";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x4F, 4, 4, prefixName + "MVMAP", 10, 0, 0, 2550);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "kPa";


            //0x01,0x50
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$50";
            row.Cells[(int)SignalColumns.Description].Value = "Maximum value for Air Flow Rate";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x50, 1, 4, prefixName + "MVAFR", 10, 0, 0, 2550);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "g/s";

            //0x01,0x51
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$51";
            row.Cells[(int)SignalColumns.Description].Value = "Type of fuel currently being utilized by vehicle";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x51, 1, 1, prefixName + "FUEL_TYP", 1, 0, 0, 255);
            row.Cells[(int)SignalColumns.DataType].Value = "BYTE";
            row.Cells[(int)SignalColumns.Unit].Value = "";

            //0x01,0x52
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$52";
            row.Cells[(int)SignalColumns.Description].Value = "Alcohol fuel percentage";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x52, 1, 1, prefixName + "ALCH_PCT", 100 / 255, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x53
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$53";
            row.Cells[(int)SignalColumns.Description].Value = "Absolute evap system vapour pressure";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x53, 1, 2, prefixName + "EVAP_VPA", 0.005, 0, 0, 327.675);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "kPa";

            //0x01,0x54
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$54";
            row.Cells[(int)SignalColumns.Description].Value = "Evap system vapour pressure";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x54, 1, 2, prefixName + "EVAP_VP", 1, 0, -32767, 32768);
            row.Cells[(int)SignalColumns.DataType].Value = "INT";
            row.Cells[(int)SignalColumns.Unit].Value = "Pa";

            //0x01,0x55
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$55";
            row.Cells[(int)SignalColumns.Description].Value = "Short term secondary O2 sensor fuel trim - Bank 1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x55, 1, 2, prefixName + "STSO2FT1", 100 / 255, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x55
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$55";
            row.Cells[(int)SignalColumns.Description].Value = "Short term secondary O2 sensor fuel trim - Bank 3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x55, 2, 2, prefixName + "STSO2FT3", 100 / 255, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x57
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$57";
            row.Cells[(int)SignalColumns.Description].Value = "Short term secondary O2 sensor fuel trim - Bank 2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x57, 1, 2, prefixName + "STSO2FT2", 100 / 255, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x57
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$57";
            row.Cells[(int)SignalColumns.Description].Value = "Short term secondary O2 sensor fuel trim - Bank 4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x57, 2, 2, prefixName + "STSO2FT4", 100 / 255, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x56
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$56";
            row.Cells[(int)SignalColumns.Description].Value = "Long term secondary O2 sensor fuel trim - Bank 1";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x56, 1, 2, prefixName + "LGSO2FT1", 100 / 255, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x56
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$56";
            row.Cells[(int)SignalColumns.Description].Value = "Long term secondary O2 sensor fuel trim - Bank 3";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x56, 2, 2, prefixName + "LGSO2FT3", 100 / 255, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";
       


            //0x01,0x58
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$58";
            row.Cells[(int)SignalColumns.Description].Value = "Long term secondary O2 sensor fuel trim - Bank 2";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x58, 1, 2, prefixName + "LGSO2FT2", 100 / 255, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";


            //0x01,0x58
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$58";
            row.Cells[(int)SignalColumns.Description].Value = "Long term secondary O2 sensor fuel trim - Bank 4";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x58, 2, 2, prefixName + "LGSO2FT4", 100 / 255, -100, -100, 99.22);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";

            //0x01,0x59
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$59";
            row.Cells[(int)SignalColumns.Description].Value = "Fuel rail pressure (absolute)";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x59, 1, 2, prefixName + "FRP", 10, 0, 0, 655350);
            row.Cells[(int)SignalColumns.DataType].Value = "UINT";
            row.Cells[(int)SignalColumns.Unit].Value = "kPa";

            //0x01,0x5A
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[(int)SignalColumns.SidPid].Value = "$0" + sid.ToString("X1") + "/$5A";
            row.Cells[(int)SignalColumns.Description].Value = "Relative accelerator pedal position";
            row.Cells[(int)SignalColumns.Description].Tag = new ChannelInfo(sid, 0x5A, 1, 1, prefixName + "APP_R", 100 / 255.0, 0, 0, 100);
            row.Cells[(int)SignalColumns.DataType].Value = "USINT";
            row.Cells[(int)SignalColumns.Unit].Value = "%";
        }

        private double GetSamplerate(int index)
        {
            switch (index)
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
            }
            return 10;
        }

        private int GetSamplerateIndex(int rate)
        {
            switch (rate)
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
            }

            return 0;
        }

        private int GetValueType(DataGridViewRow row)
        {
            string dataType = row.Cells[(int)SignalColumns.DataType].Value as string;

            switch (dataType)
            {
                case "BOOL":
                    return (int) SignalDataType.BOOL;

                case "BYTE":
                case "USINT":                    
                    return (int) SignalDataType.USINT;

                case "WORD":
                case "UINT":
                    return (int)SignalDataType.UINT;

                case "DWORD":
                case "UDINT":
                    return (int)SignalDataType.UDINT;

                case "LWORD":
                case "ULINT":
                    return (int)SignalDataType.ULINT;

                case "SINT":
                    return (int)SignalDataType.SINT;

                case "INT":
                    return (int)SignalDataType.INT;

                case "DINT":
                    return (int)SignalDataType.DINT;

                case "LINT":
                    return (int)SignalDataType.LINT;

                case "REAL":
                    return (int)SignalDataType.REAL;

                case "LREAL":
                    return (int)SignalDataType.LREAL;
            }

            return (int)SignalDataType.USINT;
        }


        private DataGridViewRow GetRowByData(int sid, int pid, int sensor, int byteOffset,DataGridView signals)
        {
            foreach (DataGridViewRow row in signals.Rows)
            {
                ChannelInfo chnInfo = (ChannelInfo)row.Cells[(int)SignalColumns.Description].Tag;
                if (chnInfo.PID == pid && chnInfo.SID == sid && chnInfo.ByteOffset == byteOffset && chnInfo.Sensor == sensor)
                    return row;
            }
            return null;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            _rateIndex = meaRate.SelectedIndex;
            SaveSignals(meaSignals, GetSamplerate(meaRate.SelectedIndex));
            SaveSignals(statusSignals, GetSamplerate(statusRate.SelectedIndex));
            SaveSignals(dtcSignals, GetSamplerate(dtcRate.SelectedIndex));
            SaveSignals(oxygenTestSignals, GetSamplerate(oxygenRate.SelectedIndex));
            SaveSignals(freezFrameSignals, GetSamplerate(freezeFrameRate.SelectedIndex));
            SaveVehicleInfo();

            XmlHelper.SetParamNumber(_xmlPS, "oxygenSensorLoc", "uint8_t", meaOxygenSensor.SelectedIndex);
            

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void LoadVehicleInfoData()
        {
            XmlElement xmlVehicleInfos = XmlHelper.GetChildByType(_xmlPS, "Mp.OBD2.VehicleInfos");

            if (xmlVehicleInfos == null)
                return;

            foreach (XmlElement xmlVehicleInfo in xmlVehicleInfos.ChildNodes)
            {
                int sid = (int) XmlHelper.GetParamNumber(xmlVehicleInfo, "sid");
                int pid = (int) XmlHelper.GetParamNumber(xmlVehicleInfo, "pid");

                DataGridViewRow row = GetVehicleInfoRowByData(sid, pid);
                
                if (row == null)
                    continue;

                row.Cells[(int)VehicleInfoCols.Property].Value = XmlHelper.GetParam(xmlVehicleInfo, "property");
                row.Tag = xmlVehicleInfo;
            }
        }

        private DataGridViewRow GetVehicleInfoRowByData(int sid, int pid)
        {
            foreach (DataGridViewRow row in vehicleInfo.Rows)
            {
                ChannelInfo chnInfo = (ChannelInfo)row.Cells[(int)VehicleInfoCols.Description].Tag;
            
                if (chnInfo.SID == sid && pid == chnInfo.PID)
                    return row;
            }

            return null;
        }

        private void SaveVehicleInfo()
        {
            XmlElement xmlVehicleInfos = XmlHelper.GetChildByType(_xmlPS, "Mp.OBD2.VehicleInfos");

            if (xmlVehicleInfos == null)
                xmlVehicleInfos = _doc.CreateXmlObject(_xmlPS, "Mp.OBD2.VehicleInfos","");

            foreach (DataGridViewRow row in vehicleInfo.Rows)
            {
                string property = (string)row.Cells[(int)VehicleInfoCols.Property].Value;

                XmlElement xmlVehicleInfo = null;

                if (property == null)
                    property = "";

                if (property == "" && row.Tag != null)
                { //Remove 
                    xmlVehicleInfo = (XmlElement)row.Tag;
                    _doc.RemoveXmlObject(xmlVehicleInfo);
                    row.Tag = null;
                    continue;
                }
                else if (property != "" && row.Tag != null)
                {//Update
                    xmlVehicleInfo = (XmlElement)row.Tag;
                }
                else if (property != "" && row.Tag == null)
                {//Create                    
                    xmlVehicleInfo = _doc.CreateXmlObject(xmlVehicleInfos, "Mp.OBD2.VehicleInfo", "");
                }
                else
                {
                    continue;
                }

                row.Tag = xmlVehicleInfo;

                ChannelInfo chnInfo = (ChannelInfo)row.Cells[(int)VehicleInfoCols.Description].Tag;
                XmlHelper.SetParam(xmlVehicleInfo, "property","string", property);
                XmlHelper.SetParamNumber(xmlVehicleInfo, "sid", "uint8_t", chnInfo.SID);
                XmlHelper.SetParamNumber(xmlVehicleInfo, "pid", "uint8_t", chnInfo.PID);
            }
        }

        private void SaveSignals(DataGridView signals, double rate)
        {
            foreach (DataGridViewRow row in signals.Rows)
            {
                string name = (string)row.Cells[(int)SignalColumns.Name].Value;

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
                    xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.OBD2.Sig");
                }
                else
                {
                    continue;
                }

                ChannelInfo chnInfo = (ChannelInfo)row.Cells[(int)SignalColumns.Description].Tag;
                XmlHelper.SetParam(xmlSignal, "name", "string", (string)row.Cells[(int)SignalColumns.Name].Value);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[(int)SignalColumns.Unit].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[(int)SignalColumns.Comment].Value);
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", rate);
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", GetValueType(row));
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", chnInfo.Min);
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", chnInfo.Max);
                XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", 0);

                XmlElement xmlScaling = XmlHelper.GetChildByType(xmlSignal, "Mp.Scaling");

                if (xmlScaling == null)
                    xmlScaling = _doc.CreateXmlObject(xmlSignal, "Mp.Scaling", "Mp.Scaling.FactorOffset");

                XmlHelper.SetParamDouble(xmlScaling, "factor", "double", chnInfo.Factor);
                XmlHelper.SetParamDouble(xmlScaling, "offset", "double", chnInfo.Offset);

                XmlHelper.SetParamNumber(xmlSignal, "pid", "uint8_t", chnInfo.PID);
                XmlHelper.SetParamNumber(xmlSignal, "sid", "uint8_t", chnInfo.SID);
                XmlHelper.SetParamNumber(xmlSignal, "sensor", "int8_t", chnInfo.Sensor);
                XmlHelper.SetParamNumber(xmlSignal, "byteOffset", "uint8_t", chnInfo.ByteOffset);
                XmlHelper.SetParamNumber(xmlSignal, "totalDataSize", "uint8_t", chnInfo.TotalDataSize);
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void activateSelectedSignalsWithDefaultNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView signals = null;
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    signals = meaSignals;
                break;
                case 1:
                    signals = dtcSignals;
                break;
                case 2:
                    return;
                case 3:
                    signals = statusSignals;
                break;
                case 4:
                    signals = oxygenTestSignals;
                break;
                case 5:
                    signals = freezFrameSignals;
                break;
                default:
                    return;
            }

            foreach (DataGridViewCell cell in signals.SelectedCells)
            {
                if (cell.RowIndex == -1)
                    continue;

                DataGridViewRow row = signals.Rows[cell.RowIndex];
                ChannelInfo chnInfo = (ChannelInfo)row.Cells[(int)SignalColumns.Description].Tag;
                row.Cells[(int)SignalColumns.Name].Value = chnInfo.DefName;
            }
        }

        private void deactivateSelectedSignalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView signals = null;
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    signals = meaSignals;
                    break;
                case 1:
                    signals = dtcSignals;
                    break;
                case 2:
                    return;
                case 3:
                    signals = statusSignals;
                    break;
                case 4:
                    signals = oxygenTestSignals;
                    break;
                case 5:
                    signals = freezFrameSignals;
                    break;
                default:
                    return;
            }
            foreach (DataGridViewCell cell in signals.SelectedCells)
            {
                if (cell.RowIndex == -1)
                    continue;

                DataGridViewRow row = signals.Rows[cell.RowIndex];
                ChannelInfo chnInfo = (ChannelInfo)row.Cells[(int)SignalColumns.Description].Tag;
                row.Cells[(int)SignalColumns.Name].Value = "";
            }
        }

        private void PortPropDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Mp.Utils.FormStateHandler.Save(this, Document.RegistryKey + "PortPropDlg");
        }

        private void vehicleInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != (int)VehicleInfoCols.AttachProp)
                return;
            
            if (e.RowIndex == -1)
                return;

            SchemePropertyDlg dlg = new SchemePropertyDlg(_doc);
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if( dlg.SelectedProperties.Count != 1)
                return;

            DataGridViewRow row = vehicleInfo.Rows[e.RowIndex];

            if (_doc.GetPropertyType(dlg.SelectedProperties[0]) != "STRING")
            {
                MessageBox.Show("The property must have a STRING type.", "MeaProcess", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            row.Cells[(int)VehicleInfoCols.Property].Value = dlg.SelectedProperties[0];
        }

        private void UpdateOxygenSensSignals()
        {
            foreach (DataGridViewRow row in oxygenTestSignals.Rows)
            {
                ChannelInfo info = (ChannelInfo) row.Cells[(int)SignalColumns.Description].Tag;

                if (meaOxygenSensor.SelectedIndex == 0)
                {//$13

                    switch (info.Sensor)
                    {
                        case 0x01:
                            switch (info.PID)
                            {
                                case 0x01:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Rich to lean sensor threshold voltage";
                                break;
                                
                                case 0x02:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Lean to rich sensor threshold voltage";
                                break;
                                
                                case 0x03:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Low sensor voltage for switch time calculation";
                                break;
                                
                                case 0x04:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) High sensor voltage for switch time calculation";
                                break;
                                case 0x05:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Rich to lean sensor switch time";
                                break;
                                case 0x06:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Lean to rich sensor switch time";
                                break;
                                case 0x07:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Minimum sensor voltage for test cycle";
                                break;
                                case 0x08:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Maximum sensor voltage for test cycle";
                                break;
                                case 0x09:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Time between sensor transitions";
                                break;
                                case 0x0A:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Sensor period";
                                break;
                            }
                       break;
                       case 0x02:
                           switch (info.PID)
                           {
                               case 0x01:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/2) Rich to lean sensor threshold voltage";
                                   break;

                               case 0x02:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/2) Lean to rich sensor threshold voltage";
                                   break;

                               case 0x03:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/2) Low sensor voltage for switch time calculation";
                                   break;

                               case 0x04:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/2) High sensor voltage for switch time calculation";
                                   break;
                               case 0x05:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/2) Rich to lean sensor switch time";
                                   break;
                               case 0x06:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/2) Lean to rich sensor switch time";
                                   break;
                               case 0x07:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/2) Minimum sensor voltage for test cycle";
                                   break;
                               case 0x08:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/2) Maximum sensor voltage for test cycle";
                                   break;
                               case 0x09:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/2) Time between sensor transitions";
                                   break;
                               case 0x0A:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/2) Sensor period";
                                   break;
                           }
                       break;
                       case 0x04:
                           switch (info.PID)
                           {
                               case 0x01:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/3) Rich to lean sensor threshold voltage";
                                   break;

                               case 0x02:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/3) Lean to rich sensor threshold voltage";
                                   break;

                               case 0x03:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/3) Low sensor voltage for switch time calculation";
                                   break;

                               case 0x04:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/3) High sensor voltage for switch time calculation";
                                   break;
                               case 0x05:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/3) Rich to lean sensor switch time";
                                   break;
                               case 0x06:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/3) Lean to rich sensor switch time";
                                   break;
                               case 0x07:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/3) Minimum sensor voltage for test cycle";
                                   break;
                               case 0x08:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/3) Maximum sensor voltage for test cycle";
                                   break;
                               case 0x09:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/3) Time between sensor transitions";
                                   break;
                               case 0x0A:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/3) Sensor period";
                                   break;
                           }
                       break;
                       case 0x08:
                           switch (info.PID)
                           {
                               case 0x01:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/4) Rich to lean sensor threshold voltage";
                                   break;

                               case 0x02:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/4) Lean to rich sensor threshold voltage";
                                   break;

                               case 0x03:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/4) Low sensor voltage for switch time calculation";
                                   break;

                               case 0x04:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/4) High sensor voltage for switch time calculation";
                                   break;
                               case 0x05:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/4) Rich to lean sensor switch time";
                                   break;
                               case 0x06:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/4) Lean to rich sensor switch time";
                                   break;
                               case 0x07:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/4) Minimum sensor voltage for test cycle";
                                   break;
                               case 0x08:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/4) Maximum sensor voltage for test cycle";
                                   break;
                               case 0x09:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/4) Time between sensor transitions";
                                   break;
                               case 0x0A:
                                   row.Cells[(int)SignalColumns.Description].Value = "(1/4) Sensor period";
                                   break;
                           }
                       break;
                       case 0x10:
                           switch (info.PID)
                           {
                               case 0x01:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/1) Rich to lean sensor threshold voltage";
                                   break;

                               case 0x02:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/1) Lean to rich sensor threshold voltage";
                                   break;

                               case 0x03:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/1) Low sensor voltage for switch time calculation";
                                   break;

                               case 0x04:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/1) High sensor voltage for switch time calculation";
                                   break;
                               case 0x05:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/1) Rich to lean sensor switch time";
                                   break;
                               case 0x06:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/1) Lean to rich sensor switch time";
                                   break;
                               case 0x07:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/1) Minimum sensor voltage for test cycle";
                                   break;
                               case 0x08:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/1) Maximum sensor voltage for test cycle";
                                   break;
                               case 0x09:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/1) Time between sensor transitions";
                                   break;
                               case 0x0A:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/1) Sensor period";
                                   break;
                           }
                       break;
                       case 0x20:
                           switch (info.PID)
                           {
                               case 0x01:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/2) Rich to lean sensor threshold voltage";
                                   break;

                               case 0x02:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/2) Lean to rich sensor threshold voltage";
                                   break;

                               case 0x03:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/2) Low sensor voltage for switch time calculation";
                                   break;

                               case 0x04:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/2) High sensor voltage for switch time calculation";
                                   break;
                               case 0x05:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/2) Rich to lean sensor switch time";
                                   break;
                               case 0x06:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/2) Lean to rich sensor switch time";
                                   break;
                               case 0x07:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/2) Minimum sensor voltage for test cycle";
                                   break;
                               case 0x08:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/2) Maximum sensor voltage for test cycle";
                                   break;
                               case 0x09:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/2) Time between sensor transitions";
                                   break;
                               case 0x0A:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/2) Sensor period";
                                   break;
                           }
                       break;
                       case 0x40:
                           switch (info.PID)
                           {
                               case 0x01:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/3) Rich to lean sensor threshold voltage";
                                   break;

                               case 0x02:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/3) Lean to rich sensor threshold voltage";
                                   break;

                               case 0x03:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/3) Low sensor voltage for switch time calculation";
                                   break;

                               case 0x04:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/3) High sensor voltage for switch time calculation";
                                   break;
                               case 0x05:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/3) Rich to lean sensor switch time";
                                   break;
                               case 0x06:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/3) Lean to rich sensor switch time";
                                   break;
                               case 0x07:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/3) Minimum sensor voltage for test cycle";
                                   break;
                               case 0x08:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/3) Maximum sensor voltage for test cycle";
                                   break;
                               case 0x09:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/3) Time between sensor transitions";
                                   break;
                               case 0x0A:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/3) Sensor period";
                                   break;
                           }
                       break;
                       case 0x80:
                           switch (info.PID)
                           {
                               case 0x01:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/4) Rich to lean sensor threshold voltage";
                                   break;

                               case 0x02:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/4) Lean to rich sensor threshold voltage";
                                   break;

                               case 0x03:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/4) Low sensor voltage for switch time calculation";
                                   break;

                               case 0x04:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/4) High sensor voltage for switch time calculation";
                                   break;
                               case 0x05:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/4) Rich to lean sensor switch time";
                                   break;
                               case 0x06:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/4) Lean to rich sensor switch time";
                                   break;
                               case 0x07:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/4) Minimum sensor voltage for test cycle";
                                   break;
                               case 0x08:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/4) Maximum sensor voltage for test cycle";
                                   break;
                               case 0x09:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/4) Time between sensor transitions";
                                   break;
                               case 0x0A:
                                   row.Cells[(int)SignalColumns.Description].Value = "(2/4) Sensor period";
                                   break;
                           }
                       break;
                    }                      
                }
                else
                {//$1D

                    switch (info.Sensor)
                    {
                        case 0x01:
                            switch (info.PID)
                            {
                                case 0x01:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Rich to lean sensor threshold voltage";
                                    break;

                                case 0x02:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Lean to rich sensor threshold voltage";
                                    break;

                                case 0x03:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Low sensor voltage for switch time calculation";
                                    break;

                                case 0x04:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) High sensor voltage for switch time calculation";
                                    break;
                                case 0x05:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Rich to lean sensor switch time";
                                    break;
                                case 0x06:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Lean to rich sensor switch time";
                                    break;
                                case 0x07:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Minimum sensor voltage for test cycle";
                                    break;
                                case 0x08:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Maximum sensor voltage for test cycle";
                                    break;
                                case 0x09:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Time between sensor transitions";
                                    break;
                                case 0x0A:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/1) Sensor period";
                                    break;
                            }
                            break;
                        case 0x02:
                            switch (info.PID)
                            {
                                case 0x01:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/2) Rich to lean sensor threshold voltage";
                                    break;

                                case 0x02:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/2) Lean to rich sensor threshold voltage";
                                    break;

                                case 0x03:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/2) Low sensor voltage for switch time calculation";
                                    break;

                                case 0x04:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/2) High sensor voltage for switch time calculation";
                                    break;
                                case 0x05:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/2) Rich to lean sensor switch time";
                                    break;
                                case 0x06:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/2) Lean to rich sensor switch time";
                                    break;
                                case 0x07:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/2) Minimum sensor voltage for test cycle";
                                    break;
                                case 0x08:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/2) Maximum sensor voltage for test cycle";
                                    break;
                                case 0x09:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/2) Time between sensor transitions";
                                    break;
                                case 0x0A:
                                    row.Cells[(int)SignalColumns.Description].Value = "(1/2) Sensor period";
                                    break;
                            }
                            break;
                        case 0x04:
                            switch (info.PID)
                            {
                                case 0x01:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/1) Rich to lean sensor threshold voltage";
                                    break;

                                case 0x02:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/1) Lean to rich sensor threshold voltage";
                                    break;

                                case 0x03:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/1) Low sensor voltage for switch time calculation";
                                    break;

                                case 0x04:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/1) High sensor voltage for switch time calculation";
                                    break;
                                case 0x05:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/1) Rich to lean sensor switch time";
                                    break;
                                case 0x06:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/1) Lean to rich sensor switch time";
                                    break;
                                case 0x07:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/1) Minimum sensor voltage for test cycle";
                                    break;
                                case 0x08:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/1) Maximum sensor voltage for test cycle";
                                    break;
                                case 0x09:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/1) Time between sensor transitions";
                                    break;
                                case 0x0A:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/1) Sensor period";
                                    break;
                            }
                            break;
                        case 0x08:
                            switch (info.PID)
                            {
                                case 0x01:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/2) Rich to lean sensor threshold voltage";
                                    break;

                                case 0x02:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/2) Lean to rich sensor threshold voltage";
                                    break;

                                case 0x03:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/2) Low sensor voltage for switch time calculation";
                                    break;

                                case 0x04:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/2) High sensor voltage for switch time calculation";
                                    break;
                                case 0x05:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/2) Rich to lean sensor switch time";
                                    break;
                                case 0x06:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/2) Lean to rich sensor switch time";
                                    break;
                                case 0x07:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/2) Minimum sensor voltage for test cycle";
                                    break;
                                case 0x08:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/2) Maximum sensor voltage for test cycle";
                                    break;
                                case 0x09:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/2) Time between sensor transitions";
                                    break;
                                case 0x0A:
                                    row.Cells[(int)SignalColumns.Description].Value = "(2/2) Sensor period";
                                    break;
                            }
                            break;
                        case 0x10:
                            switch (info.PID)
                            {
                                case 0x01:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/1) Rich to lean sensor threshold voltage";
                                    break;

                                case 0x02:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/1) Lean to rich sensor threshold voltage";
                                    break;

                                case 0x03:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/1) Low sensor voltage for switch time calculation";
                                    break;

                                case 0x04:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/1) High sensor voltage for switch time calculation";
                                    break;
                                case 0x05:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/1) Rich to lean sensor switch time";
                                    break;
                                case 0x06:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/1) Lean to rich sensor switch time";
                                    break;
                                case 0x07:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/1) Minimum sensor voltage for test cycle";
                                    break;
                                case 0x08:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/1) Maximum sensor voltage for test cycle";
                                    break;
                                case 0x09:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/1) Time between sensor transitions";
                                    break;
                                case 0x0A:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/1) Sensor period";
                                    break;
                            }
                            break;
                        case 0x20:
                            switch (info.PID)
                            {
                                case 0x01:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/2) Rich to lean sensor threshold voltage";
                                    break;

                                case 0x02:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/2) Lean to rich sensor threshold voltage";
                                    break;

                                case 0x03:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/2) Low sensor voltage for switch time calculation";
                                    break;

                                case 0x04:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/2) High sensor voltage for switch time calculation";
                                    break;
                                case 0x05:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/2) Rich to lean sensor switch time";
                                    break;
                                case 0x06:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/2) Lean to rich sensor switch time";
                                    break;
                                case 0x07:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/2) Minimum sensor voltage for test cycle";
                                    break;
                                case 0x08:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/2) Maximum sensor voltage for test cycle";
                                    break;
                                case 0x09:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/2) Time between sensor transitions";
                                    break;
                                case 0x0A:
                                    row.Cells[(int)SignalColumns.Description].Value = "(3/2) Sensor period";
                                    break;
                            }
                            break;
                        case 0x40:
                            switch (info.PID)
                            {
                                case 0x01:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/1) Rich to lean sensor threshold voltage";
                                    break;

                                case 0x02:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/1) Lean to rich sensor threshold voltage";
                                    break;

                                case 0x03:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/1) Low sensor voltage for switch time calculation";
                                    break;

                                case 0x04:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/1) High sensor voltage for switch time calculation";
                                    break;
                                case 0x05:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/1) Rich to lean sensor switch time";
                                    break;
                                case 0x06:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/1) Lean to rich sensor switch time";
                                    break;
                                case 0x07:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/1) Minimum sensor voltage for test cycle";
                                    break;
                                case 0x08:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/1) Maximum sensor voltage for test cycle";
                                    break;
                                case 0x09:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/1) Time between sensor transitions";
                                    break;
                                case 0x0A:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/1) Sensor period";
                                    break;
                            }
                            break;
                        case 0x80:
                            switch (info.PID)
                            {
                                case 0x01:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/2) Rich to lean sensor threshold voltage";
                                    break;

                                case 0x02:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/2) Lean to rich sensor threshold voltage";
                                    break;

                                case 0x03:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/2) Low sensor voltage for switch time calculation";
                                    break;

                                case 0x04:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/2) High sensor voltage for switch time calculation";
                                    break;
                                case 0x05:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/2) Rich to lean sensor switch time";
                                    break;
                                case 0x06:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/2) Lean to rich sensor switch time";
                                    break;
                                case 0x07:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/2) Minimum sensor voltage for test cycle";
                                    break;
                                case 0x08:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/2) Maximum sensor voltage for test cycle";
                                    break;
                                case 0x09:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/2) Time between sensor transitions";
                                    break;
                                case 0x0A:
                                    row.Cells[(int)SignalColumns.Description].Value = "(4/2) Sensor period";
                                    break;
                            }
                            break;
                    }         
                }
            }
        }

        private void meaOxygenSensor_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMeaChannels(meaSignals);
            UpdateMeaChannels(freezFrameSignals);
            UpdateOxygenSensSignals();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1000);
        }

        private void PortPropDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
