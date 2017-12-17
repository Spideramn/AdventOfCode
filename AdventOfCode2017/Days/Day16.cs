using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day16 : Day
	{
		public Day16(string input)
			: base(16, input)//"s1,x3/4,pe/b")
		{
		}

		public override object RunPart1()
		{
			var programs = new char[16];
			for (var i = 0; i < programs.Length; i++)
				programs[i] = (char)('a' + i);
			foreach (var instruction in Input.Split(',', StringSplitOptions.RemoveEmptyEntries))
			{
				switch (instruction[0])
				{
					case 's':
						var shift = int.Parse(instruction.Substring(1));
						var buffer = new char[shift];
						// take last "shift" programs and put in buffer
						Array.Copy(programs, programs.Length - shift, buffer, 0, shift);
						// move programs "shift" to the right
						Array.Copy(programs, 0, programs, shift, programs.Length - shift);
						// insert buffer at beginning
						Array.Copy(buffer, 0, programs, 0, shift);
						break;
					case 'x':
						var p = instruction.Substring(1).Split('/').Select(int.Parse).ToArray();
						var t = programs[p[0]];
						programs[p[0]] = programs[p[1]];
						programs[p[1]] = t;
						break;
					case 'p':
						var a = Array.IndexOf(programs, instruction[1]);
						var b = Array.IndexOf(programs, instruction[3]);
						var x = programs[a];
						programs[a] = programs[b];
						programs[b] = x;
						break;
				}
			}
			
			return new string(programs);
		}

		public override object RunPart2()
		{
			var instructions = Input.Split(',', StringSplitOptions.RemoveEmptyEntries);
			var programs = new char[16];
			for (var i = 0; i < programs.Length; i++)
				programs[i] = (char)('a' + i);

			int loop;
			var knownStates = new List<string>();
			for (loop = 0; loop < 1000000000; loop++)
			{
				foreach (var instruction in instructions)
				{
					switch (instruction[0])
					{
						case 's':
							var shift = int.Parse(instruction.Substring(1));
							var buffer = new char[shift];
							Array.Copy(programs, programs.Length - shift, buffer, 0, shift);
							Array.Copy(programs, 0, programs, shift, programs.Length - shift);
							Array.Copy(buffer, 0, programs, 0, shift);
							break;
						case 'x':
							var p = instruction.Substring(1).Split('/').Select(int.Parse).ToArray();
							var t = programs[p[0]];
							programs[p[0]] = programs[p[1]];
							programs[p[1]] = t;
							break;
						case 'p':
							var a = Array.IndexOf(programs, instruction[1]);
							var b = Array.IndexOf(programs, instruction[3]);
							var x = programs[a];
							programs[a] = programs[b];
							programs[b] = x;
							break;
					}
				}
				// look for early skip
				if(knownStates.Contains(new string(programs)))
					break;
				knownStates.Add(new string(programs));
			}

			// no early skip found
			if(loop == 1000000000)
				return programs;
			return knownStates[(1000000000 % knownStates.Count) - 1];
		}
	}
}
