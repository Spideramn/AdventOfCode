using System.Linq;

namespace AdventOfCode2019.Days.Day05
{
	internal class Program : Day
	{
		public override int DayNumber => 5;

		public override object RunPart1()
		{
			var input = GetInputString().Split(',').Select(long.Parse).ToArray();
			return string.Join(',', new Intcode(input, 1).Run());
		}

		public override object RunPart2()
		{
			var input = GetInputString().Split(',').Select(long.Parse).ToArray();
			return string.Join(',', new Intcode(input, 5).Run());
		}
	}
}