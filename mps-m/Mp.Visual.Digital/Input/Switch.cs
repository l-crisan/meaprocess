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
using System.Windows.Forms;

namespace Mp.Visual.Digital
{
    public partial class Switch : UserControl
    {
        public Switch()
        {
            InitializeComponent();
        }

        private void on_CheckedChanged(object sender, EventArgs e)
        {
            off.Checked = !on.Checked;
        }

        private void off_CheckedChanged(object sender, EventArgs e)
        {
            on.Checked = !off.Checked;
        }

        public byte Value
        {
            get 
            {
                if (on.Checked)
                    return 1;
                else
                    return 0;
            }
        }

        private void Switch_FontChanged(object sender, EventArgs e)
        {
            on.Font = Font;
            off.Font = Font;
        }
        
        [SRDescription("OnText")]
        [SRCategory("View")]
        public string OnText
        {
            get { return on.Text; }
            set { on.Text = value; }
        }

        [SRDescription("OffText")]
        [SRCategory("View")]
        public string OffText
        {
            get { return off.Text; }
            set { off.Text = value; }
        }

    }
}
