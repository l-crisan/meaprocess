using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;
using Mp.Scheme.Sdk;
using Atesion.Utils;

namespace Mp.Analysis
{
    internal class PIDControlPS : WorkPS
    {
        public PIDControlPS()
        {
            base.Type = "PS_PID";
            base.Text = "PID Control";
            base.Group = "Analysis/Control";
            base.Symbol = Mp.Analysis.Resource.PID;
            base.Icon = Mp.Analysis.Resource.PIDIcon;
            base.IsSingleton = false;
        }

        public override void OnLoadResources()
        {
            base.Text = "PID";
            base.Group = StringResource.Analysis;
        }

        public override string RuntimeModule
        {
            get { return "mps-analysis"; }
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data out port.
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_OUTPUT", false, true);
            port.SignalList = Document.CreateSignalList();
            AddPort(port);

            //Data port in port. 
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_INPUT", true, false);
            AddPort(port);

            //Data port in port. 
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + 2*PortTopOffset)), "PORT_INPUT", true, false);
            AddPort(port);

        }

        protected override void OnDocumentChanged()
        {
            Port inPort = InputPorts[0];

            if (inPort.SignalList == null)
                return;

            if (inPort.SignalList.ChildNodes.Count == 0)
                return;

            XmlElement xmlInSignal = (XmlElement)inPort.SignalList.ChildNodes[0];

            uint id = XmlHelper.GetObjectID(xmlInSignal);

            if (id == 0)
                xmlInSignal = Document.GetXmlObjectById(Convert.ToUInt32(xmlInSignal.InnerText));

            double rate = XmlHelper.GetParamDouble(xmlInSignal, "samplerate");

            XmlElement xmlSignalList = OutputPorts[0].SignalList;

            foreach (XmlElement xmlSignal in xmlSignalList.ChildNodes)
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", rate);
        }

        public override void OnPortDoubleClick(Port port)
        {
            base.OnPortDoubleClick(port);

            OnPropertyDataPort(null, null);
        }

        protected void OnPropertyDataPort(object sender, EventArgs e)
        {
            Port port = OutputPorts[0];
        }


        public override void OnHelpRequested()
        {
            Assembly asm = Assembly.GetAssembly(this.GetType());
            string path = Path.GetDirectoryName(asm.Location);
            //            string file = Path.Combine(path, "MpAudio.chm");
            //            Help.ShowHelp(this.Site, file, HelpNavigator.TopicId, "30");    
        }
        protected override void OnProperties(object sender, EventArgs e)
        {
        }
    }
}
