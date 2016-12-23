using System;
using System.Linq;

namespace AdventOfCode.Days
{
	public class Day16 : Day
	{
		public Day16(string input)
			: base(16, input)
		{
		}

		public override object RunPart1()
		{
			var sue = new[] {"children: 3", "cats: 7", "samoyeds: 2", "pomeranians: 3", "akitas: 0", "vizslas: 0", "goldfish: 5", "trees: 3", "cars: 2", "perfumes: 1"};
			return Input
				.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split(new[] {':'}, 2))
				.Where(parts => parts[1].Trim().Replace(", ", ",").Split(',').All(sue.Contains))
				.Select(parts => parts[0].Split(' ')[1])
				.FirstOrDefault();
		}

		public override object RunPart2()
		{
			var sue = new[] {"children: 3", "cats: 7", "samoyeds: 2", "pomeranians: 3", "akitas: 0", "vizslas: 0", "goldfish: 5", "trees: 3", "cars: 2", "perfumes: 1"}
				.ToDictionary(s => s.Split(':')[0], s => int.Parse(s.Split(':')[1]));
			foreach(var line in Input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Split(new[] {':'}, 2)))
			{
				var valid = true;
				foreach (var part in line[1].Trim().Replace(", ", ",").Split(',').ToDictionary(s => s.Split(':')[0], s => int.Parse(s.Split(':')[1])))
				{
					switch (part.Key)
					{
						case "cats":
						case "trees":
							if (part.Value <= sue[part.Key])
								valid = false;
							break;

						case "pomeranians":
						case "goldfish":
							if (part.Value >= sue[part.Key])
								valid = false;
							break;

						default:
							if (sue[part.Key] != part.Value)
								valid = false;
							break;
					}
				}
				if (valid)
					return line[0].Split(' ')[1];
			}
			return null;
		}
	}
}