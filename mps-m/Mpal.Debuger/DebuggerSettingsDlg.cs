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
using System.ComponentModel;
using System.Windows.Forms;
using Mpal.Model;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;


namespace Mpal.Debugger
{
    public partial class DebuggerSettingsDlg : Form
    {
        private DebuggerSettings _settings;

        public DebuggerSettingsDlg(DebuggerSettings settings, Unit unit)
        {
            _settings = settings;
            InitializeComponent();
            InitInVarTree(unit);
            InitOutVarTree(unit);
            inputVarTree.ExpandAll();            
            
        }

        public string ServerIP
        {
            get { return serverIP.Text; }
            set { serverIP.Text = value; }
        }

        public uint ServerPort
        {
            get { return Convert.ToUInt32(serverPort.Text); }
            set { serverPort.Text = value.ToString(); }
        }

        public bool BuildInDebugger
        {
            get { return useBuildInDebugger.Checked; }
            set 
            { 
                useBuildInDebugger.Checked = value;
                memSize.Enabled = useBuildInDebugger.Checked;                     
            }
        }

        public uint MemSize
        {
            get{ return Convert.ToUInt32(memSize.Text); }
            set { memSize.Text = value.ToString(); }
        }

        void InitInVarTree(Unit unit)
        {
            NodeStateIcon nodeStateIcon = new NodeStateIcon();
            NodeTextBox nodeTextBoxVarName = new NodeTextBox();
            NodeTextBox nodeTextDefValue = new NodeTextBox();
            NodeTextBox nodeTextBoxVarType = new NodeTextBox();

            InVarTreeModel model = new InVarTreeModel();
            inputVarTree.Model = model;
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
            nodeTextBoxVarType.EditEnabled = true;
            nodeTextBoxVarType.ParentColumn = inputVarTree.Columns[1];

            nodeTextDefValue.DataPropertyName = "Type";
            nodeTextDefValue.EditEnabled = false;
            nodeTextDefValue.ParentColumn = inputVarTree.Columns[2];
            model.MpalUnit = unit;
        }

        void InitOutVarTree(Unit unit)
        {
            NodeStateIcon nodeStateIcon = new NodeStateIcon();
            NodeTextBox nodeTextBoxVarName = new NodeTextBox();
            NodeTextBox nodeTextDefValue = new NodeTextBox();
            NodeTextBox nodeTextBoxVarType = new NodeTextBox();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            _settings.InputVarValues.Clear();
            SaveVarValues(inputVarTree.Root, "", "", _settings.InputVarValues);
            Close();
        }

        private static List<int> BuildTreePath(string[] strArray)
        {
            List<int> path = new List<int>();
            int lenght = 0;
            lenght = strArray.Length;
            for (int i = 1; i < lenght; ++i)
            {
                string[] treeIndexStr = strArray[i].Split('#');
                path.Add(Convert.ToInt32(treeIndexStr[1]));
            }

            return path;
        }

        private void LoadValueVarItem(List<string> pathStrings, TreeViewAdv treeView)
        {
            foreach (string pathData in pathStrings)
            {
                string[] sections = pathData.Split(';');
                string[] varNames = sections[0].Split('/');
                string[] pathString = sections[1].Split('/');

                //Build the path.
                List<int> path = BuildTreePath(pathString);

                string[] paramIndex = pathString[1].Split('#');
                int index = Convert.ToInt32(paramIndex[0]);

                TreeNodeAdv node = treeView.Root;
                bool error = false;

                VarTreeItem item = null;
                int pos = 0;

                foreach (int i in path)
                {
                    if (i >= node.Children.Count)
                    {
                        error = true;
                        break;
                    }

                    node = node.Children[i];
                    item = (VarTreeItem)node.Tag;

                    if(varNames[pos] != item.Name)
                    {
                        error = true;
                        break;
                    }
                    ++pos;
                }

                if (error)
                    continue;

                item = (VarTreeItem)node.Tag;

                if (pathString[0] != "")
                    item.SetRowValue(pathString[0]);
            }
        }

        private void DebugerSettingsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {            
            //FormStateHandler.Save(this, "Atesion\\MPALEditor\\DebuggerSettingsDlg");            
        }

        private void SaveVarValues(TreeNodeAdv parentNode, string parentPath, string varName, List<string> valueMapping)
        {
            int treeIndex = 0;

            foreach (TreeNodeAdv childNode in parentNode.Children)
            {
                VarTreeItem item = (VarTreeItem)childNode.Tag;

                switch (item.Param.ParamDataType)
                {
                    case DataType.STRUCT:
                    case DataType.UDT:
                        SaveVarValues(childNode, parentPath + "/" + item.Param.Index.ToString() + "#" + treeIndex.ToString(),varName + "/" + item.Name, valueMapping);
                        break;

                    case DataType.ARRAY:
                    case DataType.FB:
                        break;

                    default:
                        {
                            string varPath = "";
                            string varNamePath = "";

                            if (parentPath == "")
                            {
                                varPath = "/" + item.Param.Index.ToString() + "#" + treeIndex.ToString();
                                varNamePath = item.Name;
                            }
                            else
                            {
                                varPath = parentPath + "/" + item.Param.Index.ToString() + "#" + treeIndex.ToString();
                                varNamePath = varName + "/" + item.Name;
                            }

                            varNamePath = varNamePath.TrimStart('/');
                            valueMapping.Add(varNamePath + ";" + item.GetRowValue() + varPath);                            
                        }
                        break;
                }
                treeIndex++;
            }
        }

        private void DebugerSettingsDlg_Load(object sender, EventArgs e)
        {
            LoadValueVarItem(_settings.InputVarValues, inputVarTree);
            StartPosition = FormStartPosition.CenterParent;
        }

        private void useBuildInDebugger_CheckedChanged(object sender, EventArgs e)
        {
            if (useBuildInDebugger.Checked)
            {
                serverIP.Text = "127.0.0.1";
                serverIP.Enabled = false;
                memSize.Enabled = true;
            }
            else
            {
                serverIP.Enabled = true;
                memSize.Enabled = false;
            }
        }

        private void serverIP_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            string[] data = serverIP.Text.Split('.');

            if (data.Length != 4)
            {
                errorProvider.SetError(serverIP, StringResource.InvalidIP);
                e.Cancel = true;
                return;
            }

            try
            {
                Convert.ToByte(data[0]);
                Convert.ToByte(data[1]);
                Convert.ToByte(data[2]);
                Convert.ToByte(data[3]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                errorProvider.SetError(serverIP, StringResource.InvalidIP);
                e.Cancel = true;
                return;
            }
        }   

        private void serverPort_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(serverPort.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(serverPort, ex.Message);
                e.Cancel = true;
            }
        }

        private void memSize_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                uint size = Convert.ToUInt32(memSize.Text);
                if (size == 0)
                {
                    e.Cancel = true;
                    errorProvider.SetError(memSize, StringResource.MemSizeErr);
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(memSize, ex.Message);
                e.Cancel = true;
            }
        }
    }
}
