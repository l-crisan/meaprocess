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
    public partial class SimpleTriggerPortDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlRep;

        public SimpleTriggerPortDlg(Document doc, XmlElement xmlRep)
        {
            _doc = doc;
            _xmlRep = xmlRep;

            InitializeComponent();

            this.Icon = Document.AppIcon;
            triggerType.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlRep, "triggerType");
            oneStartStopTrigger.Checked = XmlHelper.GetParamNumber(_xmlRep, "oneStartStopSignal") > 0;
        }


        private void OnTriggerSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (triggerType.SelectedIndex)
            {
                case 0: //None
                case 1:
                case 2:
                    oneStartStopTrigger.Enabled = false;
                    break;

                case 3: //Start/Stop
                    oneStartStopTrigger.Enabled = true;
                    break;
            }
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            XmlHelper.SetParamNumber(_xmlRep, "triggerType", "uint8_t", triggerType.SelectedIndex);

            if (oneStartStopTrigger.Checked)
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
    }
}
