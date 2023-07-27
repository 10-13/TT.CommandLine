using System;
using System.Linq;
using System.Collections.Generic;

namespace TT.CommandLine;

public interface IHandler 
{       
	string Description { get; }  
	string[] ArgumentDescription { get; }
	int ArgumentsCount { get; }
	void Invoke(Root root,string[] args);
}	
