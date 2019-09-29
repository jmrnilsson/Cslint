using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LineFeedLinter
{
    public class ArgsParser
    {
        public DirectoryInfo Parse(string[] kwargs)
        {
            DirectoryInfo workingDirectory = null;
            
            foreach (string kwarg in kwargs)
            {
                if (Regex.Match(kwarg, "^(poop|check)$").Success)
                {
                    continue;
                }

                Match path = Regex.Match(kwarg, "(?<=(path=))(.*)+");

                if (path.Success)
                {
                    if (!Directory.Exists(path.Value))
                    {
                        throw new ArgumentOutOfRangeException($"{path.Value} doesn't exist.");
                    }
                    workingDirectory = new DirectoryInfo(path.Value);
                    continue;
                }

                throw new ArgumentException($"{kwarg} is an unrecognized argument");
            }

            const string noWorkingDirectoryMessage = "No arguments assigned a working directory";
            return workingDirectory ?? throw new ArgumentNullException(noWorkingDirectoryMessage);
        }
    }
}
