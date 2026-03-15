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
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Storage
{
    /// <summary>
    /// The data storage property dialog.
    /// </summary>
    internal partial class DataStoragePSDlg : Form
    {
        Document _doc;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DataStoragePSDlg(Document doc)
        {
            _doc = doc;
            InitializeComponent();
            metaFileFormat.SelectedIndex = 0;
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            askUserForOverwrite.Enabled = doc.RuntimeEngine.HasGUI;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (fileCtrl.Text == "" || fileCtrl.Text == null)
            {
                errorProvider.SetError(fileCtrl, StringResource.DataStorageFileErr);
                return;
            }

            PSName = _PSName.Text;

            XmlHelper.SetParam(XmlRep,"fileName","string",fileCtrl.Text);
            XmlHelper.SetParamNumber(XmlRep, "metaFileFormat", "uint8_t", metaFileFormat.SelectedIndex);

            if(radioButtonOverwrite.Checked)
                XmlHelper.SetParamNumber(XmlRep,"fileOptions","uint8_t",0);
            
            if(radioButtonCreateNewFile.Checked)
                XmlHelper.SetParamNumber(XmlRep, "fileOptions", "uint8_t", 1);

            if(askUserForOverwrite.Checked)
                XmlHelper.SetParamNumber(XmlRep, "fileOptions", "uint8_t", 2);

            if(genarateTimeStampSignal.Checked)
                XmlHelper.SetParamNumber(XmlRep,"writeTimeSignal","bool",1);
            else
                XmlHelper.SetParamNumber(XmlRep,"writeTimeSignal","bool",0);

            XmlHelper.SetParam(XmlRep,"meaComment","string",storageDescription.Text);
            XmlHelper.SetParam(XmlRep, "command", "string", command.Text);
            XmlHelper.SetParam(XmlRep, "cmdParam", "string", commandParam.Text);

            for(int i = 0; i < XmlRep.ChildNodes.Count; ++i)                
            {
                XmlElement xmlProp = (XmlElement)XmlRep.ChildNodes[i];

                if (!xmlProp.HasAttributes)
                    continue;

                if (xmlProp.Attributes["name"] == null)
                    continue;

                if (xmlProp.Attributes["name"].Value != "property")
                    continue;

                XmlRep.RemoveChild(xmlProp);
                --i;
            }

            foreach (ListViewItem item in stProp.Items)
                XmlHelper.CreateElement(XmlRep, "string", "property", "$(" + item.SubItems[0].Text + ")");

            DialogResult = DialogResult.OK;
            _doc.Modified = true;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = false;
            dlg.CheckPathExists = true;
            dlg.Filter = "*.mmf|*.mmf|*.tdm|*.tdm|*.*|*.*";
            dlg.DefaultExt = "*.mmf";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            fileCtrl.Text = dlg.FileName;

            string ext = Path.GetExtension(dlg.FileName);
            ext = ext.ToUpper();

            switch (ext)
            {
                case ".MMF":
                    metaFileFormat.SelectedIndex = 0;
                    break;
                case ".TDM":
                    metaFileFormat.SelectedIndex = 1;
                    break;
            }            
        }

        private void PoDataStoragePSDlg_Load(object sender, EventArgs e)
        {
            _PSName.Text = PSName;
            metaFileFormat.SelectedIndex = (int) XmlHelper.GetParamNumber(XmlRep, "metaFileFormat");

            switch (XmlHelper.GetParamNumber(XmlRep, "fileOptions"))
            {
                case 0:
                    radioButtonOverwrite.Checked = true;
                break;
                
                case 1:
                    radioButtonCreateNewFile.Checked = true;
                break;      
                
                case 2:
                    askUserForOverwrite.Checked = true;
                break;
            }

            command.Text  =  XmlHelper.GetParam(XmlRep, "command");
            commandParam.Text = XmlHelper.GetParam(XmlRep, "cmdParam");


            genarateTimeStampSignal.Checked = XmlHelper.GetParamNumber(XmlRep, "writeTimeSignal") > 0;

            storageDescription.Text = XmlHelper.GetParam(XmlRep, "meaComment");
            fileCtrl.Text = XmlHelper.GetParam(XmlRep, "fileName");

            for (int i = 0; i < XmlRep.ChildNodes.Count; ++i)
            {
                XmlElement xmlProp = (XmlElement)XmlRep.ChildNodes[i];

                if (!xmlProp.HasAttributes)
                    continue;

                if (xmlProp.Attributes["name"] == null)
                    continue;

                if (xmlProp.Attributes["name"].Value != "property")
                    continue;

                string[] item = new string[1];

                string property = xmlProp.InnerText;
                property = property.Remove(0, 2);
                property = property.Remove(property.Length - 1, 1);
                item[0] = property;
                stProp.Items.Add(new ListViewItem(item));
            }
        }

        /// <summary>
        /// The process station name.
        /// </summary>
        public string PSName;

        /// <summary>
        /// The process station xml represenation.
        /// </summary>
        public XmlElement XmlRep;

        private void fromProperty_Click(object sender, EventArgs e)
        {
            SchemePropertyDlg dlg = new SchemePropertyDlg(_doc);
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if(dlg.SelectedProperties.Count != 0)
                fileCtrl.Text = dlg.SelectedProperties[0];
        }

        private void DataStoragePSDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Document.ShowHelp(this,360);
        }

        private void help_Click(object sender, EventArgs e)
        {
            DataStoragePSDlg_HelpRequested(sender,null);
        }

        private void desFromProperty_Click(object sender, EventArgs e)
        {
            SchemePropertyDlg dlg = new SchemePropertyDlg(_doc);

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if(dlg.SelectedProperties.Count != 0)
                storageDescription.Text = dlg.SelectedProperties[0];
        }

        private void addProperty_Click(object sender, EventArgs e)
        {
            SchemePropertyDlg dlg = new SchemePropertyDlg(_doc);

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if (dlg.SelectedProperties.Count == 0)
                return;

            foreach (string prop in dlg.SelectedProperties)
            {
                string[] item = new string[1];

                string property = prop;
                property = property.Remove(0, 2);
                property = property.Remove(property.Length - 1, 1);
                item[0] = property;
                bool found = false;
                foreach (ListViewItem it in stProp.Items)
                {
                    if (it.SubItems[0].Text == item[0])
                    {
                        found = true;
                        break;
                    }
                }

                if(!found)
                    stProp.Items.Add(new ListViewItem(item));
            }
        }

        private void remove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in stProp.SelectedItems)
                stProp.Items.Remove(item);
        }

        private void onCommandFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.exe|*.exe|*.cmd|*.cmd|*.*|*.*";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            command.Text = dlg.FileName;
        }

        private void cmdFromProp_Click(object sender, EventArgs e)
        {
            SchemePropertyDlg dlg = new SchemePropertyDlg(_doc);

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if (dlg.SelectedProperties.Count != 0)
                command.Text = dlg.SelectedProperties[0];
        }
    }
}