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

namespace Mp.Visual.Oscilloscope
{
    public partial class PropertyDlg : Form
    {
        private OscilloscopeView _osci;

        public PropertyDlg(OscilloscopeView osci)
        {
            _osci = osci;
            InitializeComponent();
            bkColor.BackColor = _osci.ChViewColor;
            ch1Color.BackColor = _osci.Ch1Color;
            ch2Color.BackColor = _osci.Ch2Color; ;
            gridColor.BackColor = osci.GridColor;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            _osci.Ch1Color = ch1Color.BackColor;
            _osci.Ch2Color = ch2Color.BackColor;
            _osci.GridColor = gridColor.BackColor;
            _osci.ChViewColor = bkColor.BackColor;            
            Close();
        }

        private void editBkColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = bkColor.BackColor;
            
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            bkColor.BackColor = dlg.Color;
        }

        private void editCh1Color_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = ch1Color.BackColor;

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            ch1Color.BackColor = dlg.Color;
        }

        private void editCh2Color_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = ch2Color.BackColor;

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            ch2Color.BackColor = dlg.Color;
        }

        private void editGridColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = gridColor.BackColor;

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            gridColor.BackColor = dlg.Color;
        }

    }
}
