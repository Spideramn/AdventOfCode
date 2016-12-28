using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AdventOfCode2016.Days;
using AdventOfCode2016.Lib;

namespace AdventOfCode2016
{
	static class Program
	{
		static void Main(string[] args)
		{
			var days = new Day[]
			{
				new Day01(ResourceManager.GetString("InputDay01")),
				new Day02(ResourceManager.GetString("InputDay02")),
				new Day03(ResourceManager.GetString("InputDay03")),
				new Day04(ResourceManager.GetString("InputDay04")),
				new Day05(ResourceManager.GetString("InputDay05")),
				new Day06(ResourceManager.GetString("InputDay06")),
				new Day07(ResourceManager.GetString("InputDay07")),
				new Day08(ResourceManager.GetString("InputDay08")),
				new Day09(ResourceManager.GetString("InputDay09")),
				new Day10(ResourceManager.GetString("InputDay10")),
				new Day11(ResourceManager.GetString("InputDay11")),
				new Day12(ResourceManager.GetString("InputDay12")),
				new Day13(ResourceManager.GetString("InputDay13")),
				new Day14(ResourceManager.GetString("InputDay14")),
				new Day15(ResourceManager.GetString("InputDay15")),
				new Day16(ResourceManager.GetString("InputDay16")),
				new Day17(ResourceManager.GetString("InputDay17")),
				new Day18(ResourceManager.GetString("InputDay18")),
				new Day19(ResourceManager.GetString("InputDay19")),
				new Day20(ResourceManager.GetString("InputDay20")),
				new Day21(ResourceManager.GetString("InputDay21")),
				new Day22(ResourceManager.GetString("InputDay22")),
				new Day23(ResourceManager.GetString("InputDay23")),
				new Day24(ResourceManager.GetString("InputDay24")),
				new Day25(ResourceManager.GetString("InputDay25")),
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
				if(input == null)
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
#if DEBUG && NETCORE
				if (Debugger.IsAttached)
					break;
#endif
			}
		}
	}
}