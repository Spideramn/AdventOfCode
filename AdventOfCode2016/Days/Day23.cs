using System.Linq;

namespace AdventOfCode2016.Days
{
	public class Day23 : Day
	{
		public Day23(string input)
			: base(23, input)
		{
		}

		public override object RunPart1()
		{
			var registers = new [] {7, 0, 0, 0};
			var instructions = Input.Split('\n').Select(l => l.Trim().Split(' ')).ToArray();
			for (var pos = 0; pos < instructions.Length; pos++)
			{
				int value;
				var instruction = instructions[pos];
				switch (instruction[0])
				{
					case "cpy":
						var reg = instruction[2][0] - 'a';
						if(reg > -1 && reg < 4)
						{
							if (int.TryParse(instruction[1], out value))
								registers[reg] = value;
							else
								registers[reg] = registers[instruction[1][0] - 'a'];
						}
						break;
					case "inc":
						if(instruction[1][0] - 'a' > -1 && instruction[1][0] - 'a' < 4)
							registers[instruction[1][0] - 'a']++;
						break;
					case "dec":
						if(instruction[1][0] - 'a' > -1 && instruction[1][0] - 'a' < 4)
							registers[instruction[1][0] - 'a']--;
						break;
					case "jnz":
						if (!int.TryParse(instruction[1], out value))
							value = registers[instruction[1][0] - 'a'];
						if (value != 0)
						{
							if(!int.TryParse(instruction[2], out value))
								value = registers[instruction[2][0] - 'a'];
							pos += value - 1;
						}
						break;
					case "tgl":
						if (!int.TryParse(instruction[1], out value))
							value = registers[instruction[1][0] - 'a'];
						var tglPos = pos + value;

						if(tglPos < instructions.Length) 
						{
							var tgl = instructions[tglPos];
							if(tgl.Length == 2){
								if(tgl[0] == "inc") tgl[0] = "dec";
								else tgl[0] = "inc";
							} else if (tgl.Length == 3) {
								if(tgl[0] == "jnz") tgl[0] = "cpy";
								else tgl[0] = "jnz";
							}
						}
					break;
				}
			}

			return registers[0];
		}

		public override object RunPart2()
		{
			var registers = new [] {12, 0, 0, 0};
			var instructions = Input.Split('\n').Select(l => l.Trim().Split(' ')).ToArray();
			for (var pos = 0; pos < instructions.Length; pos++)
			{
				int value;
				var instruction = instructions[pos];
				switch (instruction[0])
				{
					case "cpy":
						var reg = instruction[2][0] - 'a';
						if(reg > -1 && reg < 4)
						{
							if (int.TryParse(instruction[1], out value))
								registers[reg] = value;
							else
								registers[reg] = registers[instruction[1][0] - 'a'];
						}
						break;
					case "inc":
						if(instruction[1][0] - 'a' > -1 && instruction[1][0] - 'a' < 4)
							registers[instruction[1][0] - 'a']++;
						break;
					case "dec":
						if(instruction[1][0] - 'a' > -1 && instruction[1][0] - 'a' < 4)
							registers[instruction[1][0] - 'a']--;
						break;
					case "jnz":
						if (!int.TryParse(instruction[1], out value))
							value = registers[instruction[1][0] - 'a'];
						if (value != 0)
						{
							if(!int.TryParse(instruction[2], out value))
								value = registers[instruction[2][0] - 'a'];
							pos += value - 1;
						}
						break;
					case "tgl":
						if (!int.TryParse(instruction[1], out value))
							value = registers[instruction[1][0] - 'a'];
						var tglPos = pos + value;

						if(tglPos < instructions.Length) 
						{
							var tgl = instructions[tglPos];
							if(tgl.Length == 2){
								if(tgl[0] == "inc") tgl[0] = "dec";
								else tgl[0] = "inc";
							} else if (tgl.Length == 3) {
								if(tgl[0] == "jnz") tgl[0] = "cpy";
								else tgl[0] = "jnz";
							}
						}
					break;
				}
			}

			return registers[0];
		}
	}
}