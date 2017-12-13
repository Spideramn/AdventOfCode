using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day13 : Day
	{
		public Day13(string input)
			: base(13, input)
		{
		}

		public override object RunPart1()
		{
			var entries = Input
				.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split(':').Select(int.Parse).ToArray())
				.ToDictionary(parts => parts[0], parts => parts[1]);

			/*
			Console.WriteLine("Initial state:");
			SeverityPrint(entries, 0, -1, true);
			var severity = 0;
			var picoSecond = 0;
			for (var position = 0; position <= entries.Keys.Max(); position++)
			{
				Console.WriteLine($"Picosecond {picoSecond}:");
				SeverityPrint(entries, picoSecond++, position, true);
				severity += SeverityPrint(entries, picoSecond, position, true).Severity;
				Console.WriteLine();
			}
			return severity;
			*/
			return entries.Sum(entry => Severity(entry.Value, entry.Key, entry.Key).Severity);
		}

		public override object RunPart2()
		{
			var entries = Input
				.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split(':').Select(int.Parse).ToArray())
				.ToDictionary(parts => parts[0], parts => parts[1]);

			var delay = 0;
			while (true)
			{
				var save = true;
				foreach (var entry in entries)
				{
					if (Severity(entry.Value, entry.Key + delay, entry.Key).Caught)
					{
						save = false;
						break;
					}
				}
				if(save)
					return delay;
				delay++;
			}
		}


		// started with the "SeverityPrint" version below. minimised to this. 
		private static (bool Caught, int Severity) Severity(int depth, int picoSecond, int position)
		{
			if (picoSecond % (depth + depth - 2) == 0)
				return (true, position * depth);
			return (false, 0);
		}

		// print complete state at picoSecond and position
		private static (bool Caught, int Severity) SeverityPrint(Dictionary<int, int> entries, int picoSecond, int position, bool print)
		{
			var caught = false;
			var severity = 0;
			var length = entries.Keys.Max() + 1;
			if (print)
			{
				for (var i = 0; i < length; i++)
					Console.Write($"{i,2}  ");
				Console.WriteLine();
			}
			var maxDepth = entries.Values.Max();
			for (var depth = 0; depth < maxDepth; depth++)
			{
				var line = "";
				for (var i = 0; i < length; i++)
				{
					if (entries.ContainsKey(i))
					{
						if (depth < entries[i])
						{
							var p = picoSecond % (entries[i] + entries[i] - 2);
							if (p >= entries[i])
								p = entries[i] - ((p - entries[i]) + 2);

							if (depth == 0 && position == i)
							{
								if (p == depth)
								{
									if (!print)
										return (true, i * entries[i]);
									severity = i * entries[i];
									caught = true;
									line += "(S) ";
								}
								else if (print)
								{
									line += "( ) ";
								}
							}
							else if (print)
							{
								if (p == depth)
									line += "[S] ";
								else
									line += "[ ] ";
							}
						}
						else if (print)
						{
							line += "    ";
						}
					}
					else if (print)
					{
						if (depth == 0 && position == i)
							line += "(.) ";
						else if (depth == 0)
							line += "... ";
						else
							line += "    ";
					}
				}
				if (print)
					Console.WriteLine(line);
			}
			if (print)
				Console.WriteLine();
			return (caught, severity);
		}
	}
}
