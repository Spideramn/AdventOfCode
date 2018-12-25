using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.Days.Day13
{
	internal class Program : Day
	{
		public override int DayNumber => 13;

		public override object RunPart1()
		{
			var carts = new List<Cart>();
			var maze = GetInputLines().Select(line => line.ToCharArray()).ToArray();
			for (var y = 0; y < maze.Length; y++)
			{
				var line = maze[y];
				for (var x = 0; x < line.Length; x++)
				{
					switch (line[x])
					{
						case '<':
						case '>':
							carts.Add(new Cart(x, y, line[x]));
							line[x] = '-';
							break;
						case '^':
						case 'v':
							carts.Add(new Cart(x, y, line[x]));
							line[x] = '|';
							break;
					}
				}
			}

			while (true)
			{
				foreach (var cart in carts.OrderBy(c => c.Location.Y).ThenBy(c => c.Location.X))
				{
					cart.Move(maze);
					if (carts.Count(c => c.Location == cart.Location) > 1)
						return $"{cart.Location.X},{cart.Location.Y}";
				}
			}
		}

		public override object RunPart2()
		{
			var carts = new List<Cart>();
			var maze = GetInputLines().Select(line => line.ToCharArray()).ToArray();
			for (var y = 0; y < maze.Length; y++)
			{
				var line = maze[y];
				for (var x = 0; x < line.Length; x++)
				{
					switch (line[x])
					{
						case '<':
						case '>':
							carts.Add(new Cart(x, y, line[x]));
							line[x] = '-';
							break;
						case '^':
						case 'v':
							carts.Add(new Cart(x, y, line[x]));
							line[x] = '|';
							break;
					}
				}
			}

			while (carts.Count(c => !c.HasCrashed) > 1)
			{
				foreach (var cart in carts.OrderBy(c => c.Location.Y).ThenBy(c => c.Location.X))
				{
					if (cart.HasCrashed)
						continue;

					cart.Move(maze);

					var cartsAtSamePosition = carts.Where(c => !c.HasCrashed && c.Location == cart.Location).ToArray();
					if (cartsAtSamePosition.Length > 1)
					{
						foreach (var c in cartsAtSamePosition)
							c.HasCrashed = true;
					}
				}
			}
			var lastCart = carts.Single(c => !c.HasCrashed);
			return $"{lastCart.Location.X},{lastCart.Location.Y}";
		}

		private class Cart
		{
			public Point Location;
			public char State { get; private set; }
			public int Counter { get; private set; }
			public bool HasCrashed { get; set; }
			public Cart(int x, int y, char state)
			{
				State = state;
				Location = new Point(x, y);
			}

			public void Move(char[][] maze)
			{
				switch (State)
				{
					case '<':
						Location.Offset(-1, 0);
						break;
					case '>':
						Location.Offset(1, 0);
						break;
					case '^':
						Location.Offset(0, -1);
						break;
					case 'v':
						Location.Offset(0, 1);
						break;
				}

				switch (maze[Location.Y][Location.X])
				{
					case '\\':
						switch (State)
						{
							case '<':
								State = '^';
								break;
							case '>':
								State = 'v';
								break;
							case '^':
								State = '<';
								break;
							case 'v':
								State = '>';
								break;
						}
						break;
					case '/':
						switch (State)
						{
							case '<':
								State = 'v';
								break;
							case '>':
								State = '^';
								break;
							case '^':
								State = '>';
								break;
							case 'v':
								State = '<';
								break;
						}
						break;
					case '+':
						switch (Counter % 3)
						{
							// turn left
							case 0:
								switch (State)
								{
									case '<':
										State = 'v';
										break;
									case '>':
										State = '^';
										break;
									case '^':
										State = '<';
										break;
									case 'v':
										State = '>';
										break;
								}
								break;

							// turn right
							case 2:
								switch (State)
								{
									case '<':
										State = '^';
										break;
									case '>':
										State = 'v';
										break;
									case '^':
										State = '>';
										break;
									case 'v':
										State = '<';
										break;
								}
								break;
						}
						Counter++;
						break;
				}

			}
		}
	}
}