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
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Clock
{
    public partial class StopWatchOutPortDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlSignalList;

        public StopWatchOutPortDlg(Document doc, XmlElement xmlSignalList)
        {
            _doc = doc;
            _xmlSignalList = xmlSignalList;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            unit.Items.Clear();
            unit.Items.Add(StringResource.Millisecond);
            unit.Items.Add(StringResource.Second);
            unit.Items.Add(StringResource.Minute);
            unit.Items.Add(StringResource.Hour);

            if (_xmlSignalList.ChildNodes.Count != 0)
            {
                XmlElement xmlSignal = (XmlElement)_xmlSignalList.ChildNodes[0];
                signalName.Text = XmlHelper.GetParam(xmlSignal, "name");

                unit.SelectedIndex = (int)XmlHelper.GetParamNumber(xmlSignal, "unitIdx");
                double rate = XmlHelper.GetParamDouble(xmlSignal, "samplerate");
                samplerate.SelectedIndex = GetRate(rate);
                comment.Text = XmlHelper.GetParam(xmlSignal, "comment");
            }
            else
            {
                unit.SelectedIndex = 0;
                samplerate.SelectedIndex = 0;
            }
        }

        private int GetRate(double r)
        {
            switch ((int)r)
            {
                case 1:
                    return 0;
                case 2:
                    return 1;
                case 5:
                    return 2;
                case 10:
                    return 3;
                case 20:
                    return 4;
                case 50:
                    return 5;
                case 100:
                    return 6;
            }
            return 0;
        }

        private double GetRate(int r)
        {
            switch ((int)r)
            {
                case 0:
                    return 1.0;
                case 1:
                    return 2.0;
                case 2:
                    return 5.0;
                case 3:
                    return 10.0;
                case 4:
                    return 20.0;
                case 5:
                    return 50.0;
                case 6:
                    return 100.0;
            }

            return 1;
        }

        private void OK_Click(object sender, EventArgs e)
        {           
            errorProvider.Clear();

            if( signalName.Text == null || signalName.Text =="")
            {
                errorProvider.SetError(signalName,"A name for the signal is expected.");
                return;
            }

            XmlElement xmlSignal = null;

            if (_xmlSignalList.ChildNodes.Count == 0)
                xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Clock.Sig.StopWatch");
            else
                xmlSignal = (XmlElement)_xmlSignalList.ChildNodes[0];

            XmlHelper.SetParam(xmlSignal, "name", "string", signalName.Text);
            XmlHelper.SetParamNumber(xmlSignal, "unitIdx", "uint8_t", unit.SelectedIndex);
            
            string unitstr = GetUnit(unit.SelectedIndex);

            XmlHelper.SetParam(xmlSignal, "unit", "string", unitstr);
            XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", GetRate(samplerate.SelectedIndex));
            XmlHelper.SetParam(xmlSignal, "comment","string",comment.Text);
            XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)SignalDataType.LREAL);
            XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", 0);
            XmlHelper.SetParamNumber(xmlSignal, "physMin", "double", 0);
            XmlHelper.SetParamNumber(xmlSignal, "physMax", "double", 1000000);
            _doc.Modified = true;
            Close();
        }

        private string GetUnit(int u)
        {
            switch (u)
            {
                case 0:
                    return "ms";
                case 1:
                    return "s";
                case 2:
                    return "min";
                case 3:
                    return "h";
            }

            return "ms";
        }
    }
}
