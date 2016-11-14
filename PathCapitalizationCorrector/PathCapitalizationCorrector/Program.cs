using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace PathCapitalizationCorrector
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 1)
            {
                string stuff = null;
                try
                {
                    stuff = File.ReadAllText(args[0]);
                }
                catch { }
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
                                    var filename = stuff.Substring(startquote, i - startquote);
                                    if(File.Exists(filename))
                                        Mappings.Add(filename, Path.GetFullPath(filename));
                                }
                                catch{ }
                            startquote = i;
                        }
                    }
                    foreach(var m in Mappings)
                        stuff.Replace(m.Key, m.Value);

                    File.WriteAllText(args[0], stuff);
                }
            }
            Console.WriteLine("Usage: PathCaptializationCorrector <target file>");
        }
    }
}
