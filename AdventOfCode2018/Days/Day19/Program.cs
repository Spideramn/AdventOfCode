using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days.Day19
{
	internal class Program : Day
	{
		public override int DayNumber => 19;

		private readonly Dictionary<string, Func<int[], int, int, int>> _operations = new Dictionary<string, Func<int[], int, int, int>>
		{
			{"addr", (r, a, b) => r[a] + r[b]},
			{"addi", (r, a, b) => r[a] + b},
			{"mulr", (r, a, b) => r[a] * r[b]},
			{"muli", (r, a, b) => r[a] * b},
			{"banr", (r, a, b) => r[a] & r[b]},
			{"bani", (r, a, b) => r[a] & b},
			{"borr", (r, a, b) => r[a] | r[b]},
			{"bori", (r, a, b) => r[a] | b},
			{"setr", (r, a, b) => r[a]},
			{"seti", (r, a, b) => a},
			{"gtir", (r, a, b) => a > r[b] ? 1 : 0},
			{"gtri", (r, a, b) => r[a] > b ? 1 : 0},
			{"gtrr", (r, a, b) => r[a] > r[b] ? 1 : 0},
			{"eqir", (r, a, b) => a == r[b] ? 1 : 0},
			{"eqri", (r, a, b) => r[a] == b ? 1 : 0},
			{"eqrr", (r, a, b) => r[a] == r[b] ? 1 : 0}
		};
		
		public override object RunPart1()
		{
			var ipRegister = 0;
			var instructions = new List<(string, int, int, int)>();
			foreach (var line in GetInputLines())
			{
				if (line.StartsWith('#'))
				{
					ipRegister = int.Parse(line.Substring(4));
				}
				else
				{
					var parts = line.Split(' ');
					instructions.Add((parts[0], int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3])));
				}
			}


			var register = new int[6];
			while (register[ipRegister] < instructions.Count)
			{
				var instruction = instructions[register[ipRegister]];
				register[instruction.Item4] = _operations[instruction.Item1](register, instruction.Item2, instruction.Item3);
				register[ipRegister]++;
			}

			return register[0];
		}

		public override object RunPart2()
		{
			/*
			long r0 = 1;
			long r1 = 911;
			if (r0 == 1)
			{
				r1 = 10551311;
				r0 = 0;
			}
			
			for (long r2 = 1; r2 <= r1; r2++)
			{
				for (long r5 = 1; r5 <= r1; r5++)
				{
					if (r2 * r5 == r1)
					{
						Console.WriteLine($"{r2} x {r5} == {r1}");
						r0 += r2;
					}
				}
			}
			return r0;
			*/

			// after running for some time, this was the output:
			// 1 x 10551311 == 10551311
			// 431 x 24481 == 10551311
			// 24481 x 431 == 10551311
			// So the solution should be 1 + 431 + 24481 + 10551311
			// these are all the "divisors" of the number 10551311
			// https://www.wolframalpha.com/input/?i=Sum+of+Divisors+of+10551311
			// "bruteforce":
			return Enumerable.Range(1, 10551311).Where(a => 10551311 % a == 0).Sum();
		}
	}
}