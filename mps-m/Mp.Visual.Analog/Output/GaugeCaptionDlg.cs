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
    public partial class GaugeCaptionDlg : Form
    {
        private CaptionList _captions;

        public GaugeCaptionDlg(CaptionList captions)
        {
            _captions = captions;
            InitializeComponent();


            foreach (Caption caption in _captions)
            {
                int index = captionGrid.Rows.Add();
                DataGridViewRow row = captionGrid.Rows[index];

                row.Cells[0].Value = caption.Text;

                if (index == 0 || index == 1)
                {
                    row.Cells[0].ReadOnly = true;
                    row.Cells[0].Style.BackColor = System.Drawing.Color.LightGray;
                }

                row.Cells[1].Value = caption.Position.X;
                row.Cells[2].Value = caption.Position.Y;
                row.Cells[3].Style.BackColor = caption.CapColor;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            int index = 0;

            foreach (DataGridViewRow row in captionGrid.Rows)
            {
                Caption caption = _captions[index];
                caption.Text = (string)row.Cells[0].Value;
                caption.Position = new PointF( Convert.ToSingle(row.Cells[1].Value), Convert.ToSingle(row.Cells[2].Value));
                caption.CapColor = row.Cells[3].Style.BackColor;
                ++index;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void captionGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 3 || e.RowIndex == -1)
                return;

            Color color = captionGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;
            ColorDialog dlg = new ColorDialog();
            dlg.Color = color;

            if (dlg.ShowDialog() == DialogResult.OK)
                captionGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = dlg.Color;

        }

        private void captionGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            e.Cancel = false;
            errorProvider.Clear();
            try
            {
                DataGridViewRow row = captionGrid.Rows[e.RowIndex];
                switch (e.ColumnIndex)
                {
                    case 1:
                    case 2:
                        uint uivalue = Convert.ToUInt16(e.FormattedValue);
                    break;

                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(captionGrid, ex.Message);
                e.Cancel = false;
            }
        }
    }
}