using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Days
{
	public class Day17 : Day
	{
		public Day17(string input)
			: base(17, input)
		{
		}

		public override object RunPart1()
		{
			var liters = 150;
			var lines = Input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
			var containers = new Dictionary<long, int>();
			for (var i = 0; i < lines.Length; i++)
				containers.Add((long) Math.Pow(2, i), int.Parse(lines[i]));

			var possibilities = new HashSet<long>();
			for (var i = 1; i < containers.Count; i++)
			{
				foreach (var set in containers.Keys.ToList().GetCombinations(i))
				{
					var usage = 0;
					var key = 0L;
					foreach (var bucketKey in set)
					{
						usage += containers[bucketKey];
						key ^= bucketKey;
						if (usage >= liters)
							break;
					}
					if (usage == liters)
						possibilities.Add(key);
				}
			}
			return possibilities.Count;
		}

		public override object RunPart2()
		{
			var liters = 150;
			var lines = Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			var containers = new Dictionary<long, int>();
			for (var i = 0; i < lines.Length; i++)
				containers.Add((long)Math.Pow(2, i), int.Parse(lines[i]));

			var possibilities = new HashSet<long>();
			var found = false;
			for (var i = 1; i < containers.Count && !found; i++)
			{
				foreach (var set in containers.Keys.ToList().GetCombinations(i))
				{
					var usage = 0;
					var key = 0L;
					foreach (var bucketKey in set)
					{
						usage += containers[bucketKey];
						key ^= bucketKey;
						if (usage >= liters)
							break;
					}
					if (usage == liters)
					{
						possibilities.Add(key);
						found = true;
					}
				}
			}
			return possibilities.Count;
		}
	}
}