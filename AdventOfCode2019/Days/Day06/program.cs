using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace AdventOfCode2019.Days.Day06
{
	internal class Program : Day
	{
		public override int DayNumber => 6;

		public override object RunPart1()
		{
			var data = GetInputLines()
				.Select(l => l.Split(')'))
				.ToDictionary(p => p[1], p => p[0]);

			var orbids = 0;
			foreach (var b in data.Values)
			{
				// direct orbids
				orbids++;

				// indirect orbids
				var planet = b;
				while (data.ContainsKey(planet))
				{
					orbids++;
					planet = data[planet];
				}
			}
			return orbids;
		}
		public override object RunPart2()
		{
			var data = GetInputLines()
				.Select(l => l.Split(')'))
				.ToDictionary(p => p[1], p => p[0]);

			var data2 = new Dictionary<string, HashSet<string>>();
			foreach (var d in data)
			{
				if (!data2.ContainsKey(d.Value))
					data2[d.Value] = new HashSet<string> {d.Key};
				else
					data2[d.Value].Add(d.Key);


				if (!data2.ContainsKey(d.Key))
					data2[d.Key] = new HashSet<string> { d.Value };
				else
					data2[d.Key].Add(d.Value);
			}

			var start = data["YOU"];
			var end = data["SAN"];

			var transfers = 0;
			var options = new HashSet<string>{start};
			while (options.Any())
			{
				transfers++;
				var o = options.ToArray();
				options.Clear();
				foreach (var position in o)
				{
					foreach (var pos in data2[position])
					{
						options.Add(pos);
						if (pos == end)
							return transfers;
					}
				}
			}

			return null;
		}
	}
}