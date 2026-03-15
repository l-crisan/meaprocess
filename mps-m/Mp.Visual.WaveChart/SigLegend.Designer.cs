namespace Mp.Visual.WaveChart
{
    partial class SigLegend
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SigLegend));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this._legendGrid = new System.Windows.Forms.DataGridView();
            this._name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._samplerate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._min = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._max = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._visible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._lineColor = new System.Windows.Forms.DataGridViewButtonColumn();
            this._lineWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._pointVisible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._pointWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._pointColor = new System.Windows.Forms.DataGridViewButtonColumn();
            this._yAxisDivision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._yAxisPrecision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._legendGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // _legendGrid
            // 
            resources.ApplyResources(this._legendGrid, "_legendGrid");
            this._legendGrid.AllowUserToAddRows = false;
            this._legendGrid.AllowUserToDeleteRows = false;
            this._legendGrid.AllowUserToOrderColumns = true;
            this._legendGrid.AllowUserToResizeRows = false;
            this._legendGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._legendGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._name,
            this._unit,
            this._comment,
            this._samplerate,
            this._min,
            this._max,
            this._visible,
            this._lineColor,
            this._lineWidth,
            this._pointVisible,
            this._pointWidth,
            this._pointColor,
            this._yAxisDivision,
            this._yAxisPrecision});
            this._legendGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this._legendGrid.MultiSelect = false;
            this._legendGrid.Name = "_legendGrid";
            this._legendGrid.RowHeadersVisible = false;
            this._legendGrid.ShowEditingIcon = false;
            this._legendGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._legendGrid_CellClick);
            this._legendGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._legendGrid_CellContentClick);
            this._legendGrid.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this._legendGrid_CellToolTipTextNeeded);
            this._legendGrid.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this._legendGrid_CellValidated);
            this._legendGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this._legendGrid_CellValidating);
            // 
            // _name
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this._name.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this._name, "_name");
            this._name.Name = "_name";
            this._name.ReadOnly = true;
            this._name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _unit
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this._unit.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this._unit, "_unit");
            this._unit.Name = "_unit";
            this._unit.ReadOnly = true;
            this._unit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _comment
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this._comment.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this._comment, "_comment");
            this._comment.Name = "_comment";
            this._comment.ReadOnly = true;
            this._comment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _samplerate
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this._samplerate.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this._samplerate, "_samplerate");
            this._samplerate.Name = "_samplerate";
            this._samplerate.ReadOnly = true;
            this._samplerate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _min
            // 
            resources.ApplyResources(this._min, "_min");
            this._min.Name = "_min";
            this._min.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _max
            // 
            resources.ApplyResources(this._max, "_max");
            this._max.Name = "_max";
            this._max.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _visible
            // 
            resources.ApplyResources(this._visible, "_visible");
            this._visible.Name = "_visible";
            // 
            // _lineColor
            // 
            this._lineColor.FillWeight = 80F;
            resources.ApplyResources(this._lineColor, "_lineColor");
            this._lineColor.Name = "_lineColor";
            this._lineColor.Text = "...";
            this._lineColor.UseColumnTextForButtonValue = true;
            // 
            // _lineWidth
            // 
            resources.ApplyResources(this._lineWidth, "_lineWidth");
            this._lineWidth.Name = "_lineWidth";
            this._lineWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _pointVisible
            // 
            resources.ApplyResources(this._pointVisible, "_pointVisible");
            this._pointVisible.Name = "_pointVisible";
            // 
            // _pointWidth
            // 
            resources.ApplyResources(this._pointWidth, "_pointWidth");
            this._pointWidth.Name = "_pointWidth";
            this._pointWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _pointColor
            // 
            this._pointColor.FillWeight = 80F;
            resources.ApplyResources(this._pointColor, "_pointColor");
            this._pointColor.Name = "_pointColor";
            this._pointColor.Text = "...";
            this._pointColor.UseColumnTextForButtonValue = true;
            // 
            // _yAxisDivision
            // 
            resources.ApplyResources(this._yAxisDivision, "_yAxisDivision");
            this._yAxisDivision.Name = "_yAxisDivision";
            this._yAxisDivision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _yAxisPrecision
            // 
            resources.ApplyResources(this._yAxisPrecision, "_yAxisPrecision");
            this._yAxisPrecision.Name = "_yAxisPrecision";
            this._yAxisPrecision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SigLegend
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._legendGrid);
            this.Name = "SigLegend";
            ((System.ComponentModel.ISupportInitialize)(this._legendGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _legendGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn _name;
        private System.Windows.Forms.DataGridViewTextBoxColumn _unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn _comment;
        private System.Windows.Forms.DataGridViewTextBoxColumn _samplerate;
        private System.Windows.Forms.DataGridViewTextBoxColumn _min;
        private System.Windows.Forms.DataGridViewTextBoxColumn _max;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _visible;
        private System.Windows.Forms.DataGridViewButtonColumn _lineColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn _lineWidth;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _pointVisible;
        private System.Windows.Forms.DataGridViewTextBoxColumn _pointWidth;
        private System.Windows.Forms.DataGridViewButtonColumn _pointColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn _yAxisDivision;
        private System.Windows.Forms.DataGridViewTextBoxColumn _yAxisPrecision;
    }
}
