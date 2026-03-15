using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Mp.Visual.OBD2
{
    public partial class OBD2StatusView : UserControl
    {

        public enum StatusType
        {
            None = 0,
            FuelSystemStatus ,
            CommandedSecondaryAirStatus,
            LocationOxygenSensors13,
            LocationOxygenSensors1D,
            AuxiliaryInputStatus,
            MonitorStatusDrivingCycle,
            FuelType,
        }

        private Hashtable _sigToItemMap = new Hashtable();
        private ImageList _imageList = new ImageList();

        public OBD2StatusView()
        {

            InitializeComponent();
            _imageList.Images.Add(Resource.Signal);
            view.SmallImageList = _imageList;
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        public void AddSignal(string name, uint sigid, StatusType statusType)
        {
            string[] data = new string[3];
            data[0] = name;
            data[1] = "";

            switch (statusType)
            {
                case StatusType.FuelSystemStatus:
                {                                                            
                    data[1] = "Fuel system status";

                    ListViewItem item = new ListViewItem(data,0);
                    item.Tag = statusType;

                    view.Items.Add(item);
                    _sigToItemMap.Add(sigid, item);
                }
                break;

                case StatusType.CommandedSecondaryAirStatus:
                {
                    data[1] = "Commanded secondary air status";

                    ListViewItem item = new ListViewItem(data,0);
                    item.Tag = statusType;

                    view.Items.Add(item);
                    _sigToItemMap.Add(sigid, item);
                }
                break;
                case StatusType.LocationOxygenSensors13:
                {
                    data[1] = "Location of oxygen sensors $13";
                    ListViewItem item = new ListViewItem(data,0);
                    item.Tag = statusType;

                    view.Items.Add(item);
                    _sigToItemMap.Add(sigid, item);
                }
                break;

                case StatusType.LocationOxygenSensors1D:
                {
                    data[1] = "Location of oxygen sensors $1D";
                    ListViewItem item = new ListViewItem(data,0);
                    item.Tag = statusType;

                    view.Items.Add(item);
                    _sigToItemMap.Add(sigid, item);
                }
                break;
                case StatusType.AuxiliaryInputStatus:
                {
                    data[1] = "Auxiliar input status";
                    ListViewItem item = new ListViewItem(data,0);
                    item.Tag = statusType;

                    view.Items.Add(item);
                    _sigToItemMap.Add(sigid, item);
                }
                break;
                case StatusType.MonitorStatusDrivingCycle:
                {
                    data[1] = "Misfire monitoring";
                    ListViewItem item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);
                    _sigToItemMap.Add(sigid, item);

                    data[1] = "Fuel system monitoring";
                    item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);

                    data[1] = "Comprehensive component monitoring";
                    item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);

                    data[1] = "Catalyst monitoring";
                    item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);

                    data[1] = "Heated catalyst monitoring";
                    item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);

                    data[1] = "Evaporative system monitoring";
                    item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);

                    data[1] = "Secondary air system monitoring";
                    item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);

                    data[1] = "A/C system refrigerant monitoring";
                    item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);

                    data[1] = "Oxygen sensor monitoring";
                    item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);

                    data[1] = "Oxygen sensor heater monitoring";
                    item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);

                    data[1] = "EGR system monitoring";
                    item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);
                }
                break;
                case StatusType.FuelType:
                {
                    data[1] = "Fuel type";
                    ListViewItem item = new ListViewItem(data,0);
                    item.Tag = statusType;
                    view.Items.Add(item);
                    _sigToItemMap.Add(sigid, item);                    
                }
                break;
            }
        }

        public void InitDone()
        {
            int count = 0;
            view.ForeColor = Color.Black;

            foreach (ListViewItem item in view.Items)
            {
                if ((count % 2) == 0)
                    item.BackColor = Color.Beige;
                else
                    item.BackColor = Color.White;

                item.SubItems[2].Font = new Font(item.Font, FontStyle.Bold);
                count++;
            }
        }

        public void SetValue(uint sigid, double value)
        {
            if (!_sigToItemMap.Contains(sigid))
                return;

           //view.SuspendLayout();

            ListViewItem item = (ListViewItem) _sigToItemMap[sigid];

            StatusType statusType = (StatusType)item.Tag;

            switch (statusType)
            {
                case StatusType.FuelSystemStatus:
                {
                    byte bvalue = (byte)value;

                    if ((bvalue & 1) != 0)
                        item.SubItems[2].Text = "Open loop - has not yet satisfied conditions to go closed loop";
                    else if((bvalue & 2) != 0)
                        item.SubItems[2].Text = "Closed loop - using oxygen sensor(s) as feedback for fuel control";
                    else if((bvalue & 4) != 0)
                        item.SubItems[2].Text = "Open loop - due to driving conditions";
                    else if ((bvalue & 8) != 0)
                        item.SubItems[2].Text = "Open loop - due to detected system fault";
                    else if ((bvalue & 16) != 0)
                        item.SubItems[2].Text = "Closed loop - but fault with at least one oxygen sensor";
                }
                break;
                case StatusType.CommandedSecondaryAirStatus:
                {
                    byte bvalue = (byte)value;

                    if ((bvalue & 1) != 0)
                        item.SubItems[2].Text = "Upstream of first catalytic converter";
                    else if ((bvalue & 2) != 0)
                        item.SubItems[2].Text = "Downstream of first catalytic converter inlet";
                    else if ((bvalue & 4) != 0)
                        item.SubItems[2].Text = "Atmosphere /off";
                }
                break;
                case StatusType.LocationOxygenSensors13:
                {
                    byte bvalue = (byte)value;
                    string location = "";
                    
                    if ((bvalue & 1) != 0)
                        location += "Bank 1 - Sensor 1 # ";
                    
                    if ((bvalue & 2) != 0)
                        location += "Bank 1 - Sensor 2 # ";

                    if ((bvalue & 4) != 0)
                        location += "Bank 1 - Sensor 3 # ";

                    if ((bvalue & 8) != 0)
                        location += "Bank 1 - Sensor 4 # ";

                    if ((bvalue & 16) != 0)
                        location += "Bank 2 - Sensor 1 # ";

                    if ((bvalue & 32) != 0)
                        location += "Bank 2 - Sensor 2 # ";

                    if ((bvalue & 64) != 0)
                        location += "Bank 2 - Sensor 3 # ";

                    if ((bvalue & 128) != 0)
                        location += "Bank 2 - Sensor 4 # ";

                    if (location != "")
                        location = location.Remove(location.Length - 2,2);

                    item.SubItems[2].Text = location;
                }
                break;

                case StatusType.LocationOxygenSensors1D:
                {
                    byte bvalue = (byte)value;
                    string location = "";

                    if ((bvalue & 1) != 0)
                        location += "Bank 1 - Sensor 1 # ";

                    if ((bvalue & 2) != 0)
                        location += "Bank 1 - Sensor 2 # ";

                    if ((bvalue & 4) != 0)
                        location += "Bank 2 - Sensor 1 # ";

                    if ((bvalue & 8) != 0)
                        location += "Bank 2 - Sensor 2 # ";

                    if ((bvalue & 16) != 0)
                        location += "Bank 3 - Sensor 1 # ";

                    if ((bvalue & 32) != 0)
                        location += "Bank 3 - Sensor 2 # ";

                    if ((bvalue & 64) != 0)
                        location += "Bank 4 - Sensor 1 # ";

                    if ((bvalue & 128) != 0)
                        location += "Bank 4 - Sensor 2 # ";

                    if (location != "")
                        location = location.Remove(location.Length - 2, 2);

                    item.SubItems[2].Text = location;
                }
                break;
                case StatusType.AuxiliaryInputStatus:
                {
                    byte bvalue = (byte)value;

                    if( (bvalue & 0x01) != 0)
                        item.SubItems[2].Text = "PTO active (ON)";
                    else
                        item.SubItems[2].Text = "PTO not active (OFF)";
                }
                break;
                case StatusType.MonitorStatusDrivingCycle:
                {
                    uint uivalue = (uint)value;
                    string misfire = "";
                    string fuelSystem = "";
                    string compComp = "";
                    
                    if ((uivalue & 0x0100) != 0)
                        misfire += "Enabled / ";
                    else
                        misfire += "Disabled / ";

                    if ((uivalue & 0x0200) != 0)
                        fuelSystem += "Enabled / ";
                    else
                        fuelSystem += "Disabled / ";

                    if ((uivalue & 0x0400) != 0)
                        compComp += "Enabled / ";
                    else
                        compComp += "Disabled / ";

                    if ((uivalue & 0x0800) != 0)
                        misfire += "Not Complete";
                    else
                        misfire += "Complete";

                    if ((uivalue & 0x1000) != 0)
                        fuelSystem += "Not Complete";
                    else
                        fuelSystem += "Complete";

                    if ((uivalue & 0x2000) != 0)
                        compComp += "Not Complete";
                    else
                        compComp += "Complete";

                    item.SubItems[2].Text = misfire;
                    
                    
                    item = view.Items[item.Index + 1];
                    item.SubItems[2].Text = fuelSystem;

                    item = view.Items[item.Index + 1];
                    item.SubItems[2].Text = compComp;
                    

                    string catalyst = "";
                    string heatedCatalyst = "";
                    string evaporativeSystem = "";
                    string secAirSystem = "";                    
                    string acSystem = "";
                    string oxigenSensor = "";
                    string oxigenHeaterSensor = "";
                    string egrSystem = "";

                    if ((uivalue & 0x010000) != 0)
                        catalyst += "Enabled / ";
                    else
                        catalyst += "Disabled / ";

                    if ((uivalue & 0x020000) != 0)
                        heatedCatalyst += "Enabled / ";
                    else
                        heatedCatalyst += "Disabled / ";

                    if ((uivalue & 0x040000) != 0)
                        secAirSystem += "Enabled / ";
                    else
                        secAirSystem += "Disabled / ";

                    if ((uivalue & 0x080000) != 0)
                        evaporativeSystem += "Enabled / ";
                    else
                        evaporativeSystem += "Disabled / ";

                    if ((uivalue & 0x100000) != 0)
                        acSystem += "Enabled / ";
                    else
                        acSystem += "Disabled / ";

                    if ((uivalue & 0x200000) != 0)
                        oxigenSensor += "Enabled / ";
                    else
                        oxigenSensor += "Disabled / ";

                    if ((uivalue & 0x40000) != 0)
                        oxigenHeaterSensor += "Enabled / ";
                    else
                        oxigenHeaterSensor += "Disabled / ";

                    if ((uivalue & 0x800000) != 0)
                        egrSystem += "Enabled / ";
                    else
                        egrSystem += "Disabled / ";


                    if ((uivalue & 0x01000000) != 0)
                        catalyst += "Not Complete";
                    else
                        catalyst += "Complete";

                    if ((uivalue & 0x02000000) != 0)
                        heatedCatalyst += "Not Complete";
                    else
                        heatedCatalyst += "Complete";

                    if ((uivalue & 0x04000000) != 0)
                        evaporativeSystem += "Not Complete";
                    else
                        evaporativeSystem += "Complete";

                    if ((uivalue & 0x08000000) != 0)
                        secAirSystem += "Not Complete";
                    else
                        secAirSystem += "Complete";

                    if ((uivalue & 0x10000000) != 0)
                        acSystem += "Not Complete";
                    else
                        acSystem += "Complete";

                    if ((uivalue & 0x20000000) != 0)
                        oxigenSensor += "Not Complete";
                    else
                        oxigenSensor += "Complete";

                    if ((uivalue & 0x40000000) != 0)
                        oxigenHeaterSensor += "Not Complete";
                    else
                        oxigenHeaterSensor += "Complete";

                    if ((uivalue & 0x80000000) != 0)
                        egrSystem += "Not Complete";
                    else
                        egrSystem += "Complete";

                    item = view.Items[item.Index + 1];
                    item.SubItems[2].Text = catalyst;

                    item = view.Items[item.Index + 1];
                    item.SubItems[2].Text = heatedCatalyst;

                    item = view.Items[item.Index + 1];
                    item.SubItems[2].Text = evaporativeSystem;
                    
                    item = view.Items[item.Index + 1];
                    item.SubItems[2].Text = secAirSystem;
                    
                    item = view.Items[item.Index + 1];
                    item.SubItems[2].Text = acSystem;

                    item = view.Items[item.Index + 1];
                    item.SubItems[2].Text = oxigenSensor;

                    item = view.Items[item.Index + 1];
                    item.SubItems[2].Text = oxigenHeaterSensor;

                    item = view.Items[item.Index + 1];
                    item.SubItems[2].Text = egrSystem;

                }
                break;
                case StatusType.FuelType:
                    byte ftype = (byte) value;
                    switch(ftype)
                    {
                        case 0x01:
                            item.SubItems[2].Text = "Gasoline/petrol";
                        break;
                        case 0x02:
                            item.SubItems[2].Text = "Methanol";
                        break;
                        case 0x03:
                            item.SubItems[2].Text = "Ethanol";
                        break;
                        case 0x04:
                            item.SubItems[2].Text = "Diesel";
                        break;
                        case 0x05:
                            item.SubItems[2].Text = "Liquefied petroleum gas (LPG)";
                        break;
                        case 0x06:
                            item.SubItems[2].Text = "Compressed natural gas (CNG)";
                        break;

                        case 0x07:
                            item.SubItems[2].Text = "Propane";
                        break;

                        case 0x08:
                            item.SubItems[2].Text = "Battery/electric";
                        break;
                        case 0x09:
                            item.SubItems[2].Text = "Bi-fuel vehicle using gasoline";
                        break;
                        case 0x0A:
                            item.SubItems[2].Text = "Bi-fuel vehicle using methanol";
                        break;
                        case 0x0B:
                            item.SubItems[2].Text = "Bi-fuel vehicle using ethanol";
                        break;
                        case 0x0C:
                            item.SubItems[2].Text = "Bi-fuel vehicle using LPG";
                        break;
                        case 0x0D:
                            item.SubItems[2].Text = "Bi-fuel vehicle using CNG";
                        break;
                        case 0x0E:
                            item.SubItems[2].Text = "Bi-fuel vehicle using propane";
                        break;
                        case 0x0F:
                            item.SubItems[2].Text = "Bi-fuel vehicle using battery";
                        break;

                    }
                break;
            }

            //view.ResumeLayout();
            view.Invalidate();
        }

    }
}
