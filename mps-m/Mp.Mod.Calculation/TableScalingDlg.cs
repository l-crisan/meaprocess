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
using Mp.Scheme.Sdk;

namespace Mp.Mod.Calculation
{
    public partial class TableScalingDlg : Form
    {
        private ScalingPSDlg.Scaling _scaling;

        public TableScalingDlg(ScalingPSDlg.Scaling scaling)
        {
            _scaling = scaling;
            InitializeComponent();
            this.Icon = Document.AppIcon;
            _scaling.ScalingType = 2;

            name.Text = _scaling.SigName;
            unit.Text = _scaling.SigUnit;
            comment.Text = _scaling.SigComment;

            foreach (PointF point in _scaling.Table)
            {
                int index = scalingTable.Rows.Add();
                DataGridViewRow row = scalingTable.Rows[index];
                row.Cells[0].Value = point.X;
                row.Cells[1].Value = point.Y;
            }
        }

        private void scalingTable_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToSingle(e.FormattedValue);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(scalingTable, ex.Message);
                e.Cancel = true;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (name.Text == null || name.Text == "")
            {
                errorProvider.SetError(name, StringResource.SigNameErr);
                return;
            }

            if (scalingTable.Rows.Count < 3)
            {
                errorProvider.SetError(scalingTable, StringResource.TableMin2ValuesErr);
                return;
            }

            double lastValue = double.MinValue;
            foreach (DataGridViewRow row in scalingTable.Rows)
            {
                if (row.IsNewRow)
                    continue;

                double value = Convert.ToDouble(row.Cells[0].Value);
                if (value < lastValue)
                {
                    errorProvider.SetError(scalingTable, StringResource.TableMonotoneErr);
                    return;                    
                }

                lastValue = value;
            }

            _scaling.SigName = name.Text;
            _scaling.SigUnit = unit.Text;
            _scaling.SigComment = comment.Text;

            _scaling.Table.Clear();

            foreach (DataGridViewRow row in scalingTable.Rows)
            {
                if (row.IsNewRow)
                    continue;

                PointF point = new PointF();
                point.X = Convert.ToSingle(row.Cells[0].Value);
                point.Y = Convert.ToSingle(row.Cells[1].Value);
                _scaling.Table.Add(point);
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void remove_Click(object sender, EventArgs e)
        {
            if (scalingTable.SelectedRows.Count == 0)
                return;

            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

            foreach(DataGridViewRow row in scalingTable.SelectedRows)
                rowsToRemove.Add(row);

            foreach (DataGridViewRow row in rowsToRemove)
                scalingTable.Rows.Remove(row);
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1400);
        }

        private void import_Click(object sender, EventArgs e)
        {
            TableScalingImportDlg dlg = new TableScalingImportDlg(scalingTable);
            dlg.ShowDialog();
        }

        private void csvImport_Click(object sender, EventArgs e)
        {
            TableScalingImportDlg dlg = new TableScalingImportDlg(scalingTable);
            dlg.ShowDialog();
            
        }
    }
}
