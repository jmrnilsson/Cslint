using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LineFeedLinter
{
    public class Program
    {
        static ArgsParser argsParser = new ArgsParser();
        static List<string> errors = new List<string>();
        static Lint lint = new Lint();
        static GitProcess gitProcess = new GitProcess();

        public static void Main(string[] args)
        {
            DirectoryInfo workingDirectory = null;
            try
            {
                argsParser.Parse(args);
            }
            catch (ArgumentNullException)
            {
                workingDirectory = new DirectoryInfo(gitProcess.TopLevel());
            }

            lint.Information += WriteLine;
            lint.Error += Error;

            if (args.Contains("check") || args.Contains("poop") || args.Length < 1)
            {
                lint.Check(workingDirectory.FullName);
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
