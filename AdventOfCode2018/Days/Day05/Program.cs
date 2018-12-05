using System;
using System.Text;

namespace AdventOfCode2018.Days.Day05
{
	internal class Program : Day
	{
		public override int DayNumber => 5;

		public override object RunPart1()
		{
			return React(new StringBuilder(GetInputString()));
		}

		public override object RunPart2()
		{
			var input = GetInputString();
			var shortest = input.Length;
			for (var c = 'a'; c <= 'z'; c++)
			{
				var line = new StringBuilder(input);
				line.Replace(c.ToString(), "").Replace(((char)(c+32)).ToString(), "");
				shortest = Math.Min(React(line), shortest);
			}
			return shortest;
		}

		private static int React(StringBuilder input)
		{
			for (var i = 0; i < input.Length - 1;)
			{
				if (Math.Abs(input[i] - input[i + 1]) == 32)
				{
					input.Remove(i, 2);
					if (--i < 0) i = 0;
				}
				else
				{
					i++;
				}
			}
			return input.Length;
		}
	}
}