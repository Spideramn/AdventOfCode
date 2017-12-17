using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2017.Days
{
	internal class Day15 : Day
	{
		public Day15(string input)
			: base(15, input)
		{
		}

		public override object RunPart1()
		{
			var parts = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();

			var startA = long.Parse(parts[0].Last());
			var startB = long.Parse(parts[1].Last());
			var count = 0;
			for (var i = 0; i < 40000000; i++)
			{
				startA = (startA * 16807L) % 2147483647L;
				startB = (startB * 48271L) % 2147483647L;
				if ((startA & 0xFFFF) == (startB & 0xFFFF))
					count++;
			}
			return count;
		}

		public override object RunPart2()
		{
			var parts = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();

			var startA = long.Parse(parts[0].Last());
			var startB = long.Parse(parts[1].Last());
			var count = 0;
			for (var i = 0; i < 5000000; i++)
			{
				do
				{
					startA = (startA * 16807L) % 2147483647L;
				} while (startA %4 != 0);

				do
				{
					startB = (startB * 48271L) % 2147483647L;
				} while (startB % 8 != 0L);

				if ((startA & 0xFFFF) == (startB & 0xFFFF))
					count++;
			}
			return count;
		}
	}
}
