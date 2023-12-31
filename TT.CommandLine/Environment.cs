﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace TT.CommandLine;

public sealed class Environment 
{         
	public List<Option> Options { get; } = new List<Option>();
	public List<IParser> Parsers { get; } = new List<IParser>();
	
	public Environment() { }
	
	public Option FindOption(string Name)
	{
		Option r = null;
		
		foreach(var opt in Options)
			if(opt.Name.StartsWith(Name))
				if(r == null)
					r = opt;
				else
					throw new Exception("Too short option name");
		
		return r;
	}
	public IParser FindParser(Type Type)
	{
		IParser r = null;
		
		foreach(var opt in Parsers)
			if(opt.TargetType == Type)
				return opt;
		
		throw new Exception("No converters for: " + Type.Name);
	}
	public object[] ParseArgumemts(string[] args,Type[] argTypes)
	{
		if(args.Length > argTypes.Length)
			throw new Exception("Too many arguments");
			
		object[] f = new object[args.Length];
		
		for(int i = 0;i < f.Length;i++)
			f[i] = FindParser(argTypes[i]).Parse(args[i]);
		
		return f;
	}
}
