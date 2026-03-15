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
using Mp.Scheme.Sdk;

namespace Mp.Mod.Event
{
    internal partial class ConfigEventDlg : Form
    {
        private EventDescription _eventDes;
        private Document _doc;

        public ConfigEventDlg(EventDescription descr, Document doc)
        {
            _eventDes = descr;
            _doc = doc;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            outputTarget.Items.Clear();

            if (_doc.RuntimeEngine.HasGUI)
            {
                outputTarget.Items.Add(StringResource.StadardOutput);
                outputTarget.Items.Add(StringResource.MessageBox);
            }
            else
            {
                outputTarget.Items.Add(StringResource.StadardOutput);
            }

            if (_doc.RuntimeEngine.SupportWindows)
                outputTarget.Items.Add(StringResource.EventLog);

            message.Text = _eventDes.Message;
            audioFile.Text = _eventDes.AudioFile;
            command.Text = _eventDes.Command;
            commandParam.Text = _eventDes.CommandParam;
            outputTarget.SelectedIndex = GetSelectedOutputTarget(_eventDes.OutputTarget);
            priority.SelectedIndex = _eventDes.Priority;
            audioPanel.Enabled = doc.RuntimeEngine.SupportPlaySound;            
        }

        private int GetItemIndex(string name)
        {
            int index = 0;
            foreach (string str in outputTarget.Items)
            {
                if (str == name)
                    return index;
                index++;
            }

            return 0;
        }

        private int GetSelectedOutputTarget(int target)
        {
            switch (target)
            {
                case 0:
                    return GetItemIndex(StringResource.StadardOutput);
                
                case 1:
                    return GetItemIndex(StringResource.MessageBox);
                
                case 2:
                    return GetItemIndex(StringResource.EventLog);
            }

            return 0;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            _eventDes.Message = message.Text;
            _eventDes.AudioFile = audioFile.Text;
            _eventDes.Command = command.Text;
            _eventDes.CommandParam = commandParam.Text;
            _eventDes.OutputTarget = GetOutputTarget();
            _eventDes.Priority = priority.SelectedIndex;

            DialogResult = DialogResult.OK;
            Close();
        }

        private int GetOutputTarget()
        {
            string item = outputTarget.SelectedItem.ToString();

            if (item == StringResource.StadardOutput)
                return 0;

            if (item == StringResource.MessageBox)
                return 1;

            if (item == StringResource.EventLog)
                return 2;

            return 0;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void onAudioFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.wav|*.wav";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            audioFile.Text = dlg.FileName;
        }

        private void onCommandFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.exe|*.exe|*.bat|*.bat|*.*|*.*";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            command.Text = dlg.FileName;
        }

        private void cmdFromProp_Click(object sender, EventArgs e)
        {
            SchemePropertyDlg propDlg = new SchemePropertyDlg(_doc);

            if (propDlg.ShowDialog() != DialogResult.OK)
                return;

            if (propDlg.SelectedProperties.Count == 0)
                return;

            command.Text = propDlg.SelectedProperties[0];
        }

        private void msgFromProp_Click(object sender, EventArgs e)
        {
            SchemePropertyDlg propDlg = new SchemePropertyDlg(_doc);
            
            if (propDlg.ShowDialog() != DialogResult.OK)
                return;

            if (propDlg.SelectedProperties.Count == 0)
                return;
            
            message.Text = propDlg.SelectedProperties[0];
        }

        private void paramFromProp_Click(object sender, EventArgs e)
        {
            SchemePropertyDlg propDlg = new SchemePropertyDlg(_doc);

            if (propDlg.ShowDialog() != DialogResult.OK)
                return;

            if (propDlg.SelectedProperties.Count == 0)
                return;

            commandParam.Text = propDlg.SelectedProperties[0];
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1110);
        }

        private void ConfigEventDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
