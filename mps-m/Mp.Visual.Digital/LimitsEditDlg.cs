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

namespace Mp.Visual.Digital
{
    public partial class LimitsEditDlg : Form
    {
        private ControlLimits _limits;

        public LimitsEditDlg(ControlLimits limits)
        {
            _limits = limits;
            InitializeComponent();

            useWarningLimits.Checked = limits.UseWarningLimit;
            warningColor.BackColor = limits.WarningColor;
            warningLower.Text = limits.WarningLower.ToString();
            warningUpper.Text = limits.WarningUpper.ToString();

            useAlarmLimits.Checked = limits.UseAlarmLimit;
            alarmColor.BackColor = limits.AlarmColor;
            alarmLower.Text = limits.AlarmLower.ToString();
            alarmUpper.Text = limits.AlarmUpper.ToString();            
        }

        private void useWarningLimits_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxWarning.Enabled = useWarningLimits.Checked;
        }

        private void useAlarmLimits_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxAlarm.Enabled = useAlarmLimits.Checked;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            double warLower = Convert.ToDouble(warningLower.Text);
            double warUpper = Convert.ToDouble(warningUpper.Text);
            errorProvider.Clear();

            if (warLower >= warUpper)
            {
                errorProvider.SetError(warningUpper, StringResource.UpperLowerErr);
                return;
            }

            double alLower = Convert.ToDouble(alarmLower.Text);
            double alUpper = Convert.ToDouble(alarmUpper.Text);

            if (alLower >= alUpper)
            {
                errorProvider.SetError(alarmUpper, StringResource.UpperLowerErr);
                return;
            }


            _limits.UseWarningLimit = useWarningLimits.Checked;
            _limits.WarningColor = warningColor.BackColor;
            _limits.WarningLower = warLower;
            _limits.WarningUpper = warUpper;

            _limits.UseAlarmLimit = useAlarmLimits.Checked;
            _limits.AlarmColor = alarmColor.BackColor;
            _limits.AlarmLower = alLower;
            _limits.AlarmUpper = alUpper;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void onWarningColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = warningColor.BackColor;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                warningColor.BackColor = dlg.Color;
            }
        }

        private void onAlarmColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = alarmColor.BackColor;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                alarmColor.BackColor = dlg.Color;
            }
        }

        private void Value_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            Control ctrl = (Control)sender;

            try
            {
                Convert.ToDouble(ctrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ctrl, ex.Message);
                e.Cancel = true;
            }
        }
    }
}
