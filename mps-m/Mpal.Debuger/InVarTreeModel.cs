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
using System.Text;
using System.Globalization;

using Mpal.Model;
using Mp.Visual.Tree;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;

namespace Mpal.Debugger
{
    internal class InVarTreeModel : TreeModelBase
    {
        private Unit _unit;
        private Function _function;
        private List<VarTreeItem> _items = new List<VarTreeItem>();

        public InVarTreeModel()
        {            
        }

        internal List<VarTreeItem> Items
        {
            get { return _items; }
        }

        public Unit MpalUnit
        {
            set 
            { 
                _unit = value;
                
                RemoveAllNodes();

                _function = _unit.Program;                
                
                InsertNewNodes();
            }

            get
            {
                return _unit;
            }
        }

        public Function MpalFunction
        {
            get { return _function; }
        }

        private void InsertNewNodes()
        {
            object[] objArray;
            int[] ind;

            List<VarTreeItem> items = new List<VarTreeItem>();
            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";

            foreach (Parameter param in _function.Parameters)
            {
                if (param.ParamAccess != Parameter.Access.Input && 
                    param.ParamAccess != Parameter.Access.InOut)
                    continue;

                VarTreeItem item = new VarTreeItem(param);
                
                if (param.ParamDataType != DataType.ARRAY &&
                    param.ParamDataType != DataType.STRUCT &&
                    param.ParamDataType != DataType.UDT &&
                    param.ParamDataType != DataType.FB &&
                    param.DefaultValue  != null)
                {
                    item.SetRowValue(Convert.ToString(param.DefaultValue,info));
                }

                _items.Add(item);
                items.Add(item);
            }


            TreePath path = new TreePath();
            objArray = new object[items.Count];
            ind = new int[items.Count];

            int i = 0;
            foreach (VarTreeItem item in items)
            {
                objArray[i] = item;
                ind[i] = i;
                ++i;
            }

            if(items.Count != 0)
                base.OnNodesInserted(new TreeModelEventArgs(path, ind, objArray));
        }

        private void RemoveAllNodes()
        {
            _items.Clear();

            if (_function == null)
                return;

            object[] objArray;
            int[] ind;

            List<VarTreeItem> items = new List<VarTreeItem>();

            foreach (Parameter param in _function.Parameters)
            {
                if (param.ParamAccess == Parameter.Access.Input ||
                    param.ParamAccess == Parameter.Access.InOut)
                {
                    VarTreeItem item = new VarTreeItem(param);
                    items.Add(item);
                }
            }

            TreePath path = new TreePath();
            objArray = new object[items.Count];
            ind = new int[items.Count];

            int i = 0;
            foreach (VarTreeItem item in items)
            {
                objArray[i] = item;
                ind[i] = i;
                ++i;
            }
            base.OnNodesRemoved(new TreeModelEventArgs(path, ind, objArray));
            
        }

        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            List<VarTreeItem> items = new List<VarTreeItem>();

            if (treePath.FullPath.Length == 0)
                return items;

            VarTreeItem item = (VarTreeItem)treePath.FullPath[treePath.FullPath.Length - 1];
            
            if (item.Param.ParamDataType == DataType.STRUCT)
            {
                foreach (Parameter param in item.Param.Structure)
                {
                    VarTreeItem item1 = new VarTreeItem(param);
                    items.Add(item1);
                    _items.Add(item1);
                }
            }

            if (item.Param.ParamDataType == DataType.UDT)
            {
                Parameter udtParam = (Parameter) _unit.Types[item.Param.TypeName];
                VarTreeItem item1 = new VarTreeItem(udtParam);
                items.Add(item1);
                _items.Add(item1);
            }

            return items;
        }
        
        
        public override bool IsLeaf(TreePath treePath)
        {
            VarTreeItem item = (VarTreeItem)treePath.FullPath[treePath.FullPath.Length - 1];
            if( item.Param.ParamDataType == DataType.STRUCT)
                return false;

            if (item.Param.ParamDataType == DataType.UDT)
                return false;

            return true;
        }
    }
}
