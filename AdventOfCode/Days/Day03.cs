using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode.Days
{
	public class Day03 : Day
	{
		public Day03(string input)
			:base(3, input)
		{
		}

		public override object RunPart1()
		{
			var houses = new Dictionary<Point,int>();
			var point = new Point(0, 0);
			houses.Add(point,1);
			foreach(var chr in Resources.InputDay3)
			{
				switch (chr)
				{
					case '^':
						point.Y++;
						break;
					case 'v':
						point.Y--;
						break;
					case '<':
						point.X--;
						break;
					case '>':
						point.X++;
						break;
				}
				if (houses.ContainsKey(point))
					houses[point]++;
				else
					houses.Add(point, 1);
			}
			return houses.Count;
		}
		public override object RunPart2()
		{
			var houses = new Dictionary<Point, int>();
			houses.Add(new Point(0, 0), 1);

			var locationSanta = new Point(0, 0);
			var locationRoboSanta = new Point(0, 0);

			var i = 0;
			foreach (var chr in Resources.InputDay3)
			{
				i++;

				var point = i % 2 == 0 ? locationRoboSanta : locationSanta;
				switch (chr)
				{
					case '^':
						point.Y++;
						break;
					case 'v':
						point.Y--;
						break;
					case '<':
						point.X--;
						break;
					case '>':
						point.X++;
						break;
				}
				if (houses.ContainsKey(point))
					houses[point]++;
				else
					houses.Add(point, 1);

				if (i % 2 == 0)
					locationRoboSanta = point;
				else
					locationSanta = point;
			}
			return houses.Count;
		}
	}
}
