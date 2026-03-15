using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NPlot;

namespace WindowsFormsApplication1
{
    public partial class NPlotChart : Form
    {
        public NPlotChart()
        {
            InitializeComponent();
            PlotWave();
            plotSurface.ShowCoordinates = true;
        }

    
        public void PlotWave()
        {
            string[] lines = {
                "Sound Wave Example. Demonstrates - ",
                "  * StepPlot (centered) and HorizontalLine IDrawables",
                "  * How to set colors of various things.",
                "  * A few plot interactions. Try left clicking and dragging (a) the axes (b) in the plot region.",
                "  * In the future I plan add a plot interaction for axis drag that knows if the ctr key is down. This will select between drag/scale" };

            FileStream fs = new FileStream(@"c:\light.wav", System.IO.FileMode.Open);


            System.Int16[] w = new short[5000];
            byte[] a = new byte[10000];
            fs.Read(a, 0, 10000);
            for (int i = 100; i < 5000; ++i)
            {
                w[i] = BitConverter.ToInt16(a, i * 2);
            }

            fs.Close();

            plotSurface.Clear();
            plotSurface.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.VerticalGuideline());
            plotSurface.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.HorizontalGuideline());
//            plotSurface.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.HorizontalRangeSelection(3));
//            plotSurface.AddInteraction(new NPlot.Windows.PlotSurface2D.Interactions.AxisDrag(true));

//            plotSurface.Add(new HorizontalLine(0.0, Color.LightBlue));

            LinePlot sp = new LinePlot();
            sp.DataSource = w;
            sp.Color = Color.Yellow;
            plotSurface.Add(sp);

            fs = new FileStream(@"c:\light.wav", System.IO.FileMode.Open);


            w = new short[5000];
            a = new byte[10000];
            fs.Read(a, 0, 10000);
            short st = -100;

            for (int i = 100; i < 5000; i++)
            {
                w[i] = BitConverter.ToInt16(a, i * 2);
                w[i] += st;
            }

            fs.Close();

            sp = new LinePlot();
            sp.DataSource = w;
            sp.Color = Color.Green;
            plotSurface.Add(sp);


            fs = new FileStream(@"c:\light.wav", System.IO.FileMode.Open);


            w = new short[5000];
            a = new byte[10000];
            fs.Read(a, 0, 10000);
            st = 100;

            for (int i = 100; i < 5000; i++)
            {
                w[i] = BitConverter.ToInt16(a, i * 2);
                w[i] += st;
            }

            fs.Close();

            sp = new LinePlot();
            sp.DataSource = w;
            sp.Color = Color.Red;
            plotSurface.Add(sp);
//            plotSurface.YAxis1.FlipTicksLabel = true;

/*            plotSurface.PlotBackColor = Color.DarkBlue;
            plotSurface.BackColor = Color.Black;
            plotSurface.XAxis1.Color = Color.White;
            plotSurface.YAxis1.Color = Color.White;
            */
            plotSurface.Refresh();

        }


    }
}
