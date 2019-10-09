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
			if (preamble.Where((p, i) => p != bytes[i]).Any())
			{
				OnError($"EE\tNo UTF8 BOM: {csFile}");
			}
			else
			{
				OnInformation($"||\tUTF8 BOM OK: {csFile}");
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
