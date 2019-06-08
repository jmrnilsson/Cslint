using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LineFeedLinter
{
    class Program
    {
        static List<string> errors = new List<string>();

        static void Main(string[] args)
        {
            var app = new LineFeedLinterApplication();
            app.InformationAppended += WriteLine;
            app.ErrorAppended += Error;
            var path = new Lazy<string>(() => app.GitTopLevel());

            //var mc = Regex.Match("path=(.*)+");
            //mc.Captures[]

            if (args.Contains("check") || args.Length < 1)
            {
                app.Check(path.Value);  
            }
            else if (args.Contains("fix"))
            {
                app.Replace(path.Value);
            }

            if (args.Contains("poop") && errors.Any())
            {
                Environment.Exit(22);
            }

             Console.WriteLine("Done!");
        }

        private static void WriteLine(string applicationInformation)
        {
            Console.WriteLine(applicationInformation);
        }

        private static void Error(string applicationError)
        {
            TextWriter errorWriter = Console.Error;
            errorWriter.WriteLine(applicationError);
            errors.Add(applicationError);
        }
    }
}
