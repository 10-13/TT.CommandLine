using System;
using System.Linq;
using System.Collections.Generic;
using TT.CommandLine;
using System.Reflection.Metadata;

namespace TT.CommandLine;

public sealed class Root 
{         
	public Environment Environment { get; private set; } = new Environment();
	public Command RootCommand { get; set; }
	
	public Root() { }
	
	public void Invoke(string source)
	{
		if(RootCommand == null)
			throw new Exception("Root command error");
		
		List<string> sourceStack = new List<string>(100);
		bool stack = false;
		foreach(var s in source.Split(' ').Where((a)=>a.Length > 0))
		{
			if(s == "--")
			{
				sourceStack.Add("--");
				stack = false;
			}	
			else if(s.StartsWith("\"") && s.EndsWith("\""))
			{
				sourceStack.Add(s.Substring(1,s.Length - 2));
			}
			else if(s.StartsWith("\""))
			{
				stack = true;
				sourceStack.Add(s.Substring(1));
			}
			else if(s.EndsWith("\"") && stack)
			{
				stack = false;
				sourceStack[sourceStack.Count - 1] += " " + s.Substring(0,s.Length - 1);
			}
			else if(stack)
			{
				sourceStack[sourceStack.Count - 1] += " " + s;
			}
			else
			{
				sourceStack.Add(s);
			}
		}
		int dIndex = sourceStack.Count;

        if (sourceStack.Contains("--"))
			dIndex = sourceStack.IndexOf("--");
		List<string> commandStack = new List<string>(5);
		
		int optArgs = 0;
		List<string> args = new List<string>(3);
		Option opt = null;
		for(int i = 0;i < dIndex;i++)
		{
			if(sourceStack[i].StartsWith("-"))
				if(optArgs <= 0)
				{
					opt = Environment.FindOption(sourceStack[i].Substring(1));
					optArgs = opt.ArgumentsCount;
					continue;
				}
				else
					throw new Exception("Skiped some option arguments");
			
			if(optArgs <= 0)
				commandStack.Add(sourceStack[i]);
			else
				args.Add(sourceStack[i]);
			
			optArgs--;

			if (optArgs == 0)
			{
				opt.Invoke(this, args.ToArray());
				opt = null;
                args = new List<string>();
            }
		}
		
		if(opt != null)
			opt.Invoke(this,args.ToArray());

		Command com = RootCommand.GetCommand(commandStack,1);
		
		com.Invoke(sourceStack.Skip(dIndex + 1).ToArray());
	}
	public Exception SafeInvoke(string source)
	{
		try
		{
			Invoke(source);
			return null;
		}
		catch(Exception ex)
		{
			return ex;
		}
	}
}
