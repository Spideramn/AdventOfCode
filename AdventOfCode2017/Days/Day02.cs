using System;
using System.Globalization;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day02 : Day
	{
		public Day02(string input)
			: base(02, input)
		{
		}

		public override object RunPart1()
		{
			return Input
				.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split('\t').Select(int.Parse).ToList())
				.Select(numbers => numbers.Max() - numbers.Min())
				.Sum();
		}

		public override object RunPart2()
		{
			var checksum = 0;
			var rows = Input
				.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split('\t').Select(int.Parse).ToList());
			foreach (var row in rows)
			{
				foreach (var number in row)
				{
					var found = false;
					foreach (var number2 in row)
					{
						if (number != number2 && number % number2 == 0)
						{
							checksum += number / number2;
							found = true;
							break;
						}
					}
					if (found)
						break;
				}
			}
			return checksum;
		}
	}
}
