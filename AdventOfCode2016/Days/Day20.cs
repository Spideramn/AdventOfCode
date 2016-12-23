using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdventOfCode2016.Days
{
	public class Day20 : Day
	{
		public Day20(string input)
			: base(20, input)
		{
		}

		public override object RunPart1()
		{
			var items = Input.Split('\n')
				.Select(l => new Item(l))
				.OrderBy(t => t.Low);

			var lastItem = items.First();
			foreach (var item in items.Skip(1))
			{
				if (lastItem.High + 1 < item.Low) // gap!
					return lastItem.High + 1;

				if(item.High> lastItem.High)
					lastItem.High = Math.Max(lastItem.High, item.High);
				
			}
			return null;
		}
		

		public override object RunPart2()
		{
			var items = Input.Split('\n')
				.Select(l => new Item(l))
				.OrderBy(t => t.Low);

			var count = 0L;
			var lastItem = items.First();
			foreach (var item in items.Skip(1))
			{
				if (lastItem.High + 1 < item.Low)
				{
					count += item.Low - lastItem.High - 1;
					lastItem = item;
				}
				else if (item.High > lastItem.High)
				{
					lastItem.High = Math.Max(lastItem.High, item.High);
				}
			}
			return count;
		}


		private class Item
		{
			public Item(string line)
			{
				var l = line.Trim().Split('-');
				Low = long.Parse(l[0]);
				High = long.Parse(l[1]);
			}
			
			public long Low { get; }
			public long High { get; set; }
		}
	}
}