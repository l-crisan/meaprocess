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
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Calculation
{
    public partial class SignalDlg : Form
    {
        private XmlElement _xmlSignalList;
        private Document _document;
        private XmlElement _xmlPS;
        private ValueHastable _generationRates = new ValueHastable();

        public SignalDlg(XmlElement xmlSignalList, Document doc, XmlElement xmlPs)
        {
            _xmlSignalList = xmlSignalList;
            _document = doc;
            _xmlPS = xmlPs;

            InitializeComponent();
            this.Icon = Document.AppIcon;

             dataTypeCtrl.Text = "LREAL";
        
            XmlElement xmlSignal = XmlHelper.GetChildByType(_xmlSignalList, "Mp.Sig");

            if (xmlSignal != null)
            {
                //Set the signal values
                SignalName.Text = XmlHelper.GetParam(xmlSignal, "name");
                Unit.Text = XmlHelper.GetParam(xmlSignal, "unit");
                Comment.Text = XmlHelper.GetParam(xmlSignal, "comment");

                double samplerate = XmlHelper.GetParamDouble(xmlSignal, "samplerate");
                sampleRate.Text = samplerate.ToString();
                double min = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                double max = XmlHelper.GetParamDouble(xmlSignal, "physMax");
                minimum.Text = min.ToString();
                maximum.Text = max.ToString();
            }
            else
            {
                sampleRate.Text = "100";
                minimum.Text = "-10";
                maximum.Text = "10";
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (SignalName.Text == "")
            {
                errorProvider.SetError(SignalName, StringResource.SigNameErr);
                return;
            }

            double min = Convert.ToDouble(minimum.Text);
            double max = Convert.ToDouble(maximum.Text);
            if (min >= max)
            {
                errorProvider.SetError(maximum, StringResource.MinMaxErr);                
                return;
            }
            _document.UpdateSource(_xmlPS);

            XmlElement xmlSignal = XmlHelper.GetChildByType(_xmlSignalList, "Mp.Sig");

            if (xmlSignal == null)
                xmlSignal = _document.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Sig");

            //Set the signal values
            XmlHelper.SetParam(xmlSignal, "name", "string", SignalName.Text);
            XmlHelper.SetParam(xmlSignal, "unit", "string", Unit.Text);
            XmlHelper.SetParam(xmlSignal, "comment", "string", Comment.Text);

            double samplerate = Convert.ToDouble(sampleRate.Text);
            XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", samplerate);

            XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)SignalDataType.LREAL);
            XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", XmlHelper.GetParamNumber(_xmlPS,"sourceId"));
            
             XmlHelper.SetParamDouble(xmlSignal, "physMin", "double",min);
             XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", max);

            _document.Modified = true;
            Close();
        }


        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sampleRate_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                double rate = Convert.ToDouble(sampleRate.Text);
                if (rate == 0)
                {
                    errorProvider.SetError(sampleRateUnit, StringResource.ZeroValueErr);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(sampleRateUnit, ex.Message);
                e.Cancel = true;
                return;
            }

        }

        private void Value_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            Control ctrl = (Control) sender;
            try
            {
                Convert.ToDouble(ctrl.Text);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(ctrl, ex.Message);
            }
        }
    }
}