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
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using Mp.Visual.Docking;

using System.IO;

namespace Mp.Runtime.App
{
    internal partial class PanelContainer : DockContent
    {
        private Form _panel;
        private int _top;
        private int _left;
        private int _width;
        private int _height;
        private int _id;

        public PanelContainer()
        {
            InitializeComponent();
            this.Icon = Runtime.Sdk.RuntimeEngine.AppIcon;
        }

        public Form Panel
        {
            set
            {
                _panel = value;
                _panel.TopLevel = false;
                _top = _panel.Top;
                _panel.Top = 0;
                _left = _panel.Left;
                _panel.Left = 0;

                _width = _panel.Width;
                _height = _panel.Height;

                _panel.AutoScroll = true;
                _panel.Dock = DockStyle.Fill;

                
                this.Icon = _panel.Icon;
                this.Text = _panel.Text;
                this.ToolTipText = _panel.Text;
                this.TabText = _panel.Text;
                this.Name = _panel.Text;                
                
                Controls.Add(_panel);         
            }
        }

        protected override string GetPersistString()
        {
            return base.GetPersistString() + "\n" + this.ID.ToString();
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public Form RestoreOrignalPanelState()
        {
            _panel.AutoScroll = false;
            _panel.Dock = DockStyle.None;
            _panel.Top = _top;
            _panel.Left = _left;
            _panel.Height = _height;
            _panel.Width = _width;
            return _panel;             
        }
   }
}