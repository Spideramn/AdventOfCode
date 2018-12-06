using System;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.Days.Day06
{
	internal class Program : Day
	{
		public override int DayNumber => 6;

		public override object RunPart1()
		{
			var coordinates = GetInputLines()
				.Select(l => l.Split(", "))
				.Select((p, i) => (Point: new Point(int.Parse(p[0]), int.Parse(p[1])), Index: (char)(i+'A')))
				.ToDictionary(x => x.Index, x => x.Point);
			
			var xMin = coordinates.Values.Min(c => c.X);
			var yMin = coordinates.Values.Min(c => c.Y);
			var xMax = coordinates.Values.Max(c => c.X);
			var yMax = coordinates.Values.Max(c => c.Y);
			
			foreach (var index in coordinates.Keys.ToList())
				coordinates[index] = new Point(coordinates[index].X - xMin, coordinates[index].Y - yMin);

			xMax -= xMin;
			yMax -= yMin;

			var grid = new char[xMax, yMax];
			for (var x = 0; x < xMax; x++)
			for (var y = 0; y < yMax; y++)
			{
				var closest = coordinates
					.Select(c => (c.Key, Distance: Math.Abs(c.Value.X - x) + Math.Abs(c.Value.Y - y)))
					.OrderBy(g => g.Distance)
					.Take(2)
					.ToArray();
				if (closest[0].Distance != closest[1].Distance)
					grid[x, y] = closest[0].Key;
				else
					grid[x, y] = '.';
			}

			// remove infinite area's
			// top bottom
			var upperX = grid.GetUpperBound(0);
			var upperY = grid.GetUpperBound(1);
			for (var x = 0; x < grid.GetLength(0); x++)
			{
				coordinates.Remove(grid[x, 0]);
				coordinates.Remove(grid[x, upperY]);
			}
			// left and right
			for (var y = 0; y < grid.GetLength(1); y++)
			{
				coordinates.Remove(grid[0, y]);
				coordinates.Remove(grid[upperX, y]);
			}

			var grid2 = grid.Cast<char>().ToArray();
			return coordinates.Max(kv => grid2.Count(c => c == kv.Key));
		}

		public override object RunPart2()
		{
			var coordinates = GetInputLines()
				.Select(l => l.Split(", "))
				.Select(p => new Point(int.Parse(p[0]), int.Parse(p[1])))
				.ToList();

			var count = 0;
			var xMin = coordinates.Min(c => c.X);
			var yMin = coordinates.Min(c => c.Y);
			var xMax = coordinates.Max(c => c.X);
			var yMax = coordinates.Max(c => c.Y);
			for (var x = xMin; x < xMax; x++)
			for (var y = yMin; y < yMax; y++)
			{
				if (coordinates.Sum(c => Math.Abs(c.X - x) + Math.Abs(c.Y - y)) < 10000)
					count ++;
			}
			return count;
		}
	}
}