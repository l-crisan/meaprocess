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
    public partial class SlopeDlg : Form
    {
        SignalControlDlg.ControlInfo _ctrlInfo;

        public SlopeDlg(SignalControlDlg.ControlInfo ctrlInfo)
        {
            _ctrlInfo = ctrlInfo;

            InitializeComponent();

            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            type.Items.Add(StringResource.Increasing);
            type.Items.Add(StringResource.Decreasing);

            threshold.Text = _ctrlInfo.SlopeValue.ToString();
            type.SelectedIndex = _ctrlInfo.SlopeType;
        }

        private void threshold_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                Convert.ToDouble(threshold.Text);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(threshold, ex.Message);
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            _ctrlInfo.SlopeValue = Convert.ToDouble(threshold.Text);
            _ctrlInfo.SlopeType = type.SelectedIndex;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
