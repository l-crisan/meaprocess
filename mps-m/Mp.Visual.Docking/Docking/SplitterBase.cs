//    MeaProcess - Meaurement and Automation framework.
//
//    The MIT License
//    Copyright (C) 2015 Laurentiu-Gheorghe Crisan
//    Copyright (C) 2007 Weifen Luo
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
//    associated documentation files (the "Software"), to deal in the Software without restriction, 
//    including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//    and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
//    subject to the following conditions: The above copyright notice and this permission notice shall be 
//    included in all copies or substantial portions of the Software.
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Mp.Visual.Docking
{
	internal class SplitterBase : Control
	{
		public SplitterBase()
		{
			SetStyle(ControlStyles.Selectable, false);
		}

		public override DockStyle Dock
		{
			get	{	return base.Dock;	}
			set
			{
				SuspendLayout();
				base.Dock = value;

				if (Dock == DockStyle.Left || Dock == DockStyle.Right)
					Width = SplitterSize;
				else if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
					Height = SplitterSize;
				else
					Bounds = Rectangle.Empty;

				if (Dock == DockStyle.Left || Dock == DockStyle.Right)
					Cursor = Cursors.VSplit;
				else if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
					Cursor = Cursors.HSplit;
				else
					Cursor = Cursors.Default;
					
				ResumeLayout();
			}
		}

		protected virtual int SplitterSize
		{
			get	{	return 0;	}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button != MouseButtons.Left)
				return;

			StartDrag();
		}

		protected virtual void StartDrag()
		{
		}

		protected override void WndProc(ref Message m)
		{
            // eat the WM_MOUSEACTIVATE message
			if (m.Msg == (int)Win32.Msgs.WM_MOUSEACTIVATE)
				return;

			base.WndProc(ref m);
		}
	}
}
