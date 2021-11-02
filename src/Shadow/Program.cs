using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Shadow
{
    class Program
    {
        static string logfile = "/home/seano/source/Shadow/log.txt";
        static string logLocation = "/home/seano/source/Shadow";

        static void Main(string[] args)
        {
          

            using var watcher = new FileSystemWatcher(logLocation);
            watcher.Changed += OnChanged;
            watcher.Filter = "log.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            //Read file at the start
            var fs = new FileStream(logfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);          
            using (fs)
            {
                var b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    //Console.WriteLine(temp.GetString(b));
                }
                totalReadBytes = fs.Length;
            }
           
            Console.ReadLine();         
        }
        static long totalReadBytes = 0;
        static long maxBufferLength = 1024;
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            var fs = new FileStream(logfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            string content = string.Empty;
            using (fs)
            {
                //totalReadBytes = fs.Length - 100;
                var b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                fs.Position = totalReadBytes;
                //fs.Seek(totalReadBytes, SeekOrigin.End);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    content = temp.GetString(b);
                }
                totalReadBytes = fs.Length;
            }
            PrintLog(content);
            Console.WriteLine($"Has NewLine: {content.Contains(Environment.NewLine)}");
            Console.WriteLine($"Changed: {e.FullPath}");
            Console.WriteLine($"File Size: {totalReadBytes.ToString()}");
        }

        private static void PrintLog(string content)
        {
            //If the string in content contains INF print with green color
            if (content.Contains("INF"))
            {
                var existingColour = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(content);
                Console.ForegroundColor = existingColour;
            }
           
        }
    }
}
