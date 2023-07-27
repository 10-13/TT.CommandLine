using System;
using System.Linq;
using System.Collections.Generic;

namespace TT.CommandLine;

public sealed class Option : IHandler
{         
	public string Name { get; }
	public IHandler Handler { get; set; } = null;
	
	public string Description { get => Handler.Description; }  
	public string[] ArgumentDescription { get => Handler.ArgumentDescription; }
	public int ArgumentsCount { get => Handler.ArgumentsCount; }
	
	public Option(string name)
	{
		Name = name;
	}
	
	public void Invoke(Root root,params string[] args) => Handler.Invoke(root,args);
}
