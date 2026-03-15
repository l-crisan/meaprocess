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
using System.IO;
using System.Text;
using System.Windows.Forms;
using Mp.Visual.Docking;

namespace Mpal.Editor
{
    internal delegate void ErrorSelected(int line, int col);

    internal partial class ErrorListWindow : DockContent
    {
        public ErrorListWindow()
        {
            InitializeComponent();
        }

        public event ErrorSelected OnErrorSelected;

        public void Clear()
        {
            errorList.Items.Clear();
        }

        public string[] WriteLine(string text)
        {
            string[] message = GetErrorInfo(text);

            errorList.Items.Add(new ListViewItem(message,0));
            return message;
        }

        public static string[] GetErrorInfo(string text)
        {
            string[] array = text.Split(':');

            string[] message = new string[5];

            //C:\Users\cr\Desktop\test.mpal (47,4): error C1002: Undefined symbol 'INCv'

            if (array.Length < 3)
                return null;

            if (array[0].Length == 1)
            {
                message[0] = array[2].Replace("error", "");
                message[1] = array[3];

                int beginFileIndex = -1;

                for (int i = array[1].Length - 1; i > -1; --i)
                {
                    if (array[1][i] == '(')
                    {
                        beginFileIndex = i;
                        break;
                    }
                }

                message[2] = array[0] + ":" + array[1].Substring(0, beginFileIndex);
                string pos = array[1].Substring(beginFileIndex, array[1].Length - beginFileIndex);
                message[3] = pos.Split(',')[0];
                message[3] = message[3].TrimStart('(');
                message[4] = pos.Split(',')[1];
                message[4] = message[4].TrimEnd(')');
            }
            else
            {
                message[0] = array[1].Replace("error", "");
                message[1] = array[2];
                string[] filePosArr = array[0].Split('(');
                message[2] = filePosArr[0];
                string pos = filePosArr[1].TrimEnd(')');
                message[3] = pos.Split(',')[0];
                message[4] = pos.Split(',')[1];
         
            }
            return message;
        }

        private void errorList_DoubleClick(object sender, EventArgs e)
        {
            if (errorList.SelectedItems.Count == 0)
                return;

            if (OnErrorSelected == null)
                return;

            ListViewItem item = errorList.SelectedItems[0];

            string line = item.SubItems[3].Text;
            string col = item.SubItems[4].Text;

            OnErrorSelected(Convert.ToInt32(line), Convert.ToInt32(col));            
        }

        private void errorList_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            if (errorList.SelectedItems.Count == 0)
                return;

            if (OnErrorSelected == null)
                return;

            ListViewItem item = errorList.SelectedItems[0];

            string code = item.SubItems[0].Text;
            code = code.TrimStart(' ');
            HelpHandler.SearchInHelp(this, code);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            errorList_HelpRequested(null, null);
        }

    }
}