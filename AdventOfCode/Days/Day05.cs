using System;
using System.Linq;

namespace AdventOfCode.Days
{
	public class Day05 : Day
	{
		public Day05(string input)
			:base(5, input)
		{
		}

		public override object RunPart1()
		{
			var niceStrings = 0;

			foreach (var l in Resources.InputDay5.Split('\n'))
			{
				var line = l.Trim();

				// check 1
				var count = "aeiou".Sum(c => line.Count(s => s == c));
				if (count < 3)
					continue;

				// check 2
				var found = false;
				for (var i = 0; i < line.Length - 1; i++)
				{
					var c1 = line[i];
					var c2 = line[i + 1];
					if (c1 == c2)
					{
						found = true;
						break;
					}
				}
				if (!found)
					continue;

				// check 3
				if (new[] {"ab", "cd", "pq", "xy"}.Any(line.Contains))
					continue;

				niceStrings++;
			}

			return (niceStrings);
		}

		public override object RunPart2()
		{
			var niceStrings = 0;

			foreach (var l in Resources.InputDay5.Split('\n'))
			{
				var line = l.Trim();

				// check 1
				var found1 = false;
				for (var i = 0; i < line.Length - 1; i++)
				{
					var c1 = line[i];
					var c2 = line[i + 1];
					var s = new string(new[] {c1, c2});

					var pos = line.IndexOf(s, i + 2, StringComparison.Ordinal);
					if (pos > -1)
					{
						found1 = true;
						break;
					}
				}
				if (!found1)
					continue;

				// check 2
				var found = false;
				for (var i = 0; i < line.Length - 2; i++)
				{
					var c1 = line[i];
					var c2 = line[i + 2];
					if (c1 == c2)
					{
						found = true;
						break;
					}
				}
				if (!found)
					continue;


				niceStrings++;
			}

			return (niceStrings);
		}
	}
}