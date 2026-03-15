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
using System.Collections;
using System.Windows.Forms;

namespace Mp.Visual.Digital
{
    public partial class ItemsUIDlg : Form
    {
        private ArrayList _items;

        public ItemsUIDlg(ArrayList items)
        {
            _items = items;
            InitializeComponent();

            foreach (ControlItem item in _items)
            {
                int index = grid.Rows.Add();
                DataGridViewRow row = grid.Rows[index];
                row.Cells[0].Value = item.Name;
                row.Cells[1].Value = item.Value;
            }

            DataGridViewRow lastRow = grid.Rows[grid.Rows.Count - 1];
            lastRow.Cells[1].Value = 0;
        }

        private void grid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if( e.RowIndex == -1)
                return;

            if (e.ColumnIndex != 1)
                return;

            errorProvider.Clear();

            try
            {
                Convert.ToDouble(e.FormattedValue);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(grid, ex.Message);
            }
        }

        private void grid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow lastRow = grid.Rows[grid.Rows.Count - 1];
            lastRow.Cells[1].Value = 0;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            _items.Clear();

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow)
                    continue;

                ControlItem item = new ControlItem();
                item.Name = row.Cells[0].Value.ToString();
                item.Value = Convert.ToDouble(row.Cells[1].Value);
                _items.Add(item);
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
            List<DataGridViewRow> toRemove = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in grid.SelectedRows)
                toRemove.Add(row);

            foreach (DataGridViewRow row in toRemove)
                grid.Rows.Remove(row);
        }
    }
}
