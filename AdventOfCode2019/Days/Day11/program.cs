using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2019.Days.Day11
{
	internal class Program : Day
	{
		public override int DayNumber => 11;

		public override object RunPart1()
		{
			var code = GetInputString().Split(',').Select(long.Parse).ToArray();
			var robot = new Intcode(code);
			using (var robotEnumerator = robot.Run().GetEnumerator())
			{
				var direction = 0;
				var location = Point.Empty;
				var panels = new Dictionary<Point, long> { { location, 0 } }; // start is black
				robot.AddInput(panels[location]); // add current location panel color to input
				while (robotEnumerator.MoveNext())
				{
					var color = robotEnumerator.Current;
					panels[location] = color; // color!
					if (!robotEnumerator.MoveNext())
						break;
					var turn = robotEnumerator.Current;
					if (turn == 0) // turn left
					{
						if (--direction < 0)
							direction = 3;
					}
					else
					{
						if (++direction > 3)
							direction = 0;
					}

					// move to new location
					switch (direction)
					{
						case 0: // north
							location += new Size(0, -1);
							break;
						case 1: // east
							location += new Size(1, 0);
							break;
						case 2: // south
							location += new Size(0, 1);
							break;
						case 3: // west
							location += new Size(-1, 0);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					// add new location panel color to input queue
					robot.AddInput(panels.TryGetValue(location, out var v) ? v : 0);
				}
				return panels.Count;
			}
		}

		public override object RunPart2()
		{
			var code = GetInputString().Split(',').Select(long.Parse).ToArray();
			var robot = new Intcode(code);
			using (var robotEnumerator = robot.Run().GetEnumerator())
			{
				var direction = 0;
				var location = Point.Empty;
				var panels = new Dictionary<Point, long> {{location, 1}}; // start is white
				robot.AddInput(panels[location]); // add current location panel color to input
				while (robotEnumerator.MoveNext())
				{
					var color = robotEnumerator.Current;
					panels[location] = color; // color!
					if (!robotEnumerator.MoveNext())
						break;
					var turn = robotEnumerator.Current;
					if (turn == 0) // turn left
					{
						if (--direction < 0)
							direction = 3;
					}
					else
					{
						if(++direction > 3)
							direction = 0;
					}

					// move to new location
					switch (direction)
					{
						case 0: // north
							location += new Size(0, -1);
							break;
						case 1: // east
							location += new Size(1, 0);
							break;
						case 2: // south
							location += new Size(0, 1);
							break;
						case 3: // west
							location += new Size(-1, 0);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					// add new location panel color to input queue
					robot.AddInput(panels.TryGetValue(location, out var v) ? v : 0);
				}

				// draw panels
				var xMin = panels.Keys.Min(p => p.X);
				var xMax = panels.Keys.Max(p => p.X);
				var yMin = panels.Keys.Min(p => p.Y);
				var yMax = panels.Keys.Max(p => p.Y);
				for (var y = yMin; y <= yMax; y++)
				{
					for (var x = xMin; x <= xMax; x++)
					{
						var color = panels.TryGetValue(new Point(x, y), out var v) ? v : 0;
						Console.Write(color == 1 ? ' ': '█');
					}
					Console.WriteLine();
				}

				// return picture size, so we can "test" the intcode program
				return ((xMax - xMin) * 100) + (yMax - yMin);
			}
		}
	}
}