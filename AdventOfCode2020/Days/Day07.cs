using System.Linq;
using System.Text.RegularExpressions;
using BagData = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, int>>;

namespace AdventOfCode2020.Days
{
	internal class Day07 : Day
	{
		public override int DayNumber => 7;

		public override object RunPart1()
		{
			var bagData = new BagData();
			foreach (var line in GetInputLines())
			{
				foreach (Match match in Regex.Matches(line, @"(?<bag>.*) bags contain(?: (?<contains>\d+ .*?) bags?[,.])*", RegexOptions.Compiled))
				{
					var bagItems = match.Groups["contains"].Captures
						.Select(c => c.Value.Split(' ', 2))
						.ToDictionary(
							p => p[1], 
							p => int.Parse(p[0]));
					bagData.Add(match.Groups["bag"].Value, bagItems);
				}
			}

			return bagData.Count(bag => CanContain(bagData, bag.Key, "shiny gold"));
		}
		
		private static bool CanContain(BagData bagData, string bagName, string searchingFor)
		{
			if (bagData[bagName].Count == 0)
				return false;

			if (bagData[bagName].ContainsKey(searchingFor))
				return true;

			foreach (var bag in bagData[bagName])
			{
				if (CanContain(bagData, bag.Key, searchingFor))
					return true;
			}
			return false;
		}

		public override object RunPart2()
		{
			var bagData = new BagData();
			foreach (var line in GetInputLines())
			{
				foreach (Match match in Regex.Matches(line, @"(?<bag>.*) bags contain(?: (?<contains>\d+ .*?) bags?[,.])*", RegexOptions.Compiled))
				{
					var bagItems = match.Groups["contains"].Captures
						.Select(c => c.Value.Split(' ', 2))
						.ToDictionary(
							p => p[1],
							p => int.Parse(p[0]));
					bagData.Add(match.Groups["bag"].Value, bagItems);
				}
			}

			return CountBags(bagData, "shiny gold");
		}

		private static int CountBags(BagData bagData, string bagName)
		{
			var counter = 0;
			foreach (var bag in bagData[bagName])
				counter += bag.Value * (CountBags(bagData, bag.Key) + 1);
			return counter;
		}
	}
}