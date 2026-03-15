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
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Scheme.App
{
    public partial class SchemePasswordDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlDoc;

        public SchemePasswordDlg(Document doc)
        {
            _doc = doc;

            InitializeComponent();
            this.Icon = Document.AppIcon;
            _xmlDoc = _doc.XmlDoc.DocumentElement;
            scheme.Checked = XmlHelper.GetParamNumber(_xmlDoc, "useSchemePassword") != 0;
            runtime.Checked = XmlHelper.GetParamNumber(_xmlDoc, "useRuntimePassword") != 0;
            schemePassword1.Text = XmlHelper.GetParam(_xmlDoc, "schemePassword");
            schemePassword2.Text = XmlHelper.GetParam(_xmlDoc, "schemePassword");
            runtimePassword1.Text = XmlHelper.GetParam(_xmlDoc, "runtimePassword");
            runtimePassword2.Text = XmlHelper.GetParam(_xmlDoc, "runtimePassword");            
        }


        private void OnSchemeCheckedChanged(object sender, EventArgs e)
        {
            schemeGroup.Enabled = scheme.Checked;
        }


        private void OnRuntimeCheckedChanged(object sender, EventArgs e)
        {
            runtimeGroup.Enabled = runtime.Checked;
        }


        private void OnOKClick(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (scheme.Checked)
            {
                if (schemePassword1.Text != schemePassword2.Text)
                {
                    errorProvider.SetError(schemePassword2, StringResource.PasswordErr);
                    return;
                }
            }

            if (runtime.Checked)
            {
                if (runtimePassword1.Text != runtimePassword2.Text)
                {
                    errorProvider.SetError(runtimePassword2, StringResource.PasswordErr);
                    return;
                }
            }

            if (scheme.Checked)
                XmlHelper.SetParamNumber(_xmlDoc, "useSchemePassword", "uint8_t", 1);
            else
                XmlHelper.SetParamNumber(_xmlDoc, "useSchemePassword", "uint8_t", 0);

            if(runtime.Checked)
                XmlHelper.SetParamNumber(_xmlDoc, "useRuntimePassword","uint8_t",1);
            else
                XmlHelper.SetParamNumber(_xmlDoc, "useRuntimePassword", "uint8_t", 0);

            XmlHelper.SetParam(_xmlDoc, "schemePassword","string",schemePassword1.Text);
            XmlHelper.SetParam(_xmlDoc, "runtimePassword", "string", runtimePassword1.Text);

            _doc.Modified = true;
            Close();
        }


        private void OnHelpClick(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1270);
        }
    }
}
