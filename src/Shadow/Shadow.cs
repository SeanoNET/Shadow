
using System;
using System.IO;

public class FileShadow
{
    public readonly string FileName;
    
    public readonly string FilePath;
    
    public FileShadow(string fileName, string filePath)
    {
        this.FileName = fileName;
        this.FilePath = filePath;
    }
        
    //Read last line of file and output to console window
    //when the file is updated append output to the console window
    public void Watch()
    {
        string line = "";
        string lastLine = "";
        FileSystemWatcher watcher = new FileSystemWatcher();
        watcher.Path = ".";
        watcher.NotifyFilter = NotifyFilters.LastWrite;
        watcher.Filter = FileName;
        watcher.Changed += new FileSystemEventHandler(OnChanged);
        watcher.EnableRaisingEvents = true;
        using (StreamReader sr = new StreamReader(FilePath + FileName))
        {
            line = sr.ReadLine();
            lastLine = line;
        }
        while (true)
        {
            using (StreamReader sr = new StreamReader(FileName))
            {
                line = sr.ReadLine();
                if (line != lastLine)
                {
                    Console.WriteLine(line);
                    lastLine = line;
                }
            }
        }
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        throw new NotImplementedException();
    }
}