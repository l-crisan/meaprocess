namespace Mp.Scheme.Sdk
{
    partial class SchemePropertyDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchemePropertyDlg));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.help = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.properties = new System.Windows.Forms.DataGridView();
            this.propertyCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.mandatoryCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.remove = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.help);
            this.flowLayoutPanel1.Controls.Add(this.cancel);
            this.flowLayoutPanel1.Controls.Add(this.OK);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // help
            // 
            resources.ApplyResources(this.help, "help");
            this.help.Name = "help";
            this.help.UseVisualStyleBackColor = true;
            this.help.Click += new System.EventHandler(this.OnHelpClick);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.Name = "cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.OnCancelClick);
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OnOKClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.properties);
            this.groupBox1.Controls.Add(this.panel1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // properties
            // 
            this.properties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.properties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.propertyCol,
            this.valueCol,
            this.typeCol,
            this.Column1,
            this.mandatoryCol,
            this.Column2,
            this.Column3});
            resources.ApplyResources(this.properties, "properties");
            this.properties.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.properties.Name = "properties";
            this.properties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.properties.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnPropertyCellClick);
            this.properties.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.OnPropertyRowsAdded);
            // 
            // propertyCol
            // 
            resources.ApplyResources(this.propertyCol, "propertyCol");
            this.propertyCol.Name = "propertyCol";
            // 
            // valueCol
            // 
            resources.ApplyResources(this.valueCol, "valueCol");
            this.valueCol.Name = "valueCol";
            // 
            // typeCol
            // 
            resources.ApplyResources(this.typeCol, "typeCol");
            this.typeCol.Items.AddRange(new object[] {
            "STRING",
            "FILE",
            "FOLDER",
            "LREAL",
            "REAL",
            "SINT",
            "INT",
            "DINT",
            "LINT",
            "BOOL",
            "BYTE",
            "WORD",
            "DWORD",
            "LWORD",
            "ENUMERATION",
            "START COUNTER"});
            this.typeCol.Name = "typeCol";
            this.typeCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            // 
            // mandatoryCol
            // 
            resources.ApplyResources(this.mandatoryCol, "mandatoryCol");
            this.mandatoryCol.Name = "mandatoryCol";
            // 
            // Column2
            // 
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.remove);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // remove
            // 
            resources.ApplyResources(this.remove, "remove");
            this.remove.Name = "remove";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.OnRemoveClick);
            // 
            // SchemePropertyDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "SchemePropertyDlg";
            this.ShowInTaskbar = false;
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.OnHelpRequested);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView properties;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.DataGridViewTextBoxColumn propertyCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn typeCol;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn mandatoryCol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
    }
}