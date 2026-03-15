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
using System.Drawing;
using System.Windows.Forms;

namespace Mp.Visual.Digital
{
    [Serializable]
    public partial class TableView : UserControl
    {
        private List<TableView.Signal> _signals = new List<Signal>();
        private string _format;
        private int _precision = 3;
        private double [] _data;
        
        public class Signal
        {
            public string Name;
            public string Unit;
            public int Index;
        }
        
        public TableView()
        {
            InitializeComponent();
            DataGridViewColumn valueCol = dataGridView.Columns[1];
            valueCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            Precision = 3;
            this.Width = 400;
            this.Height = 400;
        }    

        public void AddSignal(TableView.Signal signal)
        {
            updateTimer.Stop();
            _signals.Add(signal);            
            signal.Index = _signals.Count -1;
        }

        public void RemoveSignal(TableView.Signal sig)
        {
            updateTimer.Stop();

            _signals.Remove(sig);
            for(int i = 0; i < _signals.Count; ++i)
            {
                TableView.Signal inSig = _signals[i];
                inSig.Index = i;
            }
        }

        public void SetValue(int signalIndex, double value)
        {
            _data[signalIndex] = value;
        }

        /// <summary>
        /// Gets or sets the value precision.
        /// </summary>
        [SRCategory("View"), SRDescriptionAttribute("Precision")]
        public int Precision
        {
            get { return _precision; }
            set 
            { 
                _precision = value; 
                _format = "0.";
                
                for (int i = 0; i < _precision; i++)
                    _format += "0";
            
            }
        }

        /// <summary>
        /// Hides the signal name.
        /// </summary>
        [SRCategory("View"), SRDescriptionAttribute("HideSignalName")]
        public bool HideSignalName
        {
            get { return hideSignalNameToolStripMenuItem.Checked; }
            set 
            { 
                dataGridView.Columns[0].Visible = !value;
                hideSignalNameToolStripMenuItem.Checked = value;
            }
        }

        /// <summary>
        /// Hides the signal unit.
        /// </summary>
        [SRCategory("View"), SRDescriptionAttribute("HideSignalUnit")]
        public bool HideSignalUnit
        {
            get { return hideSignalUnitToolStripMenuItem.Checked; }
            set 
            {
                dataGridView.Columns[2].Visible = !value;
                hideSignalUnitToolStripMenuItem.Checked = value;
            }
        }

        /// <summary>
        /// Define the signal column width.
        /// </summary>
        [SRCategory("View"), SRDescriptionAttribute("SignalWidth")]
        public int SignalWidth
        {
            get { return dataGridView.Columns[0].Width; }
            set { dataGridView.Columns[0].Width = value; }
        }

        /// <summary>
        /// Define the signal column width.
        /// </summary>
        [SRCategory("View"), SRDescriptionAttribute("ValueWidthCol")]
        public int ValueWidth
        {
            get { return dataGridView.Columns[1].Width; }
            set { dataGridView.Columns[1].Width = value; }
        }

        /// <summary>
        /// Define the signal column width.
        /// </summary>
        [SRCategory("View"), SRDescriptionAttribute("UnitWidth")]
        public int UnitWidth
        {
            get { return dataGridView.Columns[2].Width; }
            set { dataGridView.Columns[2].Width = value; }
        }
        public void InitDone()
        {
            DataGridViewRow row;
            int index;
            dataGridView.Rows.Clear();

            foreach (Signal signal in _signals)
            {
                index = dataGridView.Rows.Add();
                row = dataGridView.Rows[index];
                row.Tag = index;

                row.Cells[0].Value = signal.Name;
                row.Cells[0].Style.BackColor = Color.LightGray;

                row.Cells[2].Value = signal.Unit;
                row.Cells[2].Style.BackColor = Color.LightGray;

            }

            _data = new double[_signals.Count];
            updateTimer.Start();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView.Rows)
            {
                int index = (int)row.Tag;
                row.Cells[1].Value = _data[index].ToString(_format);
            }
        }

        private void hideSignalNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView.Columns[0].Visible = !hideSignalNameToolStripMenuItem.Checked;
        }

        private void hideSignalUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView.Columns[2].Visible = !hideSignalUnitToolStripMenuItem.Checked;
        }
    }
}
