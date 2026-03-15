using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Mp.Visual.CAN
{
    public partial class CANLoggerViewDlg : Form
    {
        private Hashtable _high;

        public CANLoggerViewDlg(Hashtable highlighting, int noOfMsg)
        {
            _high = highlighting;
            InitializeComponent();

            messages.Text = noOfMsg.ToString();
            int index = 0;
            DataGridViewRow row;
            foreach (DictionaryEntry entry in _high)
            {
                index = hightLightingView.Rows.Add();
                row = hightLightingView.Rows[index];
                row.Cells[0].Value = Convert.ToUInt32(entry.Key);
                row.Cells[1].Value = "...";
                row.Cells[1].Style.BackColor = (Color)entry.Value;
                row.Cells[1].Style.SelectionBackColor = row.Cells[1].Style.BackColor;
                index++;
            }
            
            row = hightLightingView.Rows[index];
            row.Cells[1].Value = "...";

        }

        public int NoOfMessages
        {
            get { return Convert.ToInt32(messages.Text); }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            _high.Clear();

            foreach (DataGridViewRow row in hightLightingView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                uint id = Convert.ToUInt32(row.Cells[0].Value);

                if (_high.ContainsKey(id))
                {
                    _high[id] = row.Cells[1].Style.BackColor;
                }
                else
                {
                    _high.Add(id, row.Cells[1].Style.BackColor);
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void hightLightingView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1 || e.RowIndex == -1)
                return;

            DataGridViewRow row = hightLightingView.Rows[e.RowIndex];
            ColorDialog dlg = new ColorDialog();
            dlg.Color = row.Cells[1].Style.BackColor;
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            row.Cells[1].Style.BackColor = dlg.Color;
            row.Cells[1].Style.SelectionBackColor = dlg.Color;                       
        }

        private void remove_Click(object sender, EventArgs e)
        {
            Hashtable rowsToRemove = new Hashtable();
            foreach (DataGridViewCell cell in hightLightingView.SelectedCells)
            {
                if (!rowsToRemove.ContainsKey(cell.RowIndex))
                {
                    DataGridViewRow row = hightLightingView.Rows[cell.RowIndex];
                    if( !row.IsNewRow)
                        rowsToRemove.Add(cell.RowIndex, 0);
                }
            }

            foreach (DictionaryEntry entry in rowsToRemove)
            {
                hightLightingView.Rows.RemoveAt((int)entry.Key);
            }
        }

        private void hightLightingView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            if (e.RowIndex == -1 || e.ColumnIndex != 0)
                return;

            try
            {
                Convert.ToUInt32(e.FormattedValue);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(hightLightingView, ex.Message);
                e.Cancel = true;
            }
        }

        private void hightLightingView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void messages_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(messages.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(messages, ex.Message);
            }
        }
    }
}
