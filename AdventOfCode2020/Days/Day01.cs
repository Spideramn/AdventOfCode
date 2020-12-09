using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days
{
	internal class Day01 : Day
	{
		public override int DayNumber => 1;

		public override object RunPart1()
		{
			var report = new HashSet<int>(GetInputLines().Select(int.Parse));
			foreach (var item in report.Where(item => report.Contains(2020 - item)))
				return item * (2020 - item);
			return "Not Found";
		}

		public override object RunPart2()
		{
			var report = new HashSet<int>(GetInputLines().Select(int.Parse));
			var sums = new Dictionary<int, List<ValueTuple<int, int>>>();
			
			foreach (var item1 in report)
			foreach (var item2 in report)
			{
				if(item1 == item2) continue;

				var sum = item1 + item2;
				if(sum > 2020) continue;
				
				if(!sums.ContainsKey(sum))
					sums.Add(sum, new List<ValueTuple<int, int>>());
				sums[sum].Add(new ValueTuple<int, int>(item1, item2));
			}

			foreach (var item in report)
			{
				var needed = 2020 - item;
				if (!sums.ContainsKey(needed)) continue;
				var (item1, item2) = sums[needed][0];
				return item * item1 * item2;
			}

			return "Not Found";
		}
	}
}
