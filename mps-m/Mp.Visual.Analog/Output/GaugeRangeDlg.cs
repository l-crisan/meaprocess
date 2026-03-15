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

namespace Mp.Visual.Analog
{
    public partial class GaugeRangeDlg : Form
    {
        private RangeList _ranges;

        public GaugeRangeDlg(RangeList ranges)
        {
            _ranges = ranges;
            InitializeComponent();
            foreach (Range range in _ranges)
            {
                int index  = rangesGrid.Rows.Add();
                DataGridViewRow row = rangesGrid.Rows[index];
                row.Cells[0].Value = range.Enabled;
                row.Cells[1].Value = range.StartValue;
                row.Cells[2].Value = range.EndValue;
                row.Cells[3].Value = range.InnerRadius;
                row.Cells[4].Value = range.OuterRadius;
                row.Cells[5].Style.BackColor = range.RangeColor;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (DataGridViewRow row in rangesGrid.Rows)
            {
                Range range = _ranges[i];
                range.Enabled = (bool) row.Cells[0].Value;
                range.StartValue = Convert.ToSingle( row.Cells[1].Value);
                range.EndValue = Convert.ToSingle( row.Cells[2].Value);
                range.InnerRadius = Convert.ToInt32( row.Cells[3].Value);
                range.OuterRadius = Convert.ToInt32( row.Cells[4].Value);
                range.RangeColor = row.Cells[5].Style.BackColor;
                ++i;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void rangesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 5 || e.RowIndex == -1)
                return;

            Color color = rangesGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;
            ColorDialog dlg = new ColorDialog();
            dlg.Color = color;

            if (dlg.ShowDialog() == DialogResult.OK)
                rangesGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = dlg.Color;

        }

        private void rangesGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            e.Cancel = false;
            errorProvider.Clear();
            try
            {
                DataGridViewRow row = rangesGrid.Rows[e.RowIndex];
                int ivalue;
                float fvalue;
                switch (e.ColumnIndex)
                {
                    case 1:
                    case 2:
                        fvalue = Convert.ToSingle(e.FormattedValue);
                    break;

                    case 3:
                    case 4:
                        ivalue = Convert.ToInt32(e.FormattedValue);
                        
                        if (ivalue < 0)
                            throw new Exception(StringResource.PosNoErr);
                    break;

                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(rangesGrid, ex.Message);
                e.Cancel = true;
            }
        }
    }
}