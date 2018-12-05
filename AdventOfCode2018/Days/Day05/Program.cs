using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.Days.Day05
{
	internal class Program : Day
	{
		public override int DayNumber => 5;

		public override object RunPart1()
		{
			return React(new StringBuilder(GetInputString()));
		}
		
		public override object RunPart2()
		{
			var input = GetInputString();
			var lengths = new ConcurrentBag<int>();
			Enumerable.Range('a', 26).AsParallel().ForAll(c => {
				var line = new StringBuilder(input.Length);
				foreach (var c2 in input)
				{
					if (c != c2 && c != c2 + 32)
						line.Append(c2);
				}
				lengths.Add(React(line));
			});
			return lengths.Min();
		}

		private static int React(StringBuilder input)
		{
			for (var i = 0; i < input.Length - 1;)
			{
				if (Math.Abs(input[i] - input[i + 1]) == 32)
				{
					input.Remove(i, 2);
					if (--i < 0) i = 0;
				}
				else
				{
					i++;
				}
			}
			return input.Length;
		}
	}
}