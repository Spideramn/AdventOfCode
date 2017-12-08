using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day07 : Day
	{
		public Day07(string input)
			: base(07, input)
		{
		}

		public override object RunPart1()
		{
			var programs = new Dictionary<string, int>();
			foreach (var line in Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
			{
				var p1 = line.Split(" -> ");
				var p2 = p1[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var names = p1.Length > 1 ? p1[1].Split(", ", StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
				names.Add(p2[0]);
				foreach (var name in names)
				{
					if (programs.ContainsKey(name))
						programs[name]++;
					else
						programs.Add(name, 1);
				}
			}
			return programs.SingleOrDefault(p => p.Value == 1).Key;
		}


		

		public override object RunPart2()
		{
			var programs = new Dictionary<string, Program>();
			foreach (var line in Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
			{
				var p = line.Split(" -> ");
				var p2 = p[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var program = new Program(p2[0]);
				program.Holding = p.Length > 1 ? p[1].Split(", ", StringSplitOptions.RemoveEmptyEntries).ToArray() : new string[0];
				program.Weight = int.Parse(p2[1].Trim('(', ')'));
				programs.Add(program.Name, program);
			}
			foreach (var program in programs.Values)
			{
				foreach (var p in program.Holding)
				{
					programs[p].Parent = program;
					program.Children.Add(programs[p]);
				}
			}
			
			var start = programs.Values.FirstOrDefault(p => p.Parent==null);
			var wrong = CalculateWeight(start);
			var needed = wrong.Item1;
			var childWeight = wrong.Item2.Children.Sum(c => CalculateWeight(c).Item1);
			return needed - childWeight;
		}

		private static (int, Program) CalculateWeight(Program program)
		{
			if (program.CaluculatedWeight.HasValue)
				return (program.CaluculatedWeight.Value, null);

			var weight = program.Weight;
			int? firstWeight=null;
			foreach (var child in program.Children)
			{
				var cw = CalculateWeight(child);
				if (cw.Item2 != null)
					return cw;
				
				var childWeight = cw.Item1;
				if (firstWeight.HasValue)
				{
					if (firstWeight.Value != childWeight)
						return (firstWeight.Value, child); // needed weight and wrong program
				}
				else
				{
					firstWeight = childWeight;
				}
				weight += childWeight;
			}
			program.CaluculatedWeight = weight;
			return (weight, null);
		}

		private class Program
		{
			public Program(string name)
			{
				Name = name;
			}

			public string Name { get; }
			public int Weight { get; set; }
			public int? CaluculatedWeight { get; set; }
			public string[] Holding { get; set; }
			public Program Parent { get; set; }
			public readonly HashSet<Program> Children = new HashSet<Program>();

			public override string ToString()
			{
				return $"[{CaluculatedWeight ?? -1,4}] {Name}";
			}
		}
	}
}
