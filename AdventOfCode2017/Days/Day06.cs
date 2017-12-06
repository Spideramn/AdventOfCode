using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day06 : Day
	{
		public Day06(string input)
			: base(06, input)//"0\t2\t7\t0")
		{
		}

		public override object RunPart1()
		{
			var steps = 0;
			var knownStates = new HashSet<int>();
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
				
				var stateHash = 0;
				foreach (var i in state)
					stateHash = (stateHash + i) * 10;
				
				if (knownStates.Contains(stateHash))
					return steps;
				knownStates.Add(stateHash);
			}
		}

		public override object RunPart2()
		{
			var first = true;
			var steps = 0;
			var knownStates = new HashSet<int>();
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

				var stateHash = 0;
				foreach (var i in state)
					stateHash = (stateHash + i) * 10;

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
