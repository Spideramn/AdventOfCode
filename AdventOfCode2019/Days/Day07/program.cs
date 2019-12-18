using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Days.Day07
{
	internal class Program : Day
	{
		public override int DayNumber => 7;

		public override object RunPart1()
		{
			var code = GetInputString().Split(',').Select(int.Parse).ToArray();
			return GetPermutations(new[] {0, 1, 2, 3, 4})
				.Select(perm => CalculateThrustersValue1(code, perm[0], perm[1], perm[2], perm[3], perm[4]))
				.Max();
		}

		public override object RunPart2()
		{
			var code = GetInputString().Split(',').Select(int.Parse).ToArray();
			return GetPermutations(new[] {5, 6, 7, 8, 9})
				.Select(perm => CalculateThrustersValue2(code, perm[0], perm[1], perm[2], perm[3], perm[4]))
				.Max();
		}
		
		private static int CalculateThrustersValue1(int[] code, int phaseA, int phaseB, int phaseC, int phaseD, int phaseE)
		{
			var signal = 0;
			signal = new Intcode(code, phaseA, signal).Run().First();
			signal = new Intcode(code, phaseB, signal).Run().First();
			signal = new Intcode(code, phaseC, signal).Run().First();
			signal = new Intcode(code, phaseD, signal).Run().First();
			return new Intcode(code, phaseE, signal).Run().First();
		}
		
		private static int CalculateThrustersValue2(int[] code, int phaseA, int phaseB, int phaseC, int phaseD, int phaseE)
		{
			var ampA = new Intcode(code, phaseA);
			var ampB = new Intcode(code, phaseB);
			var ampC = new Intcode(code, phaseC);
			var ampD = new Intcode(code, phaseD);
			var ampE = new Intcode(code, phaseE);

			using (var enumA = ampA.Run().GetEnumerator())
			using (var enumB = ampB.Run().GetEnumerator())
			using (var enumC = ampC.Run().GetEnumerator())
			using (var enumD = ampD.Run().GetEnumerator())
			using (var enumE = ampE.Run().GetEnumerator())
			{
				var signal = 0;
				while (true)
				{
					ampA.AddInput(signal);
					if (!enumA.MoveNext()) break;
					
					ampB.AddInput(enumA.Current);
					if (!enumB.MoveNext()) break;
					
					ampC.AddInput(enumB.Current);
					if (!enumC.MoveNext()) break;
					
					ampD.AddInput(enumC.Current);
					if (!enumD.MoveNext()) break;
					
					ampE.AddInput(enumD.Current);
					if (!enumE.MoveNext()) break;

					signal = enumE.Current;
				}
				return signal;
			}
		}

		// source: https://stackoverflow.com/a/10629938/1251423
		private static IEnumerable<T[]> GetPermutations<T>(IList<T> list, int? length = null)
		{
			var l = length.GetValueOrDefault(list.Count);
			if (l == 1) return list.Select(t => new[] { t });

			return GetPermutations(list, l - 1)
				.SelectMany(
					t => list.Where(e => !t.Contains(e)).ToArray(),
					(t1, t2) => t1.Concat(new[] { t2 }).ToArray()
				);
		}

		private class Intcode
		{
			private bool _isRunning;
			private readonly int[] _memory;
			private readonly Queue<int> _inputs;

			public Intcode(int[] code, params int[] initialInput)
			{
				_memory = new int[code.Length];
				Array.Copy(code, _memory, code.Length);

				_inputs = new Queue<int>(initialInput);
			}

			public void AddInput(params int[] input)
			{
				foreach(var i in input)
					_inputs.Enqueue(i);
			}

			public IEnumerable<int> Run()
			{
				if (_isRunning)
					throw new Exception("Allready running!");

				_isRunning = true;

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
								_memory[param1] = _inputs.Dequeue();
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
							_isRunning = false;
							yield break;

						default:
							_isRunning = false;
							throw new Exception($"Unknown opcode: {opCode}");
					}
				}
			}
		}
	}
}