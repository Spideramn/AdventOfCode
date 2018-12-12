using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.Days.Day11
{
	internal class Program : Day
	{
		public override int DayNumber => 11;

		public override object RunPart1()
		{
			var serial = int.Parse(GetInputString());

			var maxPower = 0L;
			var coord = "0,0";
			for (var y = 1; y <= 298; y++)
			for (var x = 1; x <= 298; x++)
			{
				var power = 
					(((((((x + 0 + 10) * (y + 0)) + serial) * (x + 0 + 10)) / 100) % 10) - 5) +
					(((((((x + 0 + 10) * (y + 1)) + serial) * (x + 0 + 10)) / 100) % 10) - 5) +
					(((((((x + 0 + 10) * (y + 2)) + serial) * (x + 0 + 10)) / 100) % 10) - 5) +
					(((((((x + 1 + 10) * (y + 0)) + serial) * (x + 1 + 10)) / 100) % 10) - 5) +
					(((((((x + 1 + 10) * (y + 1)) + serial) * (x + 1 + 10)) / 100) % 10) - 5) +
					(((((((x + 1 + 10) * (y + 2)) + serial) * (x + 1 + 10)) / 100) % 10) - 5) +
					(((((((x + 2 + 10) * (y + 0)) + serial) * (x + 2 + 10)) / 100) % 10) - 5) +
					(((((((x + 2 + 10) * (y + 1)) + serial) * (x + 2 + 10)) / 100) % 10) - 5) +
					(((((((x + 2 + 10) * (y + 2)) + serial) * (x + 2 + 10)) / 100) % 10) - 5);
				if (power > maxPower)
				{
					maxPower = power;
					coord = x + "," + y;
				}
			}

			return coord;
		}

		public override object RunPart2()
		{
			var serial = int.Parse(GetInputString());
			return Enumerable.Range(1, 299)
				.AsParallel()
				.Select(y =>
				{
					var maxPower = 0L;
					var result = "0,0,0";
					for (var x = 1; x <= 299; x++)
					{
						var maxSize = Math.Min(300 - x, 300 - y);
						for (var size = maxSize - 1; size >= 1; size--)
						{
							var power = 0L;
							var coordinatesToGo = size * size;
							for (var yOffset = 0; yOffset < size; yOffset++)
							for (var xOffset = 0; xOffset < size; xOffset++)
							{
								power += (((((((x + xOffset + 10) * (y + yOffset)) + serial) * (x + xOffset + 10)) / 100) % 10) - 5);
								// max power per coordinate is 4...
								// so if (coordinates to go * 4) is not enough to reach max power, quit!
								if (power + (coordinatesToGo-- * 4L) < maxPower)
									goto Next;
							}
							if (power > maxPower)
							{
								maxPower = power;
								result = x + "," + y + "," + size;
							}
							Next: ;
						}
					}
					return (maxPower, result);
				})
				.OrderByDescending(c => c.maxPower)
				.First()
				.result;
		}
	}
}