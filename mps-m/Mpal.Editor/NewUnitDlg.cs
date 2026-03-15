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
    internal partial class NewUnitDlg : Form
    {
        private string _unitTemplate;

        public NewUnitDlg()
        {
            InitializeComponent();
        }
        
        public string UnitTemplate
        {
            get { return _unitTemplate; }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if( unitName.Text == "")
            {
                errorProvider.SetError(unitName, StringResource.UnitNameErr);
                return;
            }

            _unitTemplate = "/*\r\n" + description.Text + "\r\n*/\r\n";
            _unitTemplate += "UNIT " + unitName.Text +"\r\n\r\n";
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void NewUnitDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            NewUnitDlg_HelpButtonClicked(sender, null);
        }

        private void NewUnitDlg_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            HelpHandler.ShowHelp(this, "190");
            
            if( e != null)
                e.Cancel = true;
        }
    }
}
