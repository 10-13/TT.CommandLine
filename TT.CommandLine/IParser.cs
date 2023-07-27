using System;
using System.Linq;
using System.Collections.Generic;

namespace TT.CommandLine;

public interface IParser
{         
	Type TargetType { get; }  
	object Parse(string str);
}
