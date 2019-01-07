using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.Days.Day20
{
	internal class Program : Day
	{
		public override int DayNumber => 20;

		public override object RunPart1()
		{
			var input = new Queue<char>(GetInputString("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$"));
			var position = new Point(0, 0);
			var left = new Size(-1, 0);
			var right = new Size(1, 0);
			var up = new Size(0, -1);
			var down = new Size(0, 1);
			var map = new Dictionary<Point, HashSet<char>> { { position, new HashSet<char>() } };
			var backtracker = new Stack<Point>();
			while (input.TryDequeue(out var direction))
			{
				switch (direction)
				{
					case 'E':
						map[position].Add('E');
						position = Point.Add(position, right);
						if (map.ContainsKey(position))
							map[position].Add('W');
						else
							map[position] = new HashSet<char> { 'W' };
						break;
					case 'W':
						map[position].Add('W');
						position = Point.Add(position, left);
						if (map.ContainsKey(position))
							map[position].Add('E');
						else
							map[position] = new HashSet<char> { 'E' };
						break;
					case 'N':
						map[position].Add('N');
						position = Point.Add(position, up);
						if (map.ContainsKey(position))
							map[position].Add('S');
						else
							map[position] = new HashSet<char> { 'S' };
						break;
					case 'S':
						map[position].Add('S');
						position = Point.Add(position, down);
						if (map.ContainsKey(position))
							map[position].Add('N');
						else
							map[position] = new HashSet<char> { 'N' };
						break;
					case '(':
						backtracker.Push(position);
						break;
					case '|':
						if (input.Peek() == ')')
						{
							input.Dequeue();
							position = backtracker.Pop();
						}
						else
							position = backtracker.Peek();
						break;
					case ')':
						position = backtracker.Pop();
						break;
				}
			}
			DrawMap(map);

			var longestPath = 0;
			var sortedPoints = map.Keys.OrderByDescending(p => Math.Abs(p.X) + Math.Abs(p.Y)).ToList();
			var roomsVisited = new HashSet<Point> { Point.Empty };

			foreach (var destination in sortedPoints)
			{
				if (roomsVisited.Contains(destination))
					continue;


				Point[] path; // insert pathfinder

				foreach (var p in path)
					roomsVisited.Add(p);
				longestPath = Math.Max(longestPath, path.Length);
			}

			return longestPath;
		}

		private void DrawMap(Dictionary<Point, HashSet<char>> map)
		{
			var minX = map.Keys.Min(p => p.X);
			var maxX = map.Keys.Max(p => p.X);
			var minY = map.Keys.Min(p => p.Y);
			var maxY = map.Keys.Max(p => p.Y);
						
			Console.WriteLine(new string('#', ((maxX - minX + 1)*2) +1));
			for (var y = minY; y <= maxY; y++)
			{
				for (var x = minX; x <= maxX; x++)
				{
					var point = new Point(x,y);
					Console.Write(map.TryGetValue(point, out var room) && room.Contains('W') ? '|': '#');
					Console.Write(Point.Empty.Equals(point) ? 'X' : '.');
				}
				Console.WriteLine('#');
				for (var x = minX; x <= maxX; x++)
					Console.Write(map.TryGetValue(new Point(x, y), out var room) && room.Contains('S') ? "#-" : "##");
				Console.WriteLine('#');
			}
			Console.WriteLine();
		}
	}
}