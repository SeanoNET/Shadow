using System;
using Serilog;

namespace logger
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File("../log.txt",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

            while (true)
            {
                //If ctrl + c is pressed, exit the loop
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.C && key.Modifiers == ConsoleModifiers.Control)
                    {
                        break;
                    }
                }

                //print to the console the current time
                Log.Information(DateTime.Now.ToString("HH:mm:ss"));
                //Sleep for between 1 and 3 seconds
                int sleepTime = new Random().Next(1, 3);
                System.Threading.Thread.Sleep(sleepTime * 1000);


            }
            // Finally, once just before the application exits...
            Log.CloseAndFlush();

        }
    }
}
