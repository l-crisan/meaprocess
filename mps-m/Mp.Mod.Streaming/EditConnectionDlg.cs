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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mp.Mod.Streaming
{
    public partial class EditConnectionDlg : Form
    {
        public EditConnectionDlg(string name, string connection)
        {
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            if (connection != "")
            {
                string[] array = connection.Split('/');

                
                switch(array[0])
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

            conName.Text = name;
        }

        private void LoadTcpCon(string ipPort)
        {
            string[] array = ipPort.Split(':');

            ip.Text = array[0];
            port.Text = array[1];
            conTab.SelectedIndex = 0;
                
        }

        public string ConName
        {
            get
            {
                return conName.Text;
            }
        }

        private string GetParamValue(string text)
        {
            string[] array = text.Split('=');
            return array[1];
        }

        public string Connection
        {
            get
            {
                string conStr = "";

                switch(conTab.SelectedIndex)
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
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if(conName.Text == "")
            {
                errorProvider.SetError(conName,StringResource.ErrorName);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ip_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            string[] data = ip.Text.Split('.');

            if(data.Length != 4)
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
            }
        }

        private void baudrate_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(port.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(port, ex.Message);
                e.Cancel = true;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
   
            Close();
        }
    }
}
