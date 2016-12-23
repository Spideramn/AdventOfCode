using System.Linq;

namespace AdventOfCode2016.Days
{
	public class Day12 : Day
	{
		public Day12(string input)
			: base(12, input)
		{
		}

		public override object RunPart1()
		{
			var registers = new int[4];
			var instructions = Input.Split('\n').Select(l => l.Trim().Split(' ')).ToArray();
			for (var pos = 0; pos < instructions.Length; pos++)
			{
				int value;
				var instruction = instructions[pos];
				switch (instruction[0])
				{
					case "cpy":
						if (int.TryParse(instruction[1], out value))
							registers[instruction[2][0] - 'a'] = value;
						else
							registers[instruction[2][0] - 'a'] = registers[instruction[1][0] - 'a'];
						break;
					case "inc":
						registers[instruction[1][0] - 'a']++;
						break;
					case "dec":
						registers[instruction[1][0] - 'a']--;
						break;
					case "jnz":
						if (!int.TryParse(instruction[1], out value))
							value = registers[instruction[1][0] - 'a'];
						if (value != 0)
							pos += int.Parse(instruction[2])-1;
						break;
				}
			}

			return registers[0];
		}
		
		public override object RunPart2()
		{
			var registers = new [] {0, 0, 1, 0};
			var instructions = Input.Split('\n').Select(l => l.Trim().Split(' ')).ToArray();
			for (var pos = 0; pos < instructions.Length; pos++)
			{
				int value;
				var instruction = instructions[pos];
				switch (instruction[0])
				{
					case "cpy":
						if (int.TryParse(instruction[1], out value))
							registers[instruction[2][0] - 'a'] = value;
						else
							registers[instruction[2][0] - 'a'] = registers[instruction[1][0] - 'a'];
						break;
					case "inc":
						registers[instruction[1][0] - 'a']++;
						break;
					case "dec":
						registers[instruction[1][0] - 'a']--;
						break;
					case "jnz":
						if (!int.TryParse(instruction[1], out value))
							value = registers[instruction[1][0] - 'a'];
						if (value != 0)
							pos += int.Parse(instruction[2]) - 1;
						break;
				}
			}

			return registers[0];
		}
	}
}