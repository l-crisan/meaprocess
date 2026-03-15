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
using System.Collections;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Mp.Visual.Digital
{
    public partial class RadioButton : UserControl
    {
        private ArrayList _items = new ArrayList();
        public RadioButton()
        {
            InitializeComponent();                        
        }

        [SRCategory("View")]
        public string Titel
        {
            get { return groupBox.Text; }
            set { groupBox.Text = value; }
        }

       [System.ComponentModel.Browsable(true),
        SRCategory("View"),
        SRDescription("RBItems"),
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
                foreach (System.Windows.Forms.RadioButton ctrl in panel.Controls)
                {
                    if( ctrl.Checked)
                    {
                        ControlItem item = (ControlItem)ctrl.Tag;
                        return item.Value;
                    }
                }
                return 0;
            }
        }

        public void UpdateView()
        {
            panel.Controls.Clear();

            int i = 0;
            foreach (ControlItem item in Items)
            {
                System.Windows.Forms.RadioButton ctrl = new System.Windows.Forms.RadioButton();
                if (i == 0)
                    ctrl.Checked = true;

                ctrl.Text = item.Name;
                ctrl.Tag = item;
                panel.Controls.Add(ctrl);
                ++i;
            }
        }
    }
}
