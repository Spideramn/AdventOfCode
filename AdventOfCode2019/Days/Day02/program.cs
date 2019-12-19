using System.Linq;

namespace AdventOfCode2019.Days.Day02
{
	internal class Program : Day
	{
		public override int DayNumber => 2;

		public override object RunPart1()
		{
			var code = GetInputString().Split(',').Select(long.Parse).ToArray();
			var program = new Intcode(code);
			foreach (var _ in program.SetValue(1, 12).SetValue(2, 2).Run()) { }
			return program.GetValue(0);
		}

		public override object RunPart2()
		{
			var code = GetInputString().Split(',').Select(long.Parse).ToArray();
			for (var noun = 0; noun <= 99; noun++)
			for (var verb = 0; verb <= 99; verb++)
			{
				var program = new Intcode(code);
				foreach (var _ in program.SetValue(1, noun).SetValue(2, verb).Run()){ }
				if (program.GetValue(0) == 19690720)
					return (100 * noun) + verb;
			}
			return "No result";
		}
	}
}