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
using Mp.Scheme.Sdk;
using Mp.Visual.Docking;
using System.Windows.Forms;

namespace Mp.Scheme.Deploy
{
    public class DeployTool : Tool
    {
        private Document _doc;

        public DeployTool()
        {
            Name = StringResource.Deploy;
            Icon = Resource.Runtime;
            Shortcut = Keys.F10;
            ToolTip = StringResource.Deploy;
        }

        public override void OnLoadDocument(Document doc, DockPanel dockPanel, Form mainFrame)
        {
            _doc = doc;
        }


        public override void LoadResources()
        {
            Name = StringResource.Deploy;
            ToolTip = StringResource.Deploy;           
        }


        public override void OnExecute()
        {
            if(! _doc.IsValid)
                return;

            using (DeployDlg dlg = new DeployDlg(_doc))
            {
                dlg.ShowDialog();
            }
        }


        public override bool NeedToSaveDocument
        {
            get { return true; }
        }


        public override bool NeedToValidateDocument
        {
            get { return true; }
        }


        public override bool NeedToReloadDocument
        {
            get { return false; }
        }
    }
}
