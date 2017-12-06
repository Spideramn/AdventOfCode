using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day06 : Day
	{
		public Day06(string input)
			: base(06, input)
		{
		}

		public override object RunPart1()
		{
			var cycles = 0;
			var knownStates = new HashSet<string>();
			var state = Input.Split('\t').Select(int.Parse).ToArray();
			do
			{
				cycles++;
				var max = state.Max();
				var index = Array.IndexOf(state, max);
				state[index] = 0;
				while (max > 0)
				{
					if (++index >= state.Length)
						index = 0;
					state[index]++;
					max--;
				}
			} while (knownStates.Add(string.Join(',', state)));
			return cycles;
		}

		public override object RunPart2()
		{
			var first = true;
			var steps = 0;
			var knownStates = new HashSet<string>();
			var state = Input.Split('\t').Select(int.Parse).ToArray();
			while (true)
			{
				steps++;
				var max = state.Max();
				var index = Array.IndexOf(state, max);
				state[index] = 0;
				while (max > 0)
				{
					if (++index >= state.Length)
						index = 0;
					state[index]++;
					max--;
				}

				var stateHash = string.Join(',', state);
				if (knownStates.Contains(stateHash))
				{
					if (!first)
						return steps;
					first = false;
					steps = 0;
					knownStates.Clear();
				}
				knownStates.Add(stateHash);
			}
		}
	}
}
