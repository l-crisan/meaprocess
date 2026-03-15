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

namespace Mp.Mod.Controller
{
    public partial class FuzzificationDlg : Form
    {
        private List<FuzzificationItem> _items;
        private FuzzificationViewCtrl _view;
        private double _min;
        private double _max;
        private string _sigName;
        private bool _singelTon;

        public FuzzificationDlg(List<FuzzificationItem> items, string sigName, double min, double max, bool singelTon)
        {
            _min = min;
            _max= max;
            _sigName = sigName;
            _items = items;
            _view = new FuzzificationViewCtrl();
            _view.Dock = DockStyle.Fill;
            _singelTon = singelTon;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            groupBox1.Controls.Add(_view);

            foreach (FuzzificationItem item in _items)
            {
                int index = linguisticVariables.Rows.Add();
                DataGridViewRow row = linguisticVariables.Rows[index];
                row.Cells[0].Value = item.ItemName;

                if (_singelTon)
                {
                    row.Cells[1].ReadOnly = true;
                    row.Cells[1].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                    row.Cells[3].ReadOnly = true;
                    row.Cells[3].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                }

                row.Cells[1].Value = item.P1x;
                row.Cells[2].Value = item.P2x;
                row.Cells[3].Value = item.P3x;
                row.Cells[4].Value = item.P4x;
            }

            DataGridViewRow lastRow = linguisticVariables.Rows[linguisticVariables.Rows.Count - 1];
            if (singelTon && lastRow.IsNewRow)
            {
                lastRow.Cells[1].ReadOnly = true;                
                lastRow.Cells[1].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                lastRow.Cells[3].ReadOnly = true;
                lastRow.Cells[3].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                lastRow.Cells[4].ReadOnly = true;
                lastRow.Cells[4].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

            }
            else 
            {
                lastRow.Cells[1].Value = 1;
                lastRow.Cells[2].Value = 2;
                lastRow.Cells[3].Value = 3;
                lastRow.Cells[4].Value = 4;
            }

            if (min >= max)
                return;

            _view.UpdateView(_items, sigName, min, max);
        }

        private void dataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            DataGridViewRow row = linguisticVariables.Rows[e.RowIndex];
            if (!_singelTon)
            {
                double x1 = Convert.ToDouble(row.Cells[1].Value);
                double x2 = Convert.ToDouble(row.Cells[2].Value);
                double x3 = Convert.ToDouble(row.Cells[3].Value);
                double x4 = Convert.ToDouble(row.Cells[4].Value);

                errorProvider.Clear();

                if (x1 > x2 || x2 > x3 || x3 > x4)
                {
                    errorProvider.SetError(linguisticVariables, StringResource.X1X2X3Err);
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void UpdateView()
        {
            List<FuzzificationItem> list = new List<FuzzificationItem>();

            foreach (DataGridViewRow row in linguisticVariables.Rows)
            {
                if (row.IsNewRow)
                    continue;

                
                FuzzificationItem item = new FuzzificationItem();

                item.Selected = linguisticVariables.SelectedRows.Contains(row);
                item.P1x = Convert.ToDouble(row.Cells[1].Value);
                item.P2x = Convert.ToDouble(row.Cells[2].Value);
                item.P3x = Convert.ToDouble(row.Cells[3].Value);
                item.P4x = Convert.ToDouble(row.Cells[4].Value);
                list.Add(item);
            }
            
            if (_min == _max)
                return;

            _view.UpdateView(list, _sigName, _min, _max);
        }

        private void linguisticVariables_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();
            
            if( e.RowIndex == -1)
                return;

            if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                try
                {
                    if (_singelTon && e.ColumnIndex == 2)
                    {
                        DataGridViewRow row = linguisticVariables.Rows[e.RowIndex];
                        double value = Convert.ToDouble(e.FormattedValue);
                        row.Cells[1].Value = value;
                        row.Cells[3].Value = value;
                        row.Cells[4].Value = value;
                    }
                    else if (!_singelTon)
                    {
                        Convert.ToDouble(e.FormattedValue);
                    }
                }
                catch (Exception ex)
                {
                    errorProvider.SetError(linguisticVariables, ex.Message);
                    e.Cancel = true;
                }
            }
        }

        private void onRemove_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in linguisticVariables.SelectedRows)
                rowsToRemove.Add(row);

            foreach (DataGridViewRow row in rowsToRemove)
                linguisticVariables.Rows.Remove(row);

            UpdateView();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (linguisticVariables.Rows.Count < 2)
            {
                errorProvider.SetError(linguisticVariables, StringResource.Min2LinguisticVarErr);

                return;
            }

            foreach (DataGridViewRow row in linguisticVariables.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string name = (string)row.Cells[0].Value;

                if (name == null || name == "")
                {
                    errorProvider.SetError(linguisticVariables, StringResource.LigNameErr);
                    return;
                }

                foreach (DataGridViewRow curRow in linguisticVariables.Rows)
                {
                    if (row == curRow)
                        continue;

                    string curName = (string)curRow.Cells[0].Value;
                    if (curName == name)
                    {
                        string message = String.Format(StringResource.NoLingVarDuppErr, curName);
                        errorProvider.SetError(linguisticVariables, message);
                        return;
                    }
                }
            }

            _items.Clear();
            foreach (DataGridViewRow row in linguisticVariables.Rows)
            {
                if (row.IsNewRow)
                    continue;

                FuzzificationItem item = new FuzzificationItem();
                item.ItemName = row.Cells[0].Value.ToString();
                item.P1x = Convert.ToDouble(row.Cells[1].Value);
                item.P2x = Convert.ToDouble(row.Cells[2].Value);
                item.P3x = Convert.ToDouble(row.Cells[3].Value);
                item.P4x = Convert.ToDouble(row.Cells[4].Value);
                _items.Add(item);
            }

            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linguisticVariables_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = linguisticVariables.Rows[e.RowIndex];

            if (_singelTon)
            {                
                row.Cells[1].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                row.Cells[1].ReadOnly = true;

                row.Cells[3].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                row.Cells[3].ReadOnly = true;

                row.Cells[4].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                row.Cells[4].ReadOnly = true;
            }
            else
            {
                row.Cells[1].Value = 1;
                row.Cells[2].Value = 2;
                row.Cells[3].Value = 3;
                row.Cells[4].Value = 4;
            }
        }

        private void linguisticVariables_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            UpdateView();
        }

        private void linguisticVariables_SelectionChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1620);
        }
    }
}
