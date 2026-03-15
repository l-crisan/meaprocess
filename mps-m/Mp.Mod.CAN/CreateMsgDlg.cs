using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mp.Mod.CAN
{
    public partial class CreateMsgDlg : Form
    {
        public CreateMsgDlg()
        {
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            id.Text = "10";
        }


        public uint ID
        {
            get { return Convert.ToUInt32(id.Text); }
        }

        public new string Name
        {
            get { return _name.Text; }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void id_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                Convert.ToUInt32(id.Text);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(id, ex.Message);
            }
        }
    }
}
