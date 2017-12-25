using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AdventOfCode2017.Days;

namespace AdventOfCode2017
{
    internal static class Program
    {
		private static void Main(string[] args)
		{
			var days = new Day[]
			{
				new Day01(Resources.InputDay01),
				new Day02(Resources.InputDay02),
				new Day03(Resources.InputDay03),
				new Day04(Resources.InputDay04),
				new Day05(Resources.InputDay05),
				new Day06(Resources.InputDay06),
				new Day07(Resources.InputDay07),
				new Day08(Resources.InputDay08),
				new Day09(Resources.InputDay09),
				new Day10(Resources.InputDay10),
				new Day11(Resources.InputDay11),
				new Day12(Resources.InputDay12),
				new Day13(Resources.InputDay13),
				new Day14(Resources.InputDay14),
				new Day15(Resources.InputDay15),
				new Day16(Resources.InputDay16),
				new Day17(Resources.InputDay17),
				new Day18(Resources.InputDay18),
				new Day19(Resources.InputDay19),
				new Day20(Resources.InputDay20),
				new Day21(Resources.InputDay21),
				new Day22(Resources.InputDay22),
				new Day23(Resources.InputDay23),
				new Day24(Resources.InputDay24),
				new Day25(Resources.InputDay25)
				
			};

			Console.WriteLine(" * Advent of Code * ");
			Console.WriteLine("====================");
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
