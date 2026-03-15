using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using Mp.Components;

namespace Mp.Schema.Sdk
{
    /// <summary>
    /// The data storage trigger port property dialog.
    /// </summary>
    public partial class TriggerPortDlg : Form
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TriggerPortDlg()
        { InitializeComponent(); }

        /// <summary>
        /// Gets or sets the trigger port xml representation.
        /// </summary>
        public XmlElement XmlRep;

        private void OK_Click(object sender, EventArgs e)
        {
            PoXmlHelper.SetParamNumber(XmlRep, "triggerType", "uint8_t", TriggerType.SelectedIndex);

            string strSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string str = PreTime.Text;
            str = str.Replace(".", strSep);
            double value = Double.Parse(str);


            PoXmlHelper.SetParamDouble(XmlRep, "preEvenTime", "double", value);
            str = PostTime.Text;
            str = str.Replace(".", strSep);
            value = Double.Parse(str);
            PoXmlHelper.SetParamDouble(XmlRep, "postEvenTime", "double", value);

            if (OneStartStopTrigger.Checked)
                PoXmlHelper.SetParamNumber(XmlRep, "oneStartStopSignal", "bool", 1);
            else
                PoXmlHelper.SetParamNumber(XmlRep, "oneStartStopSignal", "bool", 0);

            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PoTriggerPortDlg_Load(object sender, EventArgs e)
        {
            TriggerType.SelectedIndex = (int) PoXmlHelper.GetParamNumber(XmlRep, "triggerType");
            string strSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            
            PreTime.Text = PoXmlHelper.GetParamDouble(XmlRep, "preEvenTime").ToString();
            PostTime.Text = PoXmlHelper.GetParamDouble(XmlRep, "postEvenTime").ToString();
            OneStartStopTrigger.Checked = PoXmlHelper.GetParamNumber(XmlRep, "oneStartStopSignal") > 0;
            TriggerType_SelectedIndexChanged(null, null);
        }

        private void TriggerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (TriggerType.SelectedIndex)
            {
                case 0: //None
                    OneStartStopTrigger.Enabled = false;
                    PreTime.Enabled = false;
                    PostTime.Enabled = false;
                break;
                case 1: //Start
                    OneStartStopTrigger.Enabled = false;
                    PreTime.Enabled = false;
                    PostTime.Enabled = false;
                break;
                case 2: //Stop
                    OneStartStopTrigger.Enabled = false;
                    PreTime.Enabled = false;
                    PostTime.Enabled = false;
                break;
                case 3: //Start/Stop
                    OneStartStopTrigger.Enabled = true;
                    PreTime.Enabled = false;
                    PostTime.Enabled = false;
                break;
                case 4: //Event
                    OneStartStopTrigger.Enabled = false;
                    PreTime.Enabled = true;
                    PostTime.Enabled = true;
                break;

            }
        }
    }
}
