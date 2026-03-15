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
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    /// <summary>
    /// The splitter process station property dialog.
    /// </summary>
    internal partial class SplitterPSDlg : Form
    {
        private bool _outputNodeRemoved = false;
        private bool _dragInputTree;
        private Hashtable _signalIDMapping = new Hashtable();
        private bool _dragOutputTree;
        private TreeModel _inputModel;
        private TreeModel _outputModel;
        private Document _doc;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SplitterPSDlg(Document doc)
        {
            _doc = doc;
            InitializeComponent();
            this.Icon = Document.AppIcon;
            _treeImages.Images.Add(Images.Port);
            _treeImages.Images.Add(Images.Port);
            _treeImages.Images.Add(Images.Signal);

            //Input tree
            _inputModel = new TreeModel();

            NodeTextBox nodeTextBox = new NodeTextBox();
            NodeStateIcon nodeStateIcon = new NodeStateIcon();
            nodeStateIcon.DataPropertyName = "Image";
            nodeTextBox.DataPropertyName = "Text";
            nodeTextBox.EditEnabled = false;
            _inputTree.NodeControls.Add(nodeStateIcon);
            _inputTree.NodeControls.Add(nodeTextBox);

            _inputTree.Model = _inputModel;

            //Output tree
            nodeTextBox = new NodeTextBox();
            nodeStateIcon = new NodeStateIcon();
            nodeStateIcon.DataPropertyName = "Image";
            nodeTextBox.DataPropertyName = "Text";
            nodeTextBox.EditEnabled = false;
            _outputModel = new TreeModel();
            _outputTree.NodeControls.Add(nodeStateIcon);
            _outputTree.NodeControls.Add(nodeTextBox);
            _outputTree.Model = _outputModel;
        }

        /// <summary>
        /// Sets the input ports of the process stattion.
        /// </summary>
        public List<Port> InputPorts;

        /// <summary>
        /// Sets the output ports of the process station.
        /// </summary>
        public List<Port> OutputPorts;

        /// <summary>
        /// Sets the process station document. 
        /// </summary>
        public Document Document;

        /// <summary>
        /// Sets or gets the process station name.
        /// </summary>
        public string PsName;

        private void LoadTheInputTree()
        {
            XmlElement xmlSignal;

            _psName.Text = PsName;
            Node parentTreeNode;
            //Load the input signals.
            int portNo = 0;
            foreach (Port port in InputPorts)
            {
                if (port.Type != "Mp.Port.In")
                    continue;

                Node node = new Node("Port " + (portNo + 1).ToString());
                node.Image = Mp.Scheme.Sdk.Images.Port.ToBitmap();
                _inputModel.Nodes.Add(node);

                parentTreeNode = node;

                if (port.SignalList == null)
                    continue;

                parentTreeNode.Tag = port;

                foreach (XmlNode xmlnode in port.SignalList.ChildNodes)
                {
                    xmlSignal = xmlnode as XmlElement;

                    if (xmlSignal == null)
                        continue;

                    uint id = XmlHelper.GetObjectID(xmlSignal);

                    if (id == 0)
                    {
                        id = Convert.ToUInt32(xmlSignal.InnerText);
                        xmlSignal = _doc.GetXmlObjectById(id);
                    }
                    _signalIDMapping[id] = xmlSignal;

                    node = new Node(XmlHelper.GetParam(xmlSignal, "name"));
                    node.Image = Mp.Scheme.Sdk.Images.Signal.ToBitmap();
                    parentTreeNode.Nodes.Add(node);
                    //treeNode.Override.NodeAppearance.Image = 2;
                    node.Tag = xmlSignal;
                }
                portNo++;
            }

            if (_inputModel.Nodes.Count > 0)
                _inputTree.ExpandAll();

            _inputTree.Update();
        }


        private void LoadTheOutputTree()
        {
            XmlElement xmlSignalRef;
            Node parentTreeNode;
            XmlElement xmlSignal;

            //Load the output signals.
            Node newNode;
            int portNo = 0;
            foreach (Port port in OutputPorts)
            {
                if (port.Type != "Mp.Port.Out")
                    continue;

                Node node = new Node("Port " + (portNo + 1).ToString());
                node.Image = Mp.Scheme.Sdk.Images.Port.ToBitmap();
                parentTreeNode = node;
                _outputModel.Nodes.Add(node);
                node.Tag = port;

                if (port.SignalList == null)
                    continue;

                foreach (XmlNode xmlnode in port.SignalList.ChildNodes)
                {
                    xmlSignalRef = xmlnode as XmlElement;

                    if (xmlSignalRef == null)
                        continue;


                    xmlSignal = (XmlElement)_signalIDMapping[Convert.ToUInt32(xmlSignalRef.InnerText)];

                    if (xmlSignal == null)
                        continue;

                    newNode = new Node(XmlHelper.GetParam(xmlSignal, "name"));
                    newNode.Image = Mp.Scheme.Sdk.Images.Signal.ToBitmap();
                    parentTreeNode.Nodes.Add(newNode);
                    newNode.Tag = xmlSignal;
                }

                portNo++;
            }

            _outputTree.ExpandAll();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadTheInputTree();
            LoadTheOutputTree();
        }


        private void RemoveTheOldRefSignals()
        {
            //Remove the old ref signal lists.
            XmlElement xmlSignalRef;

            foreach (Port port in OutputPorts)
            {
                if (port.Type != "Mp.Port.Out")
                    continue;

                for (int signalRefIdx = 0; signalRefIdx < port.SignalList.ChildNodes.Count; signalRefIdx++)
                {
                    xmlSignalRef = (port.SignalList.ChildNodes[signalRefIdx] as XmlElement);

                    if (xmlSignalRef == null)
                        continue;

                    port.SignalList.RemoveChild(xmlSignalRef);
                    signalRefIdx--;

                }
            }
        }


        private void InsertTheNewRefSignals()
        {
            //Insert the new signal refs.
            Node portNode;
            Node signalNode;
            Port outPort;
            XmlElement xmlSignal;
            uint signalId;

            for (int portIdx = 0; portIdx < _outputModel.Nodes.Count; portIdx++)
            {
                portNode = _outputModel.Nodes[portIdx];
                outPort = (Port)OutputPorts[portIdx];

                for (int signalIdx = 0; signalIdx < portNode.Nodes.Count; signalIdx++)
                {
                    signalNode = portNode.Nodes[signalIdx];
                    xmlSignal = signalNode.Tag as XmlElement;
                    signalId = XmlHelper.GetObjectID(xmlSignal);

                    XmlHelper.CreateElement(outPort.SignalList, "uint32_t", "signalRef", signalId.ToString());
                }
            }
        }


        private void OnOKClick(object sender, EventArgs e)
        {
            PsName = _psName.Text;

            RemoveTheOldRefSignals();
            InsertTheNewRefSignals();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();

        }


        private bool IsSignalAvailable(XmlElement signal, Node mnode)
        {
            foreach (Node node in mnode.Nodes)
            {
                XmlElement serachSignal = (XmlElement)node.Tag;

                if (XmlHelper.GetObjectID(serachSignal) == XmlHelper.GetObjectID(signal))
                    return true;
            }
            return false;
        }


        private void OnAddClick(object sender, EventArgs e)
        {
            Port port;
            XmlElement signal;
            Node newTreeNode;
            Node modelNode;

            foreach (TreeNodeAdv treeNode in _inputTree.SelectedNodes)
            {
                modelNode = treeNode.Tag as Node;

                port = (modelNode.Tag as Port);

                if (port != null)
                {//Add a port 
                    foreach (TreeNodeAdv childTreeNode in treeNode.Children)
                    {
                        modelNode = childTreeNode.Tag as Node;
                        signal = (modelNode.Tag as XmlElement);

                        Node mnode = _outputTree.SelectedNodes[0].Tag as Node;

                        if (IsSignalAvailable(signal, mnode))
                            continue;

                        newTreeNode = new Node(XmlHelper.GetParam(signal, "name"));
                        newTreeNode.Image = Mp.Scheme.Sdk.Images.Signal.ToBitmap();
                        newTreeNode.Tag = signal;

                        mnode.Nodes.Add(newTreeNode);
                    }
                }
                else
                {//Add a signal 

                    signal = (modelNode.Tag as XmlElement);

                    if (signal == null)
                        continue;

                    Node mnode = _outputTree.SelectedNodes[0].Tag as Node;

                    if (IsSignalAvailable(signal, mnode))
                        continue;

                    newTreeNode = new Node(XmlHelper.GetParam(signal, "name"));
                    newTreeNode.Image = Mp.Scheme.Sdk.Images.Signal.ToBitmap();
                    newTreeNode.Tag = signal;

                    mnode.Nodes.Add(newTreeNode);
                }
            }
        }


        private void OnRemoveClick(object sender, EventArgs e)
        {
            if (_outputNodeRemoved)
                return;

            Node parent;
            Node modelNode;

            for (int index = 0; index < _outputTree.SelectedNodes.Count; index++)
            {
                modelNode = _outputTree.SelectedNodes[index].Tag as Node;

                if (modelNode.Tag as XmlElement != null)
                {
                    parent = modelNode.Parent;
                    parent.Nodes.Remove(modelNode);
                    --index;
                }
                else if (modelNode.Tag as Port != null)
                {
                    parent = modelNode;
                    parent.Nodes.Clear();
                }
            }

            _outputNodeRemoved = true;
        }


        private bool EnableAddButton()
        {
            if (_outputTree.SelectedNodes.Count != 1)
            {
                add.Enabled = false;
                return false;
            }

            if (_inputTree.SelectedNodes.Count == 0)
            {
                add.Enabled = false;
                return false;
            }
            Node node = _outputTree.SelectedNode.Tag as Node;
            if (node.Tag as Port == null)
            {
                add.Enabled = false;
                return false;
            }

            add.Enabled = true;
            return add.Enabled;
        }


        private void OnOutputTreeDragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }


        private void OnOutputTreeDragDrop(object sender, DragEventArgs e)
        {
            Node targetTreeNode;
            Node newNode;
            Port targetPort;
            XmlElement signal;

            if (_outputTree.DropPosition.Position != NodePosition.Inside)
                return;

            targetTreeNode = _outputTree.DropPosition.Node.Tag as Node;

            TreeNodeAdv[] selectedNodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));

            if (selectedNodes.Length == 0)
                return;

            if (_dragOutputTree && targetTreeNode != null)
            {//Drag output tree item.
                TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
                Node dropNode = _outputTree.DropPosition.Node.Tag as Node;

                _dragOutputTree = false;

                Node parent;
                if (dropNode.Tag as Port != null)
                    parent = dropNode;
                else
                    parent = dropNode.Parent;

                Node nextItem = dropNode;

                if (_outputTree.DropPosition.Position == NodePosition.After)
                    nextItem = dropNode.NextNode;

                foreach (TreeNodeAdv node in nodes)
                {
                    Node nd = (node.Tag as Node);

                    XmlElement xmlSig = nd.Tag as XmlElement;

                    nd.Parent = null;
                }

                int index = -1;
                index = parent.Nodes.IndexOf(nextItem);
                foreach (TreeNodeAdv node in nodes)
                {
                    Node item = node.Tag as Node;
                    XmlElement xmlSig = item.Tag as XmlElement;

                    if (IsSignalAvailable(xmlSig, parent))
                        continue;

                    if (index == -1)
                        parent.Nodes.Add(item);
                    else
                    {
                        parent.Nodes.Insert(index, item);
                        index++;
                    }
                }

                return;
            }

            if (_dragInputTree)
            {//Drag input tree item.

                _dragInputTree = false;

                targetPort = (targetTreeNode.Tag as Port);

                if (targetPort == null)
                    return;

                foreach (TreeNodeAdv inputTreeNode in selectedNodes)
                {
                    Node modelNode = inputTreeNode.Tag as Node;

                    if (modelNode.Tag as Port != null)
                    {
                        foreach (Node child in modelNode.Nodes)
                        {
                            signal = (child.Tag as XmlElement);

                            if (IsSignalAvailable(signal, targetTreeNode))
                                continue;

                            newNode = new Node(child.Text);
                            newNode.Image = Mp.Scheme.Sdk.Images.Signal.ToBitmap();
                            newNode.Tag = signal;
                            targetTreeNode.Nodes.Add(newNode);
                        }
                    }
                    else if (modelNode.Tag as XmlElement != null)
                    {
                        signal = (modelNode.Tag as XmlElement);

                        if (IsSignalAvailable(signal, targetTreeNode))
                            continue;

                        newNode = new Node(modelNode.Text);
                        newNode.Image = Mp.Scheme.Sdk.Images.Signal.ToBitmap();
                        newNode.Tag = signal;
                        targetTreeNode.Nodes.Add(newNode);

                    }
                }
            }

        }


        private void OnInputTreeSelectionChanged(object sender, EventArgs e)
        {
            EnableAddButton();
        }


        private void OnOutputTreeSelectionChanged(object sender, EventArgs e)
        {
            _outputNodeRemoved = false;
            EnableAddButton();
            remove.Enabled = _outputTree.SelectedNodes.Count > 0;
        }


        private void OnOutputTreeItemDrag(object sender, ItemDragEventArgs e)
        {
            _dragInputTree = false;
            _dragOutputTree = true;

            TreeNodeAdv[] nodes = new TreeNodeAdv[_outputTree.SelectedNodes.Count];
            _outputTree.SelectedNodes.CopyTo(nodes, 0);
            DoDragDrop(nodes, DragDropEffects.Move);
        }


        private void OnInputTreeItemDrag(object sender, ItemDragEventArgs e)
        {
            _dragInputTree = true;
            _dragOutputTree = false;

            if (_inputTree.SelectedNodes.Count == 0)
                return;

            TreeNodeAdv[] nodes = new TreeNodeAdv[_inputTree.SelectedNodes.Count];
            _inputTree.SelectedNodes.CopyTo(nodes, 0);
            DoDragDrop(nodes, DragDropEffects.Move);
        }

        private void OnHelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Document.ShowHelp(this, 330);    
        }


        private void OnHelpClick(object sender, EventArgs e)
        {
            OnHelpRequested(null, null);
        }
    }
}
