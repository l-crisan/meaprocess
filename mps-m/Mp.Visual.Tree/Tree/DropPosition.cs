using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Visual.Tree.Tree
{
	public struct DropPosition
	{
		private TreeNodeAdv _node;
		public TreeNodeAdv Node
		{
			get { return _node; }
			set { _node = value; }
		}

		private NodePosition _position;
		public NodePosition Position
		{
			get { return _position; }
			set { _position = value; }
		}
	}
}
