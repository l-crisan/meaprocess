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
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Statistics
{
    public partial class StatisticValuesDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlPS;

        public StatisticValuesDlg(Document doc, XmlElement xmlPS)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            name.Text = XmlHelper.GetParam(_xmlPS, "name");
            uint noOfSamples = (uint) XmlHelper.GetParamNumber(_xmlPS, "samples");
            
            if( noOfSamples == 0)
                noOfSamples = 10;

            samples.Text = noOfSamples.ToString();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);
            long noOfSamples = Convert.ToUInt32(samples.Text);
                
            XmlHelper.SetParamNumber(_xmlPS, "samples","uint32_t", noOfSamples);

            DialogResult = System.Windows.Forms.DialogResult.OK;
            _doc.Modified = true;
            Close();
        }

        private void samples_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                uint noOfSamples = Convert.ToUInt32(samples.Text);

                if( noOfSamples == 0)
                    errorProvider.SetError(samples, StringResource.ZeroErr);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(samples, ex.Message);
                e.Cancel = true;
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1530);
        }
    }
}
