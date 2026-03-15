using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Visual.Tree.Threading
{
	public enum WorkItemStatus 
	{ 
		Completed, 
		Queued, 
		Executing, 
		Aborted 
	}
}
