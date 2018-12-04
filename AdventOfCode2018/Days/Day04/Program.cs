using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace AdventOfCode2018.Days.Day04
{
	internal class Program : Day
	{
		public override int DayNumber => 4;

		public override object RunPart1()
		{
			var actions = GetInputLines()
				.Select(line => (Date: DateTime.Parse(line.Substring(1, 16)), Line: line.Substring(19)))
				.OrderBy(l => l.Date);

			var currentGuard=-1;
			var sleeps = 0;
			var guards = new Dictionary<int, List<int>>();
			foreach (var (date, line) in actions)
			{
				switch (line.Substring(0, 5))
				{
					case "Guard":
						currentGuard = int.Parse(line.Split(new[] {' '}, 3, StringSplitOptions.RemoveEmptyEntries)[1].Substring(1));
						break;
					case "falls":
						sleeps = date.Minute;
						break;
					case "wakes":
						if (guards.ContainsKey(currentGuard))
							guards[currentGuard].AddRange(Enumerable.Range(sleeps, date.Minute - sleeps));
						else
							guards[currentGuard] = Enumerable.Range(sleeps, date.Minute - sleeps).ToList();
						break;
				}
			}

			var guardMostAsleep = guards.OrderByDescending(kv => kv.Value.Count).FirstOrDefault();
			var guardId = guardMostAsleep.Key;
			var groupedMinutes = guardMostAsleep.Value.GroupBy(i => i).OrderByDescending(gr => gr.Count()).First().Key;
			return guardId * groupedMinutes;
		}

		public override object RunPart2()
		{
			var actions = GetInputLines()
				.Select(line => (Date: DateTime.Parse(line.Substring(1, 16)), Line: line.Substring(19)))
				.OrderBy(l => l.Date);

			var currentGuard = -1;
			var sleeps = 0;
			var guards = new Dictionary<int, List<int>>();
			foreach (var (date, line) in actions)
			{
				switch (line.Substring(0, 5))
				{
					case "Guard":
						currentGuard = int.Parse(line.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries)[1].Substring(1));
						break;
					case "falls":
						sleeps = date.Minute;
						break;
					case "wakes":
						if (guards.ContainsKey(currentGuard))
							guards[currentGuard].AddRange(Enumerable.Range(sleeps, date.Minute - sleeps));
						else
							guards[currentGuard] = Enumerable.Range(sleeps, date.Minute - sleeps).ToList();
						break;
				}
			}

			var sortedGuards = new List<(int guardId, int minute, int count)>();
			foreach (var g in guards)
			{
				var mostAsleep = g.Value.GroupBy(i => i).OrderByDescending(gr => gr.Count()).First();
				sortedGuards.Add((g.Key, mostAsleep.Key, mostAsleep.Count()));
			}
			return sortedGuards.OrderByDescending(g => g.count).Select(g => g.guardId * g.minute).First();
		}
	}
}