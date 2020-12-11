using System.Drawing;
using System.Linq;

namespace AdventOfCode2020.Days
{
	internal class Day03 : Day
	{
		public override int DayNumber => 3;

		public override object RunPart1()
		{
			var map = GetInputLines().Select(line => line.ToCharArray()).ToArray();
			return CountTrees(map, new Size(3, 1));
		}

		public override object RunPart2()
		{
			var map = GetInputLines().Select(line => line.ToCharArray()).ToArray();
			var result = 1L;
			var slopes = new []
			{
				new Size(1,1),
				new Size(3,1),
				new Size(5,1),
				new Size(7,1),
				new Size(1,2),
			};
			foreach (var slope in slopes)
				result *= CountTrees(map, slope);
			return result;
		}

		private static int CountTrees(char[][] map, Size step)
		{
			var treeCount = 0;
			var pos = new Point(step);
			while (pos.Y < map.Length)
			{
				// check tile for a tree
				var line = map[pos.Y];
				var tile = line[pos.X % line.Length];
				if (tile == '#')
					treeCount++;

				// move
				pos += step;
			}

			return treeCount;
		}
	}
}