using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2016.Days
{
	public class Day03 : Day
	{
		public Day03(string input)
			: base(3, input)
		{
		}

		public override object RunPart1()
		{
			var count = 0;
			foreach (var triangle in Input.Split('\n').Select(l => l.Trim()))
			{
				var sides = triangle.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
				var max = sides.Max();
				sides.Remove(max);
				if (sides.Sum() > max)
					count++;
			}
			return count;
		}

		public override object RunPart2()
		{
			int count = 0, max;
			List<int> sides;
			var items = Input.Split('\n').Select(l => l.Trim()).SelectMany(l => l.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)).ToArray();
			for(var i=0; i < items.Length; i+=9)
			{
				// triangle 1 
				sides = new List<int>{items[i], items[i+3] , items[i+6] };
				max = sides.Max();
				sides.Remove(max);
				if (sides.Sum() > max)
					count++;

				// triangle 2 
				sides = new List<int> { items[i+1], items[i + 4], items[i + 7] };
				max = sides.Max();
				sides.Remove(max);
				if (sides.Sum() > max)
					count++;

				// triangle 3 
				sides = new List<int> { items[i+2], items[i + 5], items[i + 8] };
				max = sides.Max();
				sides.Remove(max);
				if (sides.Sum() > max)
					count++;
			}
			return count;
		}
	}
}