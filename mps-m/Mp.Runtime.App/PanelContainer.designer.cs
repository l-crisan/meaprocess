namespace Mp.Runtime.App
{
    partial class PanelContainer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelContainer));
            this.SuspendLayout();
            // 
            // PanelContainer
            // 
            this.AllowDrop = true;
            this.ClientSize = new System.Drawing.Size(813, 428);
            this.CloseButton = false;
            this.DockAreas = ((Mp.Visual.Docking.DockAreas)((Mp.Visual.Docking.DockAreas.Float | Mp.Visual.Docking.DockAreas.Document)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PanelContainer";
            this.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

		}
		#endregion

    }
}