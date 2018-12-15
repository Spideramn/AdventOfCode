using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days.Day12
{
	internal class Program : Day
	{
		public override int DayNumber => 12;

		public override object RunPart1()
		{
			var input = GetInputLines().ToList();
			var pots = input[0].Substring(15).Select(c => c == '#').ToList();
			var mutations = input.Skip(2).Select(line => new Mutation(line)).ToList();

			// 20 generations
			var negative = 0;
			for (var gen = 0; gen < 20; gen++)
			{
				if (pots[0])
				{
					pots.Insert(0, false);
					pots.Insert(0, false);
					pots.Insert(0, false);
					negative += 3;
				}
				else if (pots[1])
				{
					pots.Insert(0, false);
					pots.Insert(0, false);
					negative += 2;
				}
				else if (pots[2])
				{
					pots.Insert(0, false);
					negative += 1;
				}

				if (pots[pots.Count - 1])
					pots.AddRange(new[] { false, false, false });
				else if (pots[pots.Count - 2])
					pots.AddRange(new[] { false, false });
				else if (pots[pots.Count - 3])
					pots.AddRange(new[] { false });
			
				var newPots = pots.Take(2).ToList();
				for (var i = 2; i < pots.Count-2; i++)
				{
					var p = false;
					foreach (var mutation in mutations)
					{
						var newPot = mutation.Mutate(pots, i);
						if (newPot != null)
						{
							p = newPot.Value;
							break;
						}
					}
					newPots.Add(p);
				}
				newPots.Add(pots[pots.Count - 2]);
				newPots.Add(pots[pots.Count - 1]);

				pots = newPots;
			}

			
			var score = 0;
			for(var i=0; i<pots.Count; i++)
			{
				if (pots[i])
					score += i - negative;
			}
			return score;
		}
		
		public override object RunPart2()
		{
			var input = GetInputLines().ToList();
			var pots = input[0].Substring(15).Select((c,k) => (c:c == '#', k)).ToDictionary(c => (long)c.k, c=>c.c);
			var mutations = input.Skip(2).Select(line => new Mutation(line)).ToList();

			long gen;
			var generations = 50000000000L;
			for (gen = 0L; gen < generations; gen++)
			{
				var newPots = new Dictionary<long, bool>();
				var start = pots.Keys.Min() - 2;
				var end = pots.Keys.Max() + 2;
				for (var index = start; index <= end; index++)
				{
					var p = false;
					foreach (var mutation in mutations)
					{
						var newPot = mutation.Mutate(pots, index);
						if (newPot != null)
						{
							p = newPot.Value;
							break;
						}
					}
					newPots[index] = p;
				}

				// remove leading / trailing empty pots
				var countStart = 0;
				foreach (var pot in newPots)
				{
					if (pot.Value) break;
					countStart++;
				}
				var countEnd = 0;
				for (var i = end; i > start; i--)
				{
					if (newPots[i]) break;
					countEnd++;
				}

				newPots = newPots.Skip(countStart).Take(newPots.Count - countStart - countEnd).ToDictionary(k=>k.Key, k=>k.Value);
				
				if (newPots.Values.SequenceEqual(pots.Values)) {
					break;
				} 
				
				pots = newPots;
			}

			var adjustment = generations - gen;
			var score = 0L;
			foreach(var pot in pots)
			{
				if (pot.Value)
					score += pot.Key + adjustment;
			}
			return score;
		}

		private void Dump(IEnumerable<bool> pots, long? negative, long? gen)
		{
			Console.WriteLine($"{gen,11} / {negative,11}");
			Console.WriteLine(string.Concat(pots.Select(p => p ? "#" : ".")));
		}

		private class Mutation
		{
			private readonly string _line;
			private readonly bool[] _pots;
			public Mutation(string line)
			{
				_line = line;

				_pots = new bool[] {
					_line[0]=='#',
					_line[1]=='#',
					_line[2]=='#',
					_line[3]=='#',
					_line[4]=='#',
					_line[9]=='#'
				};

			}

			public override string ToString()
			{
				return _line;
			}

			internal bool? Mutate(List<bool> pots, int potIndex)
			{
				if (pots[potIndex-2] == _pots[0] &&
					pots[potIndex-1] == _pots[1] &&
					pots[potIndex]   == _pots[2] &&
					pots[potIndex+1] == _pots[3] &&
					pots[potIndex+2] == _pots[4])
					return _pots[5];

				return null;
			}
			internal bool? Mutate(Dictionary<long, bool> pots, long potIndex)
			{
				if (pots.GetValueOrDefault(potIndex - 2, false) == _pots[0] &&
					pots.GetValueOrDefault(potIndex - 1, false) == _pots[1] &&
					pots.GetValueOrDefault(potIndex - 0, false) == _pots[2] &&
					pots.GetValueOrDefault(potIndex + 1, false) == _pots[3] &&
					pots.GetValueOrDefault(potIndex + 2, false) == _pots[4])
					return _pots[5];

				return null;
			}
		}
	}
}