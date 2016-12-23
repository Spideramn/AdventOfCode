using System;
using System.Collections.Generic;

namespace AdventOfCode.Days
{
	public class Day02 : Day
	{
		public Day02(string input)
			:base(2, input)
		{
		}
		public override object RunPart1()
		{
			var totalSurface = 0;

			foreach(var l in Resources.InputDay2.Split('\n'))
			{
				var line = l.Trim();

				var dimensions = line.Split('x');
				var length = int.Parse(dimensions[0]);
				var width = int.Parse(dimensions[1]);
				var height = int.Parse(dimensions[2]);

				var side1 = length*width;
				var side2 = width*height;
				var side3 = height*length;
				
				var surface = (2 * side1) + (2 * side2) + (2 * side3);
				surface += Math.Min(Math.Min(side1, side2), side3);

				totalSurface += surface;
			}

			return totalSurface;
		}

		public override object RunPart2()
		{
			var totalRibbon = 0;
			foreach (var l in Resources.InputDay2.Split('\n'))
			{
				var line = l.Trim();
				var dimensions = line.Split('x');
				var length = int.Parse(dimensions[0]);
				var width = int.Parse(dimensions[1]);
				var height = int.Parse(dimensions[2]);

				// wrap!
				var list = new List<int> {length, width, height};
				list.Sort();
				var s1 = list[0];
				var s2 = list[1];
				var wrap = s1 + s1 + s2 + s2;
				totalRibbon += wrap;

				// bow
				var bow = length*width*height;
				totalRibbon += bow;
			}

			return totalRibbon;
		}
	}
}
