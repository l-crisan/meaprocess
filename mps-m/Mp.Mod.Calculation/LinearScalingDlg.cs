//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2015  Laurentiu-Gheorghe Crisan
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Calculation
{
    public partial class LinearScalingDlg : Form
    {
        private ScalingPSDlg.Scaling _scaling;

        public LinearScalingDlg(ScalingPSDlg.Scaling scaling)
        {
            _scaling = scaling;

            InitializeComponent();
            this.Icon = Document.AppIcon;
            _scaling.ScalingType = 0;

            name.Text = _scaling.SigName;
            unit.Text = _scaling.SigUnit;
            comment.Text = _scaling.SigComment;
            factor.Text = _scaling.Factor.ToString();
            offset.Text = _scaling.Offset.ToString();
        }

        private void value_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            Control ctrl = (Control) sender;
            try
            {
                Convert.ToDouble(ctrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ctrl, ex.Message);
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (name.Text == null || name.Text == "")
            {
                errorProvider.SetError(name, StringResource.SigNameErr);
                return;
            }

            _scaling.SigName = name.Text;
            _scaling.SigUnit = unit.Text;
            _scaling.SigComment = comment.Text;
            _scaling.Factor = Convert.ToDouble(factor.Text);
            _scaling.Offset = Convert.ToDouble(offset.Text);
            DialogResult = System.Windows.Forms.DialogResult.OK;

            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1400);
        }
    }
}
