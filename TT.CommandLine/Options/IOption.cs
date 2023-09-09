using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.CommandLine.Handlers;

namespace TT.CommandLine.Options
{
    public abstract class OptionBase : IHandler
    {
        public IHandler Handler { get; }

        public string Description { get => Handler.Description; }
        public string[] ArgumentDescription { get => Handler.ArgumentDescription; }
        public int ArgumentsCount { get => Handler.ArgumentsCount; }

        public OptionBase(IHandler handler)
        {
            Handler = handler;
        }

        public abstract void Invoke(Root root, params string[] args);
    }
}
