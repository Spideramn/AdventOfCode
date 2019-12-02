using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AdventOfCode2019.Days
{
	internal abstract class Day
	{
		public abstract int DayNumber { get; }

		public virtual object RunPart1() => "NOTHING";
		public virtual object RunPart2() => "NOTHING";

		private static Assembly Assembly => typeof(Day).GetTypeInfo().Assembly;

		protected string GetInputString(string debugOverride = null)
		{
			if (debugOverride != null)
				return debugOverride;

			using (var inputStream = Assembly.GetManifestResourceStream($"AdventOfCode2019.Days.Day{DayNumber:00}.Input.txt"))
			using (TextReader txtReader = new StreamReader(inputStream))
				return txtReader.ReadToEnd();
		}
		protected IEnumerable<string> GetInputLines(string debugOverride = null)
		{
			if (debugOverride != null)
			{
				foreach (var line in debugOverride.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
					yield return line;
				yield break;
			}

			using(var inputStream = Assembly.GetManifestResourceStream($"AdventOfCode2019.Days.Day{DayNumber:00}.Input.txt"))
			using (TextReader txtReader = new StreamReader(inputStream))
			{
				string line;
				while ((line = txtReader.ReadLine()) != null)
					yield return line;
			}
		}
	}
}
