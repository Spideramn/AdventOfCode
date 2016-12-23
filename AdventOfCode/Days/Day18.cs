using System;
using System.Linq;

namespace AdventOfCode.Days
{
	public class Day18 : Day
	{
		public Day18(string input)
			: base(18, input)
		{
		}

		public override object RunPart1()
		{
			var lines = Input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
			var gridSize = lines.Length;
			var grid = new bool[gridSize, gridSize];
			for (var y = 0; y < gridSize; y++) 
				for (var x = 0; x < gridSize; x++)
					grid[x, y] = lines[y][x] == '#';

			for (var step = 0; step < 100; step++)
			{
				var newGrid = new bool[lines.Length, lines.Length];
				for (var y = 0; y < gridSize; y++)
					for (var x = 0; x < gridSize; x++)
					{
						var neighbors = 0;
						// 123
						// 8#4
						// 765
						// 1)
						if (x > 0 && y > 0)
							if (grid[x - 1, y - 1])
								neighbors++;
						// 2)
						if (y > 0)
							if (grid[x, y - 1])
								neighbors++;
						// 3)
						if (x < gridSize - 1 && y > 0)
							if (grid[x + 1, y - 1])
								neighbors++;
						// 4)
						if (x < gridSize - 1)
							if (grid[x + 1, y])
								neighbors++;
						// 5)
						if (x < gridSize - 1 && y < gridSize - 1)
							if (grid[x + 1, y + 1])
								neighbors++;
						// 6)
						if (y < gridSize - 1)
							if (grid[x, y + 1])
								neighbors++;
						// 7)
						if (x > 0 && y < gridSize - 1)
							if (grid[x - 1, y + 1])
								neighbors++;
						// 8)
						if (x > 0)
							if (grid[x - 1, y])
								neighbors++;

						if (grid[x, y])
							newGrid[x, y] = neighbors == 2 || neighbors == 3;
						else
							newGrid[x, y] = neighbors == 3;
					}
				grid = newGrid;
			}
			
			return grid.Cast<bool>().Count(l => l);
		}

		public override object RunPart2()
		{
			var lines = Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			var gridSize = lines.Length;
			var grid = new bool[gridSize, gridSize];
			for (var y = 0; y < gridSize; y++)
				for (var x = 0; x < gridSize; x++)
				{
					if ((x == 0 && y == 0) || (x == 0 && y == gridSize - 1) || (x == gridSize - 1 && y == gridSize - 1) || (x == gridSize - 1 && y == 0))
						grid[x, y] = true;
					else
						grid[x, y] = lines[y][x] == '#';
				}

			for (var step = 0; step < 100; step++)
			{
				var newGrid = new bool[lines.Length, lines.Length];
				for (var y = 0; y < gridSize; y++)
					for (var x = 0; x < gridSize; x++)
					{
						if ((x == 0 && y == 0) || (x == 0 && y == gridSize - 1) || (x == gridSize - 1 && y == gridSize - 1) || (x == gridSize - 1 && y == 0))
						{
							newGrid[x, y] = true;
							continue;
						}
						
						var neighbors = 0;
						// 123
						// 8#4
						// 765
						// 1)
						if (x > 0 && y > 0)
							if (grid[x - 1, y - 1])
								neighbors++;
						// 2)
						if (y > 0)
							if (grid[x, y - 1])
								neighbors++;
						// 3)
						if (x < gridSize - 1 && y > 0)
							if (grid[x + 1, y - 1])
								neighbors++;
						// 4)
						if (x < gridSize - 1)
							if (grid[x + 1, y])
								neighbors++;
						// 5)
						if (x < gridSize - 1 && y < gridSize - 1)
							if (grid[x + 1, y + 1])
								neighbors++;
						// 6)
						if (y < gridSize - 1)
							if (grid[x, y + 1])
								neighbors++;
						// 7)
						if (x > 0 && y < gridSize - 1)
							if (grid[x - 1, y + 1])
								neighbors++;
						// 8)
						if (x > 0)
							if (grid[x - 1, y])
								neighbors++;

						if (grid[x, y])
							newGrid[x, y] = neighbors == 2 || neighbors == 3;
						else
							newGrid[x, y] = neighbors == 3;
					}
				grid = newGrid;
			}

			return grid.Cast<bool>().Count(l => l);
		}
	}
}