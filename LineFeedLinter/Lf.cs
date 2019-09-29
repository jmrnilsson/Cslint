using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LineFeedLinter
{
    class Lf : IChecker
    {
        public event ApplicationInformation Information;
        public event ApplicationInformation Error;
        public event ApplicationInformation Verbose;

        public void Check(string path)
        {
            string fileName = path.Replace("/", "\\");
            string content = File.ReadAllText(fileName);
            content = Regex.Replace(content, "\r\n", "_");
            if (Regex.IsMatch(content, "\n", RegexOptions.Multiline))
            {
                OnError($"EE\tMatches LF: {fileName}");
            }
            else
            {
                OnInformation($"||\tMatches CRLF: {fileName}");
            }
        }

        private void OnInformation(string information)
        {
            Information?.Invoke(information);
        }

        private void OnError(string information)
        {
            Error?.Invoke(information);
        }

        private void OnVerbose(string information)
        {
            Verbose?.Invoke(information);
        }
    }
}

