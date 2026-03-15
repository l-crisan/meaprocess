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
using System.Globalization;
using Mp.Scheme.Sdk;


namespace Mp.Mod.Statistics
{
    public partial class ClassingDlg : Form
    {
        private Classing _cls;
        private Document _doc;
        private NumberFormatInfo _ninfo = new NumberFormatInfo();

        public ClassingDlg(Classing cls, Document doc)
        {
            _doc = doc;
            _cls = cls;
            _ninfo.NumberDecimalSeparator = ".";

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            type.Items.Clear();
            type.Items.Add(StringResource.Sampling);
            type.Items.Add(StringResource.ZeroCrossingPeak);
            type.Items.Add(StringResource.PeakCounting1);
            type.Items.Add(StringResource.PeakCounting2);
            type.Items.Add(StringResource.LevelCrossingCounting);
            

            type.SelectedIndex = cls.ClassingType;
            classes.Text = cls.NoOfClasses.ToString();
            hysterese.Text = cls.Hysteresis.ToString();
            refValue.Text = cls.ReferenceValue.ToString();
            upperLimit.Text = cls.UpperLimit.ToString();
            lowerLimit.Text = cls.LowerLimit.ToString();
            resetNValues.Text = cls.ResetNValues.ToString();

            name.Text = cls.SigName;
            unit.Text = cls.SigUnit;
            comment.Text = cls.SigComment;
            SetSampleRate(cls.SigRate);
        }

        private void type_SelectedIndexChanged(object sender, EventArgs e)
        {
            refValue.Enabled = true;
            hysterese.Enabled = true;

            switch (type.SelectedIndex)
            {
                case 0:
                    refValue.Enabled = false;
                break;
                case 3:
                    refValue.Enabled = false;
                break;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            double lower = Convert.ToDouble(lowerLimit.Text);
            double upper = Convert.ToDouble(upperLimit.Text);
            if (lower >= upper)
            {
                errorProvider.SetError(upperLimit, StringResource.LowerUpperError);
                return;
            }
            
            _cls.ClassingType = type.SelectedIndex;
            _cls.NoOfClasses = Convert.ToInt32(classes.Text);
            _cls.Hysteresis = Convert.ToDouble(hysterese.Text);
            _cls.ReferenceValue = Convert.ToDouble(refValue.Text);
            _cls.ResetNValues = Convert.ToUInt32(resetNValues.Text);
            _cls.UpperLimit = upper;
            _cls.LowerLimit = lower;
            _cls.SigName = name.Text;
            _cls.SigRate = GetSampleRate();
            _cls.SigUnit = unit.Text;
            _cls.SigComment = comment.Text;

            DialogResult = DialogResult.OK;
            Close();
        }


        private double GetSampleRate()
        {
            switch (sampleRate.SelectedIndex)
            {
                case 0:
                    return 5.0;
                case 1:
                    return 10.0;
                case 2:
                    return 10.0;
                case 3:
                    return 50.0;
                case 4:
                    return 100.0;
                case 5:
                    return 200.0;
                case 6:
                    return 500.0;
                case 7:
                    return 1000.0;
            }

            return 5.0;
        }

        private void SetSampleRate(double rate)
        {
            sampleRate.SelectedIndex = 0;

            if (rate == 5.0)
                sampleRate.SelectedIndex = 0;
            else if( rate == 10.0)
                sampleRate.SelectedIndex = 1;
            else if( rate == 20.0)
                sampleRate.SelectedIndex = 2;
            else if (rate == 50.0)
                sampleRate.SelectedIndex = 3;
            else if (rate == 100.0)
                sampleRate.SelectedIndex = 4;
            else if (rate == 200.0)
                sampleRate.SelectedIndex = 5;
            else if (rate == 500.0)
                sampleRate.SelectedIndex = 6;
            else if (rate == 1000.0)
                sampleRate.SelectedIndex = 7;

        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
   
        private void classes_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                uint cs = Convert.ToUInt32(classes.Text);
                if (cs == 0)
                {
                    errorProvider.SetError(classes, StringResource.NoOfClassesError);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(classes,ex.Message);
                e.Cancel = true;
            }
        }

        private void hysterese_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                double hyst = Convert.ToDouble(hysterese.Text);
                if (hyst < 0 || hyst > 100)
                {
                    errorProvider.SetError(classes, StringResource.HystError);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(hysterese, ex.Message);
                e.Cancel = true;
            }
        }

        private void refValue_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToDouble(refValue.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(refValue, ex.Message);
                e.Cancel = true;
            }
        }

        private void help_Click(object sender, EventArgs e)
        {

        }

        private void ClassingDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {

        }

        private void resetNValues_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt32(resetNValues.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(resetNValues, ex.Message);
                e.Cancel = true;
            }
        }
    }
}
