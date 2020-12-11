using System.Collections.Generic;

namespace AdventOfCode2020.Days
{
	internal class Day06 : Day
	{
		public override int DayNumber => 6;

		public override object RunPart1()
		{
			var sum = 0;

			var group = new HashSet<char>();
			foreach (var line in GetInputLines())
			{
				if (string.IsNullOrWhiteSpace(line))
				{
					sum += group.Count;
					group = new HashSet<char>();
				}
				else
				{
					foreach (var c in line)
						group.Add(c);
				}
			}

			// add sum of last group
			sum += group.Count;

			return sum;
		}


		public override object RunPart2()
		{
			var sum = 0;

			HashSet<char> group = null;
			foreach (var person in GetInputLines())
			{
				if (string.IsNullOrWhiteSpace(person))
				{
					sum += group?.Count ?? 0;
					group = null;
				}
				else
				{
					if (group == null)
						group = new HashSet<char>(person);
					else
						group.IntersectWith(person); // remove items from group that are not in person
				}
			}

			// add sum of last group
			sum += group?.Count ?? 0;

			return sum;
		}
	}
}