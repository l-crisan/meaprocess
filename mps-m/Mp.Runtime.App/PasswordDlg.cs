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
    internal partial class PasswordDlg : Form
    {
        public PasswordDlg(string f)
        {            
            InitializeComponent();
            this.Icon = Runtime.Sdk.RuntimeEngine.AppIcon;
            file.Text = f;
        }

        public string Password;

        private void OK_Click(object sender, EventArgs e)
        {
            Password = passwordCtrl.Text;
            Close();
        }

        private void PasswordDlg_Load(object sender, EventArgs e)
        {

        }

        private void file_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
