using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day01 : Day
	{
		public Day01(string input)
			: base(01, input)
		{
		}

		public override object RunPart1()
		{
			var code = 0;
			var lastV = Input.Last();
			foreach (var v in Input)
			{
				if (v == lastV)
					code += v - 48;
				lastV = v;
			}
			return code;
		}

		public override object RunPart2()
		{
			var code = 0;
			var i2 = Input + Input;
			for (var i = 0; i < Input.Length; i++)
			{
				if (i2[i] == i2[i + Input.Length / 2])
					code += Input[i] - 48;
			}
			return code;
		}
	}
}
