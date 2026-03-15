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
using System.IO;
using System.Reflection;
using Mp.Scheme.Sdk;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;
using Mp.Utils;

namespace Mp.Scheme.MM
{
    /// <summary>
    /// Show the module management dialog.
    /// </summary>
    public partial class ModuleManagerDlg : Form
    {
        private ModuleManager _moduleManager = new ModuleManager();
        private string _meaProcessModulePath;
        private bool _modified = false;
        private TreeModel _model;

        public ModuleManagerDlg()
        { 
            InitializeComponent();
            FormStateHandler.Restore(this, "Mp.Scheme.MM.ModuleManagerDlg");
        }


        private void OnCloseClick(object sender, EventArgs e)
        {
            try
            {
                _moduleManager.SaveAllRegModules(_meaProcessModulePath);
                if (_modified)
                    MessageBox.Show(StringResource.ModifiedMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
          
            Close();
        }


        private void OnnAddClick(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "*.dll|*.dll";
            
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            try
            {
                Mp.Scheme.Sdk.Module module = ModuleLoader.GetInstance(dlg.FileName);

                if (module != null)
                {
                    if (module.ParentType == "" || module.ParentType == null)
                    {//Is a runtime
                        _moduleManager.AllRuntimeEngines.Add(module);
                        moduleTree.Update();
                        _modified = true;
                    }
                    else
                    {
                        TreeNodeAdv treeNode = moduleTree.SelectedNode;
                        if (treeNode == null)
                        {
                            MessageBox.Show(StringResource.LoadErr, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        TreeItem item = treeNode.Tag as TreeItem;
                        if (item.ItemModule == null)
                        {
                            MessageBox.Show(StringResource.LoadErr, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (!item.ItemModule.Type.Contains(module.ParentType) && module.ParentType != "General")
                        {
                            MessageBox.Show(StringResource.LoadErr, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        item.ItemModule.Modules.Add(module);
                    }
                }
                else
                {
                    TreeNodeAdv treeNode = moduleTree.SelectedNode;
                    if (treeNode == null)
                    {
                        MessageBox.Show(StringResource.LoadErrSelRuntime, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    TreeItem item = treeNode.Tag as TreeItem;
                    if (item.ItemModule == null)
                    {
                        MessageBox.Show(StringResource.LoadErrSelRuntime, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!item.ItemModule.SupportWindows)
                    {
                        MessageBox.Show(StringResource.LoadErrSelRuntime, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(StringResource.LoadErr, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _modified = true;

            _model.Reload();
            moduleTree.ExpandAll();
            moduleTree.Update();
        }


        private void OnRemoveClick(object sender, EventArgs e)
        {
            TreeNodeAdv treeNode = moduleTree.SelectedNode;

            if (treeNode == null)
                return;

            DialogResult res = MessageBox.Show(StringResource.RemoveConfirmation, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res != System.Windows.Forms.DialogResult.Yes)
                return;

            TreeItem item = treeNode.Tag as TreeItem;
            if (item.ItemModule != null)
            {
                _moduleManager.RemoveModule(item.ItemModule);            
            }
            _modified = true;
            _model.Reload();
            moduleTree.ExpandAll();
            moduleTree.Update();

        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);        
            FormStateHandler.Save(this, "Mp.Scheme.MM.ModuleManagerDlg");
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            Close();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                Assembly asm = Assembly.GetAssembly(this.GetType());
                string file = Path.GetDirectoryName(asm.Location);
                _meaProcessModulePath = Path.Combine(file, "module.set");

                using (FileStream fs = new FileStream(_meaProcessModulePath, FileMode.Open, FileAccess.Read))
                {
                    StreamReader sr = new StreamReader(fs);
                    _moduleManager.LoadAllRegModules(sr.ReadToEnd(), this.Handle);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            NodeStateIcon nodeStateIcon = new NodeStateIcon();
            NodeTextBox nodeTextBoxModule = new NodeTextBox();
            NodeTextBox nodeTextBoxVersion = new NodeTextBox();
            NodeTextBox nodeTextBoxCreated = new NodeTextBox();
            NodeTextBox nodeTextBoxManufacturer = new NodeTextBox();
            NodeTextBox nodeTextBoxDescription = new NodeTextBox();
            NodeTextBox nodeTextBoxFile = new NodeTextBox();

            _model = new TreeModel(_moduleManager);

            moduleTree.Model = _model;

            moduleTree.GridLineStyle = GridLineStyle.HorizontalAndVertical;

            moduleTree.NodeControls.Add(nodeStateIcon);
            moduleTree.NodeControls.Add(nodeTextBoxModule);
            moduleTree.NodeControls.Add(nodeTextBoxVersion);
            moduleTree.NodeControls.Add(nodeTextBoxCreated);
            moduleTree.NodeControls.Add(nodeTextBoxManufacturer);
            moduleTree.NodeControls.Add(nodeTextBoxDescription);
            moduleTree.NodeControls.Add(nodeTextBoxFile);

            nodeStateIcon.DataPropertyName = "Icon";
            nodeStateIcon.ParentColumn = moduleTree.Columns[0];

            nodeTextBoxModule.EditEnabled = false;
            nodeTextBoxModule.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxModule.DisplayHiddenContentInToolTip = true;
            nodeTextBoxModule.DataPropertyName = "Name";
            nodeTextBoxModule.ParentColumn = moduleTree.Columns[0];

            nodeTextBoxVersion.EditEnabled = false;
            nodeTextBoxVersion.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxVersion.DisplayHiddenContentInToolTip = true;
            nodeTextBoxVersion.DataPropertyName = "Version";
            nodeTextBoxVersion.ParentColumn = moduleTree.Columns[1];

            nodeTextBoxCreated.EditEnabled = false;
            nodeTextBoxCreated.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxCreated.DisplayHiddenContentInToolTip = true;
            nodeTextBoxCreated.DataPropertyName = "Created";
            nodeTextBoxCreated.ParentColumn = moduleTree.Columns[2];

            nodeTextBoxManufacturer.EditEnabled = false;
            nodeTextBoxManufacturer.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxManufacturer.DisplayHiddenContentInToolTip = true;
            nodeTextBoxManufacturer.DataPropertyName = "Manufacturer";
            nodeTextBoxManufacturer.ParentColumn = moduleTree.Columns[3];

            nodeTextBoxDescription.EditEnabled = false;
            nodeTextBoxDescription.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxDescription.DisplayHiddenContentInToolTip = true;
            nodeTextBoxDescription.DataPropertyName = "Description";
            nodeTextBoxDescription.ParentColumn = moduleTree.Columns[4];

            nodeTextBoxFile.EditEnabled = false;
            nodeTextBoxFile.Trimming = StringTrimming.EllipsisCharacter;
            nodeTextBoxFile.DisplayHiddenContentInToolTip = true;
            nodeTextBoxFile.DataPropertyName = "File";
            nodeTextBoxFile.ParentColumn = moduleTree.Columns[5];

            moduleTree.ExpandAll();
            moduleTree.Update();
        }


        private void OnHelpClick(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 290);
        }
    }
}