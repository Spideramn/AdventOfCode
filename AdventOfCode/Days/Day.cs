namespace AdventOfCode.Days
{
	public abstract class Day
	{
		protected readonly string Input;
		protected Day(int day, string input)
		{
			Input = input;
			DayNumber = day;
		}

		public int DayNumber { get; private set; }
		public abstract object RunPart1();
		public abstract object RunPart2();
	}
}
