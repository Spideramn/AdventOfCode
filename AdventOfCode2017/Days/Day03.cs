using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day03 : Day
	{
		public Day03(string input)
			: base(03, input)
		{
		}

		public override object RunPart1()
		{
			var pos = GetPoints().Skip(int.Parse(Input) - 1).First();
			return Math.Abs(pos.X) + Math.Abs(pos.Y);
		}

		public override object RunPart2()
		{
			var input = int.Parse(Input);
			var dict = new Dictionary<Point, int> {{new Point(0, 0), 1}};
			var neighbours = new[]
			{
				new Size(1,1),
				new Size(1,0),
				new Size(1,-1),
				new Size(0,1),
				new Size(0,-1),
				new Size(-1,1),
				new Size(-1,0),
				new Size(-1,-1)
			};
			foreach (var point in GetPoints().Skip(1))
			{
				var value = neighbours
					.Select(v => Point.Add(point, v))
					.Where(neighbour => dict.ContainsKey(neighbour))
					.Sum(neighbour => dict[neighbour]);
				if (value > input)
					return value;
				dict.Add(point, value);
			}
			return "Not found...";
		}

		private static IEnumerable<Point> GetPoints()
		{
			var r = 0;
			var point = new Point(0, 0);
			yield return point;
			while (r < int.MaxValue)
			{
				r++;

				// right
				while (point.X < r)
				{
					point.X += 1;
					yield return point;
				}

				// up
				while (point.Y < r)
				{
					point.Y += 1;
					yield return point;
				}

				// left
				while (point.X > -r)
				{
					point.X -= 1;
					yield return point;
				}

				// down
				while (point.Y > -r)
				{
					point.Y -= 1;
					yield return point;
				}
			}
		}
	}
}
