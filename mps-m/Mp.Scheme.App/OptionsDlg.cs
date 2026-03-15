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
using System.Threading;

namespace Mp.Scheme.App
{
    public partial class OptionsDlg : Form
    {
        private ImageList _imgList = new ImageList();

        public OptionsDlg()
        {
            InitializeComponent();
            Icon = Sdk.Document.AppIcon;
            _imgList.Images.Add(Resource.English);
            _imgList.Images.Add(Resource.Deutsch);
            language.ImageList = _imgList;

            showSplashScreen.Checked = Properties.Settings.Default.ShowSplashScreen;
            loadLastFile.Checked = Properties.Settings.Default.LoadLastFile;
            snapToGrid.Checked = Properties.Settings.Default.SnapToGrid;
            gridInterval.Text = Properties.Settings.Default.GridInterval.ToString();

            language.Items.Add(new Utils.ComboBoxExItem("English",0));
            language.Items.Add(new Utils.ComboBoxExItem("Deutsch",1));

            appaerance.SelectedIndex = 0;

            if (Properties.Settings.Default.Language != -1)
                language.SelectedIndex = Properties.Settings.Default.Language;
            else
                language.SelectedIndex = 0;
        }


        private void OnOKClick(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowSplashScreen = showSplashScreen.Checked;
            Properties.Settings.Default.LoadLastFile = loadLastFile.Checked;
            Properties.Settings.Default.SnapToGrid = snapToGrid.Checked;
            Properties.Settings.Default.GridInterval = Convert.ToInt32(gridInterval.Text);
            Properties.Settings.Default.Language = language.SelectedIndex;
            DialogResult = DialogResult.OK;

            if (Properties.Settings.Default.Language == 0)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
            }

            Close();
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
