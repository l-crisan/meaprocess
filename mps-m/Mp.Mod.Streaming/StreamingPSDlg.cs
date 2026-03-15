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
using System.Windows.Forms;
using Mp.Scheme.Sdk;
using System.Xml;
using Mp.Utils;

namespace Mp.Mod.Streaming
{
    public partial class StreamingPSDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlPS;

        private class ConnectionItem
        {
            private readonly string _name;
            private readonly string _connection;

            public ConnectionItem(string name, string connection)
            {
                _connection = connection;
                _name = name;
            }

            public string Name
            {
                get { return _name; }
            }

            public string Connection
            {
                get { return _connection; }
            }

            public override string ToString()
            {
                return _name;
            }
        }

        public StreamingPSDlg(Document doc, XmlElement  xmlPS, bool remote)
        {
            _doc = doc;
            _xmlPS = xmlPS;

            InitializeComponent();

            ip.Items.Clear();

            List<string> ipList = _doc.GetResource("IPS");

            if (ipList != null)
            {
                foreach (string ipAdr in ipList)
                {
                    if (ipAdr != "" && !ip.Items.Contains(ipAdr))
                        ip.Items.Add(ipAdr);
                }
            }

            if (ip.Items.Count > 0)
                ip.SelectedIndex = 0;

            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            psName.Text = XmlHelper.GetParam(_xmlPS, "name");
            string connection = XmlHelper.GetParam(_xmlPS, "connection");

            if (connection != "")
            {
                string[] array = connection.Split('/');


                switch (array[0])
                {
                    case "stcp:":
                        server.Checked = true;
                        LoadTcpCon(array[2]);
                        break;

                    case "tcp:":
                        server.Checked = false;
                        LoadTcpCon(array[2]);
                        break;

                    case "serial:":
                        conTab.SelectedIndex = 1;
                        array = array[2].Split('?');
                        device.Text = array[0];
                        array = array[1].Split('&');
                        baudrate.Text = GetParamValue(array[0]);
                        break;
                }

            }

        }

        private string GetParamValue(string text)
        {
            string[] array = text.Split('=');
            return array[1];
        }


        private void LoadTcpCon(string ipPort)
        {
            string[] array = ipPort.Split(':');

            ip.Text = array[0];
            port.Text = array[1];
            conTab.SelectedIndex = 0;

        }

        private string GetConnection()
        {
            string conStr = "";

            switch (conTab.SelectedIndex)
            {
                case 0:
                    {//Socket
                        if (server.Checked)
                            conStr = "stcp://";
                        else
                            conStr = "tcp://";

                        conStr += ip.Text;
                        conStr += ":";
                        conStr += port.Text;
                    }
                    break;

                case 1:
                    {//Serial
                        conStr = "serial://" + device.Text + "?baudrate=" + baudrate.Text;
                    }
                    break;
            }


            return conStr;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            XmlHelper.SetParam(_xmlPS, "name", "string", psName.Text);
            XmlHelper.SetParam(_xmlPS, "connection", "string", GetConnection());

            DialogResult = DialogResult.OK;


            List<string> ipList = new List<string>();

            foreach (string ipStr in ip.Items)
                ipList.Add(ipStr);


            if (ip.Text != "" && ip.Text != null)
            {
                if (!ipList.Contains(ip.Text))
                    ipList.Add(ip.Text);
            }

            _doc.AddResource("IPS", ipList);
            _doc.Modified = true;
            Close();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1640);
        }

        private void clearIPs_Click(object sender, EventArgs e)
        {
            ip.Items.Clear();
        }

        private void ip_Validating(object sender, CancelEventArgs e)
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

        private void port_Validating(object sender, CancelEventArgs e)
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
    }
}
