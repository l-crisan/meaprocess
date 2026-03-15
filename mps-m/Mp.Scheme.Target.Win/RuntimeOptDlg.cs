using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Atesion.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Scheme.Win
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

            language.Items.Add(new Atesion.Utils.ComboBoxExItem("English", 0));
            language.Items.Add(new Atesion.Utils.ComboBoxExItem("Deutsch", 1));

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

                showMenu.Checked = XmlHelper.GetParamNumber(_xmlGUI, "showMenu") > 0;
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
                language.SelectedIndex = (int) XmlHelper.GetParamNumber(_xmlGUI, "language");
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

            XmlHelper.SetParamNumber(_xmlGUI, "language", "uint8_t", language.SelectedIndex);

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

            _doc.Modify = true;
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

            Image img = Image.FromFile(dlg.FileName);            
            icon.Image = img;
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
    }
}
