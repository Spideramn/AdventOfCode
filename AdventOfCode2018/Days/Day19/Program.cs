using System;
using System.Collections.Generic;

namespace AdventOfCode2018.Days.Day19
{
	internal class Program : Day
	{
		public override int DayNumber => 19;

		private readonly Dictionary<string, Func<int[], int, int, int>> _operations = new Dictionary<string, Func<int[], int, int, int>>
		{
			{ "addr", (r,a,b) => r[a] + r[b] },
			{ "addi", (r,a,b) => r[a] + b },
			{ "mulr", (r,a,b) => r[a] * r[b] }, // ?
			{ "muli", (r,a,b) => r[a] * b }, // ?
			{ "banr", (r,a,b) => r[a] & r[b] },
			{ "bani", (r,a,b) => r[a] & b },
			{ "borr", (r,a,b) => r[a] | r[b] },
			{ "bori", (r,a,b) => r[a] | b },
			{ "setr", (r,a,b) => r[a] },
			{ "seti", (r,a,b) => a },
			{ "gtir", (r,a,b) => a > r[b] ? 1 : 0 },
			{ "gtri", (r,a,b) => r[a] > b ? 1 : 0 },
			{ "gtrr", (r,a,b) => r[a] > r[b] ? 1 : 0 },
			{ "eqir", (r,a,b) => a == r[b] ? 1 : 0 },
			{ "eqri", (r,a,b) => r[a] == b ? 1 : 0 },
			{ "eqrr", (r,a,b) => r[a] == r[b] ? 1 : 0 },
		};


		public override object RunPart1()
		{
			var ipRegister=0;
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

			var ip = 0;
			var register = new int[6];
			while (ip < instructions.Count)
			{
				register[ipRegister] = ip;
				var instruction = instructions[ip];
				register[instruction.Item4] = _operations[instruction.Item1](register, instruction.Item2, instruction.Item3);
				ip = register[ipRegister] +1;
			}

			return register[0];
		}

		public override object RunPart2()
		{
			return base.RunPart2();

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

			var ip = 0;
			var register = new[] { 1, 0, 0, 0, 0, 0 };
			while (ip < instructions.Count)
			{
				register[ipRegister] = ip;
				var instruction = instructions[ip];
				register[instruction.Item4] = _operations[instruction.Item1](register, instruction.Item2, instruction.Item3);
				ip = register[ipRegister] + 1;
			}

			return register[0];
		}
	}
}