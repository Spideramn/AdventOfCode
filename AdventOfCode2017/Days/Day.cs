namespace AdventOfCode2017.Days
{
	internal abstract class Day
	{
		protected readonly string Input;

		protected Day(int day, string input)
		{
			Input = input;
			DayNumber = day;
		}

		public int DayNumber { get; }
		public abstract object RunPart1();
		public abstract object RunPart2();
	}
}
