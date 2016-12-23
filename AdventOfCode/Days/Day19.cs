using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Lib;

namespace AdventOfCode.Days
{
	public class Day19 : Day
	{
		public Day19(string input)
			: base(19, @"
e => H
e => O
H => HO
H => OH
O => HH

HOHOHO")
		{
		}

		public override object RunPart1()
		{
			var start = "";
			var replacements = new List<KeyValuePair<string, string>>();
			foreach(var line in Input.Split(new [] {'\r','\n'}, StringSplitOptions.RemoveEmptyEntries))
			{
				var parts = line.Split(new[] {'=', '>'}, StringSplitOptions.RemoveEmptyEntries);
				if(parts.Length == 2)
					replacements.Add(new KeyValuePair<string, string>(parts[0].Trim(), parts[1].Trim()));
				else
					start = line.Trim();
			}
			
			var sets = new HashSet<string>();
			foreach (var replacement in replacements)
			{
				int i;
				var si = 0;
				while((i = start.IndexOf(replacement.Key, si, StringComparison.Ordinal)) != -1)
				{
					sets.Add(string.Concat(start.Substring(0, i), replacement.Value, start.Substring(i + replacement.Key.Length, start.Length - (i + replacement.Key.Length))));
					si = i+1;
				}
			}

			return sets.Count;
		}

		public override object RunPart2()
		{
			var end = "";
			var replacements = new List<KeyValuePair<string, string>>();
			foreach (var line in Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
			{
				var parts = line.Split(new[] { '=', '>' }, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length == 2)
					replacements.Add(new KeyValuePair<string, string>(parts[0].Trim(), parts[1].Trim()));
				else
					end = line.Trim();
			}
			
			var min = Compute("e", end, replacements).Min();

			return min;
		}



		private IEnumerable<int> Compute(string start, string end, List<KeyValuePair<string, string>> replacements, int counter = 0)
		{
			if (start == end)
				yield return counter;
			if(!replacements.Any() || start.Length > end.Length)
				yield return int.MaxValue;

			foreach(var replacement in replacements)
			{
				if (start.Contains(replacement.Key))
				{
					int i;
					var si = 0;
					while ((i = start.IndexOf(replacement.Key, si, StringComparison.Ordinal)) != -1)
					{
						var s = string.Concat(start.Substring(0, i), replacement.Value, start.Substring(i + replacement.Key.Length, start.Length - (i + replacement.Key.Length)));
						var r = replacements.ToList();
						r.Remove(replacement);
						foreach (var c in Compute(s, end, r, counter++))
							yield return c;
						si = i + 1;
					}
				}
			}
		}
	}
}