using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadow
{
    internal class FileShadow
    {
        string path = @"C:\Temp\";
        string file = "";
        FileSystemWatcher watcher;
        public FileShadow(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                throw new ArgumentException($"'{nameof(file)}' cannot be null or empty.", nameof(file));
            }

            this.file = file;
        }
        public void Read()
        {
            watcher = new FileSystemWatcher(path);
            watcher.Changed += OnChanged;
            watcher.Filter = file;
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            //File to screen based on buffer size starting at the end of the file
            var fs = new FileStream(path + file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (fs)
            {
                var b = new byte[1024];
                if (fs.Length > b.Length)
                {
                    fs.Seek(-b.Length, SeekOrigin.End);
                }
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    Console.WriteLine(temp.GetString(b));
                    //editor.Redraw(new Rect(0, 0, 10, 10));
                }
                totalReadBytes = fs.Length;
            }
        }


        private long totalReadBytes = 0;
        private int lineCount;
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            var fs = new FileStream(path + file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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

            foreach (var line in content.Split(Environment.NewLine))
            {
                lineCount++;
                Console.WriteLine($"[{lineCount.ToString()}] {line}");
            }
            Console.WriteLine($"Has NewLine: {content.Contains(Environment.NewLine)}");
            Console.WriteLine($"Changed: {e.FullPath}");
            Console.WriteLine($"Buffer Size: {totalReadBytes.ToString()}");
        }
    }
}
