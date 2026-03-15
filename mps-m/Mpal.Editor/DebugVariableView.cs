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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Mpal.Debugger;
using Mpal.Model;
using Mp.Visual.Tree;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;
using Mp.Visual.Docking;

namespace Mpal.Editor
{
    public partial class DebugVariableView : DockContent
    {
        private VarTreeModel _model;
        private Unit _unit;
        public DebugVariableView(Unit unit)
        {
            _unit = unit;
            _model = new VarTreeModel(_unit);
            InitializeComponent();
            InitInVarTree();            
        }

        private void InitInVarTree()
        {
            NodeStateIcon nodeStateIcon = new NodeStateIcon();
            NodeTextBox nodeTextBoxVarName = new NodeTextBox();
            NodeTextBox nodeTextDefValue = new NodeTextBox();
            NodeTextBox nodeTextBoxVarType = new NodeTextBox();


            inputVarTree.Model = _model;
            inputVarTree.GridLineStyle = GridLineStyle.HorizontalAndVertical;

            inputVarTree.NodeControls.Add(nodeStateIcon);
            inputVarTree.NodeControls.Add(nodeTextBoxVarName);
            inputVarTree.NodeControls.Add(nodeTextDefValue);
            inputVarTree.NodeControls.Add(nodeTextBoxVarType);

            nodeStateIcon.DataPropertyName = "Icon";
            nodeStateIcon.ParentColumn = inputVarTree.Columns[0];

            nodeTextBoxVarName.DataPropertyName = "Name";
            nodeTextBoxVarName.EditEnabled = false;
            nodeTextBoxVarName.ParentColumn = inputVarTree.Columns[0];

            nodeTextBoxVarType.DataPropertyName = "Value";
            nodeTextBoxVarType.EditEnabled = false;
            nodeTextBoxVarType.ParentColumn = inputVarTree.Columns[1];

            nodeTextDefValue.DataPropertyName = "Type";
            nodeTextDefValue.EditEnabled = false;
            nodeTextDefValue.ParentColumn = inputVarTree.Columns[2];            
        }

        public void UpdateView(List<VariableInfo>  variables, string fname)
        {
            if (variables == null)
            {
                _model = new VarTreeModel(_unit);
                inputVarTree.Model = _model;
            }
            else
            {
                if(_model.UpdateModel(variables, fname))
                    inputVarTree.ExpandAll();
            }

            inputVarTree.Invalidate();
        }
    }
}
