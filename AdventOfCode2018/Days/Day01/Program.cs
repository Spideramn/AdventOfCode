using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days.Day01
{
	internal class Program : Day
	{
		public override int DayNumber => 1;
		public override object RunPart1()
		{
			return GetInputLines().Select(int.Parse).Sum();
		}

		public override object RunPart2()
		{
			var currentFrequency = 0;
			var frequencies = new HashSet<int> {currentFrequency};

			var lines = GetInputLines().Select(int.Parse).ToList();
			while (true)
			{
				foreach (var line in lines)
				{
					currentFrequency += line;
					if (!frequencies.Add(currentFrequency))
						return currentFrequency;
				}
			}
		}
	}
}