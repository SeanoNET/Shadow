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
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using var watcher = new FileSystemWatcher(@"C:\Users\seanp\source\repos\Shadow");
            watcher.Changed += OnChanged;
            watcher.Filter = "default.log";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            //Read file at the start
            var fs = new FileStream(@"C:\Users\seanp\source\repos\Shadow\default.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);          
            using (fs)
            {
                var b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    Console.WriteLine(temp.GetString(b));
                }
                totalReadBytes = fs.Length;
            }
           
            Console.ReadLine();         
        }
        static long totalReadBytes = 0;
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            var fs = new FileStream(@"C:\Users\seanp\source\repos\Shadow\default.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            string content = string.Empty;
            using (fs)
            {
                var b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                fs.Position = totalReadBytes;
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    content = temp.GetString(b);
                }
                totalReadBytes = fs.Length;
            }
            Console.WriteLine(content);
            Console.WriteLine($"Has NewLine: {content.Contains(Environment.NewLine)}");
            Console.WriteLine($"Changed: {e.FullPath}");
            Console.WriteLine($"Buffer Size: {totalReadBytes.ToString()}");
        }
    }
}
