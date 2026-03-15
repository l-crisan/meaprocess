using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Mp.Visual.Analog
{
    public partial class ThermometerBlock : Mp.Visual.Base.VisualBlock
    {
        private ThermometerCtrl.Unit _display;
        private int _smallTicFreq = 1;
        private int _largeTicFreq;
        private double _rangeMin;
        private double _rangeMax;

        public ThermometerBlock()
        {
            InitializeComponent();
            ThermometerCtrl ctrl = new ThermometerCtrl();
            _display = ctrl.Display;
            _smallTicFreq = ctrl.SmallTicFreq;
            _largeTicFreq = ctrl.LargeTicFreq;
            _rangeMin = ctrl.RangeMin;
            _rangeMax = ctrl.RangeMax;        
        }

        public override void UpdateProperties()
        {
            foreach (ThermometerCtrl ctrl in base.ControlsInContainer)
            {
                ctrl.Display = _display;
                ctrl.SmallTicFreq = _smallTicFreq;
                ctrl.LargeTicFreq = _largeTicFreq;
                ctrl.RangeMin = _rangeMin;
                ctrl.RangeMax = _rangeMax;        
                ctrl.Invalidate();
            }

            base.UpdateProperties();
        }
        /// <summary>
        /// Gets or sets the display mode.
        /// </summary>
        [SRDescription("Display")]
        [SRCategory("Thermometer")]
        public ThermometerCtrl.Unit Display
        {
            get { return _display; }

            set
            {
                _display = value;               
            }
        }
        /// <summary>
        /// Gets or sets the small tic frequency.
        /// </summary>
        [SRDescription("SmallTicFreq")]
        [SRCategory("Thermometer")]
        public int SmallTicFreq
        {
            get { return _smallTicFreq; }
            set
            {
                _smallTicFreq = value;             
            }
        }

        /// <summary>
        /// Gets or sets the large tic frequency.
        /// </summary>
        [SRDescription("LargeTicFreq")]
        [SRCategory("Thermometer")]
        public int LargeTicFreq
        {
            get { return _largeTicFreq; }
            set
            {
                _largeTicFreq = value;
            }
        }

        /// <summary>
        /// Gets otr sets the range minimum.
        /// </summary>
        [System.ComponentModel.Browsable(true),
        SRCategory("Thermometer"),
        SRDescription("RangeMin")]
        public double RangeMin
        {
            get
            {
                return _rangeMin;
            }
            set
            {
                _rangeMin = value;
            }
        }

        /// <summary>
        /// Gets or sets the range maximum.
        /// </summary>
        [System.ComponentModel.Browsable(true),
        SRCategory("Thermometer"),
        SRDescription("RangeMax")]
        public double RangeMax
        {
            get
            {
                return _rangeMax;
            }

            set
            {
                _rangeMax = value;
            }
        }    

    }
}
