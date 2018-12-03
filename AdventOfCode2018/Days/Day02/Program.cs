using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days.Day02
{
	internal class Program : Day
	{
		public override int DayNumber => 2;

		public override object RunPart1()
		{
			var two = 0;
			var three = 0;
			foreach (var line in GetInputLines())
			{
				var groups = line.GroupBy(c => c).ToList();
				if (groups.Any(g => g.Count() == 2))
					two++;
				if (groups.Any(g => g.Count() == 3))
					three++;
			}
			return two * three;
		}

		public override object RunPart2()
		{
			var list = GetInputLines().ToList();
			foreach (var item in list)
			{
				var result = CheckItem(item, list);
				if (result != null)
					return result;
			}
			return "NOT FOUND";
		}

		private static string CheckItem(string item, IEnumerable<string> list)
		{
			foreach (var item2 in list)
			{
				if (item == item2)
					continue;
				var diffCount = 0;
				var diffIndex = 0;
				for (var index = 0; index < item.Length; index++)
				{
					if (item[index] != item2[index])
					{
						if (++diffCount > 1)
							break;
						diffIndex = index;
					}
				}
				if (diffCount == 1)
					return item.Substring(0, diffIndex) + item.Substring(diffIndex + 1);
			}
			return null;
		}
	}
}