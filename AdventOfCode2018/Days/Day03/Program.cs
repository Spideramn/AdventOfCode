using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.Days.Day03
{
	internal class Program : Day
	{
		public override int DayNumber => 3;

		public override object RunPart1()
		{
			var overlappedInches = 0;
			var fabric = new Dictionary<Point, int>();
			foreach (var line in GetInputLines())
			{
				var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var start = parts[2].TrimEnd(':').Split(',');
				var size = parts[3].Split('x');

				var left = int.Parse(start[0]);
				var top = int.Parse(start[1]);
				var width = int.Parse(size[0]);
				var height = int.Parse(size[1]);

				for (var x = left; x < left + width; x++)
				for (var y = top; y < top + height; y++)
				{
					var point = new Point(x, y);
					if (!fabric.ContainsKey(point))
						fabric[point] = 1;
					else if (fabric[point]++ == 1)
						overlappedInches++;
				}
			}

			return overlappedInches;
		}

		public override object RunPart2()
		{
			var options = new HashSet<int>();
			var fabric = new Dictionary<Point, List<int>>();
			foreach (var line in GetInputLines())
			{
				var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var start = parts[2].TrimEnd(':').Split(',');
				var size = parts[3].Split('x');

				var claimId = int.Parse(parts[0].TrimStart('#'));
				var left = int.Parse(start[0]);
				var top = int.Parse(start[1]);
				var width = int.Parse(size[0]);
				var height = int.Parse(size[1]);

				options.Add(claimId);
				for (var x = left; x < left + width; x++)
				{
					for (var y = top; y < top + height; y++)
					{
						var point = new Point(x, y);
						if (!fabric.ContainsKey(point))
						{
							fabric.Add(point, new List<int> {claimId});
						}
						else
						{
							fabric[point].Add(claimId);
							options.ExceptWith(fabric[point]);
						}
					}
				}
			}

			return options.FirstOrDefault();
		}
	}
}
