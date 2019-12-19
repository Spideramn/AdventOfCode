using System.Linq;

namespace AdventOfCode2019.Days.Day09
{
	internal class Program : Day
	{
		public override int DayNumber => 9;

		public override object RunPart1()
		{
			var code = GetInputString().Split(',').Select(long.Parse).ToArray();
			return string.Join(',', new Intcode(code, 1).Run());
		}

		public override object RunPart2()
		{
			var code = GetInputString().Split(',').Select(long.Parse).ToArray();
			return string.Join(',', new Intcode(code, 2).Run());
		}
	}
}