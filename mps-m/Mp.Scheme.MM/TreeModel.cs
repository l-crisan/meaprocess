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
using System.Collections.Generic;
using Mp.Visual.Tree.Tree;
using Mp.Scheme.Sdk;

namespace Mp.Scheme.MM
{
    internal class TreeModel : TreeModelBase
    {
        private ModuleManager _mm;
        private List<TreeItem> _rootItems = new List<TreeItem>();

        public TreeModel(ModuleManager mm)
        {
            _mm = mm;
        }

        public void Reload()
        {
            RemoveAllNodes();
            InsertNewNodes();
        }

        private void RemoveAllNodes()
        {            

            TreePath path = new TreePath();
            object[] objArray = new object[_rootItems.Count];
            int[] ind = new int[_rootItems.Count];

            int i = 0;
            foreach (TreeItem item in _rootItems)
            {
                objArray[i] = item;
                ind[i] = i;
                ++i;
            }

            OnNodesRemoved(new TreeModelEventArgs(path, ind, objArray));
        }

        private void InsertNewNodes()
        {
            object[] objArray;
            int[] ind;

            _rootItems.Clear();

            foreach (Mp.Scheme.Sdk.Module module in _mm.AllRuntimeEngines)
                _rootItems.Add(new TreeItem(module));


            TreePath path = new TreePath();
            objArray = new object[_rootItems.Count];
            ind = new int[_rootItems.Count];

            int i = 0;
            foreach (TreeItem item in _rootItems)
            {
                objArray[i] = item;
                ind[i] = i;
                ++i;
            }

            if (_rootItems.Count != 0)
                OnNodesInserted(new TreeModelEventArgs(path, ind, objArray));
        }


        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            List<TreeItem> items = new List<TreeItem>();

            if (treePath.FullPath.Length == 0)
            {
                _rootItems.Clear();

                foreach(Mp.Scheme.Sdk.Module module in _mm.AllRuntimeEngines)
                    _rootItems.Add(new TreeItem(module));

                return _rootItems;
            }

            TreeItem item = (TreeItem)treePath.FullPath[treePath.FullPath.Length - 1];

            if (item.ItemModule != null)
            {
                foreach (Mp.Scheme.Sdk.Module module in item.ItemModule.Modules)
                {
                    TreeItem newItem = new TreeItem(module);
                    items.Add(newItem);
                }
            }

            return items;
        }


        public override bool IsLeaf(TreePath treePath)
        {
            TreeItem item = (TreeItem)treePath.FullPath[treePath.FullPath.Length - 1];
            
            if (item.ItemModule == null)
                return true;

            return (item.ItemModule.ParentType != null);
        }
    }
}
