namespace Mp.Visual.Analog
{
    partial class GaugeRangeDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GaugeRangeDlg));
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rangesGrid = new System.Windows.Forms.DataGridView();
            this.Enable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StartValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InnerRadius = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OuterRadius = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RangeColor = new System.Windows.Forms.DataGridViewButtonColumn();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangesGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            resources.ApplyResources(this.OK, "OK");
            this.OK.Name = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.Cancel, "Cancel");
            this.Cancel.Name = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rangesGrid);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // rangesGrid
            // 
            this.rangesGrid.AllowUserToAddRows = false;
            this.rangesGrid.AllowUserToDeleteRows = false;
            this.rangesGrid.AllowUserToResizeRows = false;
            this.rangesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.rangesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Enable,
            this.StartValue,
            this.EndValue,
            this.InnerRadius,
            this.OuterRadius,
            this.RangeColor});
            this.rangesGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            resources.ApplyResources(this.rangesGrid, "rangesGrid");
            this.rangesGrid.Name = "rangesGrid";
            this.rangesGrid.RowHeadersVisible = false;
            this.rangesGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.rangesGrid_CellClick);
            this.rangesGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.rangesGrid_CellValidating);
            // 
            // Enable
            // 
            this.Enable.Frozen = true;
            resources.ApplyResources(this.Enable, "Enable");
            this.Enable.Name = "Enable";
            // 
            // StartValue
            // 
            this.StartValue.Frozen = true;
            resources.ApplyResources(this.StartValue, "StartValue");
            this.StartValue.Name = "StartValue";
            // 
            // EndValue
            // 
            this.EndValue.Frozen = true;
            resources.ApplyResources(this.EndValue, "EndValue");
            this.EndValue.Name = "EndValue";
            // 
            // InnerRadius
            // 
            this.InnerRadius.Frozen = true;
            resources.ApplyResources(this.InnerRadius, "InnerRadius");
            this.InnerRadius.Name = "InnerRadius";
            // 
            // OuterRadius
            // 
            this.OuterRadius.Frozen = true;
            resources.ApplyResources(this.OuterRadius, "OuterRadius");
            this.OuterRadius.Name = "OuterRadius";
            // 
            // RangeColor
            // 
            this.RangeColor.Frozen = true;
            resources.ApplyResources(this.RangeColor, "RangeColor");
            this.RangeColor.Name = "RangeColor";
            this.RangeColor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.RangeColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.RangeColor.Text = "...";
            this.RangeColor.UseColumnTextForButtonValue = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // GaugeRangeDlg
            // 
            this.AcceptButton = this.OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GaugeRangeDlg";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rangesGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView rangesGrid;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Enable;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn InnerRadius;
        private System.Windows.Forms.DataGridViewTextBoxColumn OuterRadius;
        private System.Windows.Forms.DataGridViewButtonColumn RangeColor;
    }
}