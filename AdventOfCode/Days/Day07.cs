namespace AdventOfCode.Days
{
	public partial class Day07 : Day
	{
		public Day07(string input)
			:base(7, input)
		{
		}

		public override object RunPart1()
		{
			var instructionSet = new InstructionSet(Input);
			return instructionSet.GetOutput("a");
		}

		public override object RunPart2()
		{
			var instructionSet = new InstructionSet(Input);
			var wireA = instructionSet.GetOutput("a");
			instructionSet.Reset();
			instructionSet["b"].Output = wireA;
			return instructionSet.GetOutput("a");
		}
	}
}