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
    public partial class PortDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlSignalList;
        private XmlElement _xmlSignal;
        private XmlElement _xmlPS;

        public PortDlg(Document doc, XmlElement xmlSignalList, XmlElement xmlPS)
        {
            _doc = doc;
            _xmlSignalList = xmlSignalList;
            _xmlPS = xmlPS;

            InitializeComponent();
            this.Icon = Document.AppIcon;
            if (xmlSignalList.ChildNodes.Count > 0)
            {
                _xmlSignal = (XmlElement)xmlSignalList.ChildNodes[0];
                name.Text = XmlHelper.GetParam(_xmlSignal, "name");
                min.Text = XmlHelper.GetParam(_xmlSignal, "physMin");
                max.Text = XmlHelper.GetParam(_xmlSignal, "physMax");
                unit.Text = XmlHelper.GetParam(_xmlSignal, "unit");
                comment.Text = XmlHelper.GetParam(_xmlSignal, "comment");
            }
            else
            {
                min.Text = "-10";
                max.Text = "10";                
            }

            _doc.UpdateSource(_xmlPS);
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (name.Text == null || name.Text == "")
            {

                errorProvider.SetError(name, StringResource.SigNameErr);
                return;
            }

            double minValue = Convert.ToDouble(min.Text);
            double maxValue = Convert.ToDouble(max.Text);

            if (minValue >= maxValue)
            {
                errorProvider.SetError(min, StringResource.MinMaxErr);
                return;
            }


            if( _xmlSignal == null)
                _xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Sig");

            XmlHelper.SetParam(_xmlSignal, "name", "string", name.Text);
            
            XmlHelper.SetParamDouble(_xmlSignal, "physMin", "double", minValue);
            XmlHelper.SetParamDouble(_xmlSignal, "physMax", "double", maxValue);

            XmlHelper.SetParam(_xmlSignal, "unit", "string", unit.Text);
            XmlHelper.SetParam(_xmlSignal, "comment", "string", comment.Text);
            
            uint id = (uint)XmlHelper.GetParamNumber(_xmlPS, "sourceId");
            XmlHelper.SetParamNumber(_xmlSignal, "sourceNumber", "uint32_t", id);
            XmlHelper.SetParamNumber(_xmlSignal, "valueDataType", "uint8_t", (int) SignalDataType.LREAL);

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }


        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void min_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToDouble(min.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(min, ex.Message);
                e.Cancel = true;
            }
        }

        private void max_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToDouble(max.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(max, ex.Message);
                e.Cancel = true;
            }
        }
    }
}
