using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Days
{
	internal class Day05 : Day
	{
		public override int DayNumber => 5;

		public override object RunPart1()
		{
			var maxSeatId = 0;
			foreach (var seat in GetInputLines())
			{
				var range = new Range(0, 127);
				foreach (var c in seat[..^3])
					range.UpdateRange(c);
				var row = range.Start;
				
				range = new Range(0, 7);
				foreach (var c in seat[^3..])
					range.UpdateRange(c);
				var column = range.Start;
				
				var id = ((row * 8) + column);
				if (id > maxSeatId)
					maxSeatId = id;
			}
			return maxSeatId;
		}

		public override object RunPart2()
		{
			var seatIds = new HashSet<int>();
			foreach (var seat in GetInputLines())
			{
				var range = new Range(0, 127);
				foreach (var c in seat[..^3])
					range.UpdateRange(c);
				var row = range.Start;

				range = new Range(0, 7);
				foreach (var c in seat[^3..])
					range.UpdateRange(c);
				var column = range.Start;

				var id = ((row * 8) + column);
				seatIds.Add(id);
			}

			return Enumerable.Range(seatIds.Min(), seatIds.Count)
				.Except(seatIds)
				.Single();
		}

		private class Range
		{
			public Range(int start, int end)
			{
				Start = start;
				End = end;
			}

			public int Start { get; private set; }
			public int End { get; private set; }

			public void UpdateRange(in char c)
			{
				var adjustment = ((End - Start) / 2) + 1;
				switch (c)
				{
					case 'F':
					case 'L':
						End -= adjustment;
						break;

					case 'B':
					case 'R':
						Start += adjustment;
						break;
				}
			}
		}
	}
}