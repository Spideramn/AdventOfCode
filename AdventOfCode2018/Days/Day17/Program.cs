using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.Days.Day17
{
	internal class Program : Day
	{
		public override int DayNumber => 17;

		public override object RunPart1()
		{
			var maxWaterY = 0;
			var soil = new SoilList { { new Point(500, 0), SoilType.FlowingWater } };
			foreach (var line in GetInputLines())
			{
				var parts = line.Split(", ");
				var a = int.Parse(parts[0].Substring(2));
				var b = parts[1].Substring(2).Split("..").Select(int.Parse).ToArray();
				var c = Enumerable.Range(b[0], b[1] - b[0] + 1);
				if (parts[0][0] == 'x')
				{
					var x = a;
					foreach (var y in c)
						soil.TryAdd(new Point(x, y), SoilType.Clay);
				}
				else
				{
					var y = a;
					foreach (var x in c)
						soil.TryAdd(new Point(x, y), SoilType.Clay);
				}
			}

			//var cursorPosition = new Point(Console.CursorLeft, Console.CursorTop);
			//Draw(soil, maxWaterY);
			//Console.ReadKey();

			var waterChanged = true;
			while (waterChanged)
			{
				waterChanged = false;
				var flowingWaterTiles = soil
					.Where(c => c.Value == SoilType.FlowingWater)
					.Select(c => c.Key)
					.ToList();
				foreach (var current in flowingWaterTiles)
				{
					var below = Point.Add(current, new Size(0, 1));
					switch (soil.Get(below))
					{
						case SoilType.FlowingWater:
							// keep flowing
							break;
						case SoilType.RestingWater:
						case SoilType.Clay:
							{
								var contained = soil.IsContained(current);
								if (contained != null)
								{
									for (var x = contained.Value.Left; x <= contained.Value.Right; x++)
										soil[new Point(x, current.Y)] = SoilType.RestingWater;
									waterChanged = true;
								}
								else
								{
									// flow left / right
									var left = Point.Add(current, new Size(-1, 0));
									if (soil.Get(left) == SoilType.Sand)
									{
										soil[left] = SoilType.FlowingWater;
										waterChanged = true;
									}
									var right = Point.Add(current, new Size(1, 0));
									if (soil.Get(right) == SoilType.Sand)
									{
										soil[right] = SoilType.FlowingWater;
										waterChanged = true;
									}
								}
							}
							break;
						case SoilType.Sand:
							if (below.Y <= soil.MaxY)
							{
								soil[below] = SoilType.FlowingWater;
								maxWaterY = Math.Max(maxWaterY, below.Y);
								waterChanged = true;
							}
							break;
					}
				}

				//Draw(soil, maxWaterY, cursorPosition);
				//if (Console.ReadKey().Key == ConsoleKey.Q)
				//	break;
			}

			return soil.Count(s =>
				(s.Value == SoilType.FlowingWater || s.Value == SoilType.RestingWater) &&
				!(s.Key.Y < soil.MinY || s.Key.Y > soil.MaxY)
				);
		}

		public override object RunPart2()
		{
			var maxWaterY = 0;
			var soil = new SoilList { { new Point(500, 0), SoilType.FlowingWater } };
			foreach (var line in GetInputLines())
			{
				var parts = line.Split(", ");
				var a = int.Parse(parts[0].Substring(2));
				var b = parts[1].Substring(2).Split("..").Select(int.Parse).ToArray();
				var c = Enumerable.Range(b[0], b[1] - b[0] + 1);
				if (parts[0][0] == 'x')
				{
					var x = a;
					foreach (var y in c)
						soil.TryAdd(new Point(x, y), SoilType.Clay);
				}
				else
				{
					var y = a;
					foreach (var x in c)
						soil.TryAdd(new Point(x, y), SoilType.Clay);
				}
			}

			//var cursorPosition = new Point(Console.CursorLeft, Console.CursorTop);
			//Draw(soil, maxWaterY);
			//Console.ReadKey();

			var waterChanged = true;
			while (waterChanged)
			{
				waterChanged = false;
				var flowingWaterTiles = soil
					.Where(c => c.Value == SoilType.FlowingWater)
					.Select(c => c.Key)
					.ToList();
				foreach (var current in flowingWaterTiles)
				{
					var below = Point.Add(current, new Size(0, 1));
					switch (soil.Get(below))
					{
						case SoilType.FlowingWater:
							// keep flowing
							break;
						case SoilType.RestingWater:
						case SoilType.Clay:
							{
								var contained = soil.IsContained(current);
								if (contained != null)
								{
									for (var x = contained.Value.Left; x <= contained.Value.Right; x++)
										soil[new Point(x, current.Y)] = SoilType.RestingWater;
									waterChanged = true;
								}
								else
								{
									// flow left / right
									var left = Point.Add(current, new Size(-1, 0));
									if (soil.Get(left) == SoilType.Sand)
									{
										soil[left] = SoilType.FlowingWater;
										waterChanged = true;
									}
									var right = Point.Add(current, new Size(1, 0));
									if (soil.Get(right) == SoilType.Sand)
									{
										soil[right] = SoilType.FlowingWater;
										waterChanged = true;
									}
								}
							}
							break;
						case SoilType.Sand:
							if (below.Y <= soil.MaxY)
							{
								soil[below] = SoilType.FlowingWater;
								maxWaterY = Math.Max(maxWaterY, below.Y);
								waterChanged = true;
							}
							break;
					}
				}

				//Draw(soil, maxWaterY, cursorPosition);
				//if (Console.ReadKey().Key == ConsoleKey.Q)
				//	break;
			}

			return soil.Count(s =>
				(s.Value == SoilType.RestingWater) &&
				!(s.Key.Y < soil.MinY || s.Key.Y > soil.MaxY)
				);
		}

		private enum SoilType
		{
			RestingWater,
			FlowingWater,
			Clay,
			Sand
		}
		private class SoilList : Dictionary<Point, SoilType>
		{
			public int MaxX { get; private set; } = 0;
			public int MaxY { get; private set; } = 0;
			public int MinX { get; private set; } = int.MaxValue;
			public int MinY { get; private set; } = int.MaxValue;

			public SoilType Get(Point p)
			{
				return this.GetValueOrDefault(p, SoilType.Sand);
			}
			public SoilType Get(int x, int y)
			{
				return this.GetValueOrDefault(new Point(x, y), SoilType.Sand);
			}


			public (int Left, int Right)? IsContained(Point p)
			{
				// check left
				var left = -1;
				for (var x = p.X - 1; x >= MinX; x--)
				{
					var below = Get(x, p.Y + 1);
					if (below != SoilType.Clay && below != SoilType.RestingWater)
						break;
					if (Get(x, p.Y) == SoilType.Clay)
					{
						left = x + 1;
						break;
					}
				}
				if (left == -1) return null;

				// check right
				var right = -1;
				for (var x = p.X + 1; x <= MaxX; x++)
				{
					var below = Get(x, p.Y + 1);
					if (below != SoilType.Clay && below != SoilType.RestingWater)
						break;
					if (Get(x, p.Y) == SoilType.Clay)
					{
						right = x - 1;
						break;
					}
				}
				if (right == -1) return null;

				return (left, right);
			}


			public new bool TryAdd(Point key, SoilType value)
			{
				if (value == SoilType.Clay)
				{
					MinX = Math.Min(key.X, MinX);
					MinY = Math.Min(key.Y, MinY);
					MaxX = Math.Max(key.X, MaxX);
					MaxY = Math.Max(key.Y, MaxY);
				}
				return base.TryAdd(key, value);
			}
		}

		private void Draw(SoilList soil, int maxWaterY, Point? cursor = null)
		{
			if (cursor.HasValue)
				Console.SetCursorPosition(cursor.Value.X, cursor.Value.Y);

			for (var y = soil.MinY - 1; y <= soil.MaxY + 1; y++)
			{
				Console.Write((y < soil.MinY || y > soil.MaxY) ? 'X' : ' ');
				Console.Write(y == maxWaterY ? '>' : ' ');
				for (var x = soil.MinX - 1; x <= soil.MaxX + 1; x++)
				{
					var point = new Point(x, y);

					if (point.Y == 0 && point.X == 500)
						Console.Write('+');
					else
						switch (soil.GetValueOrDefault(point, SoilType.Sand))
						{
							case SoilType.RestingWater:
								Console.Write('~');
								break;
							case SoilType.FlowingWater:
								Console.Write('|');
								break;
							case SoilType.Clay:
								Console.Write('#');
								break;
							case SoilType.Sand:
								Console.Write('.');
								break;
						}
					Console.ResetColor();
				}
				Console.Write((y < soil.MinY || y > soil.MaxY) ? 'X' : ' ');
				Console.WriteLine(y == maxWaterY ? '<' : ' ');
			}
		}
	}
}