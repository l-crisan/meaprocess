using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mp.Scheme.Win
{
    public partial class SizeWindow : Form
    {
        public SizeWindow()
        {
            InitializeComponent();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            width.Text = Width.ToString();
            height.Text = Height.ToString();
        }
    }
}
