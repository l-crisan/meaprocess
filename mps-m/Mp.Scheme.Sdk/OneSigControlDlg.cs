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

namespace Mp.Scheme.Sdk
{
    public partial class OneSigControlDlg : Form
    {
        private XmlElement      _xmlSignalList;
        private Document        _document;
        private ValueHastable   _generationRates = new ValueHastable();
        private SignalDataType  _dataType = SignalDataType.DINT;

        public OneSigControlDlg(XmlElement xmlSignalList, Document doc, SignalDataType dataType)
        {
            _dataType = dataType;
            _xmlSignalList = xmlSignalList;
            _document = doc;
            InitializeComponent();
            Icon = Document.AppIcon; 
        }
      

        private void OnOKClick(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (SignalName.Text == "")
            {
                errorProvider.SetError(SignalName, StringResource.SignalNameErr);
                return;
            }

            double minValue = Convert.ToDouble(min.Text);
            double maxValue = Convert.ToDouble(max.Text);
            if (minValue >= maxValue)
            {
                errorProvider.SetError(max, StringResource.SigMinMaxErr);
                return;
            }

            XmlElement xmlSignal = XmlHelper.GetChildByType(_xmlSignalList, "Mp.Sig");

            if (xmlSignal == null)
                xmlSignal = _document.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Sig");

            //Set the signal values
            XmlHelper.SetParam(xmlSignal, "name", "string", SignalName.Text);
            XmlHelper.SetParam(xmlSignal, "unit", "string", Unit.Text);
            XmlHelper.SetParam(xmlSignal, "comment", "string", Comment.Text);

            double samplerate = (double)_generationRates.GetKeyByValue(Samplerate.SelectedIndex);
            XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", samplerate);

            XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)_dataType);
            XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", 0);

            XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Convert.ToDouble(min.Text));
            XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble(max.Text));

            _document.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private void OnValueValidating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            Control ctrl = (Control)sender;
            try
            {
                if (_dataType == SignalDataType.LREAL)
                {
                    Convert.ToDouble(ctrl.Text);
                }
                else
                {
                    Convert.ToInt32(ctrl.Text);
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ctrl, ex.Message);
                e.Cancel = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Samplerate.SelectedIndex = 0;

            _generationRates.Add(1.0, 0);
            _generationRates.Add(2.0, 1);
            _generationRates.Add(5.0, 2);
            _generationRates.Add(10.0, 3);
            _generationRates.Add(20.0, 4);
            _generationRates.Add(50.0, 5);
            _generationRates.Add(100.0, 6);

            dataTypeCtrl.Text = _dataType.ToString();

            min.Enabled = false;
            max.Enabled = false;
            min.Text = "0";
            max.Text = "1";

            XmlElement xmlSignal = XmlHelper.GetChildByType(_xmlSignalList, "Mp.Sig");

            if (xmlSignal != null)
            {
                SignalName.Text = XmlHelper.GetParam(xmlSignal, "name");
                Unit.Text = XmlHelper.GetParam(xmlSignal, "unit");
                Comment.Text = XmlHelper.GetParam(xmlSignal, "comment");
                double samplerate = XmlHelper.GetParamDouble(xmlSignal, "samplerate");
                Samplerate.SelectedIndex = (int)_generationRates[samplerate];

                min.Text = XmlHelper.GetParamDouble(xmlSignal, "physMin").ToString();
                max.Text = XmlHelper.GetParamDouble(xmlSignal, "physMax").ToString();
            }

            if (_dataType != SignalDataType.BOOL)
            {
                min.Enabled = true;
                max.Enabled = true;
            }
        }
    }
}