namespace Mp.Visual.Analog
{
    partial class GaugeCaptionDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GaugeCaptionDlg));
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.captionGrid = new System.Windows.Forms.DataGridView();
            this.CapText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Color = new System.Windows.Forms.DataGridViewButtonColumn();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.captionGrid)).BeginInit();
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
            this.groupBox1.Controls.Add(this.captionGrid);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // captionGrid
            // 
            this.captionGrid.AllowUserToAddRows = false;
            this.captionGrid.AllowUserToDeleteRows = false;
            this.captionGrid.AllowUserToResizeRows = false;
            this.captionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.captionGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CapText,
            this.PositionX,
            this.PositionY,
            this.Color});
            this.captionGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            resources.ApplyResources(this.captionGrid, "captionGrid");
            this.captionGrid.Name = "captionGrid";
            this.captionGrid.RowHeadersVisible = false;
            this.captionGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.captionGrid_CellClick);
            this.captionGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.captionGrid_CellValidating);
            // 
            // CapText
            // 
            resources.ApplyResources(this.CapText, "CapText");
            this.CapText.Name = "CapText";
            // 
            // PositionX
            // 
            resources.ApplyResources(this.PositionX, "PositionX");
            this.PositionX.Name = "PositionX";
            // 
            // PositionY
            // 
            resources.ApplyResources(this.PositionY, "PositionY");
            this.PositionY.Name = "PositionY";
            // 
            // Color
            // 
            resources.ApplyResources(this.Color, "Color");
            this.Color.Name = "Color";
            this.Color.Text = "...";
            this.Color.UseColumnTextForButtonValue = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // GaugeCaptionDlg
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
            this.Name = "GaugeCaptionDlg";
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.captionGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView captionGrid;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn CapText;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionX;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionY;
        private System.Windows.Forms.DataGridViewButtonColumn Color;
    }
}