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
using System.Drawing;
using System.Windows.Forms;

namespace Mp.Visual.Base
{
    [Serializable]
    public partial class VisualBlock : UserControl
    {
        public delegate void ColumnsChangedDelegate();

        public event ColumnsChangedDelegate ColumnsChanged;

        public VisualBlock()
        {
            InitializeComponent();
        }

        public Color ContainerBackColor
        {
            get
            {
                return this.BackColor;
            }

            set
            {
                this.BackColor = value;
            }
        }


        public int ColumnCount
        {
            get { return tableLayoutPanel.ColumnCount; }
            set 
            { 
                tableLayoutPanel.ColumnCount = value;

                if(ColumnsChanged != null)
                    ColumnsChanged();
            }
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        public virtual void UpdateProperties()
        {

            int percentPerCol = tableLayoutPanel.ColumnCount != 0 ? 100 / tableLayoutPanel.ColumnCount:  1;
            int percentPerRow = tableLayoutPanel.RowCount != 0 ? 100 / tableLayoutPanel.RowCount : 1;

            int cols = ColumnCount;
            int rows = RowCount;

            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();

            for(int i = 0; i <cols ; ++i)
               tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percentPerCol));

            for (int i = 0; i < rows; ++i)
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, percentPerRow));

            Invalidate();
        }


        public int RowCount
        {
            get
            {
              return tableLayoutPanel.RowCount;
            }

            set
            {
              tableLayoutPanel.RowCount = value;
            }
        }


        public void AddControl(Control ctrl)
        {
            ctrl.Dock = DockStyle.Fill;

            if (tableLayoutPanel.Controls.Contains(ctrl))
                RemoveControl(ctrl);

            tableLayoutPanel.Controls.Add(ctrl);
        }


        public TableLayoutControlCollection ControlsInContainer
        {
            get { return tableLayoutPanel.Controls; }
        }


        public void RemoveControl(Control ctrl)
        {
            tableLayoutPanel.Controls.Remove(ctrl);
        }


        public void Clear()
        {
            RowCount = 0;
            tableLayoutPanel.Controls.Clear();
        }
    }
}
