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
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Calculation
{
    public partial class BitSelectDialog : Form
    {
        private List<BitExtractionDlg.OutputItem> _items;
        private XmlElement _xmlInSignal;
        public BitSelectDialog(XmlElement xmlInSignal, List<BitExtractionDlg.OutputItem> items)
        {
            _items = items;
            _xmlInSignal = xmlInSignal;
            InitializeComponent();
            this.Icon = Document.AppIcon;
            SignalDataType dataType = (SignalDataType)XmlHelper.GetParamNumber(xmlInSignal, "valueDataType");
            int bits = 0;

            switch (dataType)
            {
                case SignalDataType.BOOL:
                    bits = 1;
                break;

                case SignalDataType.USINT:
                case SignalDataType.SINT:
                    bits = 8;
                break;

                case SignalDataType.INT:
                case SignalDataType.UINT:
                    bits = 16;
                break;

                case SignalDataType.DINT:
                case SignalDataType.UDINT:
                case SignalDataType.REAL:
                    bits = 32;
                break;
                case SignalDataType.LINT:
                case SignalDataType.ULINT:
                case SignalDataType.LREAL:
                    bits = 64;
                break;
            }

            for (int i = 0; i < bits; ++i)
            {
                int index = outputGrid.Rows.Add();
                DataGridViewRow row  = outputGrid.Rows[index];
                row.Cells[0].Value = i + 1;
                row.Cells[1].Value = "";
                row.Cells[2].Value = "";
                row.Cells[3].Value = "";
            }

            foreach (BitExtractionDlg.OutputItem item in items)
            {
                if (item.BitNumber >= bits)
                    continue;

                DataGridViewRow row = outputGrid.Rows[item.BitNumber];
                row.Tag = item.xmlOutSignal;
                row.Cells[1].Value = item.Name;
                row.Cells[2].Value = item.Unit;
                row.Cells[3].Value = item.Comment;
            }

            FormStateHandler.Restore(this, "Mp.Calculation.BitSelectDialog");
        }

        private void OK_Click(object sender, EventArgs e)
        {
            _items.Clear();
            foreach (DataGridViewRow row in outputGrid.Rows)
            {
                if (row.Cells[1].Value == null)
                    continue;

                string name = row.Cells[1].Value.ToString();
                if (name == "")
                    continue;

                BitExtractionDlg.OutputItem item = new BitExtractionDlg.OutputItem();
                item.xmlOutSignal = (XmlElement) row.Tag;
                item.Name = name;
                item.Unit = row.Cells[2].Value.ToString();
                item.Comment = row.Cells[3].Value.ToString();
                item.BitNumber = row.Index;
                _items.Add(item);
            }
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BitSelectDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, "Mp.Calculation.BitSelectDialog");
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1470);
        }
    }
}
