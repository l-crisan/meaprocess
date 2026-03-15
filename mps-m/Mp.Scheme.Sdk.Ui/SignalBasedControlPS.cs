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
using System.Windows.Forms;
using System.Xml;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    /// <summary>
    /// This class implements a signal based process station.
    /// </summary>
    /// <remarks>
    /// A signal based process station is a process station which have for each signal a control.
    /// For example a gauge control per signal.
    /// </remarks>
    public class SignalBasedControlPS: VisualPS
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SignalBasedControlPS()
        {
        }
        

        /// <summary>
        /// Called by the framework when is neccessery to create a new control for the given signal.
        /// </summary>
        /// <param name="xmlSignal">The signal representation.</param>
        /// <returns>The new created control.</returns>
        protected virtual Control OnCreateNewControl(XmlElement xmlSignal)
        {
            return null;
        }
        
        /// <summary>
        /// Called by the framework when a control should be updated because the signal data has been changed-
        /// </summary>
        /// <param name="control">The control to update.</param>
        /// <param name="xmlSignal">The signal which has been changed.</param>
        protected virtual void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
        }


        public override string CopyToXml()
        {
            OnSaveXml();

            StringBuilder sb = new StringBuilder();
            sb.Append("<Station>");
            sb.Append(XmlRep.OuterXml);
            sb.Append("</Station>");
            return sb.ToString();
        }

        protected override void  OnUpdateSignalList()
        {
            //Number of gauges = number of signals.
            if (InputPorts.Count != 1)
                return;

            Port port = InputPorts[0];

            if (port.SignalList == null)
            {
                RemoveAllControls();
                return;
            }

            if( port.SignalList.ChildNodes.Count == 0)
            {
                RemoveAllControls();
                return;
            }


            List<ulong> signalIds = new List<ulong>();
            List<Control> controlsToRegister = new List<Control>();
            XmlElement xmlSignal;

            //Check for new signals .
            foreach (XmlElement xmlElement in port.SignalList.ChildNodes)
            {
                uint sigID = XmlHelper.GetObjectID(xmlElement);

                if (sigID == 0)
                {//We have a signal reference.
                    sigID = Convert.ToUInt32(xmlElement.InnerText);
                    xmlSignal = Document.GetXmlObjectById(sigID);

                    if (xmlSignal == null)
                        continue;
                }
                else
                {
                    xmlSignal = xmlElement;
                }

                signalIds.Add(sigID);

                Control control = GetControlForSignal(sigID);

                if (control == null)
                {
                    control = OnCreateNewControl(xmlSignal);

                    ControlData ctrlData = (ControlData) control.Tag; ;
                    ctrlData.SignalId = sigID;
                    ctrlData.SignalListId = XmlHelper.GetObjectID(port.SignalList);
                    controlsToRegister.Add(control);
                }

                OnUpdateControl(control, xmlSignal);
            }

            //Remove controls without signals.
            bool available = false;
            List<Control> controlsToRemove = new List<Control>();
            foreach (Control control in Controls)
            {
                ControlData ctrlData = (ControlData)control.Tag;
                available = false;

                foreach (ulong signalId in signalIds)
                {
                    if (signalId == ctrlData.SignalId)
                    {
                        available = true;
                        break;
                    }
                }

                if (!available)
                    controlsToRemove.Add(control);
            }

            //Remove the unassigned controls.
            foreach (Control control in controlsToRemove)
                RemoveControl(control);            

            //Register the new controls.
            if(controlsToRegister.Count != 0)
                RegisterControls(controlsToRegister);
        }

        private Control GetControlForSignal(ulong signalId)
        {
            foreach (Control control in Controls)
            {
                ControlData ctrlData = (ControlData)control.Tag;

                if (ctrlData.SignalId == signalId)
                    return control;
            }

            return null;
        }
    }
}
