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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public partial class RuntimeOptDlg : Form
    {
        private XmlElement _xmlGUI;
        private Document _doc;
        
        public RuntimeOptDlg(Document doc)
        {
            _doc = doc;
            _xmlGUI = XmlHelper.GetChildByType(_doc.XmlDoc.DocumentElement, "GUI");
            
            InitializeComponent();
            this.Icon = Document.AppIcon;
            images.Images.Add(Mp.Scheme.Sdk.Ui.Images.English);
            images.Images.Add(Mp.Scheme.Sdk.Ui.Images.Deutsch);

            language.Items.Add(new Mp.Utils.ComboBoxExItem("English",0));
            language.Items.Add(new Mp.Utils.ComboBoxExItem("Deutsch",1));

            if (XmlHelper.GetParamNumber(_xmlGUI, "roptions") > 0)
            {
                title.Text = XmlHelper.GetParam(_xmlGUI, "title");
                try
                {                    
                    string iconstr = XmlHelper.GetParam(_xmlGUI, "icon");
                    if (iconstr != "")
                    {
                        byte[] buffer = Convert.FromBase64String(iconstr);

                        MemoryStream stream = new MemoryStream(buffer);
                        BinaryFormatter formater = new BinaryFormatter();

                        icon.Image = (Image) formater.Deserialize(stream);
                        stream.Flush();
                        stream.Seek(0, 0);
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (_doc.TimerResolution == 10)
                    systemClock.SelectedIndex = 0;
                else
                    systemClock.SelectedIndex = 1;

                showMenu.Checked = XmlHelper.GetParamNumber(_xmlGUI, "showMenu") > 0;
                fullScreenMode.Checked = XmlHelper.GetParamNumber(_xmlGUI, "fullScreen") > 0;
                showControlBar.Checked = XmlHelper.GetParamNumber(_xmlGUI, "showControlBar") > 0;
                showStatusBar.Checked = XmlHelper.GetParamNumber(_xmlGUI, "showStatusBar") > 0;
                undockPanels.Checked = XmlHelper.GetParamNumber(_xmlGUI, "undockPanels") > 0;
                startOnOpen.Checked = XmlHelper.GetParamNumber(_xmlGUI, "startOnOpen") > 0;
                fixedWinSize.Checked = XmlHelper.GetParamNumber(_xmlGUI, "fixedWinSize") > 0;
                width.Text = XmlHelper.GetParamNumber(_xmlGUI, "rwidth").ToString();
                height.Text = XmlHelper.GetParamNumber(_xmlGUI, "rheight").ToString();
                resetPropOnStart.Checked =  XmlHelper.GetParamNumber(_xmlGUI, "resetPropOnStart")> 0;
                mandatoryPropFlag.Checked =  XmlHelper.GetParamNumber(_xmlGUI, "mandatoryFlagVisible")> 0;
                closeOnStop.Checked = XmlHelper.GetParamNumber(_xmlGUI, "closeOnStop") > 0;
                editPrpBtFlag.Checked = XmlHelper.GetParamNumber(_xmlGUI, "editPropBt") > 0;
                ctrlBarEditProp.Checked = XmlHelper.GetParamNumber(_xmlGUI, "ctrlBarEditPropBt") > 0;
                showPanelTab.Checked = XmlHelper.GetParamNumber(_xmlGUI, "hideTabForPanal") == 0;
                logFilePath.Text = XmlHelper.GetParam(_xmlGUI, "logFile").Replace("file:///","");
                logLevel.SelectedIndex = (int) XmlHelper.GetParamNumber(_xmlGUI, "logLevel")/100;
                UpdateLanguage();
            }
        }


        private void UpdateLanguage()
        {
            int lang = (int)XmlHelper.GetParamNumber(_xmlGUI, "language");

            if (lang == 1)
                language.SelectedIndex = 0;
            else if (lang == 2)
                language.SelectedIndex = 1;
            else
            {
                if (Thread.CurrentThread.CurrentCulture.IetfLanguageTag == "de-DE")
                    language.SelectedIndex = 1;
                else
                    language.SelectedIndex = 0;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            XmlHelper.SetParam(_xmlGUI, "title", "string", title.Text);

            if (icon.Image != null)
            {
                MemoryStream stream = new MemoryStream();
                BinaryFormatter formater = new BinaryFormatter();

                formater.Serialize(stream, icon.Image);
                stream.Flush();
                stream.Seek(0, 0);

                byte[] buffer = new byte[stream.Length];

                stream.Read(buffer, 0, (int)stream.Length);
                stream.Close();
                XmlHelper.SetParam(_xmlGUI, "icon", "string", Convert.ToBase64String(buffer));
            }
            else
            {
                XmlHelper.SetParam(_xmlGUI, "icon", "string", "");
            }

            XmlHelper.SetParamNumber(_xmlGUI, "language", "uint8_t", language.SelectedIndex + 1);

            if (fullScreenMode.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "fullScreen", "uint8_t", 1);
            else
                XmlHelper.SetParamNumber(_xmlGUI, "fullScreen", "uint8_t", 0);

            if (showMenu.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "showMenu", "uint8_t", 1);
            else
                XmlHelper.SetParamNumber(_xmlGUI, "showMenu", "uint8_t", 0);

            if(showControlBar.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "showControlBar","uint8_t",1);
            else
                XmlHelper.SetParamNumber(_xmlGUI, "showControlBar", "uint8_t", 0);

            if(showStatusBar.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "showStatusBar", "uint8_t", 1);
            else
                XmlHelper.SetParamNumber(_xmlGUI, "showStatusBar", "uint8_t", 0);


            if(undockPanels.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "undockPanels","uint8_t",1);
            else
                XmlHelper.SetParamNumber(_xmlGUI, "undockPanels", "uint8_t", 0);


            if (startOnOpen.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "startOnOpen", "uint8_t", 1);
            else
                XmlHelper.SetParamNumber(_xmlGUI, "startOnOpen", "uint8_t", 0);

            if(fixedWinSize.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "fixedWinSize","uint8_t",1);
            else
                XmlHelper.SetParamNumber(_xmlGUI, "fixedWinSize", "uint8_t", 0);

            XmlHelper.SetParamNumber(_xmlGUI, "rwidth", "uint32_t", Convert.ToUInt32(width.Text));
            XmlHelper.SetParamNumber(_xmlGUI, "rheight", "uint32_t", Convert.ToUInt32(height.Text));
            XmlHelper.SetParamNumber(_xmlGUI, "roptions", "uint8_t", 1);

            if( resetPropOnStart.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "resetPropOnStart", "uint8_t", 1);
            else
                XmlHelper.SetParamNumber(_xmlGUI, "resetPropOnStart", "uint8_t", 0);

            if(mandatoryPropFlag.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "mandatoryFlagVisible", "uint8_t", 1);
            else
                XmlHelper.SetParamNumber(_xmlGUI, "mandatoryFlagVisible", "uint8_t", 0);


            if(closeOnStop.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "closeOnStop","uint8_t",1);
            else
                XmlHelper.SetParamNumber(_xmlGUI, "closeOnStop", "uint8_t", 0);

            if(editPrpBtFlag.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "editPropBt","uint8_t",1);
            else
                XmlHelper.SetParamNumber(_xmlGUI, "editPropBt", "uint8_t", 0);

            if(ctrlBarEditProp.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "ctrlBarEditPropBt","uint8_t",1);  
            else
                XmlHelper.SetParamNumber(_xmlGUI, "ctrlBarEditPropBt", "uint8_t", 0);

            if(showPanelTab.Checked)
                XmlHelper.SetParamNumber(_xmlGUI, "hideTabForPanal","uint8_t", 0);  
            else
                XmlHelper.SetParamNumber(_xmlGUI, "hideTabForPanal", "uint8_t", 1);

            if(logFilePath.Text == "")
                XmlHelper.SetParam(_xmlGUI, "logFile", "string", "");
            else
                XmlHelper.SetParam(_xmlGUI, "logFile", "string", "file:///" + logFilePath.Text);

            XmlHelper.SetParamNumber(_xmlGUI, "logLevel", "int32_t", logLevel.SelectedIndex*100);

            if (systemClock.SelectedIndex == 0)
                _doc.TimerResolution = 10;
            else
                _doc.TimerResolution = 1;

            _doc.Modified = true;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void defineSize_Click(object sender, EventArgs e)
        {
            SizeWindow wnd = new SizeWindow();
            wnd.Height = Convert.ToInt32(height.Text);
            wnd.Width = Convert.ToInt32(width.Text);
            wnd.ShowDialog();
            height.Text = wnd.Height.ToString();
            width.Text = wnd.Width.ToString();
        }

        private void fixetWinSize_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxSize.Enabled = fixedWinSize.Checked;
        }

        private void width_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(width.Text);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(width, ex.Message);
            }
        }

        private void height_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(height.Text);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(height, ex.Message);
            }
        }

        private void onIcon_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.ico|*.ico";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                Image img = Image.FromFile(dlg.FileName);
                icon.Image = img;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            icon.Image = null;
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 640);
        }

        private void RuntimeOptDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }

        private void getLogFilePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = false;
            dlg.Filter = "*.log|*.log|*.txt|*.txt|*.*|*.*";
            
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            logFilePath.Text = dlg.FileName;
        }
    }
}
