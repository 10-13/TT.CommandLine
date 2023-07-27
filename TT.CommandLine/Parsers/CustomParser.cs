using System;
using System.Linq;
using System.Collections.Generic;

namespace TT.CommandLine.Parsers;

public class CustomParser : IParser
{         
	public Type TargetType { get; }
	
	private Func<string,object> _convert = null;
	
	public CustomParser(Type targetType,Func<string,object> convert)
	{
		TargetType = targetType;
		_convert = convert;
	}
	
	public object Parse(string str) => _convert(str);
}
