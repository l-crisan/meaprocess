namespace Mp.Mod.Controller
{
    partial class FuzzyControllerDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FuzzyControllerDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.channels = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.signalsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeSignalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.fuzzyRulesCtrl = new ICSharpCode.TextEditor.TextEditorControl();
            this.rulesContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemCut6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findAndReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonCut = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRedo = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.help = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.onLingOutput = new System.Windows.Forms.Button();
            this.outLingVar = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.outComment = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.outUnit = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.outMax = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.outMin = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.outRate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.sigName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.channels)).BeginInit();
            this.signalsContextMenuStrip.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.rulesContextMenuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.errorProvider.SetError(this.splitContainer1, resources.GetString("splitContainer1.Error"));
            this.errorProvider.SetIconAlignment(this.splitContainer1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer1, ((int)(resources.GetObject("splitContainer1.IconPadding"))));
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.errorProvider.SetError(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.Error"));
            this.errorProvider.SetIconAlignment(this.splitContainer1.Panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer1.Panel1, ((int)(resources.GetObject("splitContainer1.Panel1.IconPadding"))));
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.errorProvider.SetError(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.Error"));
            this.errorProvider.SetIconAlignment(this.splitContainer1.Panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer1.Panel2, ((int)(resources.GetObject("splitContainer1.Panel2.IconPadding"))));
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.channels);
            this.errorProvider.SetError(this.groupBox2, resources.GetString("groupBox2.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject("groupBox2.IconPadding"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // channels
            // 
            resources.ApplyResources(this.channels, "channels");
            this.channels.AllowDrop = true;
            this.channels.AllowUserToAddRows = false;
            this.channels.AllowUserToDeleteRows = false;
            this.channels.AllowUserToResizeRows = false;
            this.channels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.channels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.channels.ContextMenuStrip = this.signalsContextMenuStrip;
            this.errorProvider.SetError(this.channels, resources.GetString("channels.Error"));
            this.errorProvider.SetIconAlignment(this.channels, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("channels.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.channels, ((int)(resources.GetObject("channels.IconPadding"))));
            this.channels.Name = "channels";
            this.channels.RowHeadersVisible = false;
            this.channels.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.channels_CellClick);
            this.channels.DragDrop += new System.Windows.Forms.DragEventHandler(this.channels_DragDrop);
            this.channels.DragOver += new System.Windows.Forms.DragEventHandler(this.channels_DragOver);
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Name = "Column3";
            // 
            // signalsContextMenuStrip
            // 
            resources.ApplyResources(this.signalsContextMenuStrip, "signalsContextMenuStrip");
            this.errorProvider.SetError(this.signalsContextMenuStrip, resources.GetString("signalsContextMenuStrip.Error"));
            this.errorProvider.SetIconAlignment(this.signalsContextMenuStrip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("signalsContextMenuStrip.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.signalsContextMenuStrip, ((int)(resources.GetObject("signalsContextMenuStrip.IconPadding"))));
            this.signalsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSignalToolStripMenuItem});
            this.signalsContextMenuStrip.Name = "contextMenuStrip";
            // 
            // removeSignalToolStripMenuItem
            // 
            resources.ApplyResources(this.removeSignalToolStripMenuItem, "removeSignalToolStripMenuItem");
            this.removeSignalToolStripMenuItem.Name = "removeSignalToolStripMenuItem";
            this.removeSignalToolStripMenuItem.Click += new System.EventHandler(this.removeSignalToolStripMenuItem_Click);
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.errorProvider.SetError(this.splitContainer2, resources.GetString("splitContainer2.Error"));
            this.errorProvider.SetIconAlignment(this.splitContainer2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer2, ((int)(resources.GetObject("splitContainer2.IconPadding"))));
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            resources.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            this.errorProvider.SetError(this.splitContainer2.Panel1, resources.GetString("splitContainer2.Panel1.Error"));
            this.errorProvider.SetIconAlignment(this.splitContainer2.Panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer2.Panel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer2.Panel1, ((int)(resources.GetObject("splitContainer2.Panel1.IconPadding"))));
            // 
            // splitContainer2.Panel2
            // 
            resources.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
            this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
            this.errorProvider.SetError(this.splitContainer2.Panel2, resources.GetString("splitContainer2.Panel2.Error"));
            this.errorProvider.SetIconAlignment(this.splitContainer2.Panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer2.Panel2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer2.Panel2, ((int)(resources.GetObject("splitContainer2.Panel2.IconPadding"))));
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.fuzzyRulesCtrl);
            this.groupBox3.Controls.Add(this.toolStrip1);
            this.errorProvider.SetError(this.groupBox3, resources.GetString("groupBox3.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox3, ((int)(resources.GetObject("groupBox3.IconPadding"))));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // fuzzyRulesCtrl
            // 
            resources.ApplyResources(this.fuzzyRulesCtrl, "fuzzyRulesCtrl");
            this.fuzzyRulesCtrl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.fuzzyRulesCtrl.ContextMenuStrip = this.rulesContextMenuStrip;
            this.errorProvider.SetError(this.fuzzyRulesCtrl, resources.GetString("fuzzyRulesCtrl.Error"));
            this.errorProvider.SetIconAlignment(this.fuzzyRulesCtrl, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fuzzyRulesCtrl.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.fuzzyRulesCtrl, ((int)(resources.GetObject("fuzzyRulesCtrl.IconPadding"))));
            this.fuzzyRulesCtrl.IsReadOnly = false;
            this.fuzzyRulesCtrl.Name = "fuzzyRulesCtrl";
            this.fuzzyRulesCtrl.TextChanged += new System.EventHandler(this.fuzzyRulesCtrl_TextChanged);
            // 
            // rulesContextMenuStrip
            // 
            resources.ApplyResources(this.rulesContextMenuStrip, "rulesContextMenuStrip");
            this.errorProvider.SetError(this.rulesContextMenuStrip, resources.GetString("rulesContextMenuStrip.Error"));
            this.errorProvider.SetIconAlignment(this.rulesContextMenuStrip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("rulesContextMenuStrip.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.rulesContextMenuStrip, ((int)(resources.GetObject("rulesContextMenuStrip.IconPadding"))));
            this.rulesContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCut6,
            this.toolStripMenuItemCopy,
            this.toolStripMenuItemPaste,
            this.toolStripMenuItemDelete,
            this.toolStripSeparator4,
            this.toolStripMenuItemSelectAll,
            this.toolStripMenuItem2,
            this.findToolStripMenuItem,
            this.findAndReplaceToolStripMenuItem});
            this.rulesContextMenuStrip.Name = "contextMenuStrip";
            // 
            // toolStripMenuItemCut6
            // 
            resources.ApplyResources(this.toolStripMenuItemCut6, "toolStripMenuItemCut6");
            this.toolStripMenuItemCut6.Name = "toolStripMenuItemCut6";
            this.toolStripMenuItemCut6.Click += new System.EventHandler(this.toolStripButtonCut_Click);
            // 
            // toolStripMenuItemCopy
            // 
            resources.ApplyResources(this.toolStripMenuItemCopy, "toolStripMenuItemCopy");
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.Click += new System.EventHandler(this.toolStripButtonCopy_Click);
            // 
            // toolStripMenuItemPaste
            // 
            resources.ApplyResources(this.toolStripMenuItemPaste, "toolStripMenuItemPaste");
            this.toolStripMenuItemPaste.Name = "toolStripMenuItemPaste";
            this.toolStripMenuItemPaste.Click += new System.EventHandler(this.toolStripButtonPaste_Click);
            // 
            // toolStripMenuItemDelete
            // 
            resources.ApplyResources(this.toolStripMenuItemDelete, "toolStripMenuItemDelete");
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            this.toolStripMenuItemDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // toolStripMenuItemSelectAll
            // 
            resources.ApplyResources(this.toolStripMenuItemSelectAll, "toolStripMenuItemSelectAll");
            this.toolStripMenuItemSelectAll.Name = "toolStripMenuItemSelectAll";
            this.toolStripMenuItemSelectAll.Click += new System.EventHandler(this.toolStripMenuItemSelectAll_Click);
            // 
            // toolStripMenuItem2
            // 
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            // 
            // findToolStripMenuItem
            // 
            resources.ApplyResources(this.findToolStripMenuItem, "findToolStripMenuItem");
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // findAndReplaceToolStripMenuItem
            // 
            resources.ApplyResources(this.findAndReplaceToolStripMenuItem, "findAndReplaceToolStripMenuItem");
            this.findAndReplaceToolStripMenuItem.Name = "findAndReplaceToolStripMenuItem";
            this.findAndReplaceToolStripMenuItem.Click += new System.EventHandler(this.findAndReplaceToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.errorProvider.SetError(this.toolStrip1, resources.GetString("toolStrip1.Error"));
            this.errorProvider.SetIconAlignment(this.toolStrip1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("toolStrip1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.toolStrip1, ((int)(resources.GetObject("toolStrip1.IconPadding"))));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonCut,
            this.toolStripButtonCopy,
            this.toolStripButtonPaste,
            this.toolStripButtonDelete,
            this.toolStripSeparator1,
            this.toolStripButtonUndo,
            this.toolStripButtonRedo});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripButtonCut
            // 
            resources.ApplyResources(this.toolStripButtonCut, "toolStripButtonCut");
            this.toolStripButtonCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCut.Name = "toolStripButtonCut";
            this.toolStripButtonCut.Click += new System.EventHandler(this.toolStripButtonCut_Click);
            // 
            // toolStripButtonCopy
            // 
            resources.ApplyResources(this.toolStripButtonCopy, "toolStripButtonCopy");
            this.toolStripButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCopy.Name = "toolStripButtonCopy";
            this.toolStripButtonCopy.Click += new System.EventHandler(this.toolStripButtonCopy_Click);
            // 
            // toolStripButtonPaste
            // 
            resources.ApplyResources(this.toolStripButtonPaste, "toolStripButtonPaste");
            this.toolStripButtonPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPaste.Name = "toolStripButtonPaste";
            this.toolStripButtonPaste.Click += new System.EventHandler(this.toolStripButtonPaste_Click);
            // 
            // toolStripButtonDelete
            // 
            resources.ApplyResources(this.toolStripButtonDelete, "toolStripButtonDelete");
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolStripButtonUndo
            // 
            resources.ApplyResources(this.toolStripButtonUndo, "toolStripButtonUndo");
            this.toolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUndo.Name = "toolStripButtonUndo";
            this.toolStripButtonUndo.Click += new System.EventHandler(this.toolStripButtonUndo_Click);
            // 
            // toolStripButtonRedo
            // 
            resources.ApplyResources(this.toolStripButtonRedo, "toolStripButtonRedo");
            this.toolStripButtonRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRedo.Name = "toolStripButtonRedo";
            this.toolStripButtonRedo.Click += new System.EventHandler(this.toolStripButtonRedo_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.name);
            this.groupBox1.Controls.Add(this.label1);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // name
            // 
            resources.ApplyResources(this.name, "name");
            this.errorProvider.SetError(this.name, resources.GetString("name.Error"));
            this.errorProvider.SetIconAlignment(this.name, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("name.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.name, ((int)(resources.GetObject("name.IconPadding"))));
            this.name.Name = "name";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.errorProvider.SetError(this.help, resources.GetString("help.Error"));
            this.errorProvider.SetIconAlignment(this.help, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("help.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.help, ((int)(resources.GetObject("help.IconPadding"))));
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.help_Click);
            // 
            // cancel
            // 
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.errorProvider.SetError(this.cancel, resources.GetString("cancel.Error"));
            this.errorProvider.SetIconAlignment(this.cancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("cancel.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.cancel, ((int)(resources.GetObject("cancel.IconPadding"))));
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.errorProvider.SetError(this.OK, resources.GetString("OK.Error"));
            this.errorProvider.SetIconAlignment(this.OK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("OK.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.OK, ((int)(resources.GetObject("OK.IconPadding"))));
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.help);
            this.flowLayoutPanel1.Controls.Add(this.cancel);
            this.flowLayoutPanel1.Controls.Add(this.OK);
            this.errorProvider.SetError(this.flowLayoutPanel1, resources.GetString("flowLayoutPanel1.Error"));
            this.errorProvider.SetIconAlignment(this.flowLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.flowLayoutPanel1, ((int)(resources.GetObject("flowLayoutPanel1.IconPadding"))));
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.onLingOutput);
            this.groupBox4.Controls.Add(this.outLingVar);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.outComment);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.outUnit);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.outMax);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.outMin);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.outRate);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.sigName);
            this.groupBox4.Controls.Add(this.label2);
            this.errorProvider.SetError(this.groupBox4, resources.GetString("groupBox4.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox4, ((int)(resources.GetObject("groupBox4.IconPadding"))));
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // onLingOutput
            // 
            resources.ApplyResources(this.onLingOutput, "onLingOutput");
            this.errorProvider.SetError(this.onLingOutput, resources.GetString("onLingOutput.Error"));
            this.errorProvider.SetIconAlignment(this.onLingOutput, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("onLingOutput.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.onLingOutput, ((int)(resources.GetObject("onLingOutput.IconPadding"))));
            this.onLingOutput.Name = "onLingOutput";
            this.onLingOutput.UseVisualStyleBackColor = true;
            this.onLingOutput.Click += new System.EventHandler(this.OnLingOutout_Click);
            // 
            // outLingVar
            // 
            resources.ApplyResources(this.outLingVar, "outLingVar");
            this.errorProvider.SetError(this.outLingVar, resources.GetString("outLingVar.Error"));
            this.errorProvider.SetIconAlignment(this.outLingVar, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("outLingVar.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.outLingVar, ((int)(resources.GetObject("outLingVar.IconPadding"))));
            this.outLingVar.Name = "outLingVar";
            this.outLingVar.ReadOnly = true;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.errorProvider.SetError(this.label9, resources.GetString("label9.Error"));
            this.errorProvider.SetIconAlignment(this.label9, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label9.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label9, ((int)(resources.GetObject("label9.IconPadding"))));
            this.label9.Name = "label9";
            // 
            // outComment
            // 
            resources.ApplyResources(this.outComment, "outComment");
            this.errorProvider.SetError(this.outComment, resources.GetString("outComment.Error"));
            this.errorProvider.SetIconAlignment(this.outComment, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("outComment.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.outComment, ((int)(resources.GetObject("outComment.IconPadding"))));
            this.outComment.Name = "outComment";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.errorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.errorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            // 
            // outUnit
            // 
            resources.ApplyResources(this.outUnit, "outUnit");
            this.errorProvider.SetError(this.outUnit, resources.GetString("outUnit.Error"));
            this.errorProvider.SetIconAlignment(this.outUnit, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("outUnit.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.outUnit, ((int)(resources.GetObject("outUnit.IconPadding"))));
            this.outUnit.Name = "outUnit";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.errorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.errorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            // 
            // outMax
            // 
            resources.ApplyResources(this.outMax, "outMax");
            this.errorProvider.SetError(this.outMax, resources.GetString("outMax.Error"));
            this.errorProvider.SetIconAlignment(this.outMax, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("outMax.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.outMax, ((int)(resources.GetObject("outMax.IconPadding"))));
            this.outMax.Name = "outMax";
            this.outMax.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // outMin
            // 
            resources.ApplyResources(this.outMin, "outMin");
            this.errorProvider.SetError(this.outMin, resources.GetString("outMin.Error"));
            this.errorProvider.SetIconAlignment(this.outMin, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("outMin.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.outMin, ((int)(resources.GetObject("outMin.IconPadding"))));
            this.outMin.Name = "outMin";
            this.outMin.Validating += new System.ComponentModel.CancelEventHandler(this.Value_Validating);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.errorProvider.SetError(this.label5, resources.GetString("label5.Error"));
            this.errorProvider.SetIconAlignment(this.label5, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label5.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label5, ((int)(resources.GetObject("label5.IconPadding"))));
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // outRate
            // 
            resources.ApplyResources(this.outRate, "outRate");
            this.errorProvider.SetError(this.outRate, resources.GetString("outRate.Error"));
            this.errorProvider.SetIconAlignment(this.outRate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("outRate.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.outRate, ((int)(resources.GetObject("outRate.IconPadding"))));
            this.outRate.Name = "outRate";
            this.outRate.Validating += new System.ComponentModel.CancelEventHandler(this.outRate_Validating);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // sigName
            // 
            resources.ApplyResources(this.sigName, "sigName");
            this.errorProvider.SetError(this.sigName, resources.GetString("sigName.Error"));
            this.errorProvider.SetIconAlignment(this.sigName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sigName.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.sigName, ((int)(resources.GetObject("sigName.IconPadding"))));
            this.sigName.Name = "sigName";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // FuzzyControllerDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "FuzzyControllerDlg";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FuzzyControllerDlg_FormClosing);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.channels)).EndInit();
            this.signalsContextMenuStrip.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.rulesContextMenuStrip.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView channels;
        private System.Windows.Forms.ContextMenuStrip signalsContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeSignalToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.GroupBox groupBox3;
        private ICSharpCode.TextEditor.TextEditorControl fuzzyRulesCtrl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonCut;
        private System.Windows.Forms.ToolStripButton toolStripButtonCopy;
        private System.Windows.Forms.ToolStripButton toolStripButtonPaste;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonUndo;
        private System.Windows.Forms.ToolStripButton toolStripButtonRedo;
        private System.Windows.Forms.ContextMenuStrip rulesContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCut6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPaste;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findAndReplaceToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button onLingOutput;
        private System.Windows.Forms.TextBox outLingVar;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox outComment;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox outUnit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox outMax;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox outMin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox outRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox sigName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewButtonColumn Column3;
    }
}