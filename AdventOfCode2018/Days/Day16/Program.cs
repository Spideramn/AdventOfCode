using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days.Day16
{
	internal class Program : Day
	{
		private readonly Dictionary<string, Func<int[], int[], int>> _operations = new Dictionary<string, Func<int[], int[], int>>
		{
			{ "addr", (p,r) => r[p[1]] + r[p[2]] },
			{ "addi", (p,r) => r[p[1]] + p[2] },
			{ "mulr", (p,r) => r[p[1]] * r[p[2]] },
			{ "muli", (p,r) => r[p[1]] * p[2] },
			{ "banr", (p,r) => r[p[1]] & r[p[2]] },
			{ "bani", (p,r) => r[p[1]] & p[2] },
			{ "borr", (p,r) => r[p[1]] | r[p[2]] },
			{ "bori", (p,r) => r[p[1]] | p[2] },
			{ "setr", (p,r) => r[p[1]] },
			{ "seti", (p,r) => p[1] },
			{ "gtir", (p,r) => p[1]>r[p[2]]?1:0 },
			{ "gtri", (p,r) => r[p[1]]>p[2]?1:0 },
			{ "gtrr", (p,r) => r[p[1]]>r[p[2]]?1:0 },
			{ "eqir", (p,r) => p[1]==r[p[2]]?1:0 },
			{ "eqri", (p,r) => r[p[1]]==p[2]?1:0 },
			{ "eqrr", (p,r) => r[p[1]]==r[p[2]]?1:0 },
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
					var register = line.Substring(9, 10).Split(", ").Select(int.Parse).ToArray();
					var parameters = queue.Dequeue().Split(' ').Select(int.Parse).ToArray();
					var expectedResult = queue.Dequeue().Substring(9, 10).Split(", ").Select(int.Parse).ToArray();

					// try all oppcodes and compare outputs
					var count = 0;
					foreach (var opp in _operations)
					{
						if (opp.Value(parameters, register) == expectedResult[parameters[3]])
						{
							if (++count >= 3)
							{
								counter++;
								break;
							}
						}
					}
				}
			}
			return counter;
		}

		public override object RunPart2()
		{
			var operationPossibilities = _operations.Keys.ToDictionary(o => o, _ => new HashSet<int>(Enumerable.Range(0, _operations.Keys.Count)));
			var operationIndex = new Dictionary<int, string>();
			var testProgram = new List<int[]>();
			var queue = new Queue<string>(GetInputLines());
			while (queue.Any())
			{
				var line = queue.Dequeue();
				if (string.IsNullOrEmpty(line))
				{
					if (queue.TryPeek(out var peek) && string.IsNullOrEmpty(peek))
					{
						queue.Dequeue();
						queue.Dequeue();
						while (queue.TryDequeue(out var testLine))
							testProgram.Add(testLine.Split(' ').Select(int.Parse).ToArray());
						break;
					}
					continue;
				}
				if (line.StartsWith("Before"))
				{
					var before = line.Substring(9, 10).Split(", ").Select(int.Parse).ToArray();
					var parameters = queue.Dequeue().Split(' ').Select(int.Parse).ToArray();
					var after = queue.Dequeue().Substring(9, 10).Split(", ").Select(int.Parse).ToArray();

					foreach (var opp in _operations)
					{
						if (operationPossibilities[opp.Key].Contains(parameters[0]) && opp.Value(parameters, before) != after[parameters[3]])
							operationPossibilities[opp.Key].Remove(parameters[0]);
					}
				}
			}

			while (operationPossibilities.Any())
			{
				var first = operationPossibilities.OrderBy(o => o.Value.Count).First();
				if (first.Value.Count == 1)
				{
					var index = first.Value.First();
					operationIndex.Add(index, first.Key);
					operationPossibilities.Remove(first.Key);
					foreach (var op in operationPossibilities)
						op.Value.Remove(index);
				}
				else {
					break; // should not happen?
				}
			}

			// execute program
			var register = new[] {0,0,0,0};
			foreach (var parameters in testProgram)
				register[parameters[3]] = _operations[operationIndex[parameters[0]]](parameters, register);
			return register[0];
		}
	}
}