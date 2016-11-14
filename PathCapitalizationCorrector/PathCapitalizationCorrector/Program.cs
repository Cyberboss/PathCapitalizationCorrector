using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace PathCapitalizationCorrector
{
    class Program
    {
        //Stolen from Ants at http://stackoverflow.com/questions/478826/c-sharp-filepath-recasing
        //Thanks man
        static string GetProperDirectoryCapitalization(DirectoryInfo dirInfo)
        {
            DirectoryInfo parentDirInfo = dirInfo.Parent;
            if (null == parentDirInfo)
                return dirInfo.Name;
            return Path.Combine(GetProperDirectoryCapitalization(parentDirInfo),
                                parentDirInfo.GetDirectories(dirInfo.Name)[0].Name);
        }
        static string GetProperFilePathCapitalization(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            DirectoryInfo dirInfo = fileInfo.Directory;
            return Path.Combine(GetProperDirectoryCapitalization(dirInfo),
                                dirInfo.GetFiles(fileInfo.Name)[0].Name);
        }
        static void Main(string[] args)
        {
            if(args.Length == 1)
            {
                string stuff = null;
                try
                {
                    Console.WriteLine("Opening: " + args[0]);
                    stuff = File.ReadAllText(args[0]);
                }
                catch (Exception e){
                    Console.WriteLine("Failed: " + e.ToString());
                    return;
                }
                if (stuff != null)
                {
                    IDictionary<string, string> Mappings = new Dictionary<string, string>();
                    var startquote = -1;
                    for(var i = 0; i < stuff.Length; ++i)
                    {
                        if(stuff[i] == '"')
                        {
                            if (startquote != -1)
                                try
                                {
                                    var filename = stuff.Substring(startquote + 1, i - startquote - 1);
                                    if(File.Exists(filename))
                                        Mappings.Add(filename, GetProperFilePathCapitalization(Path.GetFullPath(filename)));
                                }
                                catch{ }
                            startquote = i;
                        }
                    }
                    foreach (var m in Mappings)
                    {
                        stuff = stuff.Replace(m.Key, m.Value);
                        Console.WriteLine("Replaced: " + m.Key + " with " + m.Value);
                    }

                    File.WriteAllText(args[0], stuff);
                    return;
                }
            }
            Console.WriteLine("Usage: PathCaptializationCorrector <target file>");
        }
    }
}
