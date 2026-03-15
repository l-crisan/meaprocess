using System.Windows.Forms;

namespace Mp.Drv.CAN
{
    public class CANDriverHelper
    {
        public static void SelectDriver(ComboBox driver, string driverType)
        {
            int index = 0;
            foreach (DriverItem item in driver.Items)
            {
                if (item.Lib == driverType)
                {
                    driver.SelectedIndex = index;
                    break;
                }
                index++;
            }

            if(driver.SelectedIndex == -1)
                driver.SelectedIndex = 0;
        }

        public static void SelectDevice(ComboBox device, string devType)
        {
            int index = 0;
            foreach (DeviceItem item in device.Items)
            {
                if (item.ID == devType)
                {
                    device.SelectedIndex = index;
                    break;
                }
                index++;
            }
        }

        public static void SetupDriver(ComboBox driver, Mp.Scheme.Sdk.Module runtimeType)
        {
            driver.Items.Clear();

            if (runtimeType.SupportWindows)
            {
//                driver.Items.Add(new DriverItem("Demo", "mps-can-demo"));
//                driver.Items.Add(new DriverItem("Vector", "mps-can-vector"));
                driver.Items.Add(new DriverItem("Peak", "mps-can-peak"));
//                driver.Items.Add(new DriverItem("Softing", "mps-can-softing"));
//                driver.Items.Add(new DriverItem("Kvaser", "mps-can-kvaser"));
                driver.Items.Add(new DriverItem("Systec", "mps-can-systec"));
                return;
            }
            
            
            if(runtimeType.SupportLinux)
            {
                driver.Items.Add(new DriverItem("Demo", "mps-can-demo"));
                driver.Items.Add(new DriverItem("SocketCAN", "mps-can-socket"));

                return;
            }
        }

        public static void SetupDevices(ComboBox device, string driverID, Mp.Scheme.Sdk.Module runtimeType)
        {
            device.Items.Clear();

            if (runtimeType.SupportWindows)
            {
                switch (driverID)
                {
                    case "mps-can-demo": //Demo
                        device.Items.Add(new DeviceItem("Demo", "0"));
                        break;

                    case "mps-can-vector": //Vector
                        device.Items.Add(new DeviceItem("Virtual", "1"));
                        device.Items.Add(new DeviceItem("CANCARDX", "2"));
                        device.Items.Add(new DeviceItem("CANPARI", "3"));
                        device.Items.Add(new DeviceItem("CANAC2", "5"));
                        device.Items.Add(new DeviceItem("CANAC2PCI", "6"));
                        device.Items.Add(new DeviceItem("CANCARDY", "12"));
                        device.Items.Add(new DeviceItem("CANCARDXL", "15"));
                        device.Items.Add(new DeviceItem("CANCARD2", "17"));
                        device.Items.Add(new DeviceItem("EDICCARD", "19"));
                        device.Items.Add(new DeviceItem("CANCASEXL", "21"));
                        device.Items.Add(new DeviceItem("CANBOARDXL", "25"));
                        device.Items.Add(new DeviceItem("CANBOARDXL COMPACT", "27"));
                        break;

                    case "mps-can-peak": //Peak
                        {
                            /*
                            device.Items.Add(new DeviceItem("ISA 1", 0x21));
                            device.Items.Add(new DeviceItem("ISA 2", 0x22));
                            device.Items.Add(new DeviceItem("ISA 3", 0x23));
                            device.Items.Add(new DeviceItem("ISA 4", 0x24));
                            device.Items.Add(new DeviceItem("ISA 5", 0x25));
                            device.Items.Add(new DeviceItem("ISA 6", 0x26));
                            device.Items.Add(new DeviceItem("ISA 7", 0x27));
                            device.Items.Add(new DeviceItem("ISA 8", 0x28));

                            device.Items.Add(new DeviceItem("DNG BUS 1", 0x31));
                            */
                            int id = 0x41;
                            device.Items.Add(new DeviceItem("PCI BUS 1", id.ToString()));

                            id = 0x42;
                            device.Items.Add(new DeviceItem("PCI BUS 2", id.ToString()));

                            id = 0x43;
                            device.Items.Add(new DeviceItem("PCI BUS 3", id.ToString()));

                            id = 0x44;
                            device.Items.Add(new DeviceItem("PCI BUS 4", id.ToString()));

                            id = 0x45;
                            device.Items.Add(new DeviceItem("PCI BUS 5", id.ToString()));

                            id = 0x46;
                            device.Items.Add(new DeviceItem("PCI BUS 6", id.ToString()));

                            id = 0x47;
                            device.Items.Add(new DeviceItem("PCI BUS 7", id.ToString()));

                            id = 0x48;
                            device.Items.Add(new DeviceItem("PCI BUS 8", id.ToString()));

                            id = 0x51;
                            device.Items.Add(new DeviceItem("USB BUS 1", id.ToString()));

                            id = 0x52;
                            device.Items.Add(new DeviceItem("USB BUS 2", id.ToString()));

                            id = 0x53;
                            device.Items.Add(new DeviceItem("USB BUS 3", id.ToString()));

                            id = 0x54;
                            device.Items.Add(new DeviceItem("USB BUS 4", id.ToString()));

                            id = 0x55;
                            device.Items.Add(new DeviceItem("USB BUS 5", id.ToString()));

                            id = 0x56;
                            device.Items.Add(new DeviceItem("USB BUS 6", id.ToString()));

                            id = 0x57;
                            device.Items.Add(new DeviceItem("USB BUS 7", id.ToString()));

                            id = 0x58;
                            device.Items.Add(new DeviceItem("USB BUS 8", id.ToString()));

                            //device.Items.Add(new DeviceItem("PCC BUS 1", 0x61));
                            //device.Items.Add(new DeviceItem("PCC BUS 2", 0x62));
                        }
                        break;
                    case "mps-can-softing"://Softing
                        {
                            long id = 0x00000005L;
                            device.Items.Add(new DeviceItem("CANCARD2", id.ToString()));
                            
                            id = 0x00000102L;
                             device.Items.Add(new DeviceItem("EDICCARDC", id.ToString()));

                             id = 0x00000105L;
                             device.Items.Add(new DeviceItem("EDICCARD2", id.ToString()));

                             id = 0x00000007L;
                             device.Items.Add(new DeviceItem("CANACPCI", id.ToString()));

                             id = 0x00000008L;
                             device.Items.Add(new DeviceItem("CANACPCIDN", id.ToString()));

                             id = 0x00000009L;
                             device.Items.Add(new DeviceItem("CANAC104", id.ToString()));

                             id = 0x0000000AL;
                             device.Items.Add(new DeviceItem("CANUSB", id.ToString()));

                             id = 0x0000000DL;
                             device.Items.Add(new DeviceItem("CANPROXPCI ", id.ToString()));

                             id = 0x0000000EL;
                             device.Items.Add(new DeviceItem("CANPROX104 ", id.ToString()));

                              id = 0x10000000L;
                             device.Items.Add(new DeviceItem("FG100CAN  ", id.ToString()));
                        }
                        break;
                    case "mps-can-kvaser": //Kvaser
                        {

                            int id = 1;
                            device.Items.Add(new DeviceItem("VIRTUAL", id.ToString()));

                            id = 2;
                            device.Items.Add(new DeviceItem("LAPCAN", id.ToString()));

                            id = 8;
                            device.Items.Add(new DeviceItem("PCCAN", id.ToString()));

                            id = 9;
                            device.Items.Add(new DeviceItem("PCCAN", id.ToString()));

                            id = 11; 
                            device.Items.Add(new DeviceItem("USBCAN", id.ToString()));

                            id = 40;
                            device.Items.Add(new DeviceItem("PCICAN_II", id.ToString()));

                            id = 48;
                            device.Items.Add(new DeviceItem("LEAF", id.ToString()));

                            id = 50;
                            device.Items.Add(new DeviceItem("PC104_PLUS", id.ToString()));

                            id = 52;
                            device.Items.Add(new DeviceItem("PCICANX_II", id.ToString()));

                            id = 54;
                            device.Items.Add(new DeviceItem("MEMORATOR_II", id.ToString()));
                            
                            id = 56;
                            device.Items.Add(new DeviceItem("USBCAN_PRO", id.ToString()));

                            id = 58;
                            device.Items.Add(new DeviceItem("IRIS", id.ToString()));

                            id = 60;
                            device.Items.Add(new DeviceItem("MEMORATOR_LIGHT", id.ToString()));
                        }
                        break;
                    case "mps-can-elm327":
                    case "mps-kline-rs232":
                        device.Items.Add(new DeviceItem("COM1", "COM1"));
                        device.Items.Add(new DeviceItem("COM2", "COM2"));
                        device.Items.Add(new DeviceItem("COM3", "COM3"));
                        device.Items.Add(new DeviceItem("COM4", "COM4"));
                        device.Items.Add(new DeviceItem("COM5", "COM5"));
                        device.Items.Add(new DeviceItem("COM6", "COM6"));
                        device.Items.Add(new DeviceItem("COM7", "COM7"));
                        device.Items.Add(new DeviceItem("COM8", "COM8"));
                        device.Items.Add(new DeviceItem("COM9", "COM9"));
                        device.Items.Add(new DeviceItem("COM10", "\\\\.\\COM10"));
                        device.Items.Add(new DeviceItem("COM11", "\\\\.\\COM11"));
                        device.Items.Add(new DeviceItem("COM12", "\\\\.\\COM12"));
                        device.Items.Add(new DeviceItem("COM13", "\\\\.\\COM13"));
                        device.Items.Add(new DeviceItem("COM14", "\\\\.\\COM14"));
                        device.Items.Add(new DeviceItem("COM15", "\\\\.\\COM15"));
                    break;
                    case "mps-can-systec":
                        device.Items.Add(new DeviceItem("USB 1 CHN", "USB1"));
                        device.Items.Add(new DeviceItem("USB 2 CHN", "USB2"));
                    break;
                    default:
                        device.Items.Add(new DeviceItem("Demo", "0"));
                    break;

                }
            }
            else
            {
                switch (driverID)
                {
                    case "mps-can-demo": //Demo
                        device.Items.Add(new DeviceItem("Virtual","virtual"));
                    break;

                    case "mps-can-elm327":
                    case "mps-kline-rs232":
                        device.Items.Add(new DeviceItem("/dev/ttyS0","/dev/ttyS0"));
                        device.Items.Add(new DeviceItem("/dev/ttyS1","/dev/ttyS1"));
                        device.Items.Add(new DeviceItem("/dev/ttyS2","/dev/ttyS2"));
                        device.Items.Add(new DeviceItem("/dev/ttyS3","/dev/ttyS3"));
                        device.Items.Add(new DeviceItem("/dev/ttyS4","/dev/ttyS4"));
                        device.Items.Add(new DeviceItem("/dev/ttyS5","/dev/ttyS5"));
                        device.Items.Add(new DeviceItem("/dev/ttyS6","/dev/ttyS6"));
                        device.Items.Add(new DeviceItem("/dev/ttyS7","/dev/ttyS7"));
                        device.Items.Add(new DeviceItem("/dev/ttyS8","/dev/ttyS8"));
                        device.Items.Add(new DeviceItem("/dev/ttyS9","/dev/ttyS9"));
                    break;

                    default:
                        device.Enabled = true;
                        if(runtimeType.SupportMCM)
                        {
                            device.Items.Add(new DeviceItem("/dev/can0","/dev/can0"));
                            device.Enabled = false;
                        }
                        else
                        {
                            device.Items.Add(new DeviceItem("/dev/can0","/dev/can0"));
                            device.Items.Add(new DeviceItem("/dev/can1","/dev/can1"));
                            device.Items.Add(new DeviceItem("/dev/can2","/dev/can2"));
                            device.Items.Add(new DeviceItem("/dev/can3","/dev/can3"));
                            device.Items.Add(new DeviceItem("/dev/can4","/dev/can4"));
                            device.Items.Add(new DeviceItem("/dev/can5","/dev/can5"));
                            device.Items.Add(new DeviceItem("/dev/can6","/dev/can6"));
                            device.Items.Add(new DeviceItem("/dev/can7","/dev/can7"));
                            device.Items.Add(new DeviceItem("/dev/can8","/dev/can8"));
                        }
                    break;
                }
            }

            device.SelectedIndex = 0;
        }

    }
}
