using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace TT.CommandLine.Handlers;

public interface IHandler
{
    string Description { get; }
    string[] ArgumentDescription { get; }
    int ArgumentsCount { get; }
    void Invoke(Root root, string[] args);
    MethodInfo GetMethodInfo();
}
