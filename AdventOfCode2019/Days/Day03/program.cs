using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2019.Days.Day03
{
	internal class Program : Day
	{
		public override int DayNumber => 3;

		public override object RunPart1()
		{
			var input = GetInputLines().ToArray();

			var position = new Point(0, 0);
			var points = new HashSet<Point>();
			foreach (var instruction in input[0].Split(','))
			{
				var length = int.Parse(instruction.Substring(1));
				switch (instruction[0])
				{
					case 'R':
					{
						var startX = position.X + 1;
						var endX = position.X + length;
						for (var x = startX; x <= endX; x++)
							points.Add(new Point(x, position.Y));
						position = new Point(endX, position.Y);
						break;
					}
					case 'L':
					{
						var startX = position.X - 1;
						var endX = position.X - length;
						for (var x = startX; x >= endX; x--)
							points.Add(new Point(x, position.Y));
						position = new Point(endX, position.Y);
						break;
					}
					case 'U':
					{
						var startY = position.Y + 1;
						var endY = position.Y + length;
						for (var y = startY; y <= endY; y++)
							points.Add(new Point(position.X, y));
						position = new Point(position.X, endY);
						break;
					}

					case 'D':
					{
						var startY = position.Y - 1;
						var endY = position.Y - length;
						for (var y = startY; y >= endY; y--)
							points.Add(new Point(position.X, y));
						position = new Point(position.X, endY);
						break;
					}
				}
			}

			position = new Point(0, 0);
			var crossings = new HashSet<Point>();
			foreach (var instruction in input[1].Split(','))
			{
				var length = int.Parse(instruction.Substring(1));
				switch (instruction[0])
				{
					case 'R':
					{
						var startX = position.X + 1;
						var endX = position.X + length;
						for (var x = startX; x <= endX; x++)
						{
							var point = new Point(x, position.Y);
							if (points.Contains(point))
								crossings.Add(point);
						}

						position = new Point(endX, position.Y);
						break;
					}
					case 'L':
					{
						var startX = position.X - 1;
						var endX = position.X - length;
						for (var x = startX; x >= endX; x--)
						{
							var point = new Point(x, position.Y);
							if (points.Contains(point))
								crossings.Add(point);
						}

						position = new Point(endX, position.Y);
						break;
					}
					case 'U':
					{
						var startY = position.Y + 1;
						var endY = position.Y + length;
						for (var y = startY; y <= endY; y++)
						{
							var point = new Point(position.X, y);
							if (points.Contains(point))
								crossings.Add(point);
						}

						position = new Point(position.X, endY);
						break;
					}

					case 'D':
					{
						var startY = position.Y - 1;
						var endY = position.Y - length;
						for (var y = startY; y >= endY; y--)
						{
							var point = new Point(position.X, y);
							if (points.Contains(point))
								crossings.Add(point);
						}

						position = new Point(position.X, endY);
						break;
					}
				}
			}

			return crossings
				.Select(c => Math.Abs(c.X) + Math.Abs(c.Y))
				.OrderBy(c => c)
				.FirstOrDefault();
		}

		public override object RunPart2()
		{
			var input = GetInputLines().ToArray();

			var steps = 0;
			var position = new Point(0, 0);
			var points = new Dictionary<Point, int>();
			foreach (var instruction in input[0].Split(','))
			{
				var length = int.Parse(instruction.Substring(1));
				switch (instruction[0])
				{
					case 'R':
					{
						var startX = position.X + 1;
						var endX = position.X + length;
						for (var x = startX; x <= endX; x++)
							points[new Point(x, position.Y)] = ++steps;
						position = new Point(endX, position.Y);
						break;
					}
					case 'L':
					{
						var startX = position.X - 1;
						var endX = position.X - length;
						for (var x = startX; x >= endX; x--)
							points[new Point(x, position.Y)] = ++steps;
						position = new Point(endX, position.Y);
						break;
					}
					case 'U':
					{
						var startY = position.Y + 1;
						var endY = position.Y + length;
						for (var y = startY; y <= endY; y++)
							points[new Point(position.X, y)] = ++steps;
						position = new Point(position.X, endY);
						break;
					}

					case 'D':
					{
						var startY = position.Y - 1;
						var endY = position.Y - length;
						for (var y = startY; y >= endY; y--)
							points[new Point(position.X, y)] = ++steps;
						position = new Point(position.X, endY);
						break;
					}
				}
			}

			steps = 0;
			position = new Point(0, 0);
			var crossings = new Dictionary<Point, Tuple<int, int>>();
			foreach (var instruction in input[1].Split(','))
			{
				var length = int.Parse(instruction.Substring(1));
				switch (instruction[0])
				{
					case 'R':
					{
						var startX = position.X + 1;
						var endX = position.X + length;
						for (var x = startX; x <= endX; x++)
						{
							steps++;
							var point = new Point(x, position.Y);
							if (points.ContainsKey(point))
								crossings.Add(point, new Tuple<int, int>(points[point], steps));
						}

						position = new Point(endX, position.Y);
						break;
					}
					case 'L':
					{
						var startX = position.X - 1;
						var endX = position.X - length;
						for (var x = startX; x >= endX; x--)
						{
							steps++;
							var point = new Point(x, position.Y);
							if (points.ContainsKey(point))
								crossings.Add(point, new Tuple<int, int>(points[point], steps));
						}

						position = new Point(endX, position.Y);
						break;
					}
					case 'U':
					{
						var startY = position.Y + 1;
						var endY = position.Y + length;
						for (var y = startY; y <= endY; y++)
						{
							steps++;
							var point = new Point(position.X, y);
							if (points.ContainsKey(point))
								crossings.Add(point, new Tuple<int, int>(points[point], steps));
						}

						position = new Point(position.X, endY);
						break;
					}
					case 'D':
					{
						var startY = position.Y - 1;
						var endY = position.Y - length;
						for (var y = startY; y >= endY; y--)
						{
							steps++;
							var point = new Point(position.X, y);
							if (points.ContainsKey(point))
								crossings.Add(point, new Tuple<int, int>(points[point], steps));
						}

						position = new Point(position.X, endY);
						break;
					}
				}
			}
			
			return crossings
				.Select(c => c.Value.Item1 + c.Value.Item2)
				.OrderBy(c => c)
				.FirstOrDefault();
		}
	}
}