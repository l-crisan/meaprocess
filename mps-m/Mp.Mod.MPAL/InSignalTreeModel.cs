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
using System.Xml;
using Mp.Utils;
using Mp.Visual.Tree.Tree;
using Mp.Scheme.Sdk;

namespace Mp.Mod.MPAL
{
    internal class MpalInSignalTreeModel : TreeModelBase
    {
        private List<Port> _inputPorts;
        private Document _doc;
        
        public MpalInSignalTreeModel(List<Port> inputPorts,Document doc)
        {
            _inputPorts = inputPorts;
            _doc = doc;
        }

        public override System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            List<MpalInSignalTreeItem> items = new List<MpalInSignalTreeItem>();
            MpalInSignalTreeItem parentItem = null;
            
            if( treePath.FullPath.Length != 0)
                parentItem = treePath.FullPath[treePath.FullPath.Length - 1] as MpalInSignalTreeItem;

            MpalInSignalTreeItem item = null;

            if (parentItem == null)
            {//Load ports
                int i = 1;
                foreach (Port port in _inputPorts)
                {
                    item = new MpalInSignalTreeItem("Port " + i.ToString(), port);
                    items.Add(item);
                    ++i;
                }
            }
            else if( parentItem.IsPort)
            {//load signals
                if (parentItem.PortObj.SignalList != null)
                {
                    foreach (XmlElement xmlSignal in parentItem.PortObj.SignalList)
                    {
                        item = new MpalInSignalTreeItem(xmlSignal, _doc);
                        items.Add(item);
                    }
                }
            }
            else if (!parentItem.IsPort)
            {
                item = new MpalInSignalTreeItem(parentItem.Signal, PropertyType.SampleRate, _doc);                
                items.Add(item);

                item = new MpalInSignalTreeItem(parentItem.Signal, PropertyType.Minimum, _doc);
                items.Add(item);

                item = new MpalInSignalTreeItem(parentItem.Signal, PropertyType.Maximum, _doc);
                items.Add(item);

                XmlElement xmlScaling = XmlHelper.GetChildByType(parentItem.Signal, "Mp.Scaling");
                if (xmlScaling != null)
                {
                    item = new MpalInSignalTreeItem(parentItem.Signal, PropertyType.Factor, _doc);
                    items.Add(item);

                    item = new MpalInSignalTreeItem(parentItem.Signal, PropertyType.Offset, _doc);
                    items.Add(item);
                }
            }

            return items;
        }

        public override bool IsLeaf(TreePath treePath)
        {
            //Is a signal => leaf

            MpalInSignalTreeItem item = treePath.FullPath[treePath.FullPath.Length - 1] as MpalInSignalTreeItem;
            return item.IsSignalProp;
        }
    }
}
