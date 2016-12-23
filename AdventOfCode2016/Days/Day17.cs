using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016.Days
{
	public class Day17 : Day
	{
		public Day17(string input)
			: base(17, input)
		{
		}

		public override object RunPart1()
		{
			return Run(Input).First();
		}

		public override object RunPart2()
		{
			return Run(Input).Last().Length;
		}

		private IEnumerable<string> Run(string passcode)
		{
			var openNodes = new Queue<Node>();
			openNodes.Enqueue(new Node(new Point(0, 0), ""));
			using (var md5 = MD5.Create())
			{
				while (openNodes.Count > 0)
				{
					var node = openNodes.Dequeue();
					if (node.Location.X == 3 && node.Location.Y == 3)
					{
						yield return node.Path;
						continue;
					}

					// next nodes
					foreach (var d in AdjecentLocations(md5, passcode, node.Location, node.Path))
						openNodes.Enqueue(new Node(d.Item2, node.Path + d.Item1));
				}
			}
		}

		public IEnumerable<Tuple<char, Point>> AdjecentLocations(MD5 md5, string passcode, Point location, string path)
		{
			var b = md5.ComputeHash(Encoding.ASCII.GetBytes(passcode + path));
			if (location.Y - 1 > -1 && (b[0] >> 4) > 0xA) // up
				yield return new Tuple<char, Point>('U', new Point(location.X, location.Y - 1));

			if (location.Y + 1 < 4 && (b[0] & 0xF) > 0xA) // down
				yield return new Tuple<char, Point>('D', new Point(location.X, location.Y + 1));

			if (location.X - 1 > -1 && (b[1] >> 4) > 0xA) // left
				yield return new Tuple<char, Point>('L', new Point(location.X - 1, location.Y));

			if (location.X + 1 < 4 && (b[1] & 0xF) > 0xA) // right
				yield return new Tuple<char, Point>('R', new Point(location.X + 1, location.Y));
		}
		
		private class Node
		{
			public Node(Point location, string path)
			{
				Location = location;
				Path = path;
			}

			public Point Location { get; }
			public string Path { get; }
		}
	}
}