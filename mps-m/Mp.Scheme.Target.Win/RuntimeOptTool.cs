using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Mp.Scheme.Sdk;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using Atesion.Utils;

namespace Mp.Scheme.Win
{
    internal class RuntimeOptTool : Tool
    {
        private Document _doc;
        private Form _mainFrame;
        
        public RuntimeOptTool()
        {
            base.Name = StringResource.RuntimeOptionsMenu;
            base.ToolTip = StringResource.RuntimeOptionsText;
        }

        public override void OnLoadDocument(Document doc, DockPanel dockPanel, Form mainFrame)
        {
            _doc = doc;
            _mainFrame = mainFrame;
        }

        public override void OnExecute()
        {
            RuntimeOptDlg dlg = new RuntimeOptDlg(_doc);
            dlg.ShowDialog();
        }

        public override bool NeedToSaveDocument
        {
            get { return false; }
        }

        public override bool NeedToReloadDocument
        {
            get { return false; }
        }
    }
}
