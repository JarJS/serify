using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serify
{
    public static class Consolo
    {
        public static void Write(string text)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("L: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text);
        }

        public static void WriteLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("L: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
        }

        public static void Error(string err)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("E: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(err);
        }

        public static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("S: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
        }

        public static void Warning(string warn)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("W: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(warn);
        }

        public static void NewLine(Action<string[]> commandCallback)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("> ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("serify ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string command = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;


            if (command == "" || command == null)
                NewLine(commandCallback);
            else
            {
                string[] args = command.Split(" ");
                if (args[args.Length - 1] == "")
                {
                    Error("Dont have a command");
                }
                else
                    commandCallback(command.Split(" "));
                    NewLine(commandCallback);
            }
        }
    }
}
