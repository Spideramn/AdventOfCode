using System;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day05 : Day
	{
		public Day05(string input)
			: base(05, input)
		{
		}

		public override object RunPart1()
		{
			var steps = 0;
			var pos = 0;
			var instructions = Input.Split(Environment.NewLine).Select(int.Parse).ToArray();
			while (pos < instructions.Length)
			{
				steps++;
				pos += instructions[pos]++;
			}
			return steps;
		}

		public override object RunPart2()
		{
			var steps = 0;
			var pos = 0;
			var instructions = Input.Split(Environment.NewLine).Select(int.Parse).ToArray();
			while (pos < instructions.Length)
			{
				steps++;
				if (instructions[pos] >= 3)
					pos += instructions[pos]--;
				else
					pos += instructions[pos]++;
			}
			return steps;
		}
	}
}
