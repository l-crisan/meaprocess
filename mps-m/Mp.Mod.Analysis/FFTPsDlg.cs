//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2010-2016  Laurentiu-Gheorghe Crisan
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
using System.Windows.Forms;
using System.Xml;
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Analysis
{

    public partial class FFTPsDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlPS;
        private XmlElement _xmlSignalList;


        public FFTPsDlg(Document doc, XmlElement xmlPS, XmlElement xmlSignalList)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            _xmlSignalList = xmlSignalList;

            InitializeComponent();
            this.Icon = Document.AppIcon;
            name.Text = XmlHelper.GetParam(_xmlPS, "name");
            window.SelectedIndex = (int) XmlHelper.GetParamNumber(_xmlPS, "window");
            int n = (int) XmlHelper.GetParamNumber(_xmlPS, "nfft");

            if (n == 0)
                nfft.Text = "1000";
            else
                nfft.Text = n.ToString();

        }

        private void OK_Click(object sender, EventArgs e)
        {
            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);
            XmlHelper.SetParamNumber(_xmlPS, "window", "uint8_t", window.SelectedIndex);
            XmlHelper.SetParamNumber(_xmlPS, "nfft", "uint32_t", Convert.ToUInt32(nfft.Text));
            

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void nfft_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                if (Convert.ToUInt32(nfft.Text) % 2 != 0)
                {
                    errorProvider.SetError(nfft, StringResource.EvenPointsErr);
                    e.Cancel = true;
                }

            }
            catch (Exception ex)
            {
                errorProvider.SetError(nfft, ex.Message);
                e.Cancel = true;
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 970);
        }    
    }
}
