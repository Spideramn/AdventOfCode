using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AdventOfCode2018.Days
{
	internal abstract class Day
	{
		public abstract int DayNumber { get; }

		public virtual object RunPart1() => "NOTHING";
		public virtual object RunPart2() => "NOTHING";

		protected Stream GetInputStream()
		{
			return typeof(Day).GetTypeInfo().Assembly.GetManifestResourceStream($"AdventOfCode2018.Days.Day{DayNumber:00}.Input.txt");
		}
		protected string GetInputString()
		{
			using (var inputStream = GetInputStream())
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

			using(var inputStream = GetInputStream())
			using (TextReader txtReader = new StreamReader(inputStream))
			{
				string line;
				while ((line = txtReader.ReadLine()) != null)
					yield return line;
			}
		}
	}
}
