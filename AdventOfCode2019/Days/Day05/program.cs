using System.Linq;

namespace AdventOfCode2019.Days.Day05
{
	internal class Program : Day
	{
		public override int DayNumber => 5;

		public override object RunPart1()
		{
			var input = GetInputString().Split(',').Select(int.Parse).ToArray();
			var program = new Intcode(input);
			return string.Concat(program.AddInput(1).Run());
		}

		public override object RunPart2()
		{
			var input = GetInputString().Split(',').Select(int.Parse).ToArray();
			var program = new Intcode(input);
			return string.Concat(program.AddInput(5).Run());
		}
	}
}