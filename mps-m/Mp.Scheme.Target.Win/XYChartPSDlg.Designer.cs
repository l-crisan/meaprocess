namespace Mp.Scheme.Win
{
    partial class XYChartPSDlg
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XYChartPSDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.curves = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.removeCurve = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.fileView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.open = new System.Windows.Forms.Button();
            this.removeFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.curves)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.AccessibleDescription = null;
            this.splitContainer1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.BackgroundImage = null;
            this.errorProvider.SetError(this.splitContainer1, resources.GetString("splitContainer1.Error"));
            this.splitContainer1.Font = null;
            this.errorProvider.SetIconAlignment(this.splitContainer1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer1, ((int)(resources.GetObject("splitContainer1.IconPadding"))));
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AccessibleDescription = null;
            this.splitContainer1.Panel1.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.BackgroundImage = null;
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.errorProvider.SetError(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.Error"));
            this.splitContainer1.Panel1.Font = null;
            this.errorProvider.SetIconAlignment(this.splitContainer1.Panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer1.Panel1, ((int)(resources.GetObject("splitContainer1.Panel1.IconPadding"))));
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AccessibleDescription = null;
            this.splitContainer1.Panel2.AccessibleName = null;
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.BackgroundImage = null;
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.errorProvider.SetError(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.Error"));
            this.splitContainer1.Panel2.Font = null;
            this.errorProvider.SetIconAlignment(this.splitContainer1.Panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("splitContainer1.Panel2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.splitContainer1.Panel2, ((int)(resources.GetObject("splitContainer1.Panel2.IconPadding"))));
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = null;
            this.groupBox2.AccessibleName = null;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackgroundImage = null;
            this.groupBox2.Controls.Add(this.curves);
            this.groupBox2.Controls.Add(this.panel1);
            this.errorProvider.SetError(this.groupBox2, resources.GetString("groupBox2.Error"));
            this.groupBox2.Font = null;
            this.errorProvider.SetIconAlignment(this.groupBox2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox2, ((int)(resources.GetObject("groupBox2.IconPadding"))));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // curves
            // 
            this.curves.AccessibleDescription = null;
            this.curves.AccessibleName = null;
            this.curves.AllowDrop = true;
            this.curves.AllowUserToAddRows = false;
            this.curves.AllowUserToDeleteRows = false;
            this.curves.AllowUserToResizeRows = false;
            resources.ApplyResources(this.curves, "curves");
            this.curves.BackgroundImage = null;
            this.curves.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.curves.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column1,
            this.Column2});
            this.curves.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.errorProvider.SetError(this.curves, resources.GetString("curves.Error"));
            this.curves.Font = null;
            this.errorProvider.SetIconAlignment(this.curves, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("curves.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.curves, ((int)(resources.GetObject("curves.IconPadding"))));
            this.curves.Name = "curves";
            this.curves.RowHeadersVisible = false;
            this.curves.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.curves.DragOver += new System.Windows.Forms.DragEventHandler(this.curves_DragOver);
            this.curves.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.curves_CellClick);
            this.curves.DragDrop += new System.Windows.Forms.DragEventHandler(this.curves_DragDrop);
            // 
            // Column3
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.removeCurve);
            this.errorProvider.SetError(this.panel1, resources.GetString("panel1.Error"));
            this.panel1.Font = null;
            this.errorProvider.SetIconAlignment(this.panel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.panel1, ((int)(resources.GetObject("panel1.IconPadding"))));
            this.panel1.Name = "panel1";
            // 
            // removeCurve
            // 
            this.removeCurve.AccessibleDescription = null;
            this.removeCurve.AccessibleName = null;
            resources.ApplyResources(this.removeCurve, "removeCurve");
            this.removeCurve.BackgroundImage = null;
            this.errorProvider.SetError(this.removeCurve, resources.GetString("removeCurve.Error"));
            this.removeCurve.Font = null;
            this.errorProvider.SetIconAlignment(this.removeCurve, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("removeCurve.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.removeCurve, ((int)(resources.GetObject("removeCurve.IconPadding"))));
            this.removeCurve.Name = "removeCurve";
            this.removeCurve.UseVisualStyleBackColor = true;
            this.removeCurve.Click += new System.EventHandler(this.removeCurve_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.AccessibleDescription = null;
            this.groupBox3.AccessibleName = null;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.BackgroundImage = null;
            this.groupBox3.Controls.Add(this.fileView);
            this.groupBox3.Controls.Add(this.panel2);
            this.errorProvider.SetError(this.groupBox3, resources.GetString("groupBox3.Error"));
            this.groupBox3.Font = null;
            this.errorProvider.SetIconAlignment(this.groupBox3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox3.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox3, ((int)(resources.GetObject("groupBox3.IconPadding"))));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // fileView
            // 
            this.fileView.AccessibleDescription = null;
            this.fileView.AccessibleName = null;
            resources.ApplyResources(this.fileView, "fileView");
            this.fileView.BackgroundImage = null;
            this.errorProvider.SetError(this.fileView, resources.GetString("fileView.Error"));
            this.fileView.Font = null;
            this.errorProvider.SetIconAlignment(this.fileView, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("fileView.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.fileView, ((int)(resources.GetObject("fileView.IconPadding"))));
            this.fileView.ImageList = this.imageList;
            this.fileView.Name = "fileView";
            this.fileView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.fileView_ItemDrag);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "DataStorage.ico");
            this.imageList.Images.SetKeyName(1, "StGroup.ico");
            this.imageList.Images.SetKeyName(2, "Signal.ico");
            // 
            // panel2
            // 
            this.panel2.AccessibleDescription = null;
            this.panel2.AccessibleName = null;
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackgroundImage = null;
            this.panel2.Controls.Add(this.open);
            this.panel2.Controls.Add(this.removeFile);
            this.errorProvider.SetError(this.panel2, resources.GetString("panel2.Error"));
            this.panel2.Font = null;
            this.errorProvider.SetIconAlignment(this.panel2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("panel2.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.panel2, ((int)(resources.GetObject("panel2.IconPadding"))));
            this.panel2.Name = "panel2";
            // 
            // open
            // 
            this.open.AccessibleDescription = null;
            this.open.AccessibleName = null;
            resources.ApplyResources(this.open, "open");
            this.open.BackgroundImage = null;
            this.errorProvider.SetError(this.open, resources.GetString("open.Error"));
            this.open.Font = null;
            this.errorProvider.SetIconAlignment(this.open, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("open.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.open, ((int)(resources.GetObject("open.IconPadding"))));
            this.open.Name = "open";
            this.open.UseVisualStyleBackColor = true;
            this.open.Click += new System.EventHandler(this.open_Click);
            // 
            // removeFile
            // 
            this.removeFile.AccessibleDescription = null;
            this.removeFile.AccessibleName = null;
            resources.ApplyResources(this.removeFile, "removeFile");
            this.removeFile.BackgroundImage = null;
            this.errorProvider.SetError(this.removeFile, resources.GetString("removeFile.Error"));
            this.removeFile.Font = null;
            this.errorProvider.SetIconAlignment(this.removeFile, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("removeFile.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.removeFile, ((int)(resources.GetObject("removeFile.IconPadding"))));
            this.removeFile.Name = "removeFile";
            this.removeFile.UseVisualStyleBackColor = true;
            this.removeFile.Click += new System.EventHandler(this.removeFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.name);
            this.groupBox1.Controls.Add(this.label1);
            this.errorProvider.SetError(this.groupBox1, resources.GetString("groupBox1.Error"));
            this.groupBox1.Font = null;
            this.errorProvider.SetIconAlignment(this.groupBox1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("groupBox1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.groupBox1, ((int)(resources.GetObject("groupBox1.IconPadding"))));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // name
            // 
            this.name.AccessibleDescription = null;
            this.name.AccessibleName = null;
            resources.ApplyResources(this.name, "name");
            this.name.BackgroundImage = null;
            this.errorProvider.SetError(this.name, resources.GetString("name.Error"));
            this.name.Font = null;
            this.errorProvider.SetIconAlignment(this.name, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("name.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.name, ((int)(resources.GetObject("name.IconPadding"))));
            this.name.Name = "name";
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.errorProvider.SetError(this.label1, resources.GetString("label1.Error"));
            this.label1.Font = null;
            this.errorProvider.SetIconAlignment(this.label1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("label1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.label1, ((int)(resources.GetObject("label1.IconPadding"))));
            this.label1.Name = "label1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AccessibleDescription = null;
            this.flowLayoutPanel1.AccessibleName = null;
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.BackgroundImage = null;
            this.flowLayoutPanel1.Controls.Add(this.cancel);
            this.flowLayoutPanel1.Controls.Add(this.OK);
            this.errorProvider.SetError(this.flowLayoutPanel1, resources.GetString("flowLayoutPanel1.Error"));
            this.flowLayoutPanel1.Font = null;
            this.errorProvider.SetIconAlignment(this.flowLayoutPanel1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("flowLayoutPanel1.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.flowLayoutPanel1, ((int)(resources.GetObject("flowLayoutPanel1.IconPadding"))));
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // cancel
            // 
            this.cancel.AccessibleDescription = null;
            this.cancel.AccessibleName = null;
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.BackgroundImage = null;
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.errorProvider.SetError(this.cancel, resources.GetString("cancel.Error"));
            this.cancel.Font = null;
            this.errorProvider.SetIconAlignment(this.cancel, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("cancel.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.cancel, ((int)(resources.GetObject("cancel.IconPadding"))));
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // OK
            // 
            this.OK.AccessibleDescription = null;
            this.OK.AccessibleName = null;
            resources.ApplyResources(this.OK, "OK");
            this.OK.BackgroundImage = null;
            this.errorProvider.SetError(this.OK, resources.GetString("OK.Error"));
            this.OK.Font = null;
            this.errorProvider.SetIconAlignment(this.OK, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("OK.IconAlignment"))));
            this.errorProvider.SetIconPadding(this.OK, ((int)(resources.GetObject("OK.IconPadding"))));
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // XYChartPSDlg
            // 
            this.AcceptButton = this.OK;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Font = null;
            this.Name = "XYChartPSDlg";
            this.ShowInTaskbar = false;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.XYChartPSDlg_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.curves)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView curves;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button removeCurve;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TreeView fileView;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button open;
        private System.Windows.Forms.Button removeFile;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewButtonColumn Column2;
    }
}