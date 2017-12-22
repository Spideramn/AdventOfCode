using System;
using System.Collections.Generic;
using System.Linq;

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
				if (image.Length % 2 == 0)
				{
					var parts = image.Length / 2;
					var newImage = new char[parts * 3][];
					for (var i = 0; i < newImage.Length; i++)
						newImage[i] = new char[newImage.Length];
					for (var part1 = 0; part1 < parts; part1++)
					for (var part2 = 0; part2 < parts; part2++)
					{
						var gridPart = new char[2][];
						gridPart[0] = image[part1 * 2 + 0].Skip(part2 * 2).Take(2).ToArray();
						gridPart[1] = image[part1 * 2 + 1].Skip(part2 * 2).Take(2).ToArray();

						var permutations = GetPermutations(gridPart);
						var replacement = rules.First(r => permutations.Any(r.Matches)).Output;
						Array.Copy(replacement[0], 0, newImage[part1 * 3 + 0], part2 * 3, 3);
						Array.Copy(replacement[1], 0, newImage[part1 * 3 + 1], part2 * 3, 3);
						Array.Copy(replacement[2], 0, newImage[part1 * 3 + 2], part2 * 3, 3);
					}
					image = newImage;
				}
				else if (image.Length % 3 == 0)
				{
					var parts = image.Length / 3;
					var newImage = new char[parts * 4][];
					for (var i = 0; i < newImage.Length; i++)
						newImage[i] = new char[newImage.Length];

					for (var part1 = 0; part1 < parts; part1++)
					for (var part2 = 0; part2 < parts; part2++)
					{
						var gridPart = new char[3][];
						gridPart[0] = image[part1 * 3 + 0].Skip(part2 * 3).Take(3).ToArray();
						gridPart[1] = image[part1 * 3 + 1].Skip(part2 * 3).Take(3).ToArray();
						gridPart[2] = image[part1 * 3 + 2].Skip(part2 * 3).Take(3).ToArray();

						var permutations = GetPermutations(gridPart);
						var replacement = rules.First(r => permutations.Any(r.Matches)).Output;
						Array.Copy(replacement[0], 0, newImage[part1 * 4 + 0], part2 * 4, 4);
						Array.Copy(replacement[1], 0, newImage[part1 * 4 + 1], part2 * 4, 4);
						Array.Copy(replacement[2], 0, newImage[part1 * 4 + 2], part2 * 4, 4);
						Array.Copy(replacement[3], 0, newImage[part1 * 4 + 3], part2 * 4, 4);
					}
					image = newImage;
				}
			}
			return image.Sum(c => c.Count(c2 => c2 == '#'));
		}
		
		public override object RunPart2()
		{
			var rules = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(line => new Rule(line)).ToList();
			var image = new[]
			{
				new[] {'.', '#', '.'},
				new[] {'.', '.', '#'},
				new[] {'#', '#', '#'}
			};

			for (var iteration = 0; iteration < 18; iteration++)
			{
				if (image.Length % 2 == 0)
				{
					var parts = image.Length / 2;
					var newImage = new char[parts * 3][];
					for (var i = 0; i < newImage.Length; i++)
						newImage[i] = new char[newImage.Length];
					for (var part1 = 0; part1 < parts; part1++)
						for (var part2 = 0; part2 < parts; part2++)
						{
							var gridPart = new char[2][];
							gridPart[0] = image[part1 * 2 + 0].Skip(part2 * 2).Take(2).ToArray();
							gridPart[1] = image[part1 * 2 + 1].Skip(part2 * 2).Take(2).ToArray();

							var permutations = GetPermutations(gridPart);
							var replacement = rules.First(r => permutations.Any(r.Matches)).Output;
							Array.Copy(replacement[0], 0, newImage[part1 * 3 + 0], part2 * 3, 3);
							Array.Copy(replacement[1], 0, newImage[part1 * 3 + 1], part2 * 3, 3);
							Array.Copy(replacement[2], 0, newImage[part1 * 3 + 2], part2 * 3, 3);
						}
					image = newImage;
				}
				else if (image.Length % 3 == 0)
				{
					var parts = image.Length / 3;
					var newImage = new char[parts * 4][];
					for (var i = 0; i < newImage.Length; i++)
						newImage[i] = new char[newImage.Length];

					for (var part1 = 0; part1 < parts; part1++)
						for (var part2 = 0; part2 < parts; part2++)
						{
							var gridPart = new char[3][];
							gridPart[0] = image[part1 * 3 + 0].Skip(part2 * 3).Take(3).ToArray();
							gridPart[1] = image[part1 * 3 + 1].Skip(part2 * 3).Take(3).ToArray();
							gridPart[2] = image[part1 * 3 + 2].Skip(part2 * 3).Take(3).ToArray();

							var permutations = GetPermutations(gridPart);
							var replacement = rules.First(r => permutations.Any(r.Matches)).Output;
							Array.Copy(replacement[0], 0, newImage[part1 * 4 + 0], part2 * 4, 4);
							Array.Copy(replacement[1], 0, newImage[part1 * 4 + 1], part2 * 4, 4);
							Array.Copy(replacement[2], 0, newImage[part1 * 4 + 2], part2 * 4, 4);
							Array.Copy(replacement[3], 0, newImage[part1 * 4 + 3], part2 * 4, 4);
						}
					image = newImage;
				}
			}
			return image.Sum(c => c.Count(c2 => c2 == '#'));
		}

		private static List<char[][]> GetPermutations(char[][] a)
		{
			// id, rot90, rot180, rot270, 
			// flipX, flipX after rot90, flipX after rot180, flipX after rot270
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
			for (int i = 0; i < n; i += 1)
			{
				for (int j = i + 1; j < n; j += 1)
				{
					var t = a[i][j];
					a[i][j] = a[j][i];
					a[j][i] = t;
				}
			}
			for (int i = 0; i < n; i += 1)
			{
				for (int j = 0; j < n / 2; j += 1)
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
			for (int i = 0; i < n; i += 1)
			{
				for (int j = 0; j < n / 2; j += 1)
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

			public Rule(string line)
			{
				var parts = line.Split(" => ");
				_input = parts[0].Split('/').Select(s => s.ToCharArray()).ToArray();
				Output = parts[1].Split('/').Select(s => s.ToCharArray()).ToArray();
			}

			public bool Matches(char[][] grid)
			{
				if (grid.Length != _input.Length)
					return false;
				for (var i = 0; i < grid.Length; i++)
				{
					if (!grid[i].SequenceEqual(_input[i]))
						return false;
				}
				return true;
			}
		}
	}
}
