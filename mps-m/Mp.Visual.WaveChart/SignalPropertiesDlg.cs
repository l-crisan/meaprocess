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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Mp.Visual.WaveChart
{
    /// <summary>
    /// Open a signal propertie dialog
    /// </summary>
    public partial class SignalPropertiesDlg : Form
    {
        public SignalPropertiesDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets or gest the signal list.
        /// </summary>
        public List<Signal> Signals
        {
            get { return _signals;  }
            set { _signals = value; }
        }
             
        /// <summary>
        /// Sets or gets the wave chart control.
        /// </summary>
        public WaveChartCtrl WaveChart
        {
            get { return _waveChart; }
            set { _waveChart = value; }
        }
        
        /// <summary>
        /// Sets or gets the wave chart legend. 
        /// </summary>
        public SigLegend Legend
        {
            get { return _legendCtrl; }
            set { _legendCtrl = value; }
        }

        private List<Signal>   _signals;
        private WaveChartCtrl   _waveChart;
        private SigLegend       _legendCtrl;
        
        protected override void OnLoad(EventArgs e)
        {
 	        base.OnLoad(e);
            SigLegend legend = new SigLegend();            
            legend.WaveChart = _waveChart;
            legend.Signals = _signals;
            legend.UpdateLegend();
            this.Controls.Add(legend);
            legend.Dock = DockStyle.Fill;
        }

        private void SignalPropertiesDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            _legendCtrl.UpdateLegend();
        }        
    }
}