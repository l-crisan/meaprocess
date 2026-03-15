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
    internal partial class OptionsDlg : Form
    {
        public OptionsDlg()
        {
            InitializeComponent();
            
            showEOLMarkers.Checked = Mpal.Editor.Properties.Settings.Default.ShowEndOfLineMarkers;
            showLineNumbers.Checked = Mpal.Editor.Properties.Settings.Default.ShowLineNumbers;
            showSpaces.Checked = Mpal.Editor.Properties.Settings.Default.ShowSpaces;
            showTabs.Checked = Mpal.Editor.Properties.Settings.Default.ShowTabs;
            showVRuler.Checked = Mpal.Editor.Properties.Settings.Default.ShowVRuler;
            enableFolding.Checked = Mpal.Editor.Properties.Settings.Default.EnableFolding;
            fullRowLineSel.Checked = Mpal.Editor.Properties.Settings.Default.FullRowLineSel;
            serverIP.Text = Mpal.Editor.Properties.Settings.Default.DebuggerServerIP;
            serverPort.Text = Mpal.Editor.Properties.Settings.Default.DebuggerServerPort.ToString();
            useBuildInDebugger.Checked = Mpal.Editor.Properties.Settings.Default.UseBuildInDebugger;
            memSize.Text = Mpal.Editor.Properties.Settings.Default.VmMemSize.ToString();
            targetMachine.SelectedIndex = Mpal.Editor.Properties.Settings.Default.TargetMachine;
            supportLREAL.Checked = Mpal.Editor.Properties.Settings.Default.SupportLREAL;
            supportINT64.Checked = Mpal.Editor.Properties.Settings.Default.SupportINT64;
            supportMathLIB.Checked = Mpal.Editor.Properties.Settings.Default.SupportMathLIB;
            expAsConst.Checked = Mpal.Editor.Properties.Settings.Default.ExpAsConst;
            expAsStatic.Checked = Mpal.Editor.Properties.Settings.Default.ExpAsStatic;
            expBytePostfix.Text = Mpal.Editor.Properties.Settings.Default.ExpBytePostfix;
            expBytePrefix.Text = Mpal.Editor.Properties.Settings.Default.ExpBytePrefix;
            expIncGuardName.Text = Mpal.Editor.Properties.Settings.Default.ExpIncGuardName;
            expVarName.Text = Mpal.Editor.Properties.Settings.Default.ExpVarName;
            expNewLineAfter.Text = Mpal.Editor.Properties.Settings.Default.ExpNewLineAfter.ToString();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Mpal.Editor.Properties.Settings.Default.ShowEndOfLineMarkers = showEOLMarkers.Checked;
            Mpal.Editor.Properties.Settings.Default.ShowLineNumbers = showLineNumbers.Checked;
            Mpal.Editor.Properties.Settings.Default.ShowSpaces = showSpaces.Checked;
            Mpal.Editor.Properties.Settings.Default.ShowTabs = showTabs.Checked;
            Mpal.Editor.Properties.Settings.Default.ShowVRuler = showVRuler.Checked;
            Mpal.Editor.Properties.Settings.Default.EnableFolding = enableFolding.Checked;
            Mpal.Editor.Properties.Settings.Default.FullRowLineSel = fullRowLineSel.Checked;
            Mpal.Editor.Properties.Settings.Default.DebuggerServerIP = serverIP.Text;
            Mpal.Editor.Properties.Settings.Default.DebuggerServerPort = Convert.ToInt32(serverPort.Text);
            Mpal.Editor.Properties.Settings.Default.UseBuildInDebugger = useBuildInDebugger.Checked;
            Mpal.Editor.Properties.Settings.Default.VmMemSize = Convert.ToUInt32(memSize.Text);
            Mpal.Editor.Properties.Settings.Default.TargetMachine = targetMachine.SelectedIndex;
            Mpal.Editor.Properties.Settings.Default.SupportLREAL = supportLREAL.Checked;
            Mpal.Editor.Properties.Settings.Default.SupportINT64 = supportINT64.Checked;
            Mpal.Editor.Properties.Settings.Default.SupportMathLIB = supportMathLIB.Checked;
            Mpal.Editor.Properties.Settings.Default.ExpAsConst = expAsConst.Checked;
            Mpal.Editor.Properties.Settings.Default.ExpAsStatic = expAsStatic.Checked;
            Mpal.Editor.Properties.Settings.Default.ExpBytePostfix = expBytePostfix.Text;
            Mpal.Editor.Properties.Settings.Default.ExpBytePrefix = expBytePrefix.Text;
            Mpal.Editor.Properties.Settings.Default.ExpIncGuardName = expIncGuardName.Text;
            Mpal.Editor.Properties.Settings.Default.ExpVarName = expVarName.Text;
            Mpal.Editor.Properties.Settings.Default.ExpNewLineAfter = Convert.ToInt32(expNewLineAfter.Text);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void useBuildInDebugger_CheckedChanged(object sender, EventArgs e)
        {
            if (useBuildInDebugger.Checked)
            {
                serverIP.Text = "127.0.0.1";
                serverIP.Enabled = false;
            }
            else
            {
                serverIP.Enabled = true;
            }
        }
      

        private void serverIP_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            string[] data = serverIP.Text.Split('.');

            if (data.Length != 4)
            {
                errorProvider.SetError(serverIP, StringResource.InvalidIP);
                e.Cancel = true;
                return;
            }

            try
            {
                Convert.ToByte(data[0]);
                Convert.ToByte(data[1]);
                Convert.ToByte(data[2]);
                Convert.ToByte(data[3]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                errorProvider.SetError(serverIP, StringResource.InvalidIP);
                e.Cancel = true;
                return;
            }
        }

        private void serverPort_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(serverPort.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(serverPort, ex.Message);
                e.Cancel = true;
            }
        }

        private void memSize_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                uint size = Convert.ToUInt32(memSize.Text);
                if (size == 0)
                {
                    e.Cancel = true;
                    errorProvider.SetError(memSize, StringResource.MemSizeErr);
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(memSize, ex.Message);
                e.Cancel = true;
            }
        }

        private void help_Click(object sender, EventArgs e)
        {

        }

        private void expNewLineAfter_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt16(expNewLineAfter.Text);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(expNewLineAfter, ex.Message);
            }
        }

    }
}
