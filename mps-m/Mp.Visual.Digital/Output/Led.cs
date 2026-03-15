using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Mp.Visual.Digital
{
    public partial class Led : UserControl
    {
        private Bitmap _backBuffer;

        public Led()
        {
            InitializeComponent();
        }

        #region new properties
        private bool _active = true;

        [Category("Behavior"),
        DefaultValue(true)]
        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    Invalidate();
                }
            }
        }

        private Color _ColorOn = Color.Red;
        [Category("Appearance")]
        public Color ColorOn
        {
            get { return _ColorOn; }
            set
            {
                _ColorOn = value;
                Invalidate();
            }
        }

        private Color _ColorOff = SystemColors.Control;
        [Category("Appearance")]
        public Color ColorOff
        {
            get { return _ColorOff; }
            set
            {
                _ColorOff = value;
                Invalidate();
            }
        }

        #endregion


        public static Color FadeColor(Color c1, Color c2, int i1, int i2)
        {
            int r = (i1 * c1.R + i2 * c2.R) / (i1 + i2);
            int g = (i1 * c1.G + i2 * c2.G) / (i1 + i2);
            int b = (i1 * c1.B + i2 * c2.B) / (i1 + i2);

            return Color.FromArgb(r, g, b);
        }

        public static Color FadeColor(Color c1, Color c2)
        {
            return FadeColor(c1, c2, 1, 1);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            int width = Math.Min(this.Width, this.Height);
            width = Math.Max(width, 2);

            _backBuffer = new Bitmap(width, width);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);
                Graphics g = Graphics.FromImage(_backBuffer);

                int radius = _backBuffer.Width - 1;

                if (Enabled)
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;

                    if (Active)
                    {
                        Rectangle r = new Rectangle(0, 0, radius, radius);
                        LinearGradientBrush br1 = new LinearGradientBrush(r, ColorOn, Color.WhiteSmoke, 45);
                        g.FillEllipse(br1, 0, 0, radius, radius);
                        g.DrawEllipse(new Pen(FadeColor(ColorOn, Color.White), 1), 0, 0, radius, radius);
                        g.DrawEllipse(new Pen(Color.Black), 0, 0, radius, radius);
                    }
                    else
                    {
                        Rectangle r = new Rectangle(0, 0, radius, radius);
                        LinearGradientBrush br1 = new LinearGradientBrush(r, ColorOff, Color.Black, 45);
                        g.FillEllipse(br1, 0, 0, radius, radius);
                        g.DrawEllipse(new Pen(FadeColor(ColorOff, Color.Black), 1), 0, 0, radius, radius);
                        g.DrawEllipse(new Pen(Color.Black), 0, 0, radius, radius);
                    }
                }
                else
                {
                    g.DrawEllipse(new Pen(SystemColors.ControlDark, 1), 0, 0, radius, radius);
                }

                e.Graphics.DrawImage(_backBuffer, new Point(0, 0));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
