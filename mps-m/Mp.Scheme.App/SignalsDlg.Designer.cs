namespace Mp.Scheme.App
{
    partial class SignalsDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignalsDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.signalGrid = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.export = new System.Windows.Forms.Button();
            this.dupplicated = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.samplerate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minimum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maximum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.signalGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.signalGrid);
            this.groupBox1.Controls.Add(this.panel1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // signalGrid
            // 
            this.signalGrid.AllowUserToAddRows = false;
            this.signalGrid.AllowUserToDeleteRows = false;
            this.signalGrid.AllowUserToResizeRows = false;
            this.signalGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.signalGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.unit,
            this.comment,
            this.samplerate,
            this.dataType,
            this.minimum,
            this.maximum,
            this.Column2,
            this.Column1,
            this.sourceCol});
            resources.ApplyResources(this.signalGrid, "signalGrid");
            this.signalGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.signalGrid.Name = "signalGrid";
            this.signalGrid.RowHeadersVisible = false;
            this.signalGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.OnSignalGridCellValidating);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.export);
            this.panel1.Controls.Add(this.dupplicated);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // export
            // 
            resources.ApplyResources(this.export, "export");
            this.export.Name = "export";
            this.export.UseVisualStyleBackColor = true;
            this.export.Click += new System.EventHandler(this.OnExportClick);
            // 
            // dupplicated
            // 
            resources.ApplyResources(this.dupplicated, "dupplicated");
            this.dupplicated.Name = "dupplicated";
            this.dupplicated.UseVisualStyleBackColor = true;
            this.dupplicated.Click += new System.EventHandler(this.OnDupplicatedClick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.cancel);
            this.flowLayoutPanel1.Controls.Add(this.OK);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
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
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // name
            // 
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            // 
            // unit
            // 
            resources.ApplyResources(this.unit, "unit");
            this.unit.Name = "unit";
            // 
            // comment
            // 
            resources.ApplyResources(this.comment, "comment");
            this.comment.Name = "comment";
            // 
            // samplerate
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.samplerate.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.samplerate, "samplerate");
            this.samplerate.Name = "samplerate";
            this.samplerate.ReadOnly = true;
            // 
            // dataType
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.dataType.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.dataType, "dataType");
            this.dataType.Name = "dataType";
            this.dataType.ReadOnly = true;
            // 
            // minimum
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.minimum.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.minimum, "minimum");
            this.minimum.Name = "minimum";
            this.minimum.ReadOnly = true;
            // 
            // maximum
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.maximum.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.maximum, "maximum");
            this.maximum.Name = "maximum";
            this.maximum.ReadOnly = true;
            // 
            // Column2
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column1
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // sourceCol
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.sourceCol.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.sourceCol, "sourceCol");
            this.sourceCol.Name = "sourceCol";
            this.sourceCol.ReadOnly = true;
            // 
            // SignalsDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "SignalsDlg";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.signalGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button export;
        private System.Windows.Forms.Button dupplicated;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.DataGridView signalGrid;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn comment;
        private System.Windows.Forms.DataGridViewTextBoxColumn samplerate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn minimum;
        private System.Windows.Forms.DataGridViewTextBoxColumn maximum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceCol;
    }
}