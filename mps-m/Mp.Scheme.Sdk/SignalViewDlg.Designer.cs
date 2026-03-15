namespace Mp.Scheme.Sdk
{
    partial class SignalViewDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignalViewDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.signalGrid = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.samplerate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minimum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maximum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.signalGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.cancel);
            this.flowLayoutPanel1.Controls.Add(this.OK);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // cancel
            // 
            resources.ApplyResources(this.cancel, "cancel");
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.signalGrid);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // signalGrid
            // 
            resources.ApplyResources(this.signalGrid, "signalGrid");
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
            this.sourceCol});
            this.signalGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.signalGrid.Name = "signalGrid";
            this.signalGrid.RowHeadersVisible = false;
            // 
            // name
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            this.name.DefaultCellStyle = dataGridViewCellStyle1;
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
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.samplerate.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.samplerate, "samplerate");
            this.samplerate.Name = "samplerate";
            this.samplerate.ReadOnly = true;
            // 
            // dataType
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.dataType.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.dataType, "dataType");
            this.dataType.Name = "dataType";
            this.dataType.ReadOnly = true;
            // 
            // minimum
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.minimum.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.minimum, "minimum");
            this.minimum.Name = "minimum";
            this.minimum.ReadOnly = true;
            // 
            // maximum
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.maximum.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.maximum, "maximum");
            this.maximum.Name = "maximum";
            this.maximum.ReadOnly = true;
            // 
            // sourceCol
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.sourceCol.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.sourceCol, "sourceCol");
            this.sourceCol.Name = "sourceCol";
            this.sourceCol.ReadOnly = true;
            // 
            // SignalViewDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "SignalViewDlg";
            this.ShowInTaskbar = false;
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.signalGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView signalGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn comment;
        private System.Windows.Forms.DataGridViewTextBoxColumn samplerate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn minimum;
        private System.Windows.Forms.DataGridViewTextBoxColumn maximum;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceCol;
    }
}