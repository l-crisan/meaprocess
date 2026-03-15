namespace Mp.Scheme.Designer
{
    partial class PropertieWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertieWindow));
            this.panel1 = new System.Windows.Forms.Panel();
            this.processStation = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.processStation);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Name = "panel1";
            // 
            // processStation
            // 
            this.processStation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.processStation, "processStation");
            this.processStation.Name = "processStation";
            this.processStation.ReadOnly = true;
            // 
            // PropertieWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CloseButton = false;
            this.Controls.Add(this.panel1);
            this.DockAreas = ((Mp.Visual.Docking.DockAreas)(((((Mp.Visual.Docking.DockAreas.Float | Mp.Visual.Docking.DockAreas.DockLeft)
                        | Mp.Visual.Docking.DockAreas.DockRight)
                        | Mp.Visual.Docking.DockAreas.DockTop)
                        | Mp.Visual.Docking.DockAreas.DockBottom)));
            this.Name = "PropertieWindow";
            this.ShowInTaskbar = false;
            this.TabText = "Properties";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox processStation;
    }
}