using System;
using System.Linq;
using System.Collections.Generic;
using TT.CommandLine.Handlers;

namespace TT.CommandLine.Options;

public sealed class DefaultOption : OptionBase
{
    public string Name { get; }

    public DefaultOption(string name, IHandler handler) : base(handler)
    {
        Name = name;
    }

    public override void Invoke(Root root, params string[] args) => Handler.Invoke(root, args);
}
