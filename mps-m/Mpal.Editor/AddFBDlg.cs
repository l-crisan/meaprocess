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

namespace Mpal.Editor
{
    internal partial class AddFBDlg : Form
    {
        private string _programCode;
        private InsertVariableCtrl _varControls = new InsertVariableCtrl(true);

        public AddFBDlg()
        {
            InitializeComponent();

            Controls.Add(_varControls);
            _varControls.BringToFront();
            _varControls.Dock = DockStyle.Fill;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (progName.Text == "")
            {
                errorProvider.SetError(progName, "A name for the function is expected.");
                return;
            }

            //Create the 
            _programCode = "FUNCTION_BLOCK " + progName.Text + "\r\n\r\n";
            _programCode += _varControls.Template;
            _programCode += "//Insert your code here\r\n";
            _programCode += "END_FUNCTION_BLOCK\r\n\r\n";
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public string ProgramCode
        {
            get { return _programCode; }
        }

        private void AddFBDlg_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            HelpHandler.ShowHelp(this, "180");
        }

        private void AddFBDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            HelpHandler.ShowHelp(this, "180");
        }
    }
}
