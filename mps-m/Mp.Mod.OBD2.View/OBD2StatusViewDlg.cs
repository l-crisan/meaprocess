using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.OBD2.View
{
    public partial class OBD2StatusViewDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlSinalList;
        private XmlElement _xmlPS;

        public OBD2StatusViewDlg(Document doc, XmlElement xmlPS, XmlElement xmlSignalList)
        {
            _doc = doc;
            _xmlSinalList = xmlSignalList;
            _xmlPS = xmlPS;
            InitializeComponent();

            DataGridViewComboBoxColumn type = (DataGridViewComboBoxColumn)signalMap.Columns[1];

            type.Items.Clear();
            type.Items.Add(StringResource.Unknown);
            type.Items.Add(StringResource.FuelSystemStatus);
            type.Items.Add(StringResource.CmdSecondaryAirStatus);
            type.Items.Add(StringResource.LocOxygen13);
            type.Items.Add(StringResource.LocOxygen1D);
            type.Items.Add(StringResource.AuxInStatus);
            type.Items.Add(StringResource.MonStatusDrivingCyc);
            type.Items.Add(StringResource.FuelType);

            psName.Text = XmlHelper.GetParam(_xmlPS, "name");
            LoadSignals();
            LoadMapping();
        }

        private void LoadSignals()
        {
            if (_xmlSinalList == null)
                return;

            foreach (XmlElement xmlElement in _xmlSinalList.ChildNodes)
            {
                XmlElement xmlSignal = xmlElement;
                
                if (XmlHelper.GetObjectID(xmlElement) == 0)
                    xmlSignal = _doc.GetXmlObjectById(Convert.ToUInt32(xmlElement.InnerText));

                int index = signalMap.Rows.Add();
                DataGridViewRow row = signalMap.Rows[index];
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[1].Value = StringResource.Unknown;
                row.Tag = xmlSignal;
            }
        }

        private void LoadMapping()
        {
            foreach (XmlElement xmlElement in _xmlPS.ChildNodes)
            {
                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");
                
                if (name != "signalTypeMap")
                    continue;

                string[] array = xmlElement.InnerText.Split(new char[]{'/'});
                DataGridViewRow row = GetRowBySigId(Convert.ToUInt32(array[0]));
                
                if (row == null)
                    continue;

                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)row.Cells[1];
                int type = Convert.ToInt32(array[1]);
                row.Cells[1].Value = cell.Items[type];
            }
        }

        private DataGridViewRow GetRowBySigId(uint id)
        {
            foreach (DataGridViewRow row in signalMap.Rows)
            {
                XmlElement xmlSignal = (XmlElement) row.Tag;
                
                if (xmlSignal == null)
                    continue;

                if (XmlHelper.GetObjectID(xmlSignal) == id)
                    return row;
            }

            return null;
        }

        private int GetStatusType(string type, DataGridViewComboBoxCell cell)
        {
            int i = 0;

            foreach (string t in cell.Items)
            {
                if (t == type)
                    return i;
                i++;
            }

            return 0;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            XmlHelper.SetParam(_xmlPS, "name", "string", psName.Text);
            DialogResult = DialogResult.OK;

            //Remove the old mapping
            for (int i = 0; i < _xmlPS.ChildNodes.Count; ++i)
            {
                XmlElement xmlElement = (XmlElement) _xmlPS.ChildNodes[i];

                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");

                if (name != "signalTypeMap")
                    continue;

                _xmlPS.RemoveChild(xmlElement);
                --i;
            }

            //Create the new mapping

            foreach (DataGridViewRow row in signalMap.Rows)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)row.Cells[1];
                int type = GetStatusType((string)row.Cells[1].Value, cell);
                
                if (type == 0)
                    continue;
                
                XmlElement xmlSignal = (XmlElement)row.Tag;
                string mapping = XmlHelper.GetObjectID(xmlSignal).ToString() + "/" + type.ToString();
                XmlHelper.CreateElement(_xmlPS, "string", "signalTypeMap", mapping);
            }
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1010);
        }

        private void OBD2StatusViewDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
