using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdventOfCode2017.Days
{
	internal class Day11 : Day
	{
		public Day11(string input)
			: base(11, input)
		{
		}

		
		public override object RunPart1()
		{
			var pos = new Hexagon();
			foreach (var direction in Input.Split(','))
				pos += Hexagon.Directions[direction];
			return pos.StepsAway();
		}

		public override object RunPart2()
		{
			var max = 0;
			var pos = new Hexagon();
			foreach (var direction in Input.Split(','))
			{
				pos += Hexagon.Directions[direction];
				max = Math.Max(max, pos.StepsAway());
			}
			return max;
		}

		// thanks to:
		// https://www.redblobgames.com/grids/hexagons/#neighbors-cube
		// https://www.redblobgames.com/grids/hexagons/#distances-cube
		private class Hexagon
		{
			public static readonly ReadOnlyDictionary<string, Hexagon> Directions =
				new ReadOnlyDictionary<string, Hexagon>(
					new Dictionary<string, Hexagon>
					{
						{"n", new Hexagon(0, 1, -1)},
						{"ne", new Hexagon(1, 0, -1)},
						{"se", new Hexagon(1, -1, 0)},
						{"s", new Hexagon(0, -1, 1)},
						{"sw", new Hexagon(-1, 0, 1)},
						{"nw", new Hexagon(-1, 1, 0)}
					}
				);

			public Hexagon():this(0,0,0)
			{

			}
			public Hexagon(int x, int y, int z)
			{
				X = x;
				Y = y;
				Z = z;
			}

			public int X { get; set; }
			public int Y { get; set; }
			public int Z { get; set; }

			public int StepsAway()
			{
				return Math.Max(Math.Max(Math.Abs(X), Math.Abs(Y)), Math.Abs(Z));
			}

			public static Hexagon operator +(Hexagon a, Hexagon b)
			{
				return new Hexagon(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
			}
		}
	}
}
