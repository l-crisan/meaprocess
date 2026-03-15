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
using System.Drawing;
using System.Windows.Forms;

namespace Mp.Visual.Docking
{
	public interface IDockContent
	{
		DockContentHandler DockHandler	{	get;	}
		void OnActivated(EventArgs e);
		void OnDeactivate(EventArgs e);
	}

	public interface INestedPanesContainer
	{
		DockState DockState	{	get;	}
		Rectangle DisplayingRectangle	{	get;	}
		NestedPaneCollection NestedPanes	{	get;	}
		VisibleNestedPaneCollection VisibleNestedPanes	{	get;	}
		bool IsFloat	{	get;	}
	}

    internal interface IDragSource
    {
        Control DragControl { get; }
    }

    internal interface IDockDragSource : IDragSource
    {
        Rectangle BeginDrag(Point ptMouse);
        bool IsDockStateValid(DockState dockState);
        bool CanDockTo(DockPane pane);
        void FloatAt(Rectangle floatWindowBounds);
        void DockTo(DockPane pane, DockStyle dockStyle, int contentIndex);
        void DockTo(DockPanel panel, DockStyle dockStyle);
    }

    internal interface ISplitterDragSource : IDragSource
    {
        void BeginDrag(Rectangle rectSplitter);
        void EndDrag();
        bool IsVertical { get; }
        Rectangle DragLimitBounds { get; }
        void MoveSplitter(int offset);
    }
}
