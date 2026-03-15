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
using Mpal.Debugger;

namespace Mpal.Editor
{
    internal class VarTreeModel : TreeModelBase
    {
        private List<VarTreeItem> _items = new List<VarTreeItem>();
        private Unit _unit = null;
        private List<VarTreeItem> _rootItems = new List<VarTreeItem>();
        private string _fname = "";

        public VarTreeModel(Unit unit)
        {
            _unit = unit;            
        }

        internal List<VarTreeItem> Items
        {
            get { return _items; }
        }

        public bool UpdateModel(List<VariableInfo> variables, string fname)
        {

            if (_fname != fname)
            {
                RemoveAllNodes();
                InsertNewNodes(variables);
                _fname = fname;
                return true;
            }

            return false;            
        }

        private void InsertNewNodes(List<VariableInfo> variables)
        {
            try
            {
                object[] objArray;
                int[] ind;
                _rootItems.Clear();

                List<VarTreeItem> items = new List<VarTreeItem>();
                NumberFormatInfo info = new NumberFormatInfo();
                info.NumberDecimalSeparator = ".";

                foreach (VariableInfo varInfo in variables)
                {
                    VarTreeItem item = new VarTreeItem(varInfo);
                    _items.Add(item);
                    items.Add(item);
                    _rootItems.Add(item);
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

                if (items.Count != 0)
                    base.OnNodesInserted(new TreeModelEventArgs(path, ind, objArray));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void RemoveAllNodes()
        {
            object[] objArray;
            int[] ind;

            List<VarTreeItem> items = new List<VarTreeItem>();

            foreach (VarTreeItem item in _rootItems)
                items.Add(item);

            _items.Clear();

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

            switch (item.VarInfo.DataType)
            {
                case DataType.STRUCT:
                case DataType.FB:
                case DataType.UDT:
                case DataType.ARRAY:
                {
                    foreach (VariableInfo param in item.VarInfo.Variables)
                    {
                        VarTreeItem item1 = new VarTreeItem(param);
                        items.Add(item1);
                        _items.Add(item1);
                    }
                }
                break;
            }

            return items;
        }
        
        
        public override bool IsLeaf(TreePath treePath)
        {
            VarTreeItem item = (VarTreeItem)treePath.FullPath[treePath.FullPath.Length - 1];

            return (item.VarInfo.Variables.Count == 0);
        }
    }
}
