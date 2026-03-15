namespace Mp.Mod.Event
{
    partial class SignalControlDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignalControlDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.outputGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeSignalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeColumn3 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.cancel = new System.Windows.Forms.Button();
            this.treeColumn1 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.help = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.name = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.treeColumn2 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outputGrid)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            resources.ApplyResources(this.splitContainer, "splitContainer");
            this.errorProvider.SetError(this.splitContainer, resources.GetString("splitContainer.Error"));
            this.errorProvider.SetIconAlignment(this.splitContainer, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer, ((int)(resources.GetObject("splitContainer.IconPadding"))));
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            resources.ApplyResources(this.splitContainer.Panel1, "splitContainer.Panel1");
            this.errorProvider.SetError(this.splitContainer.Panel1, resources.GetString("splitContainer.Panel1.Error"));
            this.errorProvider.SetIconAlignment(this.splitContainer.Panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer.Panel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer.Panel1, ((int)(resources.GetObject("splitContainer.Panel1.IconPadding"))));
            // 
            // splitContainer.Panel2
            // 
            resources.ApplyResources(this.splitContainer.Panel2, "splitContainer.Panel2");
            this.splitContainer.Panel2.Controls.Add(this.groupBox2);
            this.errorProvider.SetError(this.splitContainer.Panel2, resources.GetString("splitContainer.Panel2.Error"));
            this.errorProvider.SetIconAlignment(this.splitContainer.Panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer.Panel2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer.Panel2, ((int)(resources.GetObject("splitContainer.Panel2.IconPadding"))));
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.outputGrid);
            this.errorProvider.SetError(this.groupBox2, resources.GetString("groupBox2.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject("groupBox2.IconPadding"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // outputGrid
            // 
            resources.ApplyResources(this.outputGrid, "outputGrid");
            this.outputGrid.AllowDrop = true;
            this.outputGrid.AllowUserToAddRows = false;
            this.outputGrid.AllowUserToDeleteRows = false;
            this.outputGrid.AllowUserToResizeRows = false;
            this.outputGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.outputGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column4,
            this.Column3,
            this.Column5,
            this.Column6,
            this.Column2,
            this.Column7,
            this.Column8});
            this.outputGrid.ContextMenuStrip = this.contextMenuStrip;
            this.outputGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.errorProvider.SetError(this.outputGrid, resources.GetString("outputGrid.Error"));
            this.errorProvider.SetIconAlignment(this.outputGrid, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("outputGrid.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.outputGrid, ((int)(resources.GetObject("outputGrid.IconPadding"))));
            this.outputGrid.Name = "outputGrid";
            this.outputGrid.RowHeadersVisible = false;
            this.outputGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.outputGrid_CellClick);
            this.outputGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.OnOutputGridCellValidating);
            this.outputGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnChannelsDragDrop);
            this.outputGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.OnChannelsDragOver);
            // 
            // Column1
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column4
            // 
            resources.ApplyResources(this.Column4, "Column4");
            this.Column4.Name = "Column4";
            // 
            // Column3
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column3.FillWeight = 180F;
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Items.AddRange(new object[] {
            "Liniareskalierung",
            "Zweipunktskalierung",
            "Tabelleskalierung"});
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column5
            // 
            resources.ApplyResources(this.Column5, "Column5");
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            resources.ApplyResources(this.Column6, "Column6");
            this.Column6.Name = "Column6";
            // 
            // Column2
            // 
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            // 
            // Column7
            // 
            resources.ApplyResources(this.Column7, "Column7");
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            resources.ApplyResources(this.Column8, "Column8");
            this.Column8.Name = "Column8";
            // 
            // contextMenuStrip
            // 
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.errorProvider.SetError(this.contextMenuStrip, resources.GetString("contextMenuStrip.Error"));
            this.errorProvider.SetIconAlignment(this.contextMenuStrip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("contextMenuStrip.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.contextMenuStrip, ((int)(resources.GetObject("contextMenuStrip.IconPadding"))));
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSignalToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            // 
            // removeSignalToolStripMenuItem
            // 
            resources.ApplyResources(this.removeSignalToolStripMenuItem, "removeSignalToolStripMenuItem");
            this.removeSignalToolStripMenuItem.Name = "removeSignalToolStripMenuItem";
            this.removeSignalToolStripMenuItem.Click += new System.EventHandler(this.removeSignalToolStripMenuItem_Click);
            // 
            // treeColumn3
            // 
            resources.ApplyResources(this.treeColumn3, "treeColumn3");
            this.treeColumn3.SortOrder = System.Windows.Forms.SortOrder.None;
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
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // treeColumn1
            // 
            resources.ApplyResources(this.treeColumn1, "treeColumn1");
            this.treeColumn1.SortOrder = System.Windows.Forms.SortOrder.None;
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
            // name
            // 
            resources.ApplyResources(this.name, "name");
            this.errorProvider.SetError(this.name, resources.GetString("name.Error"));
            this.errorProvider.SetIconAlignment(this.name, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("name.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.name, ((int)(resources.GetObject("name.IconPadding"))));
            this.name.Name = "name";
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // treeColumn2
            // 
            resources.ApplyResources(this.treeColumn2, "treeColumn2");
            this.treeColumn2.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // SignalControlDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "SignalControlDlg";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SignalControlDlg_FormClosing);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.outputGrid)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Mp.Visual.Tree.Tree.TreeColumn treeColumn3;
        private System.Windows.Forms.Button cancel;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn1;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView outputGrid;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeSignalToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn2;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewButtonColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}