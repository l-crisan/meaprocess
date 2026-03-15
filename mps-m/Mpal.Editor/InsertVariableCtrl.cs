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
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;

namespace Mpal.Editor
{
    internal partial class InsertVariableCtrl : UserControl
    {
        public InsertVariableCtrl(bool enableTempVar)
        {
            InitializeComponent();

            DataGridViewRow row = inputVar.Rows[0];
            row.Cells[1].Value = "DINT";

            row = outputVar.Rows[0];
            row.Cells[1].Value = "DINT";

            row = inoutVar.Rows[0];
            row.Cells[1].Value = "DINT";

            row = variables.Rows[0];
            row.Cells[1].Value = "DINT";

            row = tempVariables.Rows[0];
            row.Cells[1].Value = "DINT";

            tempVariables.Enabled = enableTempVar;

        }

        public string Template
        {
            get
            {
                string template = "";
                //Input var
                if (inputVar.Rows.Count != 1)
                {
                    template += "VAR_INPUT\r\n";

                    for (int i = 0; i < inputVar.Rows.Count - 1; ++i)
                    {
                        DataGridViewRow row = inputVar.Rows[i];
                        template += "\t" + row.Cells[0].Value + "\t: " + row.Cells[1].Value + ";\r\n";
                    }

                    template += "END_VAR\r\n\r\n";
                }


                //output var
                if (outputVar.Rows.Count != 1)
                {
                    template += "VAR_OUTPUT\r\n";

                    for (int i = 0; i < outputVar.Rows.Count - 1; ++i)
                    {
                        DataGridViewRow row = outputVar.Rows[i];
                        template += "\t" + row.Cells[0].Value + "\t: " + row.Cells[1].Value + ";\r\n";
                    }

                    template += "END_VAR\r\n\r\n";
                }


                //inout var
                if (inoutVar.Rows.Count != 1)
                {
                    template += "VAR_IN_OUT\r\n";

                    for (int i = 0; i < inoutVar.Rows.Count - 1; ++i)
                    {
                        DataGridViewRow row = inoutVar.Rows[i];
                        template += "\t" + row.Cells[0].Value + "\t: " + row.Cells[1].Value + ";\r\n";
                    }

                    template += "END_VAR\r\n\r\n";
                }


                //variables var
                if (variables.Rows.Count != 1)
                {
                    template += "VAR\r\n";

                    for (int i = 0; i < variables.Rows.Count - 1; ++i)
                    {
                        DataGridViewRow row = variables.Rows[i];
                        template += "\t" + row.Cells[0].Value + "\t: " + row.Cells[1].Value + ";\r\n";
                    }

                    template += "END_VAR\r\n\r\n";
                }

                //temp var var
                if (tempVariables.Rows.Count != 1)
                {
                    template += "VAR_TEMP\r\n";

                    for (int i = 0; i < tempVariables.Rows.Count - 1; ++i)
                    {
                        DataGridViewRow row = tempVariables.Rows[i];
                        template += "\t" + row.Cells[0].Value + "\t: " + row.Cells[1].Value + ";\r\n";
                    }

                    template += "END_VAR\r\n\r\n";
                }

                return template;
            }
        }

        private void inputVar_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = inputVar.Rows[e.RowIndex];
            row.Cells[1].Value = "DINT";
        }

        private void outputVar_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = outputVar.Rows[e.RowIndex];
            row.Cells[1].Value = "DINT";
        }

        private void inoutVar_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = inoutVar.Rows[e.RowIndex];
            row.Cells[1].Value = "DINT";
        }

        private void variables_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = variables.Rows[e.RowIndex];
            row.Cells[1].Value = "DINT";
        }

        private void tempVariables_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = tempVariables.Rows[e.RowIndex];
            row.Cells[1].Value = "DINT";
        }
    }
}
