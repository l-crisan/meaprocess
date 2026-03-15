namespace Mp.Mod.Audio
{
    partial class AudioInPsPortDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioInPsPortDlg));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.help = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.signalsGroup = new System.Windows.Forms.GroupBox();
            this.signals = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.deviceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.unitCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            this.signalsGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.signals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
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
            // signalsGroup
            // 
            this.signalsGroup.Controls.Add(this.signals);
            resources.ApplyResources(this.signalsGroup, "signalsGroup");
            this.signalsGroup.Name = "signalsGroup";
            this.signalsGroup.TabStop = false;
            // 
            // signals
            // 
            this.signals.AllowUserToResizeRows = false;
            this.signals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.signals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.deviceCol,
            this.Column6,
            this.nameCol,
            this.Column2,
            this.Column3,
            this.unitCol,
            this.commentCol});
            resources.ApplyResources(this.signals, "signals");
            this.signals.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.signals.Name = "signals";
            this.signals.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnSignalsCellContentClick);
            this.signals.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.OnSignalsCellValidating);
            this.signals.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.OnSignalsRowsAdded);
            // 
            // Column5
            // 
            resources.ApplyResources(this.Column5, "Column5");
            this.Column5.Name = "Column5";
            // 
            // deviceCol
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            this.deviceCol.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.deviceCol, "deviceCol");
            this.deviceCol.Name = "deviceCol";
            this.deviceCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column6
            // 
            resources.ApplyResources(this.Column6, "Column6");
            this.Column6.Name = "Column6";
            // 
            // nameCol
            // 
            resources.ApplyResources(this.nameCol, "nameCol");
            this.nameCol.Name = "nameCol";
            this.nameCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Items.AddRange(new object[] {
            "8 Bit",
            "16 Bit"});
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column3
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.Column3, "Column3");
            this.Column3.Items.AddRange(new object[] {
            "11.025 (kHz)",
            "22.050 (kHz)",
            "44.100 (kHz)",
            "48.000 (KHz)",
            "96.000 (kHz)",
            "192.000 (kHz)"});
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // unitCol
            // 
            resources.ApplyResources(this.unitCol, "unitCol");
            this.unitCol.Name = "unitCol";
            this.unitCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // commentCol
            // 
            resources.ApplyResources(this.commentCol, "commentCol");
            this.commentCol.Name = "commentCol";
            this.commentCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // AudioInPsPortDlg
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.signalsGroup);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "AudioInPsPortDlg";
            this.ShowInTaskbar = false;
            this.flowLayoutPanel1.ResumeLayout(false);
            this.signalsGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.signals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button help;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.GroupBox signalsGroup;
        private System.Windows.Forms.DataGridView signals;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.DataGridViewButtonColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column2;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn unitCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentCol;
    }
}