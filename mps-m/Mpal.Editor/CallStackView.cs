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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Mpal.Debugger;
using Mpal.Model;
using Mp.Visual.Tree;
using Mp.Visual.Tree.Tree;
using Mp.Visual.Tree.Tree.NodeControls;
using Mp.Visual.Docking;

namespace Mpal.Editor
{
    public partial class CallStackView : DockContent
    {
        private string _callStack;

        public CallStackView()
        {
            InitializeComponent();         
        }

        public string CallStack
        {
            get { return _callStack; }
            set
            {
                _callStack = value;
                callView.Items.Clear();

                if (_callStack == "" || _callStack == null)
                    return;                

                string[] array = _callStack.Split('\n');
                int pos = 1;
                foreach (string func in array)
                {
                    if (func == "")
                        continue;

                    string[] data = new string[3];
                    data[0] = pos.ToString();
                    
                    string[] types = func.Split(':');
                    data[1] = types[0];
                    data[2] = types[1];

                    ListViewItem item = new ListViewItem(data);
                    callView.Items.Add(item);
                    ++pos;
                }
            }
        }
    }
}
