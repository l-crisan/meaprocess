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
using System.Windows.Forms;

namespace Mp.Scheme.Designer
{
    public partial class DesignerOptionDlg : Form
    {
        public DesignerOptionDlg()
        {
            InitializeComponent();

            gridSize.SelectedIndex = GetIndex(Properties.Settings.Default.GridSize);
            snapLines.Checked = Properties.Settings.Default.SnapToLine;
            snapGrid.Checked = Properties.Settings.Default.SnapToGrid;
        }

        private int GetIndex(int size)
        {
            switch (size)
            {
                case 4:
                    return 0;
                case 8:
                    return 1;
                case 16:
                    return 2;
                case 32:
                    return 3;
            }

            return 0;
        }

        
        private int GetGridSize(int index)
        {
            switch (index)
            {
                case 0:
                    return 4;
                case 1:
                    return 8;
                case 2:
                    return 16;
                case 3:
                    return 32;
            }

            return 16;
        }


        private void OnOKClick(object sender, EventArgs e)
        {
            Properties.Settings.Default.GridSize = GetGridSize(gridSize.SelectedIndex);
            Properties.Settings.Default.SnapToLine = snapLines.Checked;
            Properties.Settings.Default.SnapToGrid = snapGrid.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
