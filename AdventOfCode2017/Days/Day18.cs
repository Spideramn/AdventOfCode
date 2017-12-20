using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day18 : Day
	{
		public Day18(string input)
			: base(18, input)
		{
		}

		public override object RunPart1()
		{
			var frequenties = new List<long>();
			var register = new Register();
			var instructions = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();
			for (var index = 0L; index < instructions.Length; index++)
			{
				var args = instructions[index];
				switch (args[0])
				{
					case "snd":
						frequenties.Add(register.GetValue(args[1]));
						break;
					case "set":
						register.Set(args);
						break;
					case "add":
						register.Add(args);
						break;
					case "mul":
						register.Mul(args);
						break;
					case "mod":
						register.Mod(args);
						break;
					case "rcv":
						var freq = register.GetValue(args[1]);
						if (freq != 0)
							return frequenties.Last();
						break;
					case "jgz":
						var x = register.GetValue(args[1]);
						if (x > 0)
							index += register.GetValue(args[2])-1;
							break;
					default:
						throw new ArgumentException("Unknown instruction");
				}
			}
			return "unknown";
		}

		public override object RunPart2()
		{
			var instructions = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();
			var program0 = new Register2(instructions, 0);
			var program1 = new Register2(instructions, 1);
			var counter = 0;
			do
			{
				var s0 = program0.Step();
				var s1 = program1.Step();
				if (s0.HasValue)
					program1.Input.Enqueue(s0.Value);
				if (s1.HasValue)
				{
					counter++;
					program0.Input.Enqueue(s1.Value);
				}
			} while (program0.WaitingCounter <= 1 || program1.WaitingCounter <= 1);
			return counter;
		}

		private class Register
		{
			protected readonly Dictionary<char, long> Registers = new Dictionary<char, long>();

			public void Set(string[] args)
			{
				var pos = args[1][0];
				if (!Registers.ContainsKey(pos))
					Registers.Add(pos, GetValue(args[2]));
				else
					Registers[pos] = GetValue(args[2]);
			}
			public void Add(string[] args)
			{
				var pos = args[1][0];
				if (!Registers.ContainsKey(pos))
					Registers.Add(pos, GetValue(args[2]));
				else
					Registers[pos] += GetValue(args[2]);
			}
			public void Mul(string[] args)
			{
				var pos = args[1][0];
				if (!Registers.ContainsKey(pos))
					Registers.Add(pos, 0);
				else
					Registers[pos] *= GetValue(args[2]);
			}
			public void Mod(string[] args)
			{
				var pos = args[1][0];
				if (!Registers.ContainsKey(pos))
					Registers.Add(pos, 0);
				else
					Registers[pos] %= GetValue(args[2]);
			}

			public long GetValue(string v)
			{
				if (v.Length != 1)
					return long.Parse(v);
				if(v[0] > '0' - 1 && v[0] < '9' + 1)
					return v[0] - '0';
				if(!Registers.ContainsKey(v[0]))
					Registers.Add(v[0],0);
				return Registers[v[0]];
			}

			public override string ToString()
			{
				return string.Concat(Registers.Select(p => $"[{p.Key}:{p.Value}]"));
			}
		}

		private class Register2 : Register
		{
			public int WaitingCounter { get; private set; }
			public readonly Queue<long> Input = new Queue<long>();
			private readonly string[][] _instructions;
			private long _index = 0;

			public Register2(string[][] instructions, int p)
			{
				_instructions = instructions;
				Registers['p'] = p;
			}

			public long? Step()
			{
				var args = _instructions[_index++];
				switch (args[0])
				{
					case "snd":
						return GetValue(args[1]);
						
					case "set":
						Set(args);
						break;
					case "add":
						Add(args);
						break;
					case "mul":
						Mul(args);
						break;
					case "mod":
						Mod(args);
						break;
					case "rcv":
						if (!Input.Any())
						{
							WaitingCounter++;
							_index--;
						}
						else
						{
							Registers[args[1][0]] = Input.Dequeue();
							WaitingCounter = 0;
						}
						break;
					case "jgz":
						var x = GetValue(args[1]);
						if (x > 0)
							_index += GetValue(args[2]) - 1;
						break;
					default:
						throw new ArgumentException("Unknown instruction");
				}
				return null;
			}
		}
	}
}
