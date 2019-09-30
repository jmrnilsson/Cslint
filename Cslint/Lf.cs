using System.IO;
using System.Text.RegularExpressions;

namespace Cslint
{
	class Lf : CheckerBase, IChecker
	{
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

	}
}

