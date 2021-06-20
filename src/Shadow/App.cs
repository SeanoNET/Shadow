using Konsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shadow
{
    class App
    {
        IConsole win;
        FileShadow fileShadow;
        public void Show()
        {
            string file = "current.log";
       
           fileShadow = new FileShadow(file);
           fileShadow.Read();

            //var myConsoleAppMainWindow = new Window();
   
            StatusBar();
            Console.BackgroundColor = ConsoleColor.Black;
            //Console.
            Thread thread = new Thread(() => WindowWatch());
            thread.Start();
        }

        int currHeight = 0;
        int currWidth = 0;
        private void WindowWatch()
        {
            currHeight = Console.WindowHeight;
            currWidth = Console.WindowWidth;

            while (true)
            {
                if(currHeight != Console.WindowHeight || currWidth != Console.WindowWidth)
                {
                    currHeight = Console.WindowHeight;
                    currWidth = Console.WindowWidth;
                    ReDraw();
                }
                Thread.Sleep(200);
            }
        }

        private void ReDraw()
        {
            List<string> consoleLines = new List<string>();
            // read 10 lines from the top of the console buffer
            foreach (string line in ConsoleReader.ReadFromBuffer(0, 0, (short)Console.BufferWidth, (short)Console.BufferHeight))
            {
                consoleLines.Add(line);
            }
            Console.Clear();
            foreach (string line in consoleLines)
            {
                Console.WriteLine(line);
                //consoleLines.Add(line);
            }
            //fileShadow.Read();
            StatusBar();
        }

        public void StatusBar()
        {
            Console.SetCursorPosition(0, Console.WindowHeight);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("test".PadRight(Console.WindowWidth));
            Console.ResetColor();
        }
    }
}
