namespace Mp.Mod.FGen
{
        partial class FuncGenPortDlg
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
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FuncGenPortDlg));
                SignalGeneratorViewer.SignalData signalData1 = new SignalGeneratorViewer.SignalData();
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
                this.OK = new System.Windows.Forms.Button();
                this.Cancel = new System.Windows.Forms.Button();
                this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
                this.help = new System.Windows.Forms.Button();
                this.groupBox1 = new System.Windows.Forms.GroupBox();
                this._signalViewer = new SignalGeneratorViewer();
                this.panel1 = new System.Windows.Forms.Panel();
                this.panel2 = new System.Windows.Forms.Panel();
                this.groupBox2 = new System.Windows.Forms.GroupBox();
                this._signalGrid = new System.Windows.Forms.DataGridView();
                this.panel4 = new System.Windows.Forms.Panel();
                this._remove = new System.Windows.Forms.Button();
                this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
                this.Function = new System.Windows.Forms.DataGridViewComboBoxColumn();
                this._Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this._dataTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.Minimum = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.Maximum = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.GenerationRate = new System.Windows.Forms.DataGridViewComboBoxColumn();
                this.Period = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.DisplofPhase = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.Comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.flowLayoutPanel1.SuspendLayout();
                this.groupBox1.SuspendLayout();
                this.panel1.SuspendLayout();
                this.panel2.SuspendLayout();
                this.groupBox2.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this._signalGrid)).BeginInit();
                this.panel4.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
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
                // flowLayoutPanel1
                // 
                this.flowLayoutPanel1.Controls.Add(this.help);
                this.flowLayoutPanel1.Controls.Add(this.Cancel);
                this.flowLayoutPanel1.Controls.Add(this.OK);
                resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
                this.flowLayoutPanel1.Name = "flowLayoutPanel1";
                // 
                // help
                // 
                resources.ApplyResources(this.help, "help");
                this.help.Name = "help";
                this.help.UseVisualStyleBackColor = true;
                this.help.Click += new System.EventHandler(this.help_Click);
                // 
                // groupBox1
                // 
                this.groupBox1.Controls.Add(this._signalViewer);
                resources.ApplyResources(this.groupBox1, "groupBox1");
                this.groupBox1.Name = "groupBox1";
                this.groupBox1.TabStop = false;
                // 
                // _signalViewer
                // 
                this._signalViewer.BackColor = System.Drawing.Color.Black;
                this._signalViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                this._signalViewer.Data = signalData1;
                resources.ApplyResources(this._signalViewer, "_signalViewer");
                this._signalViewer.Name = "_signalViewer";
                // 
                // panel1
                // 
                this.panel1.Controls.Add(this.groupBox1);
                resources.ApplyResources(this.panel1, "panel1");
                this.panel1.Name = "panel1";
                // 
                // panel2
                // 
                this.panel2.Controls.Add(this.groupBox2);
                resources.ApplyResources(this.panel2, "panel2");
                this.panel2.Name = "panel2";
                // 
                // groupBox2
                // 
                this.groupBox2.Controls.Add(this._signalGrid);
                this.groupBox2.Controls.Add(this.panel4);
                resources.ApplyResources(this.groupBox2, "groupBox2");
                this.groupBox2.Name = "groupBox2";
                this.groupBox2.TabStop = false;
                // 
                // _signalGrid
                // 
                this._signalGrid.AllowDrop = true;
                this._signalGrid.AllowUserToResizeRows = false;
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                this._signalGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                this._signalGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Function,
            this._Name,
            this._dataTypeCol,
            this.Minimum,
            this.Maximum,
            this.GenerationRate,
            this.Period,
            this.DisplofPhase,
            this.Column2,
            this.Column1,
            this.Unit,
            this.Comment});
                dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
                dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
                dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
                this._signalGrid.DefaultCellStyle = dataGridViewCellStyle5;
                resources.ApplyResources(this._signalGrid, "_signalGrid");
                this._signalGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                this._signalGrid.Name = "_signalGrid";
                dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
                dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
                dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                this._signalGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
                this._signalGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
                this._signalGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this._signalGrid_CellEndEdit);
                this._signalGrid.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this._signalGrid_CellLeave);
                this._signalGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this._signalGrid_CellValidating);
                this._signalGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this._signalGrid_RowEnter);
                this._signalGrid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this._signalGrid_RowsAdded);
                this._signalGrid.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this._signalGrid_RowValidating);
                this._signalGrid.Validating += new System.ComponentModel.CancelEventHandler(this._signalGrid_Validating);
                // 
                // panel4
                // 
                this.panel4.Controls.Add(this._remove);
                resources.ApplyResources(this.panel4, "panel4");
                this.panel4.Name = "panel4";
                // 
                // _remove
                // 
                resources.ApplyResources(this._remove, "_remove");
                this._remove.Name = "_remove";
                this._remove.UseVisualStyleBackColor = true;
                this._remove.Click += new System.EventHandler(this._remove_Click);
                // 
                // _errorProvider
                // 
                this._errorProvider.ContainerControl = this;
                // 
                // Function
                // 
                this.Function.FlatStyle = System.Windows.Forms.FlatStyle.System;
                resources.ApplyResources(this.Function, "Function");
                this.Function.Name = "Function";
                this.Function.Resizable = System.Windows.Forms.DataGridViewTriState.True;
                // 
                // _Name
                // 
                resources.ApplyResources(this._Name, "_Name");
                this._Name.Name = "_Name";
                // 
                // _dataTypeCol
                // 
                dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.InactiveCaption;
                this._dataTypeCol.DefaultCellStyle = dataGridViewCellStyle2;
                resources.ApplyResources(this._dataTypeCol, "_dataTypeCol");
                this._dataTypeCol.Name = "_dataTypeCol";
                this._dataTypeCol.ReadOnly = true;
                // 
                // Minimum
                // 
                resources.ApplyResources(this.Minimum, "Minimum");
                this.Minimum.Name = "Minimum";
                // 
                // Maximum
                // 
                resources.ApplyResources(this.Maximum, "Maximum");
                this.Maximum.Name = "Maximum";
                // 
                // GenerationRate
                // 
                resources.ApplyResources(this.GenerationRate, "GenerationRate");
                this.GenerationRate.Name = "GenerationRate";
                this.GenerationRate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
                // 
                // Period
                // 
                resources.ApplyResources(this.Period, "Period");
                this.Period.Name = "Period";
                // 
                // DisplofPhase
                // 
                resources.ApplyResources(this.DisplofPhase, "DisplofPhase");
                this.DisplofPhase.Name = "DisplofPhase";
                // 
                // Column2
                // 
                dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.InactiveCaption;
                this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
                resources.ApplyResources(this.Column2, "Column2");
                this.Column2.Name = "Column2";
                this.Column2.ReadOnly = true;
                // 
                // Column1
                // 
                dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.InactiveCaption;
                this.Column1.DefaultCellStyle = dataGridViewCellStyle4;
                resources.ApplyResources(this.Column1, "Column1");
                this.Column1.Name = "Column1";
                this.Column1.ReadOnly = true;
                // 
                // Unit
                // 
                resources.ApplyResources(this.Unit, "Unit");
                this.Unit.Name = "Unit";
                // 
                // Comment
                // 
                resources.ApplyResources(this.Comment, "Comment");
                this.Comment.Name = "Comment";
                // 
                // FuncGenPortDlg
                // 
                this.AcceptButton = this.OK;
                resources.ApplyResources(this, "$this");
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.CancelButton = this.Cancel;
                this.Controls.Add(this.panel2);
                this.Controls.Add(this.panel1);
                this.Controls.Add(this.flowLayoutPanel1);
                this.Name = "FuncGenPortDlg";
                this.ShowInTaskbar = false;
                this.Load += new System.EventHandler(this.FuncGenPortDlg_Load);
                this.ResizeEnd += new System.EventHandler(this.FuncGenPortDlg_ResizeEnd);
                this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FuncGenPortDlg_HelpRequested);
                this.flowLayoutPanel1.ResumeLayout(false);
                this.groupBox1.ResumeLayout(false);
                this.panel1.ResumeLayout(false);
                this.panel2.ResumeLayout(false);
                this.groupBox2.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this._signalGrid)).EndInit();
                this.panel4.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.Button OK;
            private System.Windows.Forms.Button Cancel;
            private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
            private System.Windows.Forms.GroupBox groupBox1;
            private System.Windows.Forms.Panel panel1;
            private System.Windows.Forms.Panel panel2;
            private System.Windows.Forms.GroupBox groupBox2;
            private SignalGeneratorViewer _signalViewer;
            private System.Windows.Forms.DataGridView _signalGrid;
            private System.Windows.Forms.Panel panel4;
            private System.Windows.Forms.Button _remove;
            private System.Windows.Forms.ErrorProvider _errorProvider;
            private System.Windows.Forms.Button help;
            private System.Windows.Forms.DataGridViewComboBoxColumn Function;
            private System.Windows.Forms.DataGridViewTextBoxColumn _Name;
            private System.Windows.Forms.DataGridViewTextBoxColumn _dataTypeCol;
            private System.Windows.Forms.DataGridViewTextBoxColumn Minimum;
            private System.Windows.Forms.DataGridViewTextBoxColumn Maximum;
            private System.Windows.Forms.DataGridViewComboBoxColumn GenerationRate;
            private System.Windows.Forms.DataGridViewTextBoxColumn Period;
            private System.Windows.Forms.DataGridViewTextBoxColumn DisplofPhase;
            private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
            private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
            private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
            private System.Windows.Forms.DataGridViewTextBoxColumn Comment;
        }
}
