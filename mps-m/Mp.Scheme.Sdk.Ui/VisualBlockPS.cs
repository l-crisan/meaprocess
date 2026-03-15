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
using System.Windows.Forms;
using System.Xml;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public abstract class VisualBlockPS : VisualPS
    {
        private Visual.Base.VisualBlock _blockView;

        public VisualBlockPS()
        {

        }

        protected abstract Control OnCreateControl();
        protected abstract void OnInitControl();

        public Visual.Base.VisualBlock BlockView
        {
            get{ return _blockView;}
            set{ _blockView = value;}
        }


        public override void AppendControl(Control control)
        {
            base.AppendControl(control);
            BlockView = (Visual.Base.VisualBlock)control;
        }

        public override void  OnDefaultInit()
        {
            OnInitControl();
          base.OnDefaultInit();
        }

        public override void OnLoadXml()
        {
            OnInitControl();
            base.OnLoadXml();
        }

        protected override void OnUpdateSignalList()
        {
            base.OnUpdateSignalList();

            if(_blockView == null)
                return;

            if (InputPorts.Count != 1)
                return;

            Port port = InputPorts[0];

            XmlElement xmlSignal;

            //Remove all controls.
            while (_blockView.ControlsInContainer.Count != 0)
            {
                Control ctrl = _blockView.ControlsInContainer[0];
                _blockView.RemoveControl(ctrl);
            }

            if (port.SignalList == null)
                return;

            //Create an replace the new controls
            int cols = _blockView.ColumnCount;
            int rows = (int)Math.Ceiling(port.SignalList.ChildNodes.Count / (double)_blockView.ColumnCount);

            _blockView.RowCount = rows;

            int row = 0;
            int col = 0;

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

                Control control = OnCreateControl();

                _blockView.AddControl(control);
                col++;

                if (col == cols)
                {
                    col = 0;
                    row++;
                }
            }

            _blockView.UpdateProperties();
        }
    }
}
