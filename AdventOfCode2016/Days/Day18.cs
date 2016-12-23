using System.Linq;

namespace AdventOfCode2016.Days
{
	public class Day18 : Day
	{
		public Day18(string input)
			: base(18, input)
		{
		}

		public override object RunPart1()
		{
			return Run(40);
		}

		public override object RunPart2()
		{
			return Run(400000);
		}

		private int Run(int rows)
		{
			var tiles = Input.Select(c => c == '.').ToArray();
			var count = tiles.Count(t => t);
			var length = tiles.Length;
			for (var row = 1; row < rows; row++)
			{
				var nextRow = new bool[length];
				for (var i = 0; i < length; i++)
				{
					var left = (i - 1 < 0) || tiles[i - 1];
					var right = (i + 1 == length) || tiles[i + 1];
					var center = tiles[i];

					if (!left && !center && right)
						nextRow[i] = false;
					else if (left && !center && !right)
						nextRow[i] = false;
					else if (!left && center && right)
						nextRow[i] = false;
					else if (left && center && !right)
						nextRow[i] = false;
					else
					{
						nextRow[i] = true;
						count++;
					}
				}
				tiles = nextRow;
			}
			return count;
		}
	}
}