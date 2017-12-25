using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day23 : Day
	{
		public Day23(string input)
			: base(23, input)
		{
		}

		public override object RunPart1()
		{
			var mulCount = 0;
			var register = new Register(8);
			var instructions = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();
			for (var index = 0L; index < instructions.Length; index++)
			{
				var args = instructions[index];
				switch (args[0])
				{
					case "set":
						register.Set(args);
						break;
					case "sub":
						register.Sub(args);
						break;
					case "mul":
						register.Mul(args);
						mulCount++;
						break;
					case "jnz":
						var x = register.GetValue(args[1]);
						if (x != 0)
							index += register.GetValue(args[2]) - 1;
						break;
					default:
						throw new ArgumentException("Unknown instruction");
				}
			}
			return mulCount;
		}
		private class Register
		{
			public readonly Dictionary<char, long> Registers = new Dictionary<char, long>();

			public Register(int count)
			{
				for (var i = 0; i < count; i++)
					Registers.Add((char)('a'+i), 0);
			}

			public void Set(string[] args)
			{
				Registers[args[1][0]] = GetValue(args[2]);
			}
			public void Sub(string[] args)
			{
				Registers[args[1][0]] -= GetValue(args[2]);
			}
			public void Mul(string[] args)
			{
				Registers[args[1][0]] *= GetValue(args[2]);
			}

			public long GetValue(string v)
			{
				return v[0] >= 'a' ? Registers[v[0]] : long.Parse(v);
			}

			public override string ToString()
			{
				return string.Concat(Registers.Select(p => $"[{p.Key}:{p.Value}]"));
			}
		}


		public override object RunPart2()
		{
			return "todo...";
		}
	}
}
