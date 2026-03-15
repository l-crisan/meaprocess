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
    public partial class IIRFilterDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlPS;

        public IIRFilterDlg(Document doc, XmlElement xmlPS)
        {
            _doc = doc;
            _xmlPS = xmlPS;

            InitializeComponent();
            this.Icon = Document.AppIcon;
            name.Text = XmlHelper.GetParam(_xmlPS, "name");
            ftype.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlPS, "ftype");
            int index = (int)XmlHelper.GetParamNumber(_xmlPS, "forder") -2;

            if (index < 0)
                forder.SelectedIndex = 0;
            else
                forder.SelectedIndex = index;

            uint lowerPassValue = (uint)XmlHelper.GetParamNumber(_xmlPS, "lowerPass");
            uint upperPassValue = (uint)XmlHelper.GetParamNumber(_xmlPS, "upperPass");

            if (upperPassValue == 0 && lowerPassValue == 0)
                upperPassValue = 100;

            lowerPass.Text = lowerPassValue.ToString();
            upperPass.Text = upperPassValue.ToString();
            long bw = XmlHelper.GetParamNumber(_xmlPS, "transitionBW");
            
            if( bw == 0)
                bw = 1;

            transitionBW.Text = bw.ToString();

            long sb = XmlHelper.GetParamNumber(_xmlPS, "stopBand");

            if (sb == 0)
                sb = 1;

            stopBand.Text = sb.ToString();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            uint lower = Convert.ToUInt32(lowerPass.Text);
            uint upper = Convert.ToUInt32(upperPass.Text);

            if (upper <= lower)
            {
                errorProvider.SetError(upperPass, StringResource.UpperLowerBandErr);
                return;
            }


            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);
            XmlHelper.SetParamNumber(_xmlPS, "ftype","uint8_t",ftype.SelectedIndex);
            XmlHelper.SetParamNumber(_xmlPS, "forder","uint8_t",forder.SelectedIndex + 2);

            XmlHelper.SetParamNumber(_xmlPS, "lowerPass", "uint32_t", lower);
            XmlHelper.SetParamNumber(_xmlPS, "upperPass", "uint32_t", upper);
            XmlHelper.SetParamNumber(_xmlPS, "transitionBW", "uint32_t", Convert.ToUInt32(transitionBW.Text));
            XmlHelper.SetParamNumber(_xmlPS, "stopBand", "uint32_t", Convert.ToUInt32(stopBand.Text));
            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lowerPass_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(lowerPass.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(lowerPass, ex.Message);
                e.Cancel = true;
            }
        }

        private void upperPass_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(upperPass.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(upperPass, ex.Message);
                e.Cancel = true;
            }
        }

        private void transitionBW_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(transitionBW.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(transitionBW, ex.Message);
                e.Cancel = true;
            }
        }

        private void stopBand_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(stopBand.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(stopBand, ex.Message);
                e.Cancel = true;
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 990);
        }

        private void IIRFilterDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
