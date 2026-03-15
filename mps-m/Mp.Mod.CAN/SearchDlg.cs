using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Mp.Visual.Tree;
using Mp.Visual.Tree.Tree;
namespace Mp.Mod.CAN
{
    public partial class SearchDlg : Form
    {
        public TreeViewAdv _tree;
        public int _foundSignals = 0;

        public SearchDlg(TreeViewAdv tree)
        {
            _tree = tree;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
        }

        private void SignalSearchDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

        private void onFindAll_Click(object sender, EventArgs e)
        {
            if (_tree.Root == null)
                return;

            TreeNodeAdv rootNode = _tree.Root;
            string findSig = findWhat.Text;
            _tree.ClearSelection();
            _foundSignals = 0;

            foreach( TreeNodeAdv dbNodes in rootNode.Children)
                SearchNode(dbNodes.Children[1], findSig);

            if( _foundSignals == 1)
                signalsFound.Text = String.Format(StringResource.SignalFound, _foundSignals.ToString());
            else
                signalsFound.Text = String.Format(StringResource.SignalsFound, _foundSignals.ToString());
        }

        private void SearchNode(TreeNodeAdv nodeAdv, string what)
        {
            foreach (TreeNodeAdv child in nodeAdv.Children)
                SelectNode(child, what);
        }

        private void SelectNode(TreeNodeAdv nodeAdv, string what)
        {
            Node node = (Node)nodeAdv.Tag;

            if (matchCase.Checked && matchWholeWord.Checked)
            {
                if (node.Text == what && nodeAdv.Children.Count == 0)
                {
                    nodeAdv.IsSelected = true;

                    if (nodeAdv.Parent != null)
                        nodeAdv.Parent.ExpandAll();

                    _tree.EnsureVisible(nodeAdv);
                    _foundSignals++;
                }
            }
            else if (matchCase.Checked && !matchWholeWord.Checked)
            {
                if (node.Text.Contains(what) && nodeAdv.Children.Count == 0)
                {
                    nodeAdv.IsSelected = true;

                    if (nodeAdv.Parent != null)
                        nodeAdv.Parent.ExpandAll();

                    _tree.EnsureVisible(nodeAdv);
                    _foundSignals++;

                }
            }
            else if (!matchCase.Checked && matchWholeWord.Checked)
            {
                if (node.Text.ToUpper() == what.ToUpper() && nodeAdv.Children.Count == 0)
                {
                    nodeAdv.IsSelected = true;
                    if (nodeAdv.Parent != null)
                        nodeAdv.Parent.ExpandAll();

                    _tree.EnsureVisible(nodeAdv);
                    _foundSignals++;
                }
            }
            else if (!matchCase.Checked && !matchWholeWord.Checked)
            {
                string nodeText = node.Text.ToUpper();

                if (nodeText.Contains(what.ToUpper()) && nodeAdv.Children.Count == 0)
                {
                    nodeAdv.IsSelected = true;
                    if (nodeAdv.Parent != null)
                        nodeAdv.Parent.ExpandAll();

                    _tree.EnsureVisible(nodeAdv);
                    _foundSignals++;
                }
            }
        }
    }
}
