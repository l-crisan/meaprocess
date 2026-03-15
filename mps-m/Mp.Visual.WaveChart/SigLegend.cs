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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Mp.Visual.WaveChart
{
    public partial class SigLegend : UserControl
    {
        private List<Signal> _signals;

        public SigLegend()
        {
            InitializeComponent();
            Top = 350;
        }
        
        /// <summary>
        /// Sets or gets the legend signal list.
        /// </summary>
        public List<Signal> Signals
        {
            get { return _signals; }
            set
            {
                _signals = value;
            }
        }

        private WaveChartCtrl _waveChart;

        /// <summary>
        /// Sets or gest the wave chart ctrl.
        /// </summary>
        public WaveChartCtrl WaveChart
        {
            set { _waveChart = value; }
            get { return _waveChart; }
        }
        /// <summary>
        /// Call this to update the legend data.
        /// </summary>
        public void UpdateLegend()
        {
            DataGridViewRow row;
            int index;

            _legendGrid.Rows.Clear();
            
            foreach (Signal signal in _signals)
            {
                index = _legendGrid.Rows.Add();
                row = _legendGrid.Rows[index];

                row.Cells[0].Value = signal.Name;
                row.Cells[0].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                
                row.Cells[1].Value = signal.Unit;
                row.Cells[1].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                
                row.Cells[2].Value = signal.Comment;
                row.Cells[2].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                row.Cells[3].Value = signal.Samplerate;
                row.Cells[3].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                string format = "f" + signal.YAxisPrecision.ToString();

                row.Cells[4].Value = signal.Minimum.ToString(format);
                row.Cells[5].Value = signal.Maximum.ToString(format);
                row.Cells[6].Value = signal.Visible;

                
                row.Cells[7].Style.BackColor = signal.LineColor;
                row.Cells[7].Style.SelectionBackColor = signal.LineColor;
                
                row.Cells[8].Value = signal.LineWidth;
                row.Cells[9].Value = signal.PointsVisible;
                row.Cells[10].Value = signal.PointSize;
                row.Cells[11].Style.BackColor = signal.PointColor;
                row.Cells[11].Style.SelectionBackColor = signal.PointColor;
                row.Cells[12].Value = signal.YAxisDivision;
                row.Cells[13].Value = signal.YAxisPrecision;
            }
        }


        private void _legendGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            switch (e.ColumnIndex)
            {
                case 7:
                {
                    Signal sig = _signals[e.RowIndex];
                    ColorDialog dlg = new ColorDialog();

                    dlg.Color = _legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;

                    DialogResult res = dlg.ShowDialog();

                    if (res == DialogResult.OK)
                    {
                        _legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = dlg.Color;
                        _legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = dlg.Color;

                        _legendGrid.Invalidate();
                        sig.LineColor = dlg.Color;
                        _waveChart.Refresh(this);
                    }
                }
                break;

                case 11:
                {
                    Signal sig = _signals[e.RowIndex];
                    ColorDialog dlg = new ColorDialog();

                    dlg.Color = _legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;

                    DialogResult res = dlg.ShowDialog();

                    if (res == DialogResult.OK)
                    {
                        _legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = dlg.Color;
                        _legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = dlg.Color;

                        _legendGrid.Invalidate();
                        sig.PointColor = dlg.Color;
                        _waveChart.Refresh(this);
                    }
                }
                break;
            }
        }

        private void _legendGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            switch (e.ColumnIndex)
            {
                case 6:
                {
                    Signal sig =_signals[e.RowIndex];

                    sig.Visible = !sig.Visible;//(bool)_legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    _waveChart.Refresh(this);
                }
                break;

                case 9:
                {
                    Signal sig = _signals[e.RowIndex];
                    sig.PointsVisible = !sig.PointsVisible;
                    _waveChart.Refresh(this);
                }
                break;
            }
        }

 
        private void _legendGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            switch (e.ColumnIndex )

            {
                case 4:
                case 5:
                try
                {
                    Signal sig = (Signal)_signals[e.RowIndex];

                    double value = (double) Convert.ToDouble( e.FormattedValue );
                    _waveChart.Refresh(this);
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                    e.Cancel = true;
                }
                break;
            case 8:
            case 10:
            case 12:
            case 13:
                try
                {
                    uint value = Convert.ToUInt32(_legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue);
                }
                catch(Exception ex)
                {
                    string str = ex.Message;
                    e.Cancel = true;
                }
            break;
            }        
        }

        private void _legendGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            if(e.RowIndex >= _signals.Count)
                return;

            Signal sig = _signals[e.RowIndex];

            switch (e.ColumnIndex)
            {
                case 4:
                    sig.Minimum = Convert.ToDouble(_legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue);
                    break;
                case 5:
                    sig.Maximum = Convert.ToDouble(_legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue);
                    break;
                case 8:
                    sig.LineWidth = Convert.ToUInt32(_legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue);
                    break;
                case 10:
                    sig.PointSize = Convert.ToUInt32(_legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue);
                    break;
                case 12:
                    sig.YAxisDivision = Convert.ToUInt32(_legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue);
                    break;
                case 13:
                    sig.YAxisPrecision = Convert.ToUInt32(_legendGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue);
                    break;
            }
            _waveChart.Refresh(this);
        }

        private void _legendGrid_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            e.ToolTipText = this.Text;
        }

    }
}
