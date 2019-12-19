using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Days.Day08
{
	internal class Program : Day
	{
		public override int DayNumber => 8;

		public override object RunPart1()
		{
			using (var pixels = GetInputString().Select(c => c - 48).GetEnumerator())
			{
				const int width = 25;
				const int height = 6;
				var picture = new List<int[][]>();
				while (true)
				{
					var layer = new int[height][];
					picture.Add(layer);
					for (var y = 0; y < height; y++)
					{
						layer[y] = new int[width];
						for (var x = 0; x < width; x++)
						{
							if (!pixels.MoveNext())
								goto Break;
							layer[y][x] = pixels.Current;
						}
					}
				}
				Break:

				// remove last layer
				picture.RemoveAt(picture.Count - 1);

				var hash = 0;
				var minDigits = int.MaxValue;
				foreach (var layer in picture)
				{
					var layerPixels = layer.SelectMany(l => l).ToArray();
					var zeroDigits = layerPixels.Count(p => p == 0);
					if (zeroDigits < minDigits)
					{
						minDigits = zeroDigits;
						hash = layerPixels.Count(p => p == 1) * layerPixels.Count(p => p == 2);
					}
				}

				return hash;
			}
		}

		public override object RunPart2()
		{
			using (var pixels = GetInputString().Select(c => c - 48).GetEnumerator())
			{
				const int width = 25;
				const int height = 6;
				var layers = new List<int[][]>();
				while (true)
				{
					var layer = new int[height][];
					layers.Add(layer);
					for (var y = 0; y < height; y++)
					{
						layer[y] = new int[width];
						for (var x = 0; x < width; x++)
						{
							if (!pixels.MoveNext())
								goto Break;
							layer[y][x] = pixels.Current;
						}
					}
				}
				Break:

				// remove last layer
				layers.RemoveAt(layers.Count - 1);

				// "draw" picture
				for (var y = 0; y < height; y++)
				{
					for (var x = 0; x < width; x++)
					{
						foreach (var pixel in layers.Select(layer => layer[y][x]))
						{
							if(pixel == 0)
							{
								Console.Write(' ');
								break;
							}
							if (pixel == 1)
							{
								Console.Write('█');
								break;
							}
						}
					}
					Console.WriteLine();
				}
				return null;
			}
		}
	}
}