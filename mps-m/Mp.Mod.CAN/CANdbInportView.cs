using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Mp.Drv.CANdb;
using Mp.Visual.Tree;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;
using Mp.Scheme.Sdk;
using System.Xml;
using System.IO;

namespace Mp.Mod.CAN
{
    public partial class CANdbInportView : UserControl
    {
        private Manager _canManager = new Manager();
        private List<string> _loadDbFiles = new List<string>();
        private MessageView _msgViewCAN = new MessageView();
        private SearchDlg _searchSignal;

        public CANdbInportView()
        {
            InitializeComponent();
            _msgViewCAN.Dock = DockStyle.Fill;
            panel2.Controls.Add(_msgViewCAN);
            NodeTextBox nodeTextBox = new NodeTextBox();
            NodeStateIcon nodeStateIcon = new NodeStateIcon();
            nodeStateIcon.DataPropertyName = "Image";
            nodeTextBox.DataPropertyName = "Text";
            nodeTextBox.EditEnabled = false;
            canDbTree.NodeControls.Add(nodeStateIcon);
            canDbTree.NodeControls.Add(nodeTextBox);

            canDbTree.Model = new TreeModel();
            _searchSignal = new SearchDlg(canDbTree);
            LoadFilesFromReg();
        }

        public class TreeItemInfo
        {
            public TreeItemInfo(Drv.CANdb.DBInfo db, Drv.CANdb.Message msg, Drv.CANdb.Signal sig)
            {
                Message = msg;
                DBInfo = db;
                Signal = sig;
            }

            public Drv.CANdb.DBInfo DBInfo;
            public Drv.CANdb.Message Message;
            public Drv.CANdb.Signal Signal;
        }

        private void loadDb_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.dbc|*.dbc|*.*|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (LoadDbFile(dlg.FileName, true))
                    if (!_loadDbFiles.Contains(dlg.FileName))
                        _loadDbFiles.Add(dlg.FileName);

                canDbTree.Update();
            }
        }

        private void canDbTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (canDbTree.SelectedNodes.Count == 0)
                return;

            Node node = (Node)canDbTree.SelectedNodes[0].Tag;

            if (node.Tag is DBInfo)
                return;

            ArrayList data = new ArrayList();
            foreach (TreeNodeAdv nd in canDbTree.SelectedNodes)
                data.Add(nd);

            DoDragDrop(data, DragDropEffects.Move);
        }

        private string _template = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                  "<CANdb></CANdb>";

        public void SaveLoadedFilesToReg()
        {
            string key = Document.RegistryKey + "CANdbFiles.mcfg";

            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string cfgFile = path + "\\" + key;

            if (!Directory.Exists(Path.GetDirectoryName(cfgFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(cfgFile));

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(_template);

            string files = "";

            foreach (string file in _loadDbFiles)
            {
                files += file;
                files += ";";

            }

            xmlDoc.DocumentElement.InnerText = files;
            xmlDoc.Save(cfgFile);
        }

        private void removeDataBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (canDbTree.SelectedNode == null)
                return;

            Node node = (Node)canDbTree.SelectedNode.Tag;
            TreeItemInfo info = (TreeItemInfo)node.Tag;

            _loadDbFiles.Remove(info.DBInfo.File);
            ((TreeModel)canDbTree.Model).Nodes.Remove(node);
            canDbTree.Update();
        }

        private void contextMenuStripCANdb_Opening(object sender, CancelEventArgs e)
        {
            if (canDbTree.SelectedNode == null)
            {
                e.Cancel = true;
                return;
            }

            Node node = (Node)canDbTree.SelectedNode.Tag;
            if (node.Tag == null)
            {
                e.Cancel = true;
                return;
            }
            TreeItemInfo info = (TreeItemInfo)node.Tag;

            e.Cancel = info.Signal != null || info.Message != null;
        }

        private void UpdateProperty()
        {

            propertyView.Items.Clear();

            if (canDbTree.SelectedNodes.Count != 1)
                return;

            TreeNodeAdv node = canDbTree.SelectedNode;
            if (node == null)
                return;


            if (((Node)node.Tag).Tag == null)
                return;

            TreeItemInfo info = (TreeItemInfo)((Node)node.Tag).Tag;

            string[] item = new string[2];

            if (info.Signal == null && info.Message != null)
            {
                Drv.CANdb.Message msg = info.Message;

                item[0] = "CAN ID:";
                item[1] = msg.ID.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = "Byte count:";
                item[1] = msg.ByteCount.ToString();
                propertyView.Items.Add(new ListViewItem(item));
            }


            if (info.Signal != null && info.Message != null)
            {
                Drv.CANdb.Signal signal = info.Signal;
                Drv.CANdb.Message msg = info.Message;

                item[0] = "CAN ID:";
                item[1] = msg.ID.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = StringResource.ByteCout;
                item[1] = msg.ByteCount.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = StringResource.Type;
                item[1] = signal.Type.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = StringResource.ModeValue;
                item[1] = signal.ModeValue.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = StringResource.ByteOrder;

                if (signal.ByteOrder == Signal.ByteType.Intel)
                    item[1] = "Intel";
                else
                    item[1] = "Motorola";

                propertyView.Items.Add(new ListViewItem(item));

                item[0] = StringResource.PivotBit;
                item[1] = signal.PivotBit.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = StringResource.BitCount;
                item[1] = signal.BitCount.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = StringResource.DataType;
                item[1] = signal.DataType.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = "Factor:";
                item[1] = signal.Factor.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = "Offset:";
                item[1] = signal.Offset.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = "Minimum:";
                item[1] = signal.Min.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = "Maximum:";
                item[1] = signal.Max.ToString();
                propertyView.Items.Add(new ListViewItem(item));

                item[0] = StringResource.Unit;
                item[1] = signal.Unit;
                propertyView.Items.Add(new ListViewItem(item));
            }
        }

        private void LoadFilesFromReg()
        {
            string key = Document.RegistryKey + "CANdbFiles.mcfg";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string cfgFile = path + "\\" + key;

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(cfgFile);

                string files = xmlDoc.DocumentElement.InnerText;

                string[] array = files.Split(';');

                foreach (string file in array)
                {
                    if (file == "")
                        continue;

                    if (LoadDbFile(file, false))
                        _loadDbFiles.Add(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void canDbTree_SelectionChanged(object sender, EventArgs e)
        {
            UpdateProperty();

            _msgViewCAN.Clear();
            if (canDbTree.SelectedNodes.Count != 1)
                return;

            TreeNodeAdv node = canDbTree.SelectedNode;

            if (node.Tag == null)
                return;

            if (((Node)node.Tag).Tag == null)
                return;

            TreeItemInfo info = (TreeItemInfo)((Node)node.Tag).Tag;
            if (info.Signal != null)
            {
                Drv.CANdb.Signal signal = info.Signal;
                _msgViewCAN.SetRange(signal.PivotBit, signal.BitCount, signal.ByteOrder == Signal.ByteType.Intel);
                _msgViewCAN.SetPivotBit(signal.PivotBit, signal.ByteOrder == Signal.ByteType.Intel);
            }

            if (info.Signal == null && info.Message != null)
            {
                Drv.CANdb.Message msg = info.Message;

                foreach (Drv.CANdb.Signal signal in msg.Signals)
                    _msgViewCAN.SetRange(signal.PivotBit, signal.BitCount, signal.ByteOrder == Signal.ByteType.Intel);
            }
        }

        public void Close()
        {
            _searchSignal.Close();
        }

        private bool LoadDbFile(string file, bool showMessage)
        {

            try
            {
                Index rootIndex = _canManager.AddDbFromFile(file);

                DBInfo db = _canManager.GetDB(rootIndex.DB);

                Node dbNode = new Node(db.Name);
                dbNode.Tag = new TreeItemInfo(db, null, null);
                dbNode.Image = Mp.Mod.CAN.Resource.DB.ToBitmap();
                ((TreeModel)canDbTree.Model).Nodes.Add(dbNode);


                Node messagesNode = new Node(StringResource.Messages);
                messagesNode.Tag = null;
                messagesNode.Image = Mp.Mod.CAN.Resource.Folder.ToBitmap();
                dbNode.Nodes.Add(messagesNode);

                Node signalsNode = new Node(StringResource.Signals);
                signalsNode.Tag = null;
                signalsNode.Image = Mp.Mod.CAN.Resource.Folder.ToBitmap();
                dbNode.Nodes.Add(signalsNode);

                foreach (Mp.Drv.CANdb.Message msg in db.Messages)
                {
                    Node msgNode = new Node(msg.Name);
                    msgNode.Tag = new TreeItemInfo(db, msg, null);
                    msgNode.Image = Mp.Mod.CAN.Resource.Message.ToBitmap();
                    messagesNode.Nodes.Add(msgNode);

                    foreach (Mp.Drv.CANdb.Signal sig in msg.Signals)
                    {
                        Node sigNode = new Node(sig.Name);
                        sigNode.Tag = new TreeItemInfo(db, msg, sig);
                        sigNode.Image = Mp.Mod.CAN.Resource.Signal.ToBitmap();
                        msgNode.Nodes.Add(sigNode);

                        sigNode = new Node(sig.Name);
                        sigNode.Tag = new TreeItemInfo(db, msg, sig);
                        sigNode.Image = Mp.Mod.CAN.Resource.Signal.ToBitmap();
                        signalsNode.Nodes.Add(sigNode);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                if (showMessage)
                    MessageBox.Show(ex.Message, "MeaProcess- Scheme Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void onSearch_Click(object sender, EventArgs e)
        {
            _searchSignal.Show();
        }
    }
}
