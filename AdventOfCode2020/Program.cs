using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AdventOfCode2020.Days;

namespace AdventOfCode2020
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			var days = new Day[]
			{
				new Day01(),
				new Day02(),
				new Day03(),
				new Day04(),
				new Day05(),
				new Day06(),
				new Day07(),
				new Day08(),
				new Day09(),
				new Day10(),
				new Day11(),
				new Day12(),
				new Day13(),
				new Day14(),
				new Day15(),
				new Day16(),
				new Day17(),
				new Day18(),
				new Day19(),
				new Day20(),
				new Day21(),
				new Day22(),
				new Day23(),
				new Day24(),
				new Day25()
			};

			Console.WriteLine(" * Advent of Code 2019 * ");
			Console.WriteLine("=========================");
			Console.WriteLine("Processor count: {0}", Environment.ProcessorCount);
			Console.WriteLine();

			var defaultInput = Math.Min(DateTime.Now.Day, 25);
			var input = args.FirstOrDefault();
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
						Console.WriteLine("    in: {0:#0.000} ms", stopwatch.Elapsed.TotalMilliseconds);

						stopwatch = Stopwatch.StartNew();
						Console.WriteLine("Part 2: {0}", day.RunPart2());
						Console.WriteLine("    in: {0:#0.000} ms", stopwatch.Elapsed.TotalMilliseconds);
					}
					else
						Console.WriteLine("Day not found");
				}
				else
				{
					Console.WriteLine("Invalid input");
				}


				Console.WriteLine();

				input = null;
			}
		}
	}
}
