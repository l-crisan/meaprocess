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
using Mp.Scheme.Sdk;
using Mp.Visual.Docking;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;

namespace Mp.Scheme.App
{
    public partial class StationPalletWindow : DockContent
    {
        private TreeModel _model = new TreeModel();
        private Module _engine;

        public StationPalletWindow()
        {
            InitializeComponent();
        
            stationPallet.Model = _model;

            NodeTextBox nodeTextBox = new NodeTextBox();
            nodeTextBox.ToolTipProvider = new PalletToolTipProvider();
            NodeStateIcon nodeStateIcon = new NodeStateIcon();
            nodeStateIcon.DataPropertyName = "Image";
            nodeTextBox.DataPropertyName = "Text";
            nodeTextBox.EditEnabled = false;
            nodeTextBox.DrawText += new EventHandler<DrawEventArgs>(OnNodeTextBoxDrawText);
            stationPallet.NodeControls.Add(nodeStateIcon);
            stationPallet.NodeControls.Add(nodeTextBox);
            TabText = Text;
        }


        public void LoadResources()
        {
            Utils.ResourceLoader.LoadResources(this);

            if (_engine != null)
            {
                RemovePalette();
                LoadPalette(_engine);
            }

            Width = Width + 1;
            TabText = Text;
        }


        private void OnNodeTextBoxDrawText(object sender, DrawEventArgs e)
        {
            Node nd = (Node)e.Node.Tag;

            
            if (nd.Tag == null)
            {
                Rectangle rect = new Rectangle(stationPallet.ClientRectangle.Left+18, e.Context.Bounds.Top, stationPallet.ClientRectangle.Width,
                                                e.Context.Bounds.Height);

                e.Context.Graphics.Clip = new Region(rect);
                e.Context.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.ControlDark)), rect);
                e.Context.Graphics.DrawRectangle(new Pen(Color.White,1), rect);
                e.Context.Graphics.DrawImage(nd.Image, new Point(rect.Left+2, rect.Top+2));
                e.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            else
                e.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }


        private bool ExistGroupInPalette(string group)
        {
            return GetNode(group) != null;
        }


        private void OnStationPalletItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Item;
            Node nd = (Node)nodes[0].Tag;
            ProcessStation station = (ProcessStation)nd.Tag;

            if (station != null)
                DoDragDrop(station.GetType().ToString(), DragDropEffects.Move);
        }


        public void RemovePalette()
        {
            _model.Nodes.Clear();
            stationPallet.Invalidate();
        }


        private Node GetNode(string text)
        {
            foreach (Node node in _model.Nodes)
            {
                if (node.Text == text)
                    return node;
            }
            return null;

        }


        public void LoadPalette(Mp.Scheme.Sdk.Module engine)
        {
            _engine = engine;

            foreach (ProcessStation station in engine.Stations)
            {
                station.OnLoadResources();

                Node parent;
                Node node;

                if (!ExistGroupInPalette(station.Group))
                {
                    parent = new Node(station.Group);
                    parent.Image = Mp.Scheme.App.Resource.Folderopen.ToBitmap();
                    _model.Nodes.Add(parent);
                }
                else
                {
                    parent = GetNode(station.Group);
                }

         
                node = new Node(station.Text);
                node.Image = station.Icon.ToBitmap();
                node.Tag = station;
                parent.Nodes.Add(node);
            }
            stationPallet.ExpandAll();
            stationPallet.Invalidate();
        }


        private void OnExpandAllClick(object sender, EventArgs e)
        {
            stationPallet.ExpandAll();
        }


        private void OnColapseAllClick(object sender, EventArgs e)
        {
            stationPallet.CollapseAll();
        }


        private void OnSearchBlockTextChanged(object sender, EventArgs e)
        {
            string what = searchBlock.Text;

            foreach(TreeNodeAdv parentNode in  stationPallet.Root.Children)
            {
                foreach(TreeNodeAdv node in parentNode.Children)
                {
                    Node item = (Node) node.Tag;

                    if (!item.Text.Contains(what))
                        continue;
                    
                    stationPallet.EnsureVisible(node);
                    stationPallet.SelectedNode = node;                             
                    stationPallet.Update();
                    return;                    
                }
            }
        }    
    }
}
