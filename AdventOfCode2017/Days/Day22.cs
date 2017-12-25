using System;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode2017.Days
{
	internal class Day22 : Day
	{
		public Day22(string input)
			: base(22, input)
		{
		}

		public override object RunPart1()
		{
			var infected = new HashSet<string>();
			var split = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			for (var y = 0; y < split.Length; y++)
			{
				for (var x = 0; x < split[y].Length; x++)
				{
					if (split[y][x] == '#')
						infected.Add(x + "," + y);
				}
			}

			var infectionCounter = 0;
			var direction = 0; // 0 north 1 east 2 south 3 west
			var currentNode = new Point(split.Length / 2, split.Length / 2);
			for (var burts = 0; burts < 10000; burts++)
			{
				var key = currentNode.X + "," + currentNode.Y;
				if (infected.Contains(key))
				{
					direction = ++direction > 3 ? 0 : direction;
					infected.Remove(key);
				}
				else
				{
					direction = --direction < 0 ? 3 : direction;
					infected.Add(key);
					infectionCounter++;
				}
				if (direction == 0)
					currentNode.Y--;
				else if (direction == 1)
					currentNode.X++;
				else if (direction == 2)
					currentNode.Y++;
				else if (direction == 3)
					currentNode.X--;
			}

			return infectionCounter;
		}

		public override object RunPart2()
		{
			var infected = new Dictionary<string, char>();
			var split = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			for (var y = 0; y < split.Length; y++)
			{
				for (var x = 0; x < split[y].Length; x++)
				{
					if (split[y][x] == '#')
						infected.Add(x + "," + y, 'i');
				}
			}

			var infectionCounter = 0;
			var direction = 0; // 0 north 1 east 2 south 3 west
			var currentNode = new Point(split.Length / 2, split.Length / 2);
			for (var burts = 0; burts < 10000000; burts++)
			{
				var key = currentNode.X + "," + currentNode.Y;
				var state = 'c';
				if (infected.ContainsKey(key))
					state = infected[key];

				switch (state)
				{
					case 'c':
						direction = (direction + 3) % 4;
						infected.Add(key, 'w');
						break;
					case 'w':
						infected[key] = 'i';
						infectionCounter++;
						break;
					case 'i':
						direction = (direction + 1) % 4;
						infected[key] = 'f';
						break;
					case 'f':
						direction = (direction + 2) % 4;
						infected.Remove(key);
						break;
				}
				switch (direction)
				{
					case 0:
						currentNode.Y--;
						break;
					case 1:
						currentNode.X++;
						break;
					case 2:
						currentNode.Y++;
						break;
					case 3:
						currentNode.X--;
						break;
				}
			}
			return infectionCounter;
		}
	}
}
