using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace LineFeedLinter
{
    public class GitProcess
    {
        public string TopLevel()
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
