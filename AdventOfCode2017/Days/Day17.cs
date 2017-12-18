using System;

namespace AdventOfCode2017.Days
{
	internal class Day17 : Day
	{
		public Day17(string input)
			: base(17, input)
		{
		}

		public override object RunPart1()
		{
			var stepSize = int.Parse(Input);
			var buffer = new int[2017 + 1];
			var pos = 0;
			var length = 1;
			while (length < buffer.Length)
			{
				pos = ((pos + stepSize) % length) + 1;
				if (length != pos)
					Array.Copy(buffer, pos, buffer, pos + 1, buffer.Length - (pos + 1));
				buffer[pos] = length++;
			}
			return buffer[pos + 1];
		}

		public override object RunPart2()
		{
			var stepSize = int.Parse(Input);
			var pos = 0;
			var target = 0;
			for (var length = 1; length <= 50000000; length++)
			{
				pos = ((pos + stepSize) % length) + 1;
				if (pos == 1) // only interested in pos 1
					target = length;
			}
			return target;
		}
	}
}
