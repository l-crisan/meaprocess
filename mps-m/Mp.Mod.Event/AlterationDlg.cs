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
    public partial class AlterationDlg : Form
    {
        SignalControlDlg.ControlInfo _ctrlInfo;

        public AlterationDlg(SignalControlDlg.ControlInfo ctrlInfo)
        {
            _ctrlInfo = ctrlInfo;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            delta.Text = _ctrlInfo.Alteration.ToString();
            signalIf.SelectedIndex = _ctrlInfo.SignalIf;
            absolut.Checked =  (_ctrlInfo.Absolut == 1);
        }

        private void delta_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToDouble(delta.Text);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(delta, ex.Message);
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            _ctrlInfo.Alteration = Convert.ToDouble(delta.Text);
            _ctrlInfo.SignalIf = signalIf.SelectedIndex;

            if (absolut.Checked)
                _ctrlInfo.Absolut = 1;
            else
                _ctrlInfo.Absolut = 0;

            Close();
        }
    }
}
