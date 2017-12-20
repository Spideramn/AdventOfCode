using System;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day19 : Day
	{
		public Day19(string input)
			: base(19, input)
		{
		}

		public override object RunPart1()
		{
			var maze = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray()).ToArray();
			var direction = 'd';
			var y = 0;
			var x = Array.IndexOf(maze[y], '|');
			var secret = "";
			do
			{
				if (direction == 'd')
					y++;
				else if (direction == 'u')
					y--;
				else if (direction == 'l')
					x--;
				else if (direction == 'r')
					x++;

				if (!BoundsCheck(maze, y, x))
					break;

				var newTile = maze[y][x];
				if (newTile == ' ')
					break; // the end...!
				if (newTile == '+') // switch direction
				{
					if (direction == 'd' || direction == 'u')
					{
						if (BoundsCheck(maze, y, x - 1) && (maze[y][x - 1] == '-' || (maze[y][x - 1] > 'A' - 1 && maze[y][x - 1] < 'Z' + 1)))
							direction = 'l';
						else if (BoundsCheck(maze, y, x + 1) && (maze[y][x + 1] == '-' || (maze[y][x + 1] > 'A' - 1 && maze[y][x + 1] < 'Z' + 1)))
							direction = 'r';
						else
							break; // death end...
					}
					else if (direction == 'l' || direction == 'r')
					{
						if (BoundsCheck(maze, y - 1, x) && (maze[y - 1][x] == '|' || (maze[y-1][x] > 'A' - 1 && maze[y-1][x] < 'Z' + 1)))
							direction = 'u';
						else if (BoundsCheck(maze, y + 1, x) && (maze[y + 1][x] == '|' || (maze[y+1][x] > 'A' - 1 && maze[y+1][x] < 'Z' + 1)))
							direction = 'd';
						else
							break; // death end...
					}
				}
				else if (newTile == '|' || newTile == '-')
				{
					// keep on going...
				}
				else if (newTile > 'A' - 1 && newTile < 'Z' + 1)
				{
					secret += newTile;
				}
				else
					throw new Exception("Unknown tile: " + newTile);
				
			} while (true);
			return secret;
		}

		private bool BoundsCheck(char[][] maze, int y, int x)
		{
			if (x < 0)
				return false;
			if (y < 0)
				return false;
			if (y >= maze.Length)
				return false;
			if (x >= maze[0].Length)
				return false;
			return true;
		}

		public override object RunPart2()
		{
			var maze = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray()).ToArray();
			var direction = 'd';
			var y = 0;
			var x = Array.IndexOf(maze[y], '|');
			var steps = 0;
			do
			{
				steps++;
				if (direction == 'd')
					y++;
				else if (direction == 'u')
					y--;
				else if (direction == 'l')
					x--;
				else if (direction == 'r')
					x++;

				if (!BoundsCheck(maze, y, x))
					break;

				var newTile = maze[y][x];
				if (newTile == ' ')
					break; // the end...!
				if (newTile == '+') // switch direction
				{
					if (direction == 'd' || direction == 'u')
					{
						if (BoundsCheck(maze, y, x - 1) && (maze[y][x - 1] == '-' || (maze[y][x - 1] > 'A' - 1 && maze[y][x - 1] < 'Z' + 1)))
							direction = 'l';
						else if (BoundsCheck(maze, y, x + 1) && (maze[y][x + 1] == '-' || (maze[y][x + 1] > 'A' - 1 && maze[y][x + 1] < 'Z' + 1)))
							direction = 'r';
						else
							break; // death end...
					}
					else if (direction == 'l' || direction == 'r')
					{
						if (BoundsCheck(maze, y - 1, x) && (maze[y - 1][x] == '|' || (maze[y - 1][x] > 'A' - 1 && maze[y - 1][x] < 'Z' + 1)))
							direction = 'u';
						else if (BoundsCheck(maze, y + 1, x) && (maze[y + 1][x] == '|' || (maze[y + 1][x] > 'A' - 1 && maze[y + 1][x] < 'Z' + 1)))
							direction = 'd';
						else
							break; // death end...
					}
				}
				else if (newTile == '|' || newTile == '-')
				{
					// keep on going...
				}
				else if (newTile > 'A' - 1 && newTile < 'Z' + 1)
				{
					// keep on going...
				}
				else
					throw new Exception("Unknown tile: " + newTile);

			} while (true);
			return steps;
		}
	}
}
