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
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.GPS
{
    public partial class GpsPSDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlPS;

        public GpsPSDlg(Document doc, XmlElement xmlPS)
        {
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            _doc = doc;
            _xmlPS = xmlPS;

            port.Text = XmlHelper.GetParam(_xmlPS, "comPort");
            rate.Text = XmlHelper.GetParam(_xmlPS, "rate");
            name.Text = XmlHelper.GetParam(_xmlPS, "name");

        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (port.Text == "")
            {
                errorProvider.SetError(port, "A device port is expected.");
                return;
            }

            XmlHelper.SetParam(_xmlPS, "comPort","string", port.Text);
            XmlHelper.SetParam(_xmlPS, "rate", "string", rate.Text);
            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);
            DialogResult = DialogResult.OK;
            _doc.Modified = true;

            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void rate_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                if(!_doc.IsPropertyAvailable(rate.Text))
                    Convert.ToUInt32(rate.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(rate, ex.Message);
                e.Cancel = true;
            }
        }

        private void onPortProp_Click(object sender, EventArgs e)
        {
            SchemePropertyDlg dlg = new SchemePropertyDlg(_doc);

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if (dlg.SelectedProperties.Count == 0)
                return;

            port.Text = dlg.SelectedProperties[0];
        }

        private void onBaudProp_Click(object sender, EventArgs e)
        {
            SchemePropertyDlg dlg = new SchemePropertyDlg(_doc);

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if (dlg.SelectedProperties.Count == 0)
                return;

            rate.Text = dlg.SelectedProperties[0];
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 830);
        }

        private void GpsPSDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
