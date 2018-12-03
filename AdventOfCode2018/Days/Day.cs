using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AdventOfCode2018.Days
{
	internal abstract class Day
	{
		public abstract int DayNumber { get; }

		public virtual object RunPart1() => -1;
		public virtual object RunPart2() => -1;

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
		protected IEnumerable<string> GetInputLines()
		{
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
