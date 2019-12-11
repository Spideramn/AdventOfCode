using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Days.Day05
{
	internal class Program : Day
	{
		public override int DayNumber => 5;

		public override object RunPart1()
		{
			var input = GetInputString().Split(',').Select(int.Parse).ToArray();
			var program = new Intcode(input);
			return string.Concat(program.Run(1));
		}

		public override object RunPart2()
		{
			var input = GetInputString().Split(',').Select(int.Parse).ToArray();
			var program = new Intcode(input);
			return string.Concat(program.Run(5));
		}

		private class Intcode
		{
			private readonly int[] _memory;

			public Intcode(int[] memory)
			{
				_memory = memory;
			}

			public IEnumerable<int> Run(int input)
			{
				var pointer = 0;
				while (true)
				{
					var instruction = _memory[pointer++];
					var opCode = instruction % 100;
					switch (opCode)
					{
						// add -> p3 = p1 + p2
						case 1:
						{
							var param1 = _memory[pointer++];
							var mode1 = (instruction / 100) % 10;
							var value1 = mode1 == 0 ? _memory[param1] : param1;

							var param2 = _memory[pointer++];
							var mode2 = (instruction / 1000) % 10;
							var value2 = mode2 == 0 ? _memory[param2] : param2;

							var param3 = _memory[pointer++];

							_memory[param3] = value1 + value2;
							break;
						}

						// multiply -> p3 = p1 * p2
						case 2:
						{
							var param1 = _memory[pointer++];
							var mode1 = (instruction / 100) % 10;
							var value1 = mode1 == 0 ? _memory[param1] : param1;

							var param2 = _memory[pointer++];
							var mode2 = (instruction / 1000) % 10;
							var value2 = mode2 == 0 ? _memory[param2] : param2;

							var param3 = _memory[pointer++];

							_memory[param3] = value1 * value2;
							break;
						}

						// set -> p1 = input
						case 3:
						{
							var param1 = _memory[pointer++];
							_memory[param1] = input;
							break;
						}

						// output -> output p1
						case 4:
						{
							var param1 = _memory[pointer++];
							var mode1 = (instruction / 100) % 10;
							var value1 = mode1 == 0 ? _memory[param1] : param1;

							yield return value1;
							break;
						}

						// jump-if-true ->  if p1 != 0 then pointer = p2
						case 5:
						{
							var param1 = _memory[pointer++];
							var mode1 = (instruction / 100) % 10;
							var value1 = mode1 == 0 ? _memory[param1] : param1;

							var param2 = _memory[pointer++];
							var mode2 = (instruction / 1000) % 10;
							var value2 = mode2 == 0 ? _memory[param2] : param2;

							if (value1 != 0)
								pointer = value2;
							break;
						}

						// jump-if-false ->  if p1 == 0 then pointer = p2
						case 6:
						{
							var param1 = _memory[pointer++];
							var mode1 = (instruction / 100) % 10;
							var value1 = mode1 == 0 ? _memory[param1] : param1;

							if (value1 == 0)
							{
								var param2 = _memory[pointer];
								var mode2 = (instruction / 1000) % 10;
								var value2 = mode2 == 0 ? _memory[param2] : param2;
								pointer = value2;
							}
							else
							{
								pointer++;
							}
							break;
						}

						// less than -> if p1 < p2 then p3 = 1 else p3 = 0
						case 7:
						{
							var param1 = _memory[pointer++];
							var mode1 = (instruction / 100) % 10;
							var value1 = mode1 == 0 ? _memory[param1] : param1;

							var param2 = _memory[pointer++];
							var mode2 = (instruction / 1000) % 10;
							var value2 = mode2 == 0 ? _memory[param2] : param2;

							var param3 = _memory[pointer++];

							if (value1 < value2)
								_memory[param3] = 1;
							else
								_memory[param3] = 0;
							break;
						}

						// equals -> if p1 == p2 then p3 = 1 else p3 = 0
						case 8:
						{
							var param1 = _memory[pointer++];
							var mode1 = (instruction / 100) % 10;
							var value1 = mode1 == 0 ? _memory[param1] : param1;

							var param2 = _memory[pointer++];
							var mode2 = (instruction / 1000) % 10;
							var value2 = mode2 == 0 ? _memory[param2] : param2;

							var param3 = _memory[pointer++];

							if (value1 == value2)
								_memory[param3] = 1;
							else
								_memory[param3] = 0;
							break;
						}

						//halt
						case 99:
							yield break;

						default:
							throw new Exception($"Unknown opcode: {opCode}");
					}
				}
			}
		}
	}
}