using System.Linq;

namespace AdventOfCode2016.Days
{
	public class Day25 : Day
	{
		public Day25(string input)
			: base(25, input)
		{
		}

		public override object RunPart1()
		{
			for (var input = 0; input < int.MaxValue; input++)
			{
				var outCount = 0;
				var expectedOutput = 0;
				var registers = new[] {input, 0, 0, 0};
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
						case "out":
							outCount++;

							var output = registers[instruction[1][0] - 'a'];
							if (output == expectedOutput)
							{
								expectedOutput = expectedOutput == 1 ? 0 : 1;
								if (outCount > 1000)
									return input;
							}
							else
							{
								// stop this sequence
								pos = instructions.Length;
							}
							break;
					}
				}
			}
			return "Not found!";
		}

		public override object RunPart2()
		{
			return null;
		}
	}
}