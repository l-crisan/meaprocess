namespace Mp.Mod.Calculation
{
    partial class BitGroupingDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BitGroupingDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.outputGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeSignalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.help = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.name = new System.Windows.Forms.TextBox();
            this.treeColumn3 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.treeColumn1 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.treeColumn2 = new Mp.Visual.Tree.Tree.TreeColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.unit = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comment = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.max = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.min = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dataType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rateUnit = new System.Windows.Forms.Label();
            this.sampleRate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.sigName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outputGrid)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.Column2});
            this.outputGrid.ContextMenuStrip = this.contextMenuStrip;
            this.outputGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.errorProvider.SetError(this.outputGrid, resources.GetString("outputGrid.Error"));
            this.errorProvider.SetIconAlignment(this.outputGrid, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("outputGrid.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.outputGrid, ((int)(resources.GetObject("outputGrid.IconPadding"))));
            this.outputGrid.Name = "outputGrid";
            this.outputGrid.RowHeadersVisible = false;
            this.outputGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.channels_DragDrop);
            this.outputGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.channels_DragOver);
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
            // name
            // 
            resources.ApplyResources(this.name, "name");
            this.errorProvider.SetError(this.name, resources.GetString("name.Error"));
            this.errorProvider.SetIconAlignment(this.name, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("name.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.name, ((int)(resources.GetObject("name.IconPadding"))));
            this.name.Name = "name";
            // 
            // treeColumn3
            // 
            resources.ApplyResources(this.treeColumn3, "treeColumn3");
            this.treeColumn3.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // treeColumn1
            // 
            resources.ApplyResources(this.treeColumn1, "treeColumn1");
            this.treeColumn1.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // treeColumn2
            // 
            resources.ApplyResources(this.treeColumn2, "treeColumn2");
            this.treeColumn2.SortOrder = System.Windows.Forms.SortOrder.None;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.name);
            this.groupBox1.Controls.Add(this.label1);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.errorProvider.SetError(this.label2, resources.GetString("label2.Error"));
            this.errorProvider.SetIconAlignment(this.label2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label2, ((int)(resources.GetObject("label2.IconPadding"))));
            this.label2.Name = "label2";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.unit);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.comment);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.max);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.min);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.dataType);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.rateUnit);
            this.groupBox3.Controls.Add(this.sampleRate);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.sigName);
            this.groupBox3.Controls.Add(this.label3);
            this.errorProvider.SetError(this.groupBox3, resources.GetString("groupBox3.Error"));
            this.errorProvider.SetIconAlignment(this.groupBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox3, ((int)(resources.GetObject("groupBox3.IconPadding"))));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // unit
            // 
            resources.ApplyResources(this.unit, "unit");
            this.errorProvider.SetError(this.unit, resources.GetString("unit.Error"));
            this.errorProvider.SetIconAlignment(this.unit, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("unit.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.unit, ((int)(resources.GetObject("unit.IconPadding"))));
            this.unit.Name = "unit";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.errorProvider.SetError(this.label10, resources.GetString("label10.Error"));
            this.errorProvider.SetIconAlignment(this.label10, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label10.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label10, ((int)(resources.GetObject("label10.IconPadding"))));
            this.label10.Name = "label10";
            // 
            // comment
            // 
            resources.ApplyResources(this.comment, "comment");
            this.errorProvider.SetError(this.comment, resources.GetString("comment.Error"));
            this.errorProvider.SetIconAlignment(this.comment, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("comment.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.comment, ((int)(resources.GetObject("comment.IconPadding"))));
            this.comment.Name = "comment";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.errorProvider.SetError(this.label9, resources.GetString("label9.Error"));
            this.errorProvider.SetIconAlignment(this.label9, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label9.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label9, ((int)(resources.GetObject("label9.IconPadding"))));
            this.label9.Name = "label9";
            // 
            // max
            // 
            resources.ApplyResources(this.max, "max");
            this.errorProvider.SetError(this.max, resources.GetString("max.Error"));
            this.errorProvider.SetIconAlignment(this.max, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("max.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.max, ((int)(resources.GetObject("max.IconPadding"))));
            this.max.Name = "max";
            this.max.Validating += new System.ComponentModel.CancelEventHandler(this.value_Validating);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.errorProvider.SetError(this.label8, resources.GetString("label8.Error"));
            this.errorProvider.SetIconAlignment(this.label8, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label8.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label8, ((int)(resources.GetObject("label8.IconPadding"))));
            this.label8.Name = "label8";
            // 
            // min
            // 
            resources.ApplyResources(this.min, "min");
            this.errorProvider.SetError(this.min, resources.GetString("min.Error"));
            this.errorProvider.SetIconAlignment(this.min, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("min.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.min, ((int)(resources.GetObject("min.IconPadding"))));
            this.min.Name = "min";
            this.min.Validating += new System.ComponentModel.CancelEventHandler(this.value_Validating);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.errorProvider.SetError(this.label7, resources.GetString("label7.Error"));
            this.errorProvider.SetIconAlignment(this.label7, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label7.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label7, ((int)(resources.GetObject("label7.IconPadding"))));
            this.label7.Name = "label7";
            // 
            // dataType
            // 
            resources.ApplyResources(this.dataType, "dataType");
            this.dataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.errorProvider.SetError(this.dataType, resources.GetString("dataType.Error"));
            this.dataType.FormattingEnabled = true;
            this.errorProvider.SetIconAlignment(this.dataType, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("dataType.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.dataType, ((int)(resources.GetObject("dataType.IconPadding"))));
            this.dataType.Items.AddRange(new object[] {
            resources.GetString("dataType.Items"),
            resources.GetString("dataType.Items1"),
            resources.GetString("dataType.Items2"),
            resources.GetString("dataType.Items3"),
            resources.GetString("dataType.Items4"),
            resources.GetString("dataType.Items5"),
            resources.GetString("dataType.Items6"),
            resources.GetString("dataType.Items7")});
            this.dataType.Name = "dataType";
            this.dataType.SelectedIndexChanged += new System.EventHandler(this.dataType_SelectedIndexChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.errorProvider.SetError(this.label6, resources.GetString("label6.Error"));
            this.errorProvider.SetIconAlignment(this.label6, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label6.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label6, ((int)(resources.GetObject("label6.IconPadding"))));
            this.label6.Name = "label6";
            // 
            // rateUnit
            // 
            resources.ApplyResources(this.rateUnit, "rateUnit");
            this.errorProvider.SetError(this.rateUnit, resources.GetString("rateUnit.Error"));
            this.errorProvider.SetIconAlignment(this.rateUnit, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("rateUnit.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.rateUnit, ((int)(resources.GetObject("rateUnit.IconPadding"))));
            this.rateUnit.Name = "rateUnit";
            // 
            // sampleRate
            // 
            resources.ApplyResources(this.sampleRate, "sampleRate");
            this.errorProvider.SetError(this.sampleRate, resources.GetString("sampleRate.Error"));
            this.errorProvider.SetIconAlignment(this.sampleRate, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sampleRate.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.sampleRate, ((int)(resources.GetObject("sampleRate.IconPadding"))));
            this.sampleRate.Name = "sampleRate";
            this.sampleRate.Validating += new System.ComponentModel.CancelEventHandler(this.sampleRate_Validating);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.errorProvider.SetError(this.label4, resources.GetString("label4.Error"));
            this.errorProvider.SetIconAlignment(this.label4, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label4.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label4, ((int)(resources.GetObject("label4.IconPadding"))));
            this.label4.Name = "label4";
            // 
            // sigName
            // 
            resources.ApplyResources(this.sigName, "sigName");
            this.errorProvider.SetError(this.sigName, resources.GetString("sigName.Error"));
            this.errorProvider.SetIconAlignment(this.sigName, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("sigName.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.sigName, ((int)(resources.GetObject("sigName.IconPadding"))));
            this.sigName.Name = "sigName";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.errorProvider.SetError(this.label3, resources.GetString("label3.Error"));
            this.errorProvider.SetIconAlignment(this.label3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label3, ((int)(resources.GetObject("label3.IconPadding"))));
            this.label3.Name = "label3";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // BitGroupingDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "BitGroupingDlg";
            this.ShowInTaskbar = false;
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.outputGrid)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button help;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView outputGrid;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem removeSignalToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.TextBox name;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn3;
        private System.Windows.Forms.Label label1;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn1;
        private Mp.Visual.Tree.Tree.TreeColumn treeColumn2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox unit;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox comment;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox max;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox min;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox dataType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label rateUnit;
        private System.Windows.Forms.TextBox sampleRate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox sigName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}