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
using System.Drawing;
using System.Windows.Forms;

namespace Mp.Visual.Docking
{
	public class FloatWindowCollection : ReadOnlyCollection<FloatWindow>
	{
		internal FloatWindowCollection()
            : base(new List<FloatWindow>())
		{
		}

		internal int Add(FloatWindow fw)
		{
			if (Items.Contains(fw))
				return Items.IndexOf(fw);

			Items.Add(fw);
            return Count - 1;
		}

		internal void Dispose()
		{
			for (int i=Count - 1; i>=0; i--)
				this[i].Close();
		}

		internal void Remove(FloatWindow fw)
		{
			Items.Remove(fw);
		}

		internal void BringWindowToFront(FloatWindow fw)
		{
			Items.Remove(fw);
			Items.Add(fw);
		}
	}
}
