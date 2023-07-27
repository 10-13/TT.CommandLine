using System;
using System.Linq;
using System.Collections.Generic;
using TT.CommandLine.Hadlers;
using System.Reflection;

namespace TT.CommandLine.Hadlers;

public class ReflectionHandler : IHandler
{     
	public string Description { get; set; } = null;
	public string[] ArgumentDescription { get; set; } = null;
	public int ArgumentsCount { get; private set; }
	
	private Delegate _func = null;
	private object _obj = null;
		 
	public ReflectionHandler(Delegate function,object obj = null)
	{
		if(Description == null)
			Description = function.Method.Name;
		if(ArgumentDescription == null)
		{
			ArgumentDescription = function.Method.GetParameters().Select<ParameterInfo,string>((arg)=>arg.Name.PadRight(10,' ')+arg.ParameterType.Name.PadRight(10,' ')+(arg.IsOptional?"[OPTIONAL] ":"")+(arg.HasDefaultValue?"[DEFAULT_VALUE] ":"")).ToArray();
		}
		ArgumentsCount = function.Method.GetParameters().Where((a) => !a.HasDefaultValue).Count();
		_func = function;
		_obj = obj;
	}
	
	public void Invoke(Root root,string[] args)
	{
		ParameterInfo[] pars = _func.Method.GetParameters();
		object[] _args = new object[pars.Length];
		root.Environment.ParseArgumemts(args,pars.Select((a)=>a.ParameterType).ToArray()).CopyTo(_args,0);
		for(int i = 0;i < _args.Length;i++)
			if(_args[i] == null)
				if(pars[i].HasDefaultValue)
					_args[i] = pars[i].DefaultValue;
				else
					throw new Exception("Too few arguments given");
		_func.DynamicInvoke(_args);
	}
}
