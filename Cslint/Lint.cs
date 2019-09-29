using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Cslint
{
    public class Lint : CheckerBase, IChecker
    {
        IChecker lf = new Lf();
        IChecker utf8 = new Utf8Bom();

        public void Check(string workingDirectory)
        {
            lf.Information += OnInformation;
            lf.Error += OnError;
            lf.Verbose += OnVerbose;
            utf8.Information += OnInformation;
            utf8.Error += OnError;
            utf8.Verbose += OnVerbose;

            var csFiles = new List<string>();
            GetCsFiles(workingDirectory, ref csFiles);

            foreach(string file in csFiles)
            {
                lf.Check(file);
                utf8.Check(file);
            }
        }

        private void GetCsFiles(string fullName, ref List<string> csFiles)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(fullName))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        if (Regex.IsMatch(f, "\\.cs$"))
                        {
                            csFiles.Add(f);
                        }
                    }
                    GetCsFiles(d, ref csFiles);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
