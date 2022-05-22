using Serify;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.Title = "Serify Dev Console";
        if (args.Length == 0)
            Interface();
        else
            RunCommand(args);
    }

    private static void Interface()
    {
        Console.WriteLine("Serify BETA Dev Console (Copyright JarJS 2022-2022)");
        Console.WriteLine("Use \"help\" to view all commands\n");

        Consolo.NewLine(RunCommand);
    }

    public static void RunCommand(string[] args)
    {
        Console.WriteLine();

        if (args[0] == "help")
        {
            Commander.ShowCommands();
            return;
        }
        else if (args[0] == "clear")
        {
            Console.Clear();
            Interface();
            return;
        }
        else if(args[0] == "add" || args[0] == "a")
        {
            if(args.Length > 1)
                if (args[1] == "g" || args[1] == "global")
                {
                    if (args.Length < 3) { 
                        Consolo.Error("Dont Have Modules to Install");
                        return;
                    }
                    else
                        Commander.AddModule(Commander.GLOBAL_MODULE, args[2]);
                    return;
                }
                else
                {
                    if(args.Length > 2)
                    {
                        for(int i = 1;i < args.Length; i++)
                        {
                            Commander.AddModule(Commander.LOCAL_MODULE, args[i]);
                        }
                    }else
                        Commander.AddModule(Commander.LOCAL_MODULE, args[1]);
                    return;
                }
            Consolo.Error("Dont Have Modules to Install");
            return;
        }

        Consolo.Error("Command not found: " + args.ToList());
    }
}
