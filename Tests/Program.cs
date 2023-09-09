using TT.CommandLine;
using TT.CommandLine.Options;
using TT.CommandLine.Parsers;

using RHandler = TT.CommandLine.Handlers.ReflectionHandler;

Root root = new Root();

CustomParser strParser = new CustomParser(typeof(string), (a) => a);
root.Environment.Parsers.Add(strParser);
CustomParser intParser = new CustomParser(typeof(int), (a)=>int.Parse(a));
root.Environment.Parsers.Add(intParser);


DefaultOption printStr = new DefaultOption("p_str", new RHandler((string str) => Console.WriteLine("[PRINT STR OUT]:\n" + str)));
root.Environment.Options.Add(printStr);


Command rootCom = new Command("root");
rootCom.Root = root;

Command mathCom = new Command("math");
rootCom.AddChild(mathCom);

Command mathAddCom = new Command("add") 
{ 
    Handler = new RHandler((int a, int b) => Console.WriteLine(a + " + " + b + " = " + (a + b))) 
};
mathCom.AddChild(mathAddCom);

Command mathSubCom = new Command("sub") 
{ 
    Handler = new RHandler((int a, int b) => Console.WriteLine(a + " - " + b + " = " + (a - b))) 
};
mathCom.AddChild(mathSubCom);



bool run = true;

Command endCom = new Command("end")
{
    Handler = new RHandler(() => run = false)
};
rootCom.AddChild(endCom);

while(run)
{
    Exception ex = root.SafeInvoke(Console.ReadLine());
    if(ex!=null)
    {
        Console.WriteLine(ex.Message);
    }
    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}
