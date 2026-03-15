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
using System.Drawing;
using System.Windows.Forms;

namespace Mp.Visual.Digital
{
    [Serializable]
    public partial class LabelLed : UserControl
    {
        private bool _showLabel = true;

        public LabelLed()
        {
            InitializeComponent();
            this.BackColorChanged += new EventHandler(LabelLed_BackColorChanged);
        }

        private void LabelLed_BackColorChanged(object sender, EventArgs e)
        {
            label.BackColor = this.BackColor;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        [SRDescription("ColorOn")]
        [SRCategory("View")]
        public Color ColorOn
        {
            get{ return led.ColorOn;}
            set { led.ColorOn = value; }
        }

        [SRDescription("ColorOff")]
        [SRCategory("View")]
        public Color ColorOff
        {
            get { return led.ColorOff; }
            set { led.ColorOff = value; }
        }

        public string LabelText
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        [SRDescription("On")]
        [SRCategory("View")]
        public bool On
        {
            get { return led.Active; }
            set { led.Active = value; }
        }

        [SRDescription("ShowLabel")]
        [SRCategory("View")]
        public bool ShowLabel
        {
            get { return _showLabel; }
            set 
            {
                _showLabel = value;
                label.Visible = value;
                /*
                if (!label.Visible)
                {
                    led.Padding = new Padding(3);
                }
                else
                {
                    led.Padding = new Padding(20, 3, 20, 3);
                }*/
            }
        }
    }
}
