using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections;

using Mp.NumericView;
using Mp.Scheme.Sdk;
using Mp.Components;

namespace Mp.Scheme.Win
{
    internal class NumericViewPS : VisualPS
    {
        private Assembly _myControlsAssembly;

        public NumericViewPS()
        {
            base.Type = "PS_OUTPUT";
            base.Text = StringResource.TableView;
            base.Group = StringResource.Display;
            base.Symbol = Mp.Scheme.Win.Images.TableViewImage;
            base.Icon = Mp.Scheme.Win.Images.TableView;
            base.SubType = "Mp.Runtime.Sdk.NumericViewPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.NumericView.dll");
        }

        public override string Description
        {
            get
            {
                return StringResource.TableViewPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            CreateControl();

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_INPUT", true, false);
            AddPort(port);

        }

        private void CreateControl()
        {
            //Control
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter = new List<string>();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Text");
            ctrlData.PropertyFilter.Add("SignalWidth");
            ctrlData.PropertyFilter.Add("ValueWidth");
            ctrlData.PropertyFilter.Add("UnitWidth");
            ctrlData.PropertyFilter.Add("Precision");
            ctrlData.PropertyFilter.Add("HideSignalName");
            ctrlData.PropertyFilter.Add("HideSignalUnit");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Dock");


            NumericView.NumericView numericViewCtrl = new NumericView.NumericView();
            numericViewCtrl.Tag = ctrlData;
            numericViewCtrl.Text = this.Text;
            numericViewCtrl.Name = this.Text;
            
            RegisterControl(numericViewCtrl);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
            Port dataInPort = InputPorts[0];

            if (base.Controls.Count == 0)
            {
                CreateControl();
                valInfoList.Add(new ValidationInfo("A new Table View was created, because a new version is available.", ValidationInfo.InfoType.Warning));
            }
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            base.OnProperties(sender, e);
            NumericView.NumericView nc = (NumericView.NumericView)Controls[0];
            nc.Text = this.Text;
        }
    }
}
