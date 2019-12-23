using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2019.Days.Day10
{
	internal class Program : Day
	{
		public override int DayNumber => 10;

		public override object RunPart1()
		{
			var asteroids = new List<Point>();
			var y = 0;
			foreach (var line in GetInputLines())
			{
				var x = 0;
				foreach (var c in line)
				{
					if(c != '.')
						asteroids.Add(new Point(x, y));
					x++;
				}
				y++;
			}

			var max = 0;
			var position = Point.Empty;
			foreach (var asteroid in asteroids)
			{
				var count = CountAsteroidsInSight(asteroid, asteroids);
				if (count <= max) continue;
				max = count;
				position = asteroid;
			}
			Console.WriteLine($"Max: {max} @ {position.X},{position.Y}");
			return max;
		}

		public override object RunPart2()
		{
			var laserPosition = new Point(8,16); // location of answer part 1
			var asteroids = new List<Asteroid>();
			var y = 0;
			foreach (var line in GetInputLines())
			{
				var x = 0;
				foreach (var c in line)
				{
					if (c != '.')
						asteroids.Add(new Asteroid(new Point(x, y), laserPosition));
					x++;
				}
				y++;
			}


			var groupedAsteroids = asteroids
				.OrderBy(a => a.Angle)
				.GroupBy(a => a.Angle)
				.ToDictionary(
					g => g.Key, 
					g => new Queue<Asteroid>(g.OrderBy(q => q.Distance).ToArray())
					);
			var angles = groupedAsteroids.Keys.ToArray();
			var index = 0;
			Asteroid zapped = null;
			for (var i = 1; i <= 200; i++)
			{
				zapped = groupedAsteroids[angles[index]].Dequeue();
				do
				{
					index = (index + 1) % angles.Length;
				} while (groupedAsteroids[angles[index]].Count == 0);
			}
			return zapped?.Location.X * 100 + zapped?.Location.Y;
		}

		private static int CountAsteroidsInSight(Point point, IList<Point> asteroids)
		{
			var maxX = asteroids.Max(p => p.X);
			var maxY = asteroids.Max(p => p.Y);

			var inSight = new HashSet<Point>(asteroids);
			inSight.Remove(point);

			foreach (var asteroid in asteroids)
			{
				if (!inSight.Contains(asteroid))
					continue;

				var offsetX = asteroid.X - point.X;
				var offsetY = asteroid.Y - point.Y;
				var max = Math.Max(Math.Abs(offsetX), Math.Abs(offsetY));
				for (var c = 2; c <= max; c++)
				{
					if (offsetX % c == 0 && offsetY % c == 0)
					{
						offsetX /= c;
						offsetY /= c;
						break;
					}
				}

				var offset = new Size(offsetX, offsetY);
				var check = new Point(asteroid.X, asteroid.Y);
				while (true)
				{
					check += offset;
					if (check.X < 0 || check.Y < 0 || check.X > maxX || check.Y > maxY)
						break;
					inSight.Remove(check);
				}
			}
			return inSight.Count;
		}


		private class Asteroid
		{
			public Asteroid(Point location, Point laser)
			{
				Location = location;
				Angle = ((Math.Atan2(Location.Y - laser.Y, Location.X - laser.X) * 180f / Math.PI + 90) + 360) % 360;
				Distance = Math.Sqrt(Math.Pow(laser.X - location.X, 2) + Math.Pow(laser.Y - location.Y, 2));
			}

			public Point Location { get; }
			public double Angle { get; }
			public double Distance { get; }
		}
	}
}