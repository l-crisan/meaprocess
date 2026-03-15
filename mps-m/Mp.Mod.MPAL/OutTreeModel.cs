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
using System.Text;

using Mp.Visual.Tree;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;
using Mpal.Model;

namespace Mp.Mod.MPAL
{
    internal class MpalOutTreeModel : TreeModelBase
    {
        private Unit _unit;
        private Function _function;
        private List<MpalOutTreeItem> _items = new List<MpalOutTreeItem>();

        public MpalOutTreeModel()
        {
        }

        public List<MpalOutTreeItem> Items
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
        }

        private void InsertNewNodes()
        {
            object[] objArray;
            int[] ind;

            List<MpalOutTreeItem> items = new List<MpalOutTreeItem>();

            foreach (Parameter param in _function.Parameters)
            {
                if (param.ParamAccess != Parameter.Access.Output && param.ParamAccess != Parameter.Access.InOut)
                    continue;

                MpalOutTreeItem item = new MpalOutTreeItem(param);
                _items.Add(item);
                items.Add(item);
            }


            TreePath path = new TreePath();
            objArray = new object[items.Count];
            ind = new int[items.Count];

            int i = 0;
            foreach (MpalOutTreeItem item in items)
            {
                objArray[i] = item;
                ind[i] = i;
                ++i;
            }

            if (items.Count != 0)
                base.OnNodesInserted(new TreeModelEventArgs(path, ind, objArray));
        }

        private void RemoveAllNodes()
        {
            _items.Clear();
            if (_function == null)
                return;

            object[] objArray;
            int[] ind;

            List<MpalOutTreeItem> items = new List<MpalOutTreeItem>();

            foreach (Parameter param in _function.Parameters)
            {
                if (param.ParamAccess == Parameter.Access.Output ||
                    param.ParamAccess == Parameter.Access.InOut)
                {
                    MpalOutTreeItem item = new MpalOutTreeItem(param);
                    items.Add(item);
                }
            }

            TreePath path = new TreePath();
            objArray = new object[items.Count];
            ind = new int[items.Count];

            int i = 0;
            foreach (MpalOutTreeItem item in items)
            {
                objArray[i] = item;
                ind[i] = i;
                ++i;
            }            
            base.OnNodesRemoved(new TreeModelEventArgs(path, ind, objArray));
        }

        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            List<MpalOutTreeItem> items = new List<MpalOutTreeItem>();

            if (treePath.FullPath.Length == 0)
                return items;

            MpalOutTreeItem item = (MpalOutTreeItem)treePath.FullPath[treePath.FullPath.Length - 1];

            if (item.Param.ParamDataType == DataType.STRUCT)
            {
                foreach (Parameter param in item.Param.Structure)
                {
                    MpalOutTreeItem item1 = new MpalOutTreeItem(param);
                    _items.Add(item1);
                    items.Add(item1);
                }
            }

            if (item.Param.ParamDataType == DataType.UDT)
            {
                Parameter udtParam = (Parameter)_unit.Types[item.Param.TypeName];

                MpalOutTreeItem item1 = new MpalOutTreeItem(udtParam);
                _items.Add(item1);
                items.Add(item1);
            }

            return items;
        }

        public override bool IsLeaf(TreePath treePath)
        {
            MpalOutTreeItem item = (MpalOutTreeItem)treePath.FullPath[treePath.FullPath.Length - 1];
            if (item.Param.ParamDataType == DataType.STRUCT)
                return false;

            if (item.Param.ParamDataType == DataType.UDT)
                return false;

            return true;
        }
    }
}
