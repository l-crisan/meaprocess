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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mp.Visual.Docking
{
	public class DockWindowCollection : ReadOnlyCollection<DockWindow>
	{
		internal DockWindowCollection(DockPanel dockPanel)
            : base(new List<DockWindow>())
		{
			Items.Add(new DockWindow(dockPanel, DockState.Document));
			Items.Add(new DockWindow(dockPanel, DockState.DockLeft));
			Items.Add(new DockWindow(dockPanel, DockState.DockRight));
			Items.Add(new DockWindow(dockPanel, DockState.DockTop));
			Items.Add(new DockWindow(dockPanel, DockState.DockBottom));
		}

		public DockWindow this [DockState dockState]
		{
			get
			{
				if (dockState == DockState.Document)
					return Items[0];
				else if (dockState == DockState.DockLeft || dockState == DockState.DockLeftAutoHide)
					return Items[1];
				else if (dockState == DockState.DockRight || dockState == DockState.DockRightAutoHide)
					return Items[2];
				else if (dockState == DockState.DockTop || dockState == DockState.DockTopAutoHide)
					return Items[3];
				else if (dockState == DockState.DockBottom || dockState == DockState.DockBottomAutoHide)
					return Items[4];

				throw (new ArgumentOutOfRangeException());
			}
		}
	}
}
