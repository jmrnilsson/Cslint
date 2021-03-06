﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

			lint.Information += WriteLine;
			// lint.Verbose += WriteLine;
			lint.Error += Error;

			try
			{
				if (workingDirectory != null)
				{
					lint.Check(workingDirectory);
				}

				if (args.Contains("poop") && errors.Any())
				{
					Environment.Exit(22);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("ERROR: " + e);
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
