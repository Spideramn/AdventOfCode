using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day25 : Day
	{
		public Day25(string input)
			: base(25, input)
		{
		}

		public override object RunPart1()
		{
			var state = 'A';
			var index = 0;
			var mem = new Dictionary<int, bool>();
			for (var step = 0; step < 12861455; step++)
			{
				var value = mem.TryGetValue(index, out var t) && t;
				switch (state)
				{
					case 'A':
						if (!value)
						{
							mem[index++] = true;
							state = 'B';
						}
						else
						{
							mem[index--] = false;
							state = 'B';
						}
						break;
					case 'B':
						if (!value)
						{
							mem[index--] = true;
							state = 'C';
						}
						else
						{
							mem[index++] = false;
							state = 'E';
						}
						break;
					case 'C':
						if (!value)
						{
							mem[index++] = true;
							state = 'E';
						}
						else
						{
							mem[index--] = false;
							state = 'D';
						}
						break;
					case 'D':
						if (!value)
						{
							mem[index--] = true;
							state = 'A';
						}
						else
						{
							mem[index--] = true;
							state = 'A';
						}
						break;
					case 'E':
						if (!value)
						{
							mem[index++] = false;
							state = 'A';
						}
						else
						{
							mem[index++] = false;
							state = 'F';
						}
						break;
					case 'F':
						if (!value)
						{
							mem[index++] = true;
							state = 'E';
						}
						else
						{
							mem[index++] = true;
							state = 'A';
						}
						break;
				}
			}
			return mem.Values.Count(v => v);
		}

		public override object RunPart2()
		{
			return null;
		}
	}
}
