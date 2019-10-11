using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cslint
{
	public class Utf8Bom : IChecker
	{
		public event ApplicationInformation Information;
		public event ApplicationInformation Error;
		public event ApplicationInformation Verbose;

		public void Check(FileInfo file)
		{
			string csFile = file.FullName;
			byte[] bytes = File.ReadAllBytes(csFile);
			UTF8Encoding enc = new UTF8Encoding(true);
			byte[] preamble = enc.GetPreamble();
			if (preamble.Where((p, i) => bytes.Length > i && p != bytes[i]).Any())
			{
				OnError($"No UTF8-BOM: {csFile}");
			}
			else
			{
				OnInformation($"UTF8-BOM found: {csFile}");
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
