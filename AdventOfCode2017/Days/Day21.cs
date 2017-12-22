using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2017.Days
{
	internal class Day21 : Day
	{
		public Day21(string input)
			: base(21, input)
		{
		}

		public override object RunPart1()
		{
			var rules = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(line => new Rule(line)).ToList();
			var image = new[]
			{
				new[] {'.', '#', '.'},
				new[] {'.', '.', '#'},
				new[] {'#', '#', '#'}
			};
			
			for (var iteration = 0; iteration < 5; iteration++)
			{
				var n = image.Length % 2 == 0 ? 2 : 3;
				var parts = image.Length / n;
				var newImage = new char[parts * (n+1)][];
				for (var i = 0; i < newImage.Length; i++)
					newImage[i] = new char[newImage.Length];
				for (var part1 = 0; part1 < parts; part1++)
				for (var part2 = 0; part2 < parts; part2++)
				{
					var gridPart = new char[n][];
					for (var i = 0; i < gridPart.Length; i++)
					{
						gridPart[i] = new char[gridPart.Length];
						Array.Copy(image[part1 * n + i], part2 * n, gridPart[i], 0, n);
					}
					var permutations = GetPermutations(gridPart);
					var replacement = rules.First(r => r.Matches(permutations)).Output;
					for (var i = 0; i < n + 1; i++)
						Array.Copy(replacement[i], 0, newImage[part1 * (n + 1) + i], part2 * (n + 1), n + 1);
				}
				image = newImage;
			}
			return image.Sum(c => c.Count(c2 => c2 == '#'));
		}
		
		public override object RunPart2()
		{
			var rules = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(line => new Rule(line)).ToList();
			var image = new[] {new[] {'.', '#', '.'}, new[] {'.', '.', '#'}, new[] {'#', '#', '#'}};
			var ruleCache = new ConcurrentDictionary<string, char[][]>();
			for (var iteration = 0; iteration < 18; iteration++)
			{
				var n = image.Length % 2 == 0 ? 2 : 3;
				var parts = image.Length / n;
				var newImage = new char[parts * (n + 1)][];
				for (var i = 0; i < newImage.Length; i++)
					newImage[i] = new char[newImage.Length];

				var image1 = image; // resharper -> modified closure
				Parallel.For(0, parts, part1 =>
				{
					for (var part2 = 0; part2 < parts; part2++)
					{
						var gridPart = new char[n][];
						for (var i = 0; i < gridPart.Length; i++)
						{
							gridPart[i] = new char[gridPart.Length];
							Array.Copy(image1[part1 * n + i], part2 * n, gridPart[i], 0, n);
						}

						var key = string.Join("|", gridPart.Select(c => new string(c)));
						var replacement = ruleCache.GetOrAdd(key, s => rules.First(r => r.Matches(GetPermutations(gridPart))).Output); // <- could also add all permutations...
						for (var i = 0; i < n + 1; i++)
							Array.Copy(replacement[i], 0, newImage[part1 * (n + 1) + i], part2 * (n + 1), n + 1);
					}
				});
				image = newImage;
			}
			return image.Sum(c => c.Count(c2 => c2 == '#'));
		}

		private static List<char[][]> GetPermutations(char[][] a)
		{
			var result = new List<char[][]>();
			result.Add(Clone(a));

			var rot90 = Clone(a);
			Rotate(rot90);
			result.Add(rot90);

			var rot180 = Clone(rot90);
			Rotate(rot180);
			result.Add(rot180);

			var rot270 = Clone(rot180);
			Rotate(rot270);
			result.Add(rot270);

			var flip = Clone(a);
			Flip(flip);
			result.Add(flip);

			var rot90Flip = Clone(rot90);
			Flip(rot90Flip);
			result.Add(rot90Flip);

			var rot180Flip = Clone(rot180);
			Flip(rot180Flip);
			result.Add(rot180Flip);

			var rot270Flip = Clone(rot270);
			Flip(rot270Flip);
			result.Add(rot270Flip);

			return result;
		}
		private static char[][] Clone(char[][] a)
		{
			var n = a.Length;
			var b = new char[n][];
			for (var i = 0; i < b.Length; i++)
				b[i] = (char[])a[i].Clone();
			return b;
		}
		private static void Rotate(char[][] a)
		{
			var n = a.Length;
			for (var i = 0; i < n; i += 1)
			{
				for (var j = i + 1; j < n; j += 1)
				{
					var t = a[i][j];
					a[i][j] = a[j][i];
					a[j][i] = t;
				}
			}
			for (var i = 0; i < n; i += 1)
			{
				for (var j = 0; j < n / 2; j += 1)
				{
					var t = a[i][j];
					a[i][j] = a[i][n - 1 - j];
					a[i][n - 1 - j] = t;
				}
			}
		}
		private static void Flip(char[][] a)
		{
			var n = a.Length;
			for (var i = 0; i < n; i += 1)
			{
				for (var j = 0; j < n / 2; j += 1)
				{
					var t = a[i][j];
					a[i][j] = a[i][n - 1 - j];
					a[i][n - 1 - j] = t;
				}
			}
		}

		private class Rule
		{
			public char[][] Output { get; }
			private readonly char[][] _input;
			private readonly int _pixelCount;

			public Rule(string line)
			{
				var parts = line.Split(" => ");
				_input = parts[0].Split('/').Select(s => s.ToCharArray()).ToArray();
				_pixelCount = _input.Sum(c => c.Count(c2 => c2 == '#'));
				Output = parts[1].Split('/').Select(s => s.ToCharArray()).ToArray();
			}
			public bool Matches(List<char[][]> grids)
			{
				var grid1 = grids[0];
				if (grid1.Length != _input.Length)
					return false;

				if (grid1.Sum(c => c.Count(c2 => c2 == '#')) != _pixelCount)
					return false;

				foreach (var grid in grids)
				{
					var correct = true;
					for (var i = 0; i < grid.Length; i++)
					{
						if (!grid[i].SequenceEqual(_input[i]))
						{
							correct = false;
							break;
						}
					}
					if (correct)
						return true;
				}
				return false;
			}
		}
	}
}
