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

namespace Mp.Mod.Event
{
    public partial class WindowControlDlg : Form
    {
        SignalControlDlg.ControlInfo _ctrlInfo;

        public WindowControlDlg(SignalControlDlg.ControlInfo ctrlInfo)
        {
            _ctrlInfo = ctrlInfo;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            windowType.Items.Clear();
            windowType.Items.Add(StringResource.InTheWindow);
            windowType.Items.Add(StringResource.OutTheWindow);

            windowType.SelectedIndex = ctrlInfo.WindowType;
            lowerLimit.Text = ctrlInfo.Lower.ToString();
            upperLimit.Text = ctrlInfo.Upper.ToString();
        }

        private void Limit_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            Control ctrl = (Control) sender;
            try
            {
                Convert.ToDouble(ctrl.Text);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(ctrl, ex.Message);
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            double lower = Convert.ToDouble(lowerLimit.Text);
            double upper = Convert.ToDouble(upperLimit.Text);

            errorProvider.Clear();

            if (lower >= upper)
            {
                errorProvider.SetError(upperLimit, StringResource.LowerUpperErr);
                return;
            }
            _ctrlInfo.WindowType = windowType.SelectedIndex;
            _ctrlInfo.Lower = lower;
            _ctrlInfo.Upper = upper;
            Close();
        }
    }
}
