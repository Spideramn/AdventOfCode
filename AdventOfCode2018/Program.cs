using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AdventOfCode2018.Days;

namespace AdventOfCode2018
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var days = new Day[]
			{
				new Days.Day01.Program(),
				new Days.Day02.Program(),
				new Days.Day03.Program(),
				new Days.Day04.Program(),
				new Days.Day05.Program(),
				new Days.Day06.Program(),
				new Days.Day07.Program(),
				new Days.Day08.Program(),
				new Days.Day09.Program(),
				new Days.Day10.Program(),
				new Days.Day11.Program(),
				new Days.Day12.Program(),
				new Days.Day13.Program(),
				new Days.Day14.Program(),
				new Days.Day15.Program(),
				new Days.Day16.Program(),
				new Days.Day17.Program(),
				new Days.Day18.Program(),
				new Days.Day19.Program(),
				new Days.Day20.Program(),
				new Days.Day21.Program(),
				new Days.Day22.Program(),
				new Days.Day23.Program(),
				new Days.Day24.Program(),
				new Days.Day25.Program()
			};

			Console.WriteLine(" * Advent of Code 2018 * ");
			Console.WriteLine("=========================");
			Console.WriteLine("Processor count: {0}", Environment.ProcessorCount);
			Console.WriteLine();

			var defaultInput = Math.Min(DateTime.Now.Day, 25);
			var input = args.FirstOrDefault();
#if DEBUG
			if (Debugger.IsAttached)
				input = input ?? defaultInput.ToString(CultureInfo.InvariantCulture);
#endif
			while (true)
			{
				Console.Write("Enter day number 1-25 (q to quit) [{0}]: ", defaultInput);
				var dayNumber = 0;
				if (input == null)
					input = Console.ReadLine();
				else
					Console.WriteLine();

				if (string.IsNullOrEmpty(input))
					dayNumber = defaultInput;
				if (input == "q")
					break;
				if (dayNumber != 0 || int.TryParse(input, out dayNumber))
				{
					if (dayNumber == -1)
						break;

					var day = days.FirstOrDefault(i => i.DayNumber == dayNumber);
					if (day != null)
					{
						Console.WriteLine("Running Day {0}", day.DayNumber);

						var stopwatch = Stopwatch.StartNew();

						Console.WriteLine("Part 1: {0}", day.RunPart1());
						Console.WriteLine("    in: {0:#0.0000} seconds", stopwatch.Elapsed.TotalSeconds);

						stopwatch = Stopwatch.StartNew();
						Console.WriteLine("Part 2: {0}", day.RunPart2());
						Console.WriteLine("    in: {0:#0.0000} seconds", stopwatch.Elapsed.TotalSeconds);
					}
					else
						Console.WriteLine("Day not found");
				}
				else
					Console.WriteLine("Invalid day");
				Console.WriteLine();

				input = null;
			}
		}
	}
}
