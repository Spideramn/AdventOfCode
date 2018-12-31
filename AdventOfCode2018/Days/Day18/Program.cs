using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.Days.Day18
{
	internal class Program : Day
	{
		public override int DayNumber => 18;

		public override object RunPart1()
		{
			var acre = new Acre();
			var y = 0;
			foreach (var line in GetInputLines())
			{
				for (var x = 0; x < line.Length; x++)
				{
					switch (line[x])
					{
						case '.':
							acre.Add(new Point(x, y), LandType.Ground);
							break;
						case '|':
							acre.Add(new Point(x, y), LandType.Trees);
							break;
						case '#':
							acre.Add(new Point(x, y), LandType.Lumberyard);
							break;
					}
				}
				y++;
			}

			for (var minute = 0; minute < 10; minute++)
			{
				var newAcre = new Acre();
				foreach (var land in acre)
				{
					var newLand = land.Value;
					switch (land.Value)
					{
						case LandType.Ground:
							if (acre.HasAdjacentLandTypeCount(land.Key, LandType.Trees, 3))
								newLand = LandType.Trees;
							break;
						case LandType.Trees:
							if (acre.HasAdjacentLandTypeCount(land.Key, LandType.Lumberyard, 3))
								newLand = LandType.Lumberyard;
							break;
						case LandType.Lumberyard:
							if (!acre.HasAdjacentLandTypeCount(land.Key, LandType.Lumberyard, 1) ||
								!acre.HasAdjacentLandTypeCount(land.Key, LandType.Trees, 1))
								newLand = LandType.Ground;
							break;
					}
					newAcre[land.Key] = newLand;
				}
				acre = newAcre;
			}

			return acre.Values.Count(a => a == LandType.Trees) * acre.Values.Count(a => a == LandType.Lumberyard);
		}

		public override object RunPart2()
		{
			var acre = new Acre();
			var y = 0;
			foreach (var line in GetInputLines())
			{
				for (var x = 0; x < line.Length; x++)
				{
					switch (line[x])
					{
						case '.':
							acre.Add(new Point(x, y), LandType.Ground);
							break;
						case '|':
							acre.Add(new Point(x, y), LandType.Trees);
							break;
						case '#':
							acre.Add(new Point(x, y), LandType.Lumberyard);
							break;
					}
				}
				y++;
			}

			//var cursorPosition = new Point(Console.CursorLeft, Console.CursorTop);
			//Draw(acre, 0L);

			var minuteAcre = new Dictionary<long, Acre>(1000);
			var acreMinute = new Dictionary<int, long>(1000);
			var duplicateMinutes = new List<long>();
			for (var minute = 0L; minute < 1000000000L; minute++)
			{
				var newAcre = new Acre();
				foreach (var land in acre)
				{
					var newLand = land.Value;
					switch (land.Value)
					{
						case LandType.Ground:
							if (acre.HasAdjacentLandTypeCount(land.Key, LandType.Trees, 3))
								newLand = LandType.Trees;
							break;
						case LandType.Trees:
							if (acre.HasAdjacentLandTypeCount(land.Key, LandType.Lumberyard, 3))
								newLand = LandType.Lumberyard;
							break;
						case LandType.Lumberyard:
							if (!acre.HasAdjacentLandTypeCount(land.Key, LandType.Lumberyard, 1) ||
								!acre.HasAdjacentLandTypeCount(land.Key, LandType.Trees, 1))
								newLand = LandType.Ground;
							break;
					}
					newAcre[land.Key] = newLand;
				}
				acre = newAcre;
				

				minuteAcre.Add(minute, acre);
				var hash = acre.GetHashCode();
				if (!acreMinute.TryAdd(hash, minute))
				{
					var previousMinute = acreMinute[hash];
					var length = minute - previousMinute;
					var toGo = 1000000000L - minute - 1;
					var steps = toGo % length;
					var newMinute = previousMinute + steps;

					var acre2 = minuteAcre[newMinute];

					//Draw(acre, minute);
					//Draw(minuteAcre[previousMinute], previousMinute);
					//Draw(acre2, newMinute);
					return acre2.Values.Count(a => a == LandType.Trees) *
						acre2.Values.Count(a => a == LandType.Lumberyard);
				}
				// Draw(acre, minute, cursorPosition);
			}

			return acre.Values.Count(a => a == LandType.Trees) * 
				acre.Values.Count(a => a == LandType.Lumberyard);
		}

		private void Draw(Acre acre, long minute, Point? cursor = null)
		{
			if (cursor.HasValue)
				Console.SetCursorPosition(cursor.Value.X, cursor.Value.Y);

			Console.WriteLine($"Minute: {minute}");
			var size = (int)Math.Sqrt(acre.Count);
			for (var y = 0; y < size; y++)
			{
				for (var x = 0; x < size; x++)
				{
					switch (acre[new Point(x, y)])
					{
						case LandType.Ground:
							Console.Write('.');
							break;
						case LandType.Trees:
							Console.Write('|');
							break;
						case LandType.Lumberyard:
							Console.Write('#');
							break;
					}
				}
				Console.WriteLine();
			}
		}

		private class Acre : Dictionary<Point, LandType>
		{
			private readonly Size[] _offsets = new Size[] {
					new Size(-1,-1),
					new Size(-1, 0),
					new Size(-1, 1),
					new Size( 0,-1),
					new Size( 0, 1),
					new Size( 1,-1),
					new Size( 1, 0),
					new Size( 1, 1),
				};
			public bool HasAdjacentLandTypeCount(Point p, LandType landType, int needed)
			{
				var count = 0;
				foreach (var o in _offsets)
				{
					if (TryGetValue(Point.Add(p, o), out var t) && t == landType && ++count == needed)
						return true;
				}
				return false;
			}
			public new int GetHashCode()
			{
				var array = Values.ToArray();
				int hc = array.Length;
				for (int i = 0; i < array.Length; ++i)
				{
					hc = unchecked(hc * 314159 + (int)array[i]);
				}
				return hc;
			}
			public LandType[] ToArray()
			{
				return Values.ToArray();
			}
		}
		private enum LandType
		{
			Ground = 1,
			Trees = 2,
			Lumberyard = 3
		}
	}
}