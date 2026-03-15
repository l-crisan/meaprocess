using System;
using System.Collections.Generic;
using System.Text;
using Mp.Scheme.Sdk;
using System.Drawing;

namespace Mp.Scheme.Win
{
    internal class SystemOutputPS : ProcessStation
    {
        public SystemOutputPS()
        {
            base.Type = "PS_OUTPUT";
            base.Text = StringResource.SystemOutput;
            base.Group = StringResource.General;
            base.Symbol = Images.SystemOutputImg;
            base.Icon = Images.SystemOutput;
            base.SubType = "Mp.Runtime.Win.SystemOutputPS";
            base.IsSingleton = true;
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.SystemOutput;
            base.Group = StringResource.General;
        }

        public override string Description
        {
            get
            {
                return StringResource.SystemOutputPsDescription;
            }
        }

        public override void OnDefaultInit()
        {

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_INPUT", true, false);
            AddPort(port);

        }

     
        public override void OnHelpRequested()
        {
            //Document.ShowHelp(this.Site, 660);
        }
    }
}
