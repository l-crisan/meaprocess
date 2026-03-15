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
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Mp.Visual.Docking;
using Mp.Utils;
using Mp.Scheme.Sdk;
using Mp.Scheme.Info;


namespace Mp.Scheme.App
{
    public partial class MainFrame : Form
    {
        private readonly ModuleManager _moduleManager = new ModuleManager();
        private readonly string _title;
        private Document _document;
        private readonly MruStripMenuInline _mruMenu;
        private readonly OutputWindow _messages;
        private readonly DiagramContainer _diagramContainer = new DiagramContainer();
        private const string MruRegKey = "MruMenu";
        private readonly string _file = "";
        private const string Pw = "52gr2541g4211";

        public MainFrame(string file)
        {
            _file = file;
            InitializeComponent();

            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            printToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;
            validateToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.T;
            contentsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F1;
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            Document.AppIcon = this.Icon;

            _messages = new OutputWindow(dockPanel);

            //Remember the application title.
            _title = this.Text;

            //Restore the windows state.
            FormStateHandler.Restore(this, Document.RegistryKey + "MainFrame");
                      
            _mruMenu = new MruStripMenuInline(fileToolStripMenuItem, recentToolStripMenuItem, new MruStripMenu.ClickedHandler(OnMruFile), (Document.RegistryKey + MruRegKey + "\\MRU"), 10);
            _mruMenu.LoadFromRegistry();

            if (Properties.Settings.Default.LoadLastFile && String.IsNullOrEmpty(_file))
            {
                string[] files = _mruMenu.GetFiles();
                if (files.Length != 0 )
                    _file = files[files.Length - 1];
            }

            //Load all registered modules.
            LoadRegModules();

            //Set status to ready.
            _statusStrip.Text = Mp.Scheme.App.StringResource.ResourceManager.GetString("Ready");

            //Load the dock panel state.
            InitDockPanel();

            UpdateAppRegPath();
        }
  

        private void LoadRegModules()
        {
            try
            {
                Assembly asm = Assembly.GetAssembly(this.GetType());
                string file = Path.GetDirectoryName(asm.Location);
                string path = Path.Combine(file, "module.set");

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    StreamReader sr = new StreamReader(fs);
                    _moduleManager.LoadAllRegModules(sr.ReadToEnd(), this.Handle);
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        private void OnSchemeInfo(object sender, EventArgs e)
        {
            SchemeInfoDlg dlg = new SchemeInfoDlg(_document.XmlDoc.DocumentElement);
            
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                _document.Modified = true;
        }


        private void OnDiagramDoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs em = e as MouseEventArgs;
            
            if (em != null)
                if (em.Button != MouseButtons.Left)
                    return;

            OnSchemeInfo(sender, e);
        }


        private void InitDockPanel()
        {
            if (Properties.Settings.Default.DockPanelState == "")
            {
                _messages.Show(dockPanel);
                _messages.DockState = DockState.DockBottom;

                _diagramContainer.Show(dockPanel);
                _diagramContainer.DockState = DockState.Document;
            }
            else
            {
                MemoryStream mm = new MemoryStream();
                StreamWriter sw = new StreamWriter(mm);
                sw.Write(Properties.Settings.Default.DockPanelState.ToCharArray());
                sw.Write(0);

                sw.Flush();
                mm.Flush();

                mm.Seek(0, SeekOrigin.Begin);

                dockPanel.LoadFromXml(mm, new DeserializeDockContent(GetDockContent), true);
            }
        }


        private DockContent GetDockContent(string padTypeName)
        {
            string[] array = padTypeName.Split(',');

            switch (array[0])
            {
                case "Mp.Visual.Docking.OutputWindow":
                    return _messages;

                case "Mp.Scheme.App.DiagramContainer":
                    return _diagramContainer;
            }

            return null;
        }


        private void UpdateAppRegPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string file = path + "\\" + Document.RegistryKey +"path.mcfg";

            if (!Directory.Exists(Path.GetDirectoryName(file)))
                Directory.CreateDirectory(Path.GetDirectoryName(file));

            using(StreamWriter sw = new StreamWriter(file, false))
            {
                sw.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
                sw.Close();
            }
        }
        

        private void OnMruFile(int number, String filename)
        {
            _mruMenu.SetFirstFile(number);

            if(!OpenFile(filename))
               _mruMenu.RemoveFile(number);
        }


        private void ToolsOnCloseDocument()
        {
            if (_document == null)
                return;

            if (_document.RuntimeEngine == null)
                return;

            foreach (Tool tool in _document.RuntimeEngine.Tools)
                tool.OnCloseDocument();
        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (Properties.Settings.Default.ShowSplashScreen)
                SplashScreen.EndDisplay();

            //Load the given file.
            if (_file != null && _file != "")
                OpenFile(_file);
        }


        private void OnExitToolClick(object sender, EventArgs e)
        {
            Close();
        }


        private void OnNewToolClick(object sender, EventArgs e)
        {
            if (!CanCloseDocument())
                return;

            NewDocumentDlg newDlg = new NewDocumentDlg(_moduleManager);

            if (DialogResult.OK != newDlg.ShowDialog())
                return;

            if (newDlg.RuntimeEngine == null)
                return;

            ToolsOnCloseDocument();

            if (_document != null)
            {
                CloseTools();
                _document.Close();
            }

            _document = new Document(_diagramContainer.InnerPanel, newDlg.RuntimeEngine, _moduleManager);
            _diagramContainer.Document = _document;
            _document.OnOutputMessage += new MessageDelegate(OutputValidationInfo);
            _document.OnDocumentStateChangedEvent += new DocumentStateDelegate(OnDocumentModified);
            _document.GridInterval = Properties.Settings.Default.GridInterval;
            _document.SnapToGrid = Properties.Settings.Default.SnapToGrid;
            _document.New();

            LoadPalette(newDlg.RuntimeEngine);
            LoadTools(newDlg.RuntimeEngine);
            
            EnableMenuItems();
            _messages.Clear();

            _statusText.Text = StringResource.ResourceManager.GetString("Unsaved");
            _diagramContainer.Show(dockPanel);
            _diagramContainer.Focus();
        }


        private void LoadToolMenuResources()
        {
            if (_document == null)
                return;

            if (_document.RuntimeEngine != null)
            {
                foreach (Tool tool in _document.RuntimeEngine.Tools)
                    tool.LoadResources();
            }

            for (int index = 3; index < _toolsToolStripMenuItem.DropDown.Items.Count; index++)
            {
                ToolStripItem item  = _toolsToolStripMenuItem.DropDown.Items[index];
                if (item.Tag == null)
                    continue;

                Tool tool = (Tool)item.Tag;
                item.Text = tool.Name;
                item.ToolTipText = tool.ToolTip;
            }

            foreach (ToolStripButton button in _toolsToolsStrip.Items)
            {
                Tool tool = (Tool)button.Tag;
                button.ToolTipText = tool.ToolTip;
            }
        }


        private void LoadTools(Mp.Scheme.Sdk.Module engine)
        {
            int index = 1;

            //First delete the old tools.
            for (index = 3; index < _toolsToolStripMenuItem.DropDown.Items.Count; index++)
            {
                _toolsToolStripMenuItem.DropDown.Items.RemoveAt(index);
                index--;
            }

            _toolsToolsStrip.Items.Clear();

            if (engine.Tools.Count > 0)
                _toolsToolStripMenuItem.DropDown.Items.Add(new ToolStripSeparator());

            //Insert the new runtime engine tools.
            foreach (Tool tool in engine.Tools)
            {
                tool.OnCreate();
                tool.OnLoadDocument(_document,dockPanel,this);
                
                if (String.IsNullOrEmpty(tool.Name))
                    continue;

                ToolStripMenuItem menuItem = new ToolStripMenuItem();


                menuItem.Text        = tool.Name;
                menuItem.ToolTipText = tool.ToolTip;

                if (tool.Shortcut != Keys.None)
                {
                    menuItem.ShowShortcutKeys = true;
                    menuItem.ShortcutKeys = tool.Shortcut;
                }

                menuItem.Tag = tool;
                
                menuItem.Click += new EventHandler(OnToolClicked);
                

                //Add to the known tools.
                _toolsToolStripMenuItem.DropDown.Items.Add( menuItem );

                //If has an icon add the tool to the toolbar
                if (tool.Icon != null)
                {
                    menuItem.Image = tool.Icon.ToBitmap();
                    ToolStripButton toolBt = new ToolStripButton();
                    toolBt.Tag = tool;
                    toolBt.Click += new EventHandler(OnToolClicked);
                    toolBt.Image = tool.Icon.ToBitmap();
                    toolBt.ToolTipText = tool.ToolTip;
                    _toolsToolsStrip.Items.Add(toolBt);
                }
            }
        }


        private void CloseTools()
        {
            int index = 1;
            Sdk.Module engine = _document.RuntimeEngine;

            if (engine == null)
                return;

            //First delete the old tools.
            for (index = 3; index < _toolsToolStripMenuItem.DropDown.Items.Count; index++)
            {
                _toolsToolStripMenuItem.DropDown.Items.RemoveAt(index);
                index--;
            }

            _toolsToolsStrip.Items.Clear();

            if (engine.Tools.Count > 0)
                _toolsToolStripMenuItem.DropDown.Items.Add(new ToolStripSeparator());

            //Insert the new runtime engine tools.
            foreach (Tool tool in engine.Tools)
                tool.OnClose();
        }


        private bool OnSave(bool saveAs)
        {
            if (_document == null)
                return false;

            if (OutputValidationInfo(_document.Validate()))
            {
                if (MessageBox.Show(StringResource.SaveInvalidDoc, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                    return false;
            }

            if (_document.File == "" || saveAs)
            {
                SaveFileDialog saveDlg = new SaveFileDialog();

                saveDlg.DefaultExt = _document.RuntimeEngine.RuntimeEngineFileExt;

                saveDlg.Filter = _document.RuntimeEngine.RuntimeEngineFileExt + " (" + _document.RuntimeEngine.FileExtDescription + ")|" + _document.RuntimeEngine.RuntimeEngineFileExt;

                if (DialogResult.OK != saveDlg.ShowDialog())
                    return false;

                _document.File = saveDlg.FileName;
                this.Text = _title + ": " + saveDlg.FileName;
            }

            string schemeState = _diagramContainer.SaveState();
            XmlHelper.SetParam(_document.XmlDoc.DocumentElement, "schemeState", "string", schemeState);
            

            ToolsOnSaveDocument();
            string file = Path.GetFileNameWithoutExtension(_document.File);
            file += ".tp";
            file = Path.Combine(Path.GetDirectoryName(_document.File), file);
            bool res = false;
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                res = _document.Save(fs, _document.File);
                fs.Close();
            }

            if( !res)
            {
                _statusText.Text = StringResource.ResourceManager.GetString("File_save_Failed");
                return false;
            }

            //Zip the file
            using (FileStream fs = File.Create(_document.File))
            {                
                ZipFile zfile = ZipFile.Create(fs);
                zfile.Password = Pw;
                
                string path = Path.GetDirectoryName(Path.GetFullPath(_document.File));
                zfile.NameTransform = new ZipNameTransform(path);
                zfile.BeginUpdate();
                zfile.Add(file);
                zfile.CommitUpdate();
                zfile.Close();
                fs.Flush();
                fs.Close();
            }

            File.Delete(file);
            _statusText.Text = StringResource.ResourceManager.GetString("File_Saved");
            
            return true;
        }


        private void ToolsOnSaveDocument()
        {
            foreach (Tool tool in _document.RuntimeEngine.Tools)
                tool.OnSaveDocument();
        }


        private void RemovePalette()
        {
            _diagramContainer.RemovePalette();
        }


        private void LoadPalette(Sdk.Module engine)
        {
            RemovePalette();
            _diagramContainer.LoadPalette(engine);
        }
        

        private void OnOpenToolClick(object sender, EventArgs e)
        {
            OpenFileDialog OpenDlg = new OpenFileDialog();
            OpenDlg.DefaultExt = "*.mpw";

            string filter = ("MeaProcess " + StringResource.Scheme + " | ");

            foreach (Sdk.Module module in _moduleManager.RuntimeEngines)
                filter += (module.RuntimeEngineFileExt + ";");

            OpenDlg.Filter = filter.TrimEnd(';');

            if (DialogResult.OK != OpenDlg.ShowDialog())
                return;

            OpenFile(OpenDlg.FileName);
        }


        private bool OpenFile(string file)
        {  
            try
            {
                LoadDocument(file);
            }
            catch (Exception ex)
            {
                CloseDocument();
                _messages.Show(dockPanel);
                _messages.WriteLine(ex.Message,MessageType.Error);
                return false;
            }
            
            return true;
        }


        private void CloseDocument()
        {
            ToolsOnCloseDocument();
            CloseTools();
            DisiableMenuItems();
            _document.Close();
            _document.File = "";
            RemovePalette();
            _document = null;
        }


        private void LoadDocument(string file)
        {
            LoadDocument(file, true);
        }


        private void LoadDocument(string file, bool chekPassword)
        {
            if (!CanCloseDocument())
                return;

            if (_document != null)
                CloseDocument();

            _document = new Document(_diagramContainer.InnerPanel, _moduleManager);
            _document.OnGetPasswordDelegate += new GetPasswordDelegate(OnGetPasswordDelegate);
            _diagramContainer.Document = _document;

            _document.OnOutputMessage += new MessageDelegate(OutputValidationInfo);
            _document.OnDocumentStateChangedEvent += new DocumentStateDelegate(OnDocumentModified);
            _document.GridInterval = Properties.Settings.Default.GridInterval;
            _document.SnapToGrid = Properties.Settings.Default.SnapToGrid;

            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                try
                {
                    ZipInputStream istream = new ZipInputStream(fs);
                    istream.Password = Pw;
                    ZipEntry entry = istream.GetNextEntry();
                    byte[] bytes = new byte[istream.Length];

                    istream.Read(bytes, 0, (int)istream.Length);
                    MemoryStream mm = new MemoryStream(bytes);
                    _document.Load(mm, file, chekPassword);
                }
                catch (ZipException em)
                {
                    Console.WriteLine(em.Message);
                    fs.Seek(0, SeekOrigin.Begin);

                    if (!_document.Load(fs, file, chekPassword))
                        throw new Exception(em.Message);
                }

                fs.Close();
            }

            string state = XmlHelper.GetParam(_document.XmlDoc.DocumentElement,"schemeState");
            
            if (state != "")
            {
                _diagramContainer.Activate();
                _document.DockPanel = _diagramContainer.Reinit();
                _diagramContainer.Activate();
                _diagramContainer.LoadState(state);

                foreach (DiagramWindow w in _document.Diagrams)
                {
                    if(!w.Visible)
                        w.Show(_diagramContainer.InnerPanel);
                }
            }

            LoadPalette(_document.RuntimeEngine);
            LoadTools(_document.RuntimeEngine);

            EnableMenuItems();

            _messages.Clear();

            OutputValidationInfo(_document.Validate());
            _diagramContainer.Show(dockPanel);
            _diagramContainer.Focus();
            _document.Modified = true;
            _document.Modified = false;
        }


        private string OnGetPasswordDelegate(string file)
        {
            PasswordDlg dlg = new PasswordDlg(file);
            dlg.ShowDialog();

            return dlg.Password;
        }


        private void OnSaveToolClick(object sender, EventArgs e)
        {
            if(OnSave(false))
                _mruMenu.AddFile(_document.File);
        }


        private void OnSaveAsToolClick(object sender, EventArgs e)
        {
            if(OnSave(true))
                _mruMenu.AddFile(_document.File);
        }  


        private void OnAboutToolClick(object sender, EventArgs e)
        {
            AboutBox dlg = new AboutBox();
            dlg.ShowDialog();
        }


        private void OnModulManagerToolClick(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                string command = AppDomain.CurrentDomain.BaseDirectory;
                psi.WorkingDirectory = command;
                psi.FileName = Path.Combine(command, "Mp.Scheme.MM.exe");

                switch (Properties.Settings.Default.Language)
                {
                    case 0:
                        psi.Arguments = "en-US";
                    break;

                    case 1:
                        psi.Arguments = "de-DE";
                    break;
                }
                
                Process proc = Process.Start(psi);

                this.Enabled = false;

                while (!proc.WaitForExit(100))
                    Application.DoEvents();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            this.Enabled = true;
            this.Focus();
            this.BringToFront();
            this.Activate();
        }


        private bool OutputValidationInfo(ValidationInfo info)
        {
            bool error = false;
            int image = 0;
            switch (info.Type)
            {
                case ValidationInfo.InfoType.Valid:
                    image = 0;
                    break;
                case ValidationInfo.InfoType.Error:
                    image = 2;
                    error = true;
                    break;
                case ValidationInfo.InfoType.Info:
                    image = 0;
                    break;
                case ValidationInfo.InfoType.Warning:
                    image = 1;
                    break;
            }

            _messages.Show(dockPanel);
            _messages.WriteLine(info.Message, (MessageType)image);
            return error;
        }


        private bool OutputValidationInfo( List<ValidationInfo> valInfoList)
        {
            _messages.Clear();
            bool error = false;

            foreach (ValidationInfo info in valInfoList)
            {
                if (OutputValidationInfo(info))
                    error = true;
            }

            return error;
        }


        private void OnToolClicked(object sender, EventArgs e)
        {
            Tool tool = null;
            ToolStripMenuItem menuTool = sender as ToolStripMenuItem;
            
            if( menuTool != null )
                tool = (menuTool.Tag as Tool);

            ToolStripButton toolBt = sender as ToolStripButton;

            if(  toolBt != null )
                tool = (toolBt.Tag as Tool);

            if (tool == null)
                return;

            if (tool.NeedToValidateDocument)
            {
                if (OutputValidationInfo(_document.Validate()))
                    return;
            }

            if (tool.NeedToSaveDocument && _document.Modified)
                if (!OnSave(_document.File == ""))
                    return;

            tool.OnExecute();

            if (tool.NeedToReloadDocument)
            {
                string file = _document.File;
                bool diagramActive = dockPanel.ActiveDocument is DiagramContainer;

                try
                {
                    LoadDocument(file, false);

                    if (diagramActive)
                        _diagramContainer.Activate();
                }
                catch (Exception ex)
                {
                    DisiableMenuItems();
                    _document.Close();
                    RemovePalette();
                    _messages.WriteLine(ex.Message, MessageType.Error);
                }

                if(!diagramActive)
                {
                    foreach (DockContent d in dockPanel.Contents)
                    {
                        if (!(d is DiagramContainer) && !(d is OutputWindow))
                        {
                            d.Activate();
                            break;
                        }
                    }
                }
            }
        }
    

        private void OnDocumentModified(bool modified)
        {
            if (_document != null)
            {
                if (modified)
                {
                    if (_document.File == null || _document.File == "")
                        this.Text = _title + ": " + StringResource.Unsaved;
                    else
                        this.Text = _title + ": " + _document.File + " *";
                }
                else
                {
                    if (_document.File == null || _document.File == "")
                        this.Text = _title + ": " + StringResource.Unsaved;
                    else
                        this.Text = _title + ": " + _document.File;
                }
            }
            else
            {
                this.Text = _title;
            }
        }


        private void OnClearToolClick(object sender, EventArgs e)
        {
            _messages.Clear();
        }


        private void EnableMenuItems()
        {
            saveAsToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            saveToolStripButton.Enabled = true;
            validateToolStripMenuItem.Enabled = true;
            validatetToolStripButton.Enabled = true;
            schemeInfoToolStripMenuItem.Enabled = true;
            schemePasswordToolStripMenuItem.Enabled = true;
            schemePropertiesToolStripMenuItem.Enabled = true;
            toolStripMenuItemSignals.Enabled = true;
        }


        private void DisiableMenuItems()
        {
            saveAsToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            saveToolStripButton.Enabled = false;
            validateToolStripMenuItem.Enabled = false;
            validatetToolStripButton.Enabled = false;
            schemeInfoToolStripMenuItem.Enabled = false;
            schemePasswordToolStripMenuItem.Enabled = false;
            schemePropertiesToolStripMenuItem.Enabled = false;
            toolStripMenuItemSignals.Enabled = false;
        }


        private void OnMainFrameFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CanCloseDocument();
        }


        private bool CanCloseDocument()
        {
            if (_document == null)
                return true;

            if (!_document.Modified)
                return true;

            if (_document.IsEmpty)
                return true;

            DialogResult res = MessageBox.Show(StringResource.ResourceManager.GetString("Save_Current_Changes"), "MeaProcess- Scheme Editor", MessageBoxButtons.YesNoCancel,
                                                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            switch (res)
            {
                case DialogResult.Yes:
                    return OnSave(_document.File == "");
                case DialogResult.Cancel:
                    return false;
            }

            return true;
        }


        private void OnValidateToolClick(object sender, EventArgs e)
        {
            OutputValidationInfo(_document.Validate());
        }


        private void OnMainFrameFormClosed(object sender, FormClosedEventArgs e)
        {
            ToolsOnCloseDocument();

            FormStateHandler.Save(this, Document.RegistryKey + "MainFrame");

            _mruMenu.SaveToRegistry();

            //Save the docking state
            using (MemoryStream mm = new MemoryStream())
            {
                dockPanel.SaveAsXml(mm, Encoding.UTF8, true);

                mm.Flush();
                mm.Seek(0, SeekOrigin.Begin);

                StreamReader sr = new StreamReader(mm);
                Properties.Settings.Default.DockPanelState = sr.ReadToEnd();
            }

            _diagramContainer.SaveState();

            //Save the runtime engines.
            Properties.Settings.Default.Save();
        }


        private void OnDragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData("FileName");
            
            if( files.Length > 0)
                OpenFile(files[0]);
        }


        private void OnDragOver(object sender, DragEventArgs e)
        {
            if( e.Data.GetDataPresent("FileName"))
                e.Effect = DragDropEffects.Move;

        }


        private void OnPageSetupClick(object sender, EventArgs e)
        {
            if (_document == null)
                return;

            if (_document.ActiveDiagram == null)
                return;
         
            PageSetupDialog dlg = new PageSetupDialog();
            dlg.Document = _document.ActiveDiagram.Diagram.PrintDoc;
            dlg.ShowDialog();
        }


        private void OnPrintPreviewClick(object sender, EventArgs e)
        {
            if( _document == null)
                return;

            if (_document.ActiveDiagram == null)
                return;

            PrintPreviewDialog dlg = new PrintPreviewDialog();
            dlg.ShowIcon = false;
            dlg.Document = _document.ActiveDiagram.Diagram.PrintDoc;
            dlg.ShowDialog();
        }


        private void OnPrintClick(object sender, EventArgs e)
        {
            if (_document == null)
                return;


            if (_document.ActiveDiagram == null)
                return;

            PrintDialog dlg = new PrintDialog();
            dlg.UseEXDialog = true;
            dlg.Document = _document.ActiveDiagram.Diagram.PrintDoc;
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            _document.ActiveDiagram.Diagram.PrintDoc.Print();
        }


        private void OnShowOutputWindowClick(object sender, EventArgs e)
        {
            _messages.Show(dockPanel);
            _messages.Focus();
        }



        private void OnSchemeInfoClick(object sender, EventArgs e)
        {
            OnSchemeInfo(sender, e);
        }


        private void OnOptionsToolClick(object sender, EventArgs e)
        {
            OptionsDlg dlg = new OptionsDlg();

            string language = Thread.CurrentThread.CurrentCulture.IetfLanguageTag;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if (_document != null)
            {
                _document.GridInterval = Properties.Settings.Default.GridInterval;
                _document.SnapToGrid = Properties.Settings.Default.SnapToGrid;
            }

            string newlanguage = Thread.CurrentThread.CurrentCulture.IetfLanguageTag;

            if(newlanguage != language)
                LoadResources();
        }


        private void LoadResources()
        {
            this.Visible = false;

            ResourceLoader.LoadResources(this);

            ResourceLoader.LoadResources(_messages);
            _messages.Height = _messages.Height + 1;

            LoadToolMenuResources();

            if( _document != null)
                EnableMenuItems();

            _diagramContainer.LoadResources();
            this.Visible = true;
        }


        private void OnSchemePropertiesClick(object sender, EventArgs e)
        {
            SchemePropertyDlg dlg = new SchemePropertyDlg(_document);
            dlg.ShowDialog();
        }


        private void OnShowSignalsClick(object sender, EventArgs e)
        {
            SignalsDlg dlg = new SignalsDlg(_document);
            dlg.ShowDialog();
        }


        private void OnHelpContentClick(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Document.HelpFile, HelpNavigator.TableOfContents);
        }
        

        private void OnWebLinkClick(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://www.atesion.de");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void OnMPALEditorClick(object sender, EventArgs e)
        {
            try
            {
                string command = Path.Combine(AppDomain.CurrentDomain.BaseDirectory , "Mpal.Editor.exe");
                Process.Start(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void OnSchemePasswordClick(object sender, EventArgs e)
        {
            SchemePasswordDlg dlg = new SchemePasswordDlg(_document);
            dlg.ShowDialog();
        }
    }
}

