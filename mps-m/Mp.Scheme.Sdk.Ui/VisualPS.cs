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
using System.Text;
using System.Collections;
using System.Xml;
using System.Windows.Forms;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    /// <summary>
    /// The base class for each visual process station.
    /// </summary>
    public class VisualPS : WorkPS
    {
        private List<Control> _savedControls = new List<Control>();
        /// <summary>
        /// Default constructor.
        /// </summary>
        public VisualPS()
        {
            base.Group = "Visual";
            base.AcceptObjectSignal = false;
        }

        static internal PanelDesignerTool DesignerTool
        {
            set 
            {   _GUIDesignerTool = value; }
        }

        protected static void UpdateProperty(ControlData ctrlData, string prop)
        {
            bool found = false;
            foreach (string ft in ctrlData.PropertyFilter)
            {
                if (ft == prop)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                ctrlData.PropertyFilter.Add(prop);
        }

        protected virtual void OnUpdateSignalList()
        {

        }

        protected override void OnDocumentChanged()
        {
            if (_GUIDesignerTool != null)
                OnUpdateSignalList();
        }


        public override string CopyToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<VisualStation>");

            string psData =  base.CopyToXml();
            sb.Append(psData);

            sb.Append("<Visual>");

            foreach (Control ctrl in Controls)
            {
                sb.Append("<Control type=\"" + ctrl.GetType().FullName + "\">");
                ControlData ctrlData = (ControlData)ctrl.Tag;
                string data = ControlSurrogate.SerializeToString(ctrl, ctrlData.PropertyFilter);
                sb.Append(data);
                sb.Append("</Control>");
            }

            sb.Append("</Visual>");
            sb.Append("</VisualStation>");
            return sb.ToString();
        }

        public override void LoadVisualData(XmlElement xmlVisualData)
        {
            Hashtable typeMappingTable = new Hashtable();
            List<Control> controls = new List<Control>();

            foreach (XmlElement xmlControl in xmlVisualData.ChildNodes)
            {
                string typeName = xmlControl.GetAttribute("type");

                Type ctrlType = ControlSurrogate.GetLoadedTypeByName(typeName);

                Control control = (Control)ControlSurrogate.DeserializeFromString(xmlControl.InnerText, ctrlType, typeMappingTable);

                if (control == null)
                    continue;

                controls.Add(control);
            }
            
            if(controls.Count != 0)
                RegisterControls(controls);
        }

        /// <summary>
        /// Called by the framework when this process station is removed. (override)
        /// </summary>
        public override void OnRemove()
        {
            base.OnRemove();

            if (_GUIDesignerTool == null)
                return;

            _savedControls.Clear();
            foreach (Control ctrl in Controls)
                _savedControls.Add(ctrl);

           _GUIDesignerTool.RemoveProcessStation(this);
        }
        
        protected override void OnTextChanged(string text)
        {
            foreach(Control ctrl in Controls)
                ctrl.Name = text;
        }

        public override void OnRestore()
        {
            foreach (Control ctrl in _savedControls)
                Controls.Add(ctrl);

            _GUIDesignerTool.AddControls(this.Controls);
            base.OnRestore();
        }

        /// <summary>
        /// Called by the framework to remove all controls.
        /// </summary>
        public void RemoveAllControls()
        {
            if (_GUIDesignerTool == null)
                return;

            foreach (Control control in Controls)
                _GUIDesignerTool.RemoveControl(control);

            Controls.Clear();
        }

        /// <summary>
        /// Called by the framework to remove the given control.
        /// </summary>
        /// <param name="control">The control to remove.</param>
        public void RemoveControl(Control control)
        {
            Controls.Remove(control);
            _GUIDesignerTool.RemoveControl(control);
        }

        /// <summary>
        /// Called by the framework to append a control.
        /// </summary>
        /// <param name="control">The control to attach to the process station.</param>
        public virtual void AppendControl(Control control)
        {
            if (control.Tag as ControlData == null)
                throw new Exception("Control data is not set. Use Control.Tag = new PoControlData");

            ControlData ctrlData = (ControlData)control.Tag;


            if (ctrlData.PropertyFilter == null)
                throw new Exception("Control property filter is not set.");

            bool tagFound = false;

            for (int i = 0; i < ctrlData.PropertyFilter.Count; i++)
            {
                if (((string)ctrlData.PropertyFilter[i]) == "Tag")
                {
                    tagFound = true;
                    break;
                }
            }

            if (!tagFound)
                ctrlData.PropertyFilter.Add("Tag");

            ctrlData.StationId = XmlHelper.GetObjectID(XmlRep);

            ctrlData.ProcessStationType = SubType;
            control.Name = this.Text;
            Controls.Add(control);
        }

        /// <summary>
        /// Call this to register your control.
        /// </summary>
        /// <param name="control">The control to </param>
        public void RegisterControl( Control control)
        {
            if (_GUIDesignerTool == null)
                return;

            AppendControl(control);
            List<Control> controls = new List<Control>();
            controls.Add(control);
            _GUIDesignerTool.AddControls(controls);
        }

        /// <summary>
        /// Register a list of controls.
        /// </summary>
        /// <param name="controls">The controls to register.</param>
        public void RegisterControls( List<Control> controls )
        {
            if (_GUIDesignerTool == null)
                return;

            foreach (Control control in controls)
                AppendControl(control);

            _GUIDesignerTool.AddControls(controls);
        }

        /// <summary>
        /// Called by the fremework after a port is connected.
        /// </summary>
        /// <param name="from">From port ( cast this to Port)</param>
        /// <param name="to">To port ( cast this to Port) </param>
        public override void OnPostConnectedConnector(Mp.Visual.Diagram.Connector from, Mp.Visual.Diagram.Connector to)
        {
            base.OnPostConnectedConnector(from, to);
            
            Port port = to as Port;
            
            uint signalListId = XmlHelper.GetObjectID( port.SignalList );

            foreach (Control ctrl in Controls)
            {
                ControlData ctrlData = (ControlData) ctrl.Tag;
                ctrlData.SignalListId  = signalListId;
            }
        }
     
        /// <summary>
        /// The controls of the visual process station.
        /// </summary>
        public List<Control> Controls
        {
            get { return _controls; }
        }

        private static PanelDesignerTool _GUIDesignerTool;
        private List<Control> _controls = new List<Control>();
    }
}
