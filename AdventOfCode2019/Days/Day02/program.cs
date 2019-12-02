using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Days.Day02
{
	internal class Program : Day
	{
		public override int DayNumber => 2;

		public override object RunPart1()
		{
			var input = GetInputString().Split(',').Select((value, index) => new {Value = int.Parse(value), Index = index}).ToDictionary(k => k.Index, k => k.Value);
			var program = new Intcode(input);
			return program.Run(12, 2);
		}

		public override object RunPart2()
		{
			var input = GetInputString().Split(',').Select((value, index) => new { Value = int.Parse(value), Index = index }).ToArray();

			for (var noun = 0; noun <= 99; noun++)
			for (var verb = 0; verb <= 99; verb++)
			{
				var program = new Intcode(input.ToDictionary(k => k.Index, k => k.Value));
				if (program.Run(noun, verb) == 19690720)
					return (100 * noun) + verb;
			}
			return "No result";
		}


		private class Intcode
		{
			private readonly Dictionary<int, int> _memory;

			public Intcode(Dictionary<int, int> memory)
			{
				_memory = memory;
			}

			public int Run(int arg1, int arg2)
			{
				var pointer = 0;
				SetValue(1, arg1);
				SetValue(2, arg2);
				while (true)
				{
					switch (GetValue(pointer))
					{
						// add
						case 1:
						{
							var param1 = GetValue(++pointer);
							var param2 = GetValue(++pointer);
							var param3 = GetValue(++pointer);
							SetValue(param3, GetValue(param1) + GetValue(param2));
						}
							break;

						// multiply
						case 2:
						{
							var param1 = GetValue(++pointer);
							var param2 = GetValue(++pointer);
							var param3 = GetValue(++pointer);
							SetValue(param3, GetValue(param1) * GetValue(param2));
						}
							break;

						//halt
						case 99:
							return GetValue(0);
					}

					// next instruction
					pointer++;
				}
			}

			private int GetValue(int address)
			{
				return _memory[address];
				//return _memory.TryGetValue(address, out var value) ? value : 0; // ?
			}
			private void SetValue(int address, int value)
			{
				_memory[address] = value;
			}
		}
	}
}