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
using System.Windows.Forms;
using Mp.Visual.Docking;
using Mp.Utils;

namespace Mp.Scheme.Designer
{
    internal partial class PropertieWindow : DockContent
    {
        private PropertyGrid _grid = new PropertyGrid();

        public PropertieWindow()
        {
            InitializeComponent();
            TabText = this.Text;
            _grid.Dock = DockStyle.Fill;
            _grid.PropertySort = PropertySort.Categorized;
            _grid.ToolbarVisible = true;
            _grid.CommandsVisibleIfAvailable = false;

            Controls.Add(_grid);
            _grid.BringToFront();
        }


        public void LoadResources()
        {
            ResourceLoader.LoadResources(_grid);
            TabText = Text;
        }


        public PropertyGrid PropertyGrid
        {
            get { return _grid; }
        }


        public CustomTypeDescriptor SelectedObject
        {
            set 
            { 
                if (value != null)
                {
                    if (value.HostControl != null)
                        processStation.Text = value.HostControl.Name;
                }

                _grid.SelectedObject = value;
            }

            get {return (CustomTypeDescriptor)_grid.SelectedObject; }
        }
   }
}
