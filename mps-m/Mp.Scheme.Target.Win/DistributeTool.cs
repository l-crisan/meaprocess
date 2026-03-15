//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2010-2016  Laurentiu-Gheorghe Crisan
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
using System.IO;
using System.Windows.Forms;
using Mp.Scheme.Sdk;
using Mp.Visual.Docking;

namespace Mp.Scheme.Target.Win
{
    internal class DistributeTool : Tool
    {
        private Document _doc;
        private Form _mainFrame;
        public DistributeTool()
        {
            base.Name = StringResource.DistributeMenu;
            base.Shortcut = Keys.F10;
            base.ToolTip = StringResource.DistributeText;
        }

        public override void OnLoadDocument(Document doc, DockPanel dockPanel, Form mainFrame)
        {
            base.OnLoadDocument(doc, dockPanel, mainFrame);
            _doc = doc;
            _mainFrame = mainFrame;
        }

        public override void LoadResources()
        {
            base.Name = StringResource.DistributeMenu;
            base.ToolTip = StringResource.DistributeText;
        }

        public override void OnExecute()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "*.zip|*.zip";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if (File.Exists(dlg.FileName))
                File.Delete(dlg.FileName);


            DistributeDlg progressDlg = new DistributeDlg(_doc.File,dlg.FileName);
            progressDlg.ShowDialog();
        }

        public override bool NeedToSaveDocument
        {
            get { return true; }
        }

        public override bool NeedToReloadDocument
        {
            get { return false; }
        }
    }
}
