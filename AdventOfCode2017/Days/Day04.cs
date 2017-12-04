using System;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day04 : Day
	{
		public Day04(string input)
			: base(04, input)
		{
		}

		public override object RunPart1()
		{
			return Input
				.Split(Environment.NewLine)
				.AsParallel()
				.Count(line =>
					!line
						.Split(' ')
						.GroupBy(w => w)
						.Any(g => g.Count() > 1)
				);
		}

		public override object RunPart2()
		{
			return Input
				.Split(Environment.NewLine)
				.AsParallel()
				.Count(line =>
					!line
						.Split(' ')
						.Select(w => new string(w.OrderBy(c => c).ToArray()))
						.GroupBy(w => w)
						.Any(g => g.Count() > 1)
				);
		}
	}
}
