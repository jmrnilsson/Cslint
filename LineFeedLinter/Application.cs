using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LineFeedLinter
{
    public delegate void ApplicationInformation(string information);

    public class LineFeedLinterApplication
    {
        public event ApplicationInformation InformationAppended;
        public event ApplicationInformation ErrorAppended;
        public void Check(string path)
        {
            AppendInformationOrError($"||\tAssuming top level: {path}");
            var csFiles = new List<string>();
            GetCsFiles(path, ref csFiles);
            AppendInformationOrError($"||\tAssuming files: {csFiles}");
            foreach (string csFile in csFiles)
            {
                string fileName = csFile.Replace("/", "\\");
                string content = File.ReadAllText(fileName);
                content = Regex.Replace(content, "\r\n", "_");
                if (Regex.IsMatch(content, "\n", RegexOptions.Multiline))
                {
                    AppendInformationOrError($"EE\tMatches LF: {fileName}");
                }
                else
                {
                    AppendInformationOrError($"||\tMatches CRLF: {fileName}", true);
                }
            }
        }

        private void AppendInformationOrError(string information, bool error = false)
        {
            if (!error)
            {
                InformationAppended?.Invoke(information);
            }
            ErrorAppended?.Invoke(information);
        }

        internal void Replace(string value)
        {
            throw new NotImplementedException();
        }

        private void GetCsFiles(string path, ref List<string> csFiles)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(path))
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

        public string GitTopLevel()
        {
            return GitCommand("rev-parse --show-toplevel");
        }

        private string GitCommand(string command)
        {
            var stderr = new StringBuilder();
            var stdout = new StringBuilder();
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "git";
                startInfo.Arguments = command;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;

                using (Process process = Process.Start(startInfo))
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        string line = process.StandardOutput.ReadLine();
                        stdout.AppendLine(line);
                        Console.WriteLine(line);
                    }

                    while (!process.StandardError.EndOfStream)
                    {
                        string line = process.StandardError.ReadLine();
                        stderr.AppendLine(line);
                        Console.WriteLine(line);
                    }

                    process.WaitForExit();
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }

            string errors = stderr.ToString();
            if (!string.IsNullOrWhiteSpace(errors))
            {
                if (Regex.IsMatch(errors, "not a git repository "))
                {
                    errors = "Path specified is not a git repository";
                }

                throw new ApplicationException(errors);
            }

            return stdout.ToString().Trim();
        }
    }
}
