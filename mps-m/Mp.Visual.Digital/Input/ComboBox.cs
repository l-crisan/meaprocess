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
using System.Collections;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Mp.Visual.Digital
{
    public partial class ComboBox : UserControl
    {
        private ArrayList _items = new ArrayList();

        public ComboBox()
        {
            InitializeComponent();
        }


       [System.ComponentModel.Browsable(true),
        SRCategory("View"),
        SRDescription("ComboItems"),
        System.ComponentModel.Editor(typeof(ItemsUIEditor), typeof(UITypeEditor))]
        public ArrayList Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public double Value
        {
            get
            {
                if (comboBox.Items.Count > 0)
                {
                    ControlItem item = (ControlItem)comboBox.Items[comboBox.SelectedIndex];
                    return item.Value;
                }

                return 0;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            comboBox.DropDownHeight = this.ClientRectangle.Height;
        }


        public void UpdateView()
        {
            comboBox.Items.Clear();
            
            foreach (ControlItem item in Items)
                comboBox.Items.Add(item);

            if(comboBox.Items.Count != 0)
                comboBox.SelectedIndex = 0;
        }
    }
}
