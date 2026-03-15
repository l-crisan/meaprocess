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
using System.IO;
using System.Globalization;

namespace Mp.Mod.Calculation
{
    public partial class TableScalingImportDlg : Form
    {
        private DataGridView _table;

        public TableScalingImportDlg(DataGridView table)
        {
            _table = table;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
        }

        private void OK_Click(object sender, EventArgs e)
        {

            errorProvider.Clear();

            if (separator.Text.Length != 1)
            {
                errorProvider.SetError(separator, StringResource.OnCharExpectedErr);
                return;
            }

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.csv|*.csv|*.*|*.*";

            dlg.CheckFileExists = true;

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            try
            {
                NumberFormatInfo info = new NumberFormatInfo();
                info.NumberDecimalSeparator = decimalPoint.Text;
                
                using (StreamReader sr = new StreamReader(dlg.FileName))
                {
                    _table.Rows.Clear();

                    while(!sr.EndOfStream)
                    {
                        string[] array = sr.ReadLine().Split(separator.Text[0]);
                        if (array.Length < 2)
                        {
                            errorProvider.SetError(OK, StringResource.WrongFileFormatErr);
                            return;
                        }

                        int index =_table.Rows.Add();

                        try
                        {
                            _table.Rows[index].Cells[0].Value = Convert.ToDouble(array[0], info);
                            _table.Rows[index].Cells[1].Value = Convert.ToDouble(array[1], info);
                        }
                        catch(Exception exception)
                        {                            
                            errorProvider.SetError(OK, StringResource.WrongFileFormatErr);
                            Console.WriteLine(exception.Message);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(OK, ex.Message);
                return;
            }

            Close();            
        }
    }
}
