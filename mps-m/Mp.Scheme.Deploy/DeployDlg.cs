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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.IO.Ports;
using System.Xml;
using System.Threading;
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Scheme.Deploy
{
    internal partial class DeployDlg : Form, IDisposable
    {
        private enum Command
        {
            CmdLoadRuntime = 1,
            CmdInitialize,
            CmdDeinitialize,
            CmdStart,
            CmdStop,
            CmdCloseRuntime,
            CmdTerminate,
            CmdGetStatus,
            CmdGetProperty,
            CmdSetProperty,
            CmdReadLogFile,
            CmdSetLanguage,
            NotifyMessage=100,
            NotifyOK
        };


        enum Status
        {
            Setup = 0,
            Loaded,
            Initialised,
            Running,
            Error
        };

        private Socket _socket = null;
        private bool _connected = false;
        private Document _doc;
        private Stream _comStream;
        private ImageList _imgList = new ImageList();

        public DeployDlg(Document doc)
        {
            _doc = doc;
            InitializeComponent();
            this.Icon = Document.AppIcon;
            Properties.Settings.Default.Reload();
            _imgList.Images.Add(Mp.Scheme.Deploy.Resource.INFO);
            _imgList.Images.Add(Mp.Scheme.Deploy.Resource.warning);
            _imgList.Images.Add(Mp.Scheme.Deploy.Resource.error);
            _imgList.Images.Add(Mp.Scheme.Deploy.Resource.question);
            _imgList.Images.Add(Mp.Scheme.Deploy.Resource.none);
            _imgList.Images.Add(Mp.Scheme.Deploy.Resource.help);

            messages.SmallImageList = _imgList;
            messages.LargeImageList = _imgList;
            port.Text = "5000";
            DateTime dt = File.GetLastWriteTime(doc.File);
            schemeName.Text = "[" + dt.ToString() + "]" + " " + Path.GetFileName(doc.File);
            LoadProperties();
            FormStateHandler.Restore(this, "Mp.Scheme.Deploy.DeployDlg");


            ip.Items.Clear();

            List<string> ipList = _doc.GetResource("IPS");

            if (ipList != null)
            {                                
                foreach( string ipAdr in ipList)
                {
                    if(ipAdr != "" && !ip.Items.Contains(ipAdr))
                        ip.Items.Add(ipAdr);
                }
            }            
            
            if(ip.Items.Count > 0)
                ip.SelectedIndex = 0;
        }

                                
        public void Dispose()
        {
            CloseConnections();
        }


        private void CloseConnections()
        {
            if (_socket != null)
            {
                try
                {
                    _socket.Close();
                    _socket = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (serialPort.IsOpen)
                serialPort.Close();
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            CloseConnections();
        }
        

        private void OnCloseClick(object sender, EventArgs e)
        {
            List<string> ipList = new List<string>();

            foreach (string ipStr in ip.Items)
                ipList.Add(ipStr);


            if (ip.Text != "" && ip.Text != null)
            {
                if(!ipList.Contains(ip.Text))
                    ipList.Add(ip.Text);
            }

            _doc.AddResource("IPS", ipList);            
            Properties.Settings.Default.Save();

            Close();
        }


        private void OnIPValidating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            string[] data = ip.Text.Split('.');

            if (data.Length != 4)
            {
                errorProvider.SetError(ip, StringResource.InvalidIP);
                e.Cancel = true;
                return;
            }

            try
            {
                Convert.ToByte(data[0]);
                Convert.ToByte(data[1]);
                Convert.ToByte(data[2]);
                Convert.ToByte(data[3]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                errorProvider.SetError(ip, StringResource.InvalidIP);
                e.Cancel = true;
                return;
            }
        }


        private void OnPortValidating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToUInt16(port.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                errorProvider.SetError(port, StringResource.InvalidPort);
                e.Cancel = true;
                return;
            }
        }


        public static Socket ConnectToServer(IPEndPoint endPoint, int timeoutMs)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);            
            
            try
            {
                socket.ReceiveTimeout = 20000;
                socket.SendTimeout = 20000;

                // do not block - do not want to be forced to wait on (too- long) timeout
                socket.Blocking = false;

                // initiate connection - will cause exception because we're not  blocking
                socket.Connect(endPoint);
                return socket;
            }
            catch (SocketException socketException)
            {
                // check if this exception is for the expected 'Would Block' error
                if (socketException.ErrorCode != 10035)
                {
                    socket.Close();
                    // the error is not 'Would Block', so propogate the exception
                    throw;
                }
                // if we get here, the error is 'Would Block' so just continue  execution   }
                // wait until connected or we timeout 
                int timeoutMicroseconds = timeoutMs * 1000;
                if (socket.Poll(timeoutMicroseconds, SelectMode.SelectWrite) == false)
                {
                    // timed out 
                    socket.Close();
                    throw new Exception(StringResource.SocketConFailed);
                }

                // *** AT THIS POINT socket.Connected SHOULD EQUAL TRUE BUT  IS FALSE!  ARGH!
                // set socket back to blocking 
                socket.Blocking = true;
                return socket;
            }
        }
 

        private void OnConnectClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            messages.Items.Clear();

            if (!_connected)
            {
                switch(connectionTab.SelectedIndex)
                {
                    case 0:
                    {//Socket Connection                        
                        try
                        {
                            _socket= ConnectToServer(new IPEndPoint(IPAddress.Parse(ip.Text), Convert.ToInt32(port.Text)), 6000);
                        
                            _comStream = new NetworkStream(_socket);
                            _comStream.Flush();

                            if (!UpdateStatus())
                            {
                                Cursor = Cursors.Default;
                                return;
                            }

                            statusConnect.BackColor = Color.YellowGreen;
                            connect.Text = StringResource.Disconnect;
                            readLogFile.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            OutputErrorMessage(ex.Message);
                            Cursor = Cursors.Default;
                            return;
                        }
                        _connected = true;
                        connectionTab.Enabled = false;
                    }
                    break;
                    case 1:
                    {//Serial connection
                        try
                        {
                            _socket = null;
                            serialPort.PortName = comPort.Text;                            
                            serialPort.Open();
                            serialPort.BaudRate = 115200 ;
                            serialPort.DataBits = 8;
                            serialPort.StopBits = StopBits.One;
                            serialPort.Parity = Parity.None;
                            serialPort.Handshake = Handshake.XOnXOff;                       

                            _comStream = serialPort.BaseStream;
                            _comStream.Flush();
                            _comStream.ReadTimeout = 1000;

                            connectionTab.Enabled = false;                        
                            UpdateStatus();
                            _connected = true;
                            statusConnect.BackColor = Color.YellowGreen;
                            connect.Text = StringResource.Disconnect;
                            readLogFile.Enabled = true;
                        
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            Cursor = Cursors.Default;
                            return;
                        }
                    }
                    break;
                    
                    case 2: //CAN

                    break;

                    case 3: //USB
                    break;

                    case 4://FILE
                    {
                        try
                        {
                            statusConnect.BackColor = Color.YellowGreen;
                            XmlDocument xmlDoc = _doc.GetRuntimeDocument();
                            xmlDoc.Save(fileToExport.Text);
                            statusConnect.BackColor = Color.YellowGreen;
                        }
                        catch (Exception ex)
                        {
                            statusConnect.BackColor = Color.DarkSlateGray;
                            MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Cursor = Cursors.Default; 
                        return;
                    }                    

                }
            }
            else
            {
                readLogFile.Enabled = false;

                if (_socket != null)
                {
                    _socket.Close();
                    _socket = null;
                }
                else
                {
                    serialPort.Close();                        
                }

                statusConnect.BackColor = Color.DarkSlateGray;
                connect.Text = StringResource.Connect;
                _connected = false;

                deploy.Enabled = false;
                initialize.Enabled = false;
                deinitialize.Enabled = false;
                start.Enabled = false;
                stop.Enabled = false;

                statusDeploy.BackColor = Color.DarkSlateGray;
                initStatus.BackColor = Color.DarkSlateGray;
                runStatus.BackColor = Color.DarkSlateGray;
                connectionTab.Enabled = true;
            }

            Cursor = Cursors.Default; 
        }


        private bool UpdateStatus()
        {
            if (!SetLanguage())
                return false;

            Status status = GetStatus();

            switch (status)
            {
                case Status.Setup:
                    deploy.Enabled = true;
                    initialize.Enabled = false;
                    deinitialize.Enabled = false;
                    start.Enabled = false;
                    stop.Enabled = false;

                    statusDeploy.BackColor = Color.DarkSlateGray;
                    initStatus.BackColor = Color.DarkSlateGray;
                    runStatus.BackColor = Color.DarkSlateGray;
                    break;

                case Status.Loaded:
                    if (loadedScheme.Text == schemeName.Text)
                    {
                        deploy.Enabled = true;
                        initialize.Enabled = true;
                        deinitialize.Enabled = false;
                        start.Enabled = false;
                        stop.Enabled = false;

                        statusDeploy.BackColor = Color.YellowGreen;
                        initStatus.BackColor = Color.DarkSlateGray;
                        runStatus.BackColor = Color.DarkSlateGray;
                    }
                    else
                    {
                        deploy.Enabled = true;
                        initialize.Enabled = false;
                        deinitialize.Enabled = false;
                        start.Enabled = false;
                        stop.Enabled = false;

                        statusDeploy.BackColor = Color.YellowGreen;
                        initStatus.BackColor = Color.DarkSlateGray;
                        runStatus.BackColor = Color.DarkSlateGray;
                    }
                    break;
                case Status.Initialised:
                    deploy.Enabled = false;
                    initialize.Enabled = false;
                    deinitialize.Enabled = true;
                    start.Enabled = true;
                    stop.Enabled = false;

                    statusDeploy.BackColor = Color.YellowGreen;
                    initStatus.BackColor = Color.YellowGreen;
                    runStatus.BackColor = Color.DarkSlateGray;               
                    break;

                case Status.Running:
                    deploy.Enabled = false;
                    initialize.Enabled = false;
                    deinitialize.Enabled = false;
                    start.Enabled = false;
                    stop.Enabled = true;

                    statusDeploy.BackColor = Color.YellowGreen;
                    initStatus.BackColor = Color.YellowGreen;
                    runStatus.BackColor = Color.YellowGreen;
                    break;
                default:
                    return false;
            }

            return true;
        }
        

        private void LoadProperties()
        {

            XmlElement xmlDoc = _doc.XmlDoc.DocumentElement;

            XmlElement xmlGUI = XmlHelper.GetChildByType(xmlDoc, "GUI");

            XmlElement xmlProperties = XmlHelper.GetChildByType(xmlDoc, "Mp.Properties");

            if (xmlProperties == null)
                return;

            foreach (XmlElement xmlProperty in xmlProperties.ChildNodes)
            {
                string type = XmlHelper.GetParam(xmlProperty, "type");

                if (type == "STRING ARRAY")
                    type = "ENUMERATION";

                if (XmlHelper.GetParamNumber(xmlProperty, "hidden") == 1)
                    continue;

                DataGridViewRow row = null;
                int index = properties.Rows.Add();
                row = properties.Rows[index];

                row.Cells[0].Value = XmlHelper.GetParam(xmlProperty, "name");
                row.Cells[1].Value = XmlHelper.GetParam(xmlProperty, "value");
                row.Cells[1].Tag = XmlHelper.GetParam(xmlProperty, "typeValue");
                row.Cells[2].Value = "...";
                row.Cells[3].Value = XmlHelper.GetParamNumber(xmlProperty, "mandatory") > 0;

                if (type == "ENUMERATION")
                {
                    DataGridViewComboBoxCell cbo = new DataGridViewComboBoxCell();

                    string data = row.Cells[1].Tag.ToString();
                    string[] array = data.Split('\n');

                    List<string> items = new List<string>();

                    for (int i = 0; i < array.Length; ++i)
                        items.Add(array[i].TrimEnd('\r'));

                    foreach (string s in items)
                        cbo.Items.Add(s);

                    try
                    {
                        if (items.Contains((string)row.Cells[1].Value))
                            cbo.Value = row.Cells[1].Value;
                        else if (items.Count > 0)
                            cbo.Value = items[0];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    cbo.ReadOnly = false;
                    row.Cells[1] = cbo;
                    row.Cells[1].Tag = XmlHelper.GetParam(xmlProperty, "typeValue");
                }

                if (XmlHelper.GetParamNumber(xmlProperty, "readOnly") > 0)
                {
                    row.Cells[2].ReadOnly = true;
                    row.Cells[1].ReadOnly = true;
                }

                if (row.Cells[1].ReadOnly)
                    row.Cells[1].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                row.Tag = xmlProperty;
            }
        }


        private void OnPropertiesCellClick(object sender, DataGridViewCellEventArgs e)
        {
            XmlElement xmlDoc = _doc.XmlDoc.DocumentElement;

            XmlElement xmlProperties = XmlHelper.GetChildByType(xmlDoc, "Mp.Properties");

            if (e.ColumnIndex != 2 || xmlProperties == null || e.RowIndex == -1)
                return;

            DataGridViewRow row = properties.Rows[e.RowIndex];

            XmlElement xmlProperty = row.Tag as XmlElement;

            if (xmlProperty == null)
                return;

            string type = XmlHelper.GetParam(xmlProperty, "type");

            if (XmlHelper.GetParamNumber(xmlProperty, "readOnly") > 0)
                return;

            switch (type)
            {
                case "FILE":
                    {
                        OpenFileDialog dlg = new OpenFileDialog();
                        dlg.CheckFileExists = false;
                        dlg.CheckPathExists = false;
                        dlg.Filter = "*.*|*.*|*.mmf|*.mmf|*.tdm|*.tdm";
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            row.Cells[1].Value = dlg.FileName;
                        }
                    }
                    break;
                case "FOLDER":
                    {
                        FolderBrowserDialog dlg = new FolderBrowserDialog();
                        dlg.ShowNewFolderButton = true;
                        if (dlg.ShowDialog() == DialogResult.OK)
                            row.Cells[1].Value = dlg.SelectedPath;
                    }
                    break;

                case "STRING":
                    {

                        EditStringDlg dlg = new EditStringDlg();
                        dlg.TextValue = (string)row.Cells[1].Value;

                        if (dlg.ShowDialog() == DialogResult.OK)
                            row.Cells[1].Value = dlg.TextValue;
                    }
                    break;

                case "STRING ARRAY":
                case "ENUMERATION":
                    {
                        string data = row.Cells[1].Tag.ToString();
                        EditStringArrayDlg dlg = new EditStringArrayDlg(row.Cells[1].Value.ToString(), data);

                        if (dlg.ShowDialog() == DialogResult.OK)
                            row.Cells[1].Value = dlg.Value;

                    }
                    break;
            }
        }


        private void OnDeployClick(object sender, EventArgs e)
        {
            try
            {
                if (!UpdateProperties())
                    return;

                XmlDocument newDoc = _doc.GetRuntimeDocument();

                Stream s = _comStream;
                MemoryStream mm = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(mm);
                bw.Write((byte)Command.CmdLoadRuntime);
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] schemeNameBytes = encoding.GetBytes(schemeName.Text);

                //Get file name data 
                string fname = Path.GetFileName(_doc.File);
                byte[] fileNameStrData = encoding.GetBytes(fname);

                byte[] schemeData = encoding.GetBytes(newDoc.OuterXml);

                //Write size
                bw.Write((uint)(fileNameStrData.Length + 1 + 8 + schemeData.Length + 1)); //Size
                
                //Write file name
                bw.Write(fileNameStrData);
                bw.Write((byte)0);

                //Write time stamp
                DateTime dt = File.GetLastWriteTime(_doc.File);
                bw.Write(dt.Ticks);

                //Write scheme 
                bw.Write(schemeData);
                bw.Write((byte)0);

                //Write to output stream
                mm.Seek(0, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(mm);
                byte[] data = br.ReadBytes((int)mm.Length);
                
                s.Write(data, 0, data.Length);
                s.Flush();

                if (!ReadResponce())
                    return;

                deploy.Enabled = true;
                initialize.Enabled = true;
                deinitialize.Enabled = false;
                start.Enabled = false;
                stop.Enabled = false;

                statusDeploy.BackColor = Color.YellowGreen;
                initStatus.BackColor = Color.DarkSlateGray;
                runStatus.BackColor = Color.DarkSlateGray;
                loadedScheme.Text =  schemeName.Text;
            }
            catch (Exception ex)
            {
                OutputErrorMessage(ex.Message);
            }
        }


        private bool ReadResponce()
        {
            try
            {

                Command res = (Command)_comStream.ReadByte();
                switch (res)
                {
                    case Command.NotifyOK:
                        return true;

                    case Command.NotifyMessage:
                    {
                        BinaryReader br = new BinaryReader(_comStream);
                        int count = (int)br.ReadUInt32();
                        bool noerror = true;
                        for (int i = 0; i < count; ++i)
                        {
                            if (!OutputMessage(_comStream))
                                noerror = false;
                        }
                        UpdateStatus();
                        return noerror;
                    }
                }
            }
            catch (Exception ex)
            {
                OutputErrorMessage(ex.Message);
                return false;
            }

            return true;
        }


        private void OutputErrorMessage(string msg)
        {
            string[] data = new string[4];
            data[0] = "";
            data[1] = "ERROR";
            data[2] = "[" + DateTime.Now.ToString() + "]";
            data[3] = msg;
            messages.Items.Add(new ListViewItem(data,2));
        }


        private bool OutputMessage(Stream s)
        {
            byte mType = (byte) s.ReadByte();
            string strMsgType = StringResource.Info;
            bool retVal = true;
            switch (mType)
            {
                case 0: //Info
                    strMsgType = StringResource.Info;
                break;
                
                case 1: //Warning
                    strMsgType = StringResource.Warning;
                break;

                case 2: //Error
                    strMsgType = StringResource.Error;
                    retVal = false;
                break;

                case 3: //Question
                    strMsgType = StringResource.Question;
                break;
                
                case 4: //Stop
                    strMsgType = StringResource.Stop;
                break;
                
                case 5: //Event
                    strMsgType = StringResource.Event;
                break;
            }

            byte target = (byte)s.ReadByte();
            BinaryReader br = new BinaryReader(s);
            long timeStamp = br.ReadInt64();
            DateTime year1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime dt = new DateTime((long)(timeStamp* 10000) + year1970.Ticks);

            string msgText = ReadString(s);

            string [] data = new string[4];
            data[0] = "";
            data[1] = strMsgType;
            data[2] = "["+dt.ToString() + "]";
            data[3] = msgText;
            messages.Items.Add(new ListViewItem(data, mType));
            return retVal;
        }


        private bool UpdateProperties()
        {
            errorProvider.Clear();

            foreach (DataGridViewRow row in properties.Rows)
            {
                XmlElement xmlProperty = (XmlElement)row.Tag;
                string value;

                if (row.Cells[1].Value != null)
                    value = row.Cells[1].Value.ToString();
                else
                    value = "";

                bool mandatory = (bool)row.Cells[3].Value;

                if (value == "" && mandatory)
                {
                    errorProvider.SetError(properties, StringResource.MandatoryPropErr);
                    return false;
                }

                XmlHelper.SetParam(xmlProperty, "value", "string", value);
            }

            return true;
        }


        private void ReadProperties()
        {
            try
            {
                foreach (DataGridViewRow row in properties.Rows)
                {
                    XmlElement xmlProperty = (XmlElement)row.Tag;
                    string propName = XmlHelper.GetParam(xmlProperty, "name");

                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] stringData = encoding.GetBytes(propName);

                    MemoryStream mm = new MemoryStream();
                    BinaryWriter bw = new BinaryWriter(mm);
                    bw.Write((byte)Command.CmdGetProperty);
                    bw.Write(stringData);
                    bw.Write((byte)0);
                    mm.Seek(0, SeekOrigin.Begin);

                    BinaryReader br = new BinaryReader(mm);
                    byte[] data = br.ReadBytes((int)mm.Length);

                    Stream s = _comStream;
                    s.Write(data, 0, data.Length);
                    s.Flush();
                    row.Cells[1].Value = ReadString(s);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        private static string ReadString(Stream sr)
        {
            List<byte> data = new List<byte>();
            byte bdata = (byte)sr.ReadByte();
            data.Add(bdata);

            while (bdata != 0)
            {
                bdata = (byte)sr.ReadByte();
                data.Add(bdata);
            }

            data.RemoveAt(data.Count - 1);

            if (data.Count != 0)
            {
                Encoding encoding = new UTF7Encoding();
                return encoding.GetString(data.ToArray());
            }

            return "";
        }


        private void OnInitializeClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            SetLanguage();
            try
            {
                MemoryStream mm = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(mm);
                bw.Write((byte)Command.CmdInitialize);
                bw.Write((uint)0);
                mm.Seek(0, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(mm);
 
                _comStream.Write(br.ReadBytes((int)mm.Length), 0, (int)mm.Length);
                _comStream.Flush();

                if (!ReadResponce())
                {
                    Cursor = Cursors.Default;
                    return;
                }

                deploy.Enabled = false;
                initialize.Enabled = false;
                deinitialize.Enabled = true;
                start.Enabled = true;
                stop.Enabled = false;

                statusDeploy.BackColor = Color.YellowGreen;
                initStatus.BackColor = Color.YellowGreen;
                runStatus.BackColor = Color.DarkSlateGray;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Cursor = Cursors.Default;
        }


        private void OnDeinitializeClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                MemoryStream mm = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(mm);
                bw.Write((byte)Command.CmdDeinitialize);
                bw.Write((uint)0);
                mm.Seek(0, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(mm);

                _comStream.Write(br.ReadBytes((int)mm.Length), 0, (int)mm.Length);
                _comStream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }

            if (!ReadResponce())
            {
                Cursor = Cursors.Default;
                return;
            }

            deploy.Enabled = true;
            
            if( schemeName.Text == loadedScheme.Text)
                initialize.Enabled = true;

            deinitialize.Enabled = false;
            start.Enabled = false;
            stop.Enabled = false;

            statusDeploy.BackColor = Color.YellowGreen;
            initStatus.BackColor = Color.DarkSlateGray;
            runStatus.BackColor = Color.DarkSlateGray;            
            Cursor = Cursors.Default;
        }


        private bool SetLanguage()
        {
            try
            {
                BinaryWriter bw = new BinaryWriter(_comStream);
                bw.Write((byte)Command.CmdSetLanguage);

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] langData = encoding.GetBytes(Thread.CurrentThread.CurrentCulture.IetfLanguageTag);
                bw.Write((uint)langData.Length + 1);
                bw.Write(langData, 0, langData.Length);
                bw.Write((byte)0);
                _comStream.Flush();
                return ReadResponce();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        private void OnStartClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {

                MemoryStream mm = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(mm);
                bw.Write((byte)Command.CmdStart);
                bw.Write((uint)0);

                BinaryReader br = new BinaryReader(mm);
                br.BaseStream.Seek(0, SeekOrigin.Begin);

                _comStream.Write(br.ReadBytes((int)br.BaseStream.Length), 0, (int)br.BaseStream.Length);
                _comStream.Flush();

                if (!ReadResponce())
                    return;

                deploy.Enabled = false;
                initialize.Enabled = false;
                deinitialize.Enabled = false;
                start.Enabled = false;
                stop.Enabled = true;

                statusDeploy.BackColor = Color.YellowGreen;
                initStatus.BackColor = Color.YellowGreen;
                runStatus.BackColor = Color.YellowGreen;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Cursor = Cursors.Default;
        }


        private void OnStopClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                MemoryStream mm = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(mm);
                bw.Write((byte)Command.CmdStop);
                bw.Write((uint)0);

                BinaryReader br = new BinaryReader(mm);
                br.BaseStream.Seek(0, SeekOrigin.Begin);

                _comStream.Write(br.ReadBytes((int)br.BaseStream.Length), 0, (int)br.BaseStream.Length);
                _comStream.Flush();

                if (!ReadResponce())
                {
                    Cursor = Cursors.Default;
                    return;
                }

                deploy.Enabled = false;
                initialize.Enabled = false;
                deinitialize.Enabled = true;
                start.Enabled = true;
                stop.Enabled = false;

                statusDeploy.BackColor = Color.YellowGreen;
                initStatus.BackColor = Color.YellowGreen;
                runStatus.BackColor = Color.DarkSlateGray;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Cursor = Cursors.Default;
        }


        private Status GetStatus()
        {
            try
            {
                MemoryStream mm = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(mm);
                bw.Write((byte)Command.CmdGetStatus);
                bw.Write((uint)0);
                mm.Seek(0, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(mm);

                _comStream.Write(br.ReadBytes((int) mm.Length), 0, (int) mm.Length);
                _comStream.Flush();

                //Status
                Status status = (Status)_comStream.ReadByte();

                //Timestamp
                BinaryReader sr = new BinaryReader(_comStream);
                long timeStamp = sr.ReadInt64();
                DateTime dt = new DateTime(timeStamp, DateTimeKind.Local);

                //Scheme 
                string scheme = ReadString(_comStream);

                if (scheme != "")
                    loadedScheme.Text = "[" + dt.ToString() + "]" + " " + scheme;

                return status;
            }
            catch (Exception ex)
            {
                OutputErrorMessage(ex.Message);
                return Status.Error;
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            FormStateHandler.Save(this, "Mp.Scheme.Deploy.DeployDlg");
        }


        private void OnClearClick(object sender, EventArgs e)
        {
            messages.Items.Clear();
        }


        private void OnReadLogFileClick(object sender, EventArgs e)
        {
            string logData;

            try
            {
                MemoryStream mm = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(mm);
                bw.Write((byte)Command.CmdReadLogFile);
                bw.Write((uint)0);
                mm.Seek(0, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(mm);

                _comStream.Write(br.ReadBytes((int)mm.Length), 0, (int)mm.Length);
                _comStream.Flush();

                br = new BinaryReader(_comStream);
                uint size = br.ReadUInt32();

                if (size != 0)
                {
                    byte[] data = br.ReadBytes((int)size);
                    UTF7Encoding encoder = new UTF7Encoding();
                    logData = encoder.GetString(data);

                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.Filter = "*.log|*.log|*.txt|*.txt|*.*|*.*";

                    if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return;

                    StreamWriter fs = new StreamWriter(dlg.FileName);
                    fs.Write(logData);
                    fs.Close();
                }
                else
                {
                    //TODO: message box
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }


        private void OnExportClick(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "*.mpex|*.mpex";
            
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            fileToExport.Text = dlg.FileName;
        }


        private void OnConnectionTabSelectedIndexChanged(object sender, EventArgs e)
        {
            if (connectionTab.SelectedIndex == 4)
                connect.Text = StringResource.SaveText;
            else
                connect.Text = StringResource.ConnectText;

            statusConnect.BackColor = Color.DarkSlateGray;

        }

        private void OnClearIPClick(object sender, EventArgs e)
        {
            ip.Items.Clear();
        }
    }
}
