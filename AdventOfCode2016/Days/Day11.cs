using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Days
{
	public class Day11 : Day
	{
		public Day11(string input)
			: base(11, "The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.\r\nThe second floor contains a hydrogen generator.\r\nThe third floor contains a lithium generator.\r\nThe fourth floor contains nothing relevant.")
		{
		}

		public override object RunPart1()
		{
			var floor = 1; // elevator start at floor 1
			var isotopes = Regex.Matches(Input, @"([a-z]*)\sgenerator").Cast<Match>().Select(m => m.Groups[1].Value.ToUpper()).ToList();
			var floorLayout = new FloorLayout();
			foreach (var line in Input.Split('\n').Select(l => l.Trim()))
			{
				floorLayout.Add(floor, isotopes.ToDictionary(i => i, i => new HashSet<char>()));
				foreach (Match m in Regex.Matches(line, @"([a-z]*)\sgenerator"))
					floorLayout[floor][m.Groups[1].Value.ToUpper()].Add('G');

				foreach (Match m in Regex.Matches(line, @"([a-z]*)\-compatible"))
					floorLayout[floor][m.Groups[1].Value.ToUpper()].Add('M');
				floor++;
			}
			floorLayout.Elevator = 1; // set elevator at floor 1
			floorLayout.Steps = 0;
			var floorLayouts = new HashSet<FloorLayout> {floorLayout};
			foreach (var layout in floorLayouts.ToList())
			{
				if (layout.IsFinished())
					return layout.Steps;

				Console.WriteLine(layout.ToString());
			}

			return null;
		}

		private class FloorLayout : Dictionary<int, Dictionary<string, HashSet<char>>>
		{
			public int Elevator { get; set; } // position of elevator
			public int Steps { get; set; } // steps to get to this layout

			public bool IsFinished()
			{
				// all elements need to be on floor 4, so elevator will be there when it buts the last element there.
				return Elevator == 4 && this[4].Values.All(e => e.Count == 2);
			}

			public bool IsValid()
			{
				foreach (var floor in Values)
				{
					var containsGenerator = floor.Values.Any(element => element.Contains('G'));
					var containsMicrochip = floor.Values.Any(element => element.Contains('M'));
					
					// geen microchips, geen generator == OK
					if (!containsGenerator && !containsMicrochip)
						continue; 
					
					// is there microchip without its generator?
					var containsMicrochipsWithoutGenerator = floor.Values.Any(element => element.Count == 1 && element.First() == 'M');
					if (containsGenerator && containsMicrochipsWithoutGenerator)
						return false;
				}
				return true;
			}

			public override string ToString()
			{
				var sb = new StringBuilder();
				foreach (var floor in this.Reverse())
				{
					sb.Append('F');
					sb.Append(floor.Key);
					sb.Append(' ');
					sb.Append(Elevator == floor.Key ? 'E' : '.');
					sb.Append(' ');
					sb.Append('|');
					sb.Append(' ');
					foreach (var element in floor.Value)
					{
						var name = element.Key;
						var generator = element.Value.Contains('G');
						var microchip = element.Value.Contains('M');
						sb.Append(generator ? new string(new[] {name[0], name[1], 'G', ' '}) : ".   ");
						sb.Append(microchip ? new string(new[] {name[0], name[1], 'M', ' '}) : ".   ");
					}
					sb.AppendLine();
				}
				return sb.ToString();
			}
		}


		public override object RunPart2()
		{
			return null;
		}
	}
}