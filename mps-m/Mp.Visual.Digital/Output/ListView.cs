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
using System.Windows.Forms;

namespace Mp.Visual.Digital
{
    [Serializable]
    public partial class ListView : UserControl
    {
        public class Signal
        {
            public string Name;
            public string Unit;
            public int Index;
        }

        private int _samples = 5;
        private int _precision = 3;
        private string _format = "";
        private int _curPos = 0;

        public ListView()
        {
            InitializeComponent();
            NoOfSamples = 5;
            Precision = 3;
            this.Width = 400;
            this.Height = 400;     
        }

        [SRCategory("View"), SRDescriptionAttribute("NoOfSamples")]
        public int NoOfSamples
        {
            get { return _samples; }
            set 
            { 
                _samples = value;
                if (_samples == 0)
                    _samples = 1;
                
                InitDone();
            }
        }

        public List<ListView.Signal> Signals
        {
            get
            {
                List<ListView.Signal> signals = new List<Signal>();

                foreach(DataGridViewColumn column in  gridView.Columns)
                    signals.Add(column.Tag as ListView.Signal);

                return signals;
            }
        } 

        public void InitDone()
        {
            try
            {
                if(gridView.Columns.Count == 0)
                    return;

                gridView.Rows.Clear();
                _curPos = 0;

                for (int i = 0; i < _samples; ++i)
                    gridView.Rows.Add();                

                ResizeColumns();
            }
            catch(Exception ex)
            {
                Console.Write(ex);
            }
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

                InitDone();

            }
        }


        public void RemoveSignal(ListView.Signal signal)
        {
            for(int i = 0; i < gridView.Columns.Count; ++i)
            {   
                if(gridView.Columns[i].Tag == signal)
                {
                    gridView.Columns.RemoveAt(i);
                    break;
                }
            }
        }

        public void AddSignal(ListView.Signal signal)
        {
            string id = signal.Name;

            if (signal.Unit != null && signal.Unit != "")
                id += " (" + signal.Unit + ")";

            ColumnHeader colHeader = new ColumnHeader();
            colHeader.Tag = signal;

            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            column.HeaderText = id;            
            column.Tag = signal;
            gridView.Columns.Add(column);
            InitDone();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ResizeColumns();
        }

        private void ResizeColumns()
        {
            if (gridView.Columns.Count == 0)
                return;

            int colWidth = (this.Width -5) / gridView.Columns.Count;

            foreach (DataGridViewColumn col in gridView.Columns)
                col.Width = colWidth;
        }

        public void AddValue(double[] values)
        {
            SuspendLayout();

            gridView.SuspendLayout();

            if (_curPos < gridView.Rows.Count)
            {
                DataGridViewRow row = gridView.Rows[_curPos];

                for (int i = 0; i < values.Length; ++i)
                    row.Cells[i].Value = values[i].ToString(_format);

                _curPos++;
            }
            else
            {
                foreach (DataGridViewRow row in gridView.Rows)
                {
                    if (row.Index == 0)
                        continue;

                    DataGridViewRow lastRow = gridView.Rows[row.Index - 1];

                    for (int i = 0; i < gridView.Columns.Count; ++i)
                        lastRow.Cells[i].Value = row.Cells[i].Value;
                }

                DataGridViewRow curRow = gridView.Rows[gridView.Rows.Count - 1];

                for (int i = 0; i < values.Length; ++i)
                    curRow.Cells[i].Value = values[i].ToString(_format);
            }

            gridView.ResumeLayout();
            ResumeLayout();
        }
    }
}
