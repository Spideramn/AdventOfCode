using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Days
{
	public class Day09 : Day
	{
		public Day09(string input)
			:base(9, input)
		{
		}

		public override object RunPart1()
		{
			var locations = new Dictionary<string, int>();
			var distances = new Dictionary<int, int>();

			foreach (var l in Resources.InputDay9.Split('\n'))
			{
				var line = l.Trim();
				var parts = line.Split(' ');
				var from = parts[0];
				var to = parts[2];
				var distance = int.Parse(parts[4]);

				if (!locations.ContainsKey(from))
					locations.Add(from, (int)Math.Pow(2, locations.Count));
				if (!locations.ContainsKey(to))
					locations.Add(to, (int)Math.Pow(2, locations.Count));
				distances.Add(locations[from] ^ locations[to], distance);
			}

			var minDistance = int.MaxValue;
			foreach (var p in locations.Keys.ToList().GetPermutations())//GetPermutations(locations.Keys.ToList(), locations.Keys.Count))
			{
				var routeLength = 0;
				for (var index = 0; index < p.Length-1; index++)
				{
					var from = p[index];
					var to = p[index+1];
					routeLength += distances[locations[from] ^ locations[to]];
				}
				minDistance = Math.Min(minDistance, routeLength);
			}
			return minDistance;
		}

		public override object RunPart2()
		{
			var locations = new Dictionary<string, int>();
			var distances = new Dictionary<int, int>();

			foreach (var l in Resources.InputDay9.Split('\n'))
			{
				var line = l.Trim();
				var parts = line.Split(' ');
				var from = parts[0];
				var to = parts[2];
				var distance = int.Parse(parts[4]);

				if (!locations.ContainsKey(from))
					locations.Add(from, (int)Math.Pow(2, locations.Count));
				if (!locations.ContainsKey(to))
					locations.Add(to, (int)Math.Pow(2, locations.Count));
				distances.Add(locations[from] ^ locations[to], distance);
			}

			var maxDistance = 0;
			foreach (var p in locations.Keys.ToList().GetPermutations()) //GetPermutations(locations.Keys.ToList(), locations.Keys.Count))
			{
				var routeLength = 0;
				for (var index = 0; index < p.Length - 1; index++)
				{
					var from = p[index];
					var to = p[index + 1];
					routeLength += distances[locations[from] ^ locations[to]];
				}
				maxDistance = Math.Max(maxDistance, routeLength);
			}
			return maxDistance;
		}

	}
}