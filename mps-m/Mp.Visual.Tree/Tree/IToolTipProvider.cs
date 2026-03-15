using System;
using System.Collections.Generic;
using System.Text;
using Mp.Visual.Tree.Tree.NodeControls;

namespace Mp.Visual.Tree.Tree
{
	public interface IToolTipProvider
	{
		string GetToolTip(TreeNodeAdv node, NodeControl nodeControl);
	}
}
