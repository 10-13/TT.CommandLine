using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using TT.CommandLine.Handlers;

namespace TT.CommandLine.CommandParsing
{
    public class HandlerParsingResault
    {
        public IHandler Handler { get; }
        public string[] Arguments { get; }
        public Root Owner { get; }

        public HandlerParsingResault(IHandler handler, IEnumerable<string> arguments, Root owner)
        {
            Handler = handler;
            Arguments = arguments.ToArray();
            Owner = owner;
        }

        public object[] ParseArguments()
        {
            Dictionary<string, List<string>> ArgumentsResault = new Dictionary<string, List<string>>();
            var push = (string Name, string Data) => 
            {
                if (!ArgumentsResault.ContainsKey(Name))
                    ArgumentsResault.Add(Name, new List<string>());
                ArgumentsResault[Name].Add(Data);
            };

            foreach(var arg in Arguments)
            {
                if(arg.Contains("::"))
                {
                    push(arg.Substring(0, arg.IndexOf("::")), arg.Substring(arg.IndexOf("::") + 2));
                }
                else
                {
                    push("Default", arg);
                }
            }

            var pars = Handler.GetMethodInfo().GetParameters();

            object[] res = new object[pars.Length];

            int defaultPos = 0;

            for(int i = 0;i < pars.Length;i++)
            {
                if (pars[i].ParameterType.IsArray)
                {
                    Type argType = pars[i].ParameterType.GetGenericArguments()[0];
                    var objects = Array.CreateInstance(argType.GetElementType(),0);
                    if (pars[i].GetCustomAttribute(typeof(ParamArrayAttribute)) != null)
                        objects = ArgumentsResault["Default"].Skip(defaultPos).Select((a) => Owner.Environment.FindParser(argType).Parse(a)).ToArray();
                    else if (ArgumentsResault.ContainsKey(pars[i].Name))
                        objects = ArgumentsResault[pars[i].Name].Select((a) => Owner.Environment.FindParser(argType).Parse(a)).ToArray();
                    else
                        objects = new object[1] { Owner.Environment.FindParser(argType).Parse(ArgumentsResault["Default"][defaultPos++]) };
                    
                    res[i] = objects;
                }
                else
                {
                    Type argType = pars[i].ParameterType;
                    if(ArgumentsResault.ContainsKey(argType.Name))
                        res[i] = Owner.Environment.FindParser(argType).Parse(ArgumentsResault[argType.Name][0]);
                    else
                        res[i] = Owner.Environment.FindParser(argType).Parse(ArgumentsResault["Default"][defaultPos++]);
                }
            }
            return res;

            
        }
    }
}
