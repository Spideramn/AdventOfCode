using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days.Day16
{
	internal class Program : Day
	{
		private readonly Dictionary<string, Action<int[], int[]>> _operations = new Dictionary<string, Action<int[], int[]>>
		{
			{ "addr", (p,r) => r[p[3]] = r[p[1]] + r[p[2]] },
			{ "addi", (p,r) => r[p[3]] = r[p[1]] + p[2] },
			{ "mulr", (p,r) => r[p[3]] = r[p[1]] * r[p[2]] },
			{ "muli", (p,r) => r[p[3]] = r[p[1]] * p[2] },
			{ "banr", (p,r) => r[p[3]] = r[p[1]] & r[p[2]] },
			{ "bani", (p,r) => r[p[3]] = r[p[1]] & p[2] },
			{ "borr", (p,r) => r[p[3]] = r[p[1]] | r[p[2]] },
			{ "bori", (p,r) => r[p[3]] = r[p[1]] | p[2] },
			{ "setr", (p,r) => r[p[3]] = r[p[1]] },
			{ "seti", (p,r) => r[p[3]] = p[1] },
			{ "gtir", (p,r) => r[p[3]] = p[1]>r[p[2]]?1:0 },
			{ "gtri", (p,r) => r[p[3]] = r[p[1]]>p[2]?1:0 },
			{ "gtrr", (p,r) => r[p[3]] = r[p[1]]>r[p[2]]?1:0 },
			{ "eqir", (p,r) => r[p[3]] = p[1]==r[p[2]]?1:0 },
			{ "eqri", (p,r) => r[p[3]] = r[p[1]]==p[2]?1:0 },
			{ "eqrr", (p,r) => r[p[3]] = r[p[1]]==r[p[2]]?1:0 },
		};

		public override int DayNumber => 16;

		public override object RunPart1()
		{
			var counter = 0;
			var queue = new Queue<string>(GetInputLines());
			while (queue.Any())
			{
				var line = queue.Dequeue();
				if (string.IsNullOrEmpty(line))
					continue;

				if (line.StartsWith("Before"))
				{
					var line2 = queue.Dequeue();
					var line3 = queue.Dequeue();

					var register = line.Substring(9, 10).Split(", ").Select(int.Parse).ToArray();
					var parameters = line2.Split(' ').Select(int.Parse).ToArray();
					var result = line3.Substring(9, 10).Split(", ").Select(int.Parse).ToArray();

					// try all oppcodes and compare outputs
					var count = 0;
					foreach (var opp in _operations)
					{
						var output = new int[4];
						register.CopyTo(output, 0);
						opp.Value(parameters, output);

						if (output.SequenceEqual(result))
						{
							count++;
							if (count >= 3)
								break;
						}
					}
					if (count >= 3)
						counter++;
				}
			}
			return counter;
		}
	}
}