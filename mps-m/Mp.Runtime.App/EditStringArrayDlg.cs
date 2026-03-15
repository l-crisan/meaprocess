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

namespace Mp.Runtime.App
{
    internal partial class EditStringArrayDlg : Form
    {
        public EditStringArrayDlg(string v, string data)
        {
            InitializeComponent();
            this.Icon = Runtime.Sdk.RuntimeEngine.AppIcon;

            string[] dataArray = data.Split('\n');

            foreach(string s in dataArray)
                value.Items.Add(s.TrimEnd('\r'));

            try
            {

                value.Text = v;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public string Value
        {
            get { return value.Text; }
        }
    }
}
