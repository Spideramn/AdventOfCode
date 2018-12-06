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
			
			var xOffset = coordinates.Values.Min(c => c.X);
			var yOffset = coordinates.Values.Min(c => c.Y);
			foreach (var index in coordinates.Keys.ToList())
				coordinates[index] = new Point(coordinates[index].X - xOffset, coordinates[index].Y - yOffset);
			
			var grid = new char[coordinates.Values.Max(c => c.X), coordinates.Values.Max(c => c.Y)];
			for (var x = 0; x < grid.GetLength(0); x++)
			for (var y = 0; y < grid.GetLength(1); y++)
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
				.Select((p, i) => (Point: new Point(int.Parse(p[0]), int.Parse(p[1])), Index: (char)(i + 'A')))
				.ToDictionary(x => x.Index, x => x.Point);

			var xOffset = coordinates.Values.Min(c => c.X);
			var yOffset = coordinates.Values.Min(c => c.Y);
			foreach (var index in coordinates.Keys.ToList())
				coordinates[index] = new Point(coordinates[index].X - xOffset, coordinates[index].Y - yOffset);

			var count = 0;
			for (var x = 0; x < coordinates.Values.Max(c => c.X); x++)
			for (var y = 0; y < coordinates.Values.Max(c => c.Y); y++)
			{
				if (coordinates.Sum(c => Math.Abs(c.Value.X - x) + Math.Abs(c.Value.Y - y)) < 10000)
					count ++;
			}
			return count;
		}
	}
}