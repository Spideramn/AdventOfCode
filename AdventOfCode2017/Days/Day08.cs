using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day08 : Day
	{
		public Day08(string input)
			: base(08, input)
		{
		}

		public override object RunPart1()
		{
			var register = new Dictionary<string, int>();
			foreach (var line in Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
			{
				var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				if (CheckCondition(register, parts))
					RunInstruction(register, parts);
			}
			return register.Values.Max();
		}


		public override object RunPart2()
		{
			var maxValue = 0;
			var register = new Dictionary<string, int>();
			foreach (var line in Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
			{
				var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				if (CheckCondition(register, parts))
					maxValue = Math.Max(RunInstruction(register, parts), maxValue);
			}
			return maxValue;
		}


		private static bool CheckCondition(Dictionary<string, int> register, string[] parts)
		{
			register.TryGetValue(parts[4], out var value);
			var value2 = int.Parse(parts[6]);
			var condition = parts[5];
			switch (condition)
			{
				case ">":
					return value > value2;
				case "<":
					return value < value2;
				case ">=":
					return value >= value2;
				case "<=":
					return value <= value2;
				case "==":
					return value == value2;
				case "!=":
					return value != value2;
				default:
					throw new Exception("Unknown argument");
			}
		}

		private static int RunInstruction(Dictionary<string, int> register, string[] parts)
		{
			if (!register.ContainsKey(parts[0]))
				register[parts[0]] = 0;
			switch (parts[1])
			{
				case "inc":
					return register[parts[0]] += int.Parse(parts[2]);
				case "dec":
					return register[parts[0]] -= int.Parse(parts[2]);
				default:
					throw new Exception("Unknown instruction");
			}
		}
	}
}
