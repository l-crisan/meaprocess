using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;

using Mp.Components;
using Mp.Scheme.Sdk;
using Atesion.Utils;

namespace Mp.Scheme.Win
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
