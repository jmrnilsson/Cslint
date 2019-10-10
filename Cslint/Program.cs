using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cslint
{
	public delegate void ApplicationInformation(string information);

	public class Program
	{
		static ArgsParser argsParser = new ArgsParser();
		static List<string> errors = new List<string>();
		static Lint lint = new Lint();
		static GitProcess gitProcess = new GitProcess();

		/// <summary>
		/// Can be invoked with 'check' or 'poop' to exit with failure code. Path is assigned by
		/// adding the switch path=C:\somedevelopment\somework.
		/// Example:
		///     - dotnet run poop path=C:\somedevelopment\somework
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			DirectoryInfo workingDirectory = null;

			try
			{
				workingDirectory = argsParser.Parse(args);
			}
			catch (ArgumentNullException)
			{
				workingDirectory = new DirectoryInfo(gitProcess.TopLevel());
			}

			lint.Information += Info;
			lint.Verbose += Verbose;
			lint.Error += Error;

			try
			{
				if (workingDirectory != null)
				{
					lint.Check(workingDirectory);
				}
			}
			catch (Exception e)
			{
				Error(e.ToString());
			}

			if (errors.Any())
			{
				Console.WriteLine();

				var sb = new StringBuilder();
				foreach(string err in errors)
				{
					sb.AppendLine(err);
				}

				Console.Error.WriteLine(sb.ToString());

				Environment.Exit(999);
			}

			Console.WriteLine("Done!");
		}

		private static void ConsoleWrite(string kind, string message, Action continueWith)
		{
			var errors = message.Split(": ", StringSplitOptions.None);
			var args = new object[] { kind.PadRight(15, ' '), errors[0].PadRight(20, ' '), errors[1] };
			Console.WriteLine("{0}{1}{2}", args);
		}

		private static void Info(string info)
		{
			Console.Write(".");
			// ConsoleWrite("Information:", info,  () => {});
		}

		private static void Verbose(string verbose)
		{
			// ConsoleWrite("Verbose:", verbose,  () => {});
		}

		private static void Error(string error)
		{
			Console.Write("E");
			// ConsoleWrite("Error:", error, () => errors.Add(error));
		}
	}
}
