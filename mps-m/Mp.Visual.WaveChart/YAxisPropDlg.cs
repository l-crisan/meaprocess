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

namespace Mp.Visual.WaveChart
{
    public partial class YAxisPropDlg : Form
    {
        public YAxisPropDlg()
        {
            InitializeComponent();
        }

        public class PoDialogProperties
        {
            public PoDialogProperties() { }
            public string ChartName;
            public Color BackColor;
        }

        public delegate void ChangeProperties(PoDialogProperties Properties);

        public PoDialogProperties   DialogProperties = new PoDialogProperties();
        public ChangeProperties     OnChangeProperties;

        private void PoYAxisPropDlg_Load(object sender, EventArgs e)
        {
            ctrlApplyOnChange.Checked   = false;
            ctrlChartName.Text          = DialogProperties.ChartName;
            _bkColor.BackColor         = DialogProperties.BackColor;
            ctrlApplyOnChange.Checked   = true;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Apply_Click(sender, e);
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            DialogProperties.BackColor = _bkColor.BackColor;

            if (OnChangeProperties != null)
                OnChangeProperties(DialogProperties);
        }

        private void OnPropertiesChanged(object sender, EventArgs e)
        {
            if (!ctrlApplyOnChange.Checked)
                return;

            Apply_Click(sender, e);
        }

        private void _bkColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = _bkColor.BackColor;
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                _bkColor.BackColor = dlg.Color;
                OnPropertiesChanged(sender, e);
            }
        }
    }
}