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
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    /// <summary>
    /// The data storage trigger port property dialog.
    /// </summary>
    public partial class TriggerPortDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlRep;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TriggerPortDlg(Document doc, XmlElement xmlRep)
        {
            _doc = doc;
            _xmlRep = xmlRep;
            InitializeComponent();
            this.Icon = Document.AppIcon;
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            double preTime = 0.0;
            double postTime = 0.0;
            errorProvider.Clear();

            try
            {              
                preTime = Double.Parse(_preTime.Text);

                if (preTime < 0)
                {
                    errorProvider.SetError(_postTime, StringResource.PreTriggerErr);
                    return;
                }

            }
            catch (Exception ex)
            {
                errorProvider.SetError(_preTime, ex.Message);
                return;
            }

            try
            {
                postTime = Double.Parse(_postTime.Text);
                if (postTime < 0)
                {
                    errorProvider.SetError(_postTime, StringResource.PostTriggerErr);
                    return;
                }
                                
            }
            catch(Exception exc)
            {
                errorProvider.SetError(_postTime, exc.Message);
                return;
            }

            if (postTime == 0 && preTime == 0 && TriggerType.SelectedIndex == 4)//Event Trigger
            {
                errorProvider.SetError(_postTime, StringResource.TriggerTimePerErr);
                return;
            }

            XmlHelper.SetParamNumber(_xmlRep, "triggerType", "uint8_t", TriggerType.SelectedIndex);
            XmlHelper.SetParamDouble(_xmlRep, "preEvenTime", "double", preTime);
            XmlHelper.SetParamDouble(_xmlRep, "postEvenTime", "double", postTime);

            if (OneStartStopTrigger.Checked)
                XmlHelper.SetParamNumber(_xmlRep, "oneStartStopSignal", "bool", 1);
            else
                XmlHelper.SetParamNumber(_xmlRep, "oneStartStopSignal", "bool", 0);

            _doc.Modified = true;
            Close();
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            TriggerType.SelectedIndex = (int) XmlHelper.GetParamNumber(_xmlRep, "triggerType");
            Double db = XmlHelper.GetParamDouble(_xmlRep, "preEvenTime");

            _preTime.Text = db.ToString();
            db = XmlHelper.GetParamDouble(_xmlRep, "postEvenTime");
            _postTime.Text = db.ToString();
            OneStartStopTrigger.Checked = XmlHelper.GetParamNumber(_xmlRep, "oneStartStopSignal") > 0;
            OnTriggerTypeSelectedIndexChanged(null, null);
        }

        private void OnTriggerTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TriggerType.SelectedIndex)
            {
                case 0: //None
                    OneStartStopTrigger.Enabled = false;
                    _preTime.Enabled = false;
                    _postTime.Enabled = false;
                break;
                case 1: //Start
                    OneStartStopTrigger.Enabled = false;
                    _preTime.Enabled = false;
                    _postTime.Enabled = false;
                break;
                case 2: //Stop
                    OneStartStopTrigger.Enabled = false;
                    _preTime.Enabled = false;
                    _postTime.Enabled = false;
                break;
                case 3: //Start/Stop
                    OneStartStopTrigger.Enabled = true;
                    _preTime.Enabled = false;
                    _postTime.Enabled = false;
                break;
                case 4: //Event
                    OneStartStopTrigger.Enabled = false;
                    _preTime.Enabled = true;
                    _postTime.Enabled = true;
                break;

            }
        }

        private void OnHelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Document.ShowHelp(this, 580);
        }


        private void OnHelpClick(object sender, EventArgs e)
        {
            OnHelpRequested(null, null);
        }
    }
}