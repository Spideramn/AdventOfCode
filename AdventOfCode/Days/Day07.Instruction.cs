namespace AdventOfCode.Days
{
	public partial class Day07
	{
		private class Instruction
		{
			public Instruction(Operation operation, string input1, string input2 = null)
			{
				Operation = operation;
				Input1 = input1;
				Input2 = input2;
			}

			public string Input1 { get; private set; }
			public string Input2 { get; private set; }
			public Operation Operation { get; private set; }
			public ushort? Output { get; set; }
		}
	}
}
