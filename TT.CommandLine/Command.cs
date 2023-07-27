using System;
using System.Linq;
using System.Collections.Generic;

namespace TT.CommandLine;

public sealed class Command 
{     
	private Command _parent = null;
	private Root _root = null;
	
	public string Name { get; }    
    public Root Root 
	{ 
		get
		{
			return _root;
		}
		set
		{
			_parent = null;
			_root = value;
			_root.RootCommand = this;
		}
	}
	public Command ParentCommand
	{ 
		get
		{
			return _parent;
		}
		set
		{
			_parent = value;
			_root = value.Root;
		}
	}
	public List<Command> ChildCommands { get; set; } = new List<Command>();
	public IHandler Handler { get; set; } = null;
	
	public bool IsCommandNode { get => ChildCommands != null && ChildCommands.Count > 0; }
	public bool IsInvokeble { get => Handler != null; }
	
	public Command(string Name) { this.Name = Name; }
	
	public void Invoke(string[] args)
	{
		if(!IsInvokeble)
			throw new Exception("This command can't be invoked");
		if(_root == null)
			throw new Exception("This command didn't connected to any root");
			
		Handler.Invoke(_root,args);
	}
	
	public Command GetCommand(List<string> Path, int thisIndex)
	{
		if(thisIndex == Path.Count)
			return this;
			
		var a = ChildCommands.First((a)=>a.Name == Path[thisIndex]);
		if(a == null)
			throw new Exception("Incorrect path exception");
			
		return a.GetCommand(Path,thisIndex + 1);
	}
	public void AddChild(Command com)
	{
		com.ParentCommand = this;
		ChildCommands.Add(com);
	}
}
