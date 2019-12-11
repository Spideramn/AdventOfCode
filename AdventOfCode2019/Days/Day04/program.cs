using System.Linq;

namespace AdventOfCode2019.Days.Day04
{
	internal class Program : Day
	{
		public override int DayNumber => 4;

		public override object RunPart1()
		{
			var parts = GetInputString().Split('-');
			var start = int.Parse(parts[0]);
			var end = int.Parse(parts[1]);

			return Enumerable.Range(start, end - start + 1)
				.AsParallel()
				.Count(ValidatePassword);
		}

		public override object RunPart2()
		{
			var parts = GetInputString().Split('-');
			var start = int.Parse(parts[0]);
			var end = int.Parse(parts[1]);

			return Enumerable.Range(start, end - start + 1)
				.AsParallel()
				.Count(ValidatePassword2);
		}

		private static bool ValidatePassword(int password)
		{
			int? prevDigit = null;
			var adjacent = false;
			foreach (var digit in password.ToString().Select(c => c - 48))
			{
				if (prevDigit != null)
				{
					if (digit < prevDigit)
						return false;
					if (!adjacent && digit == prevDigit)
						adjacent = true;
				}
				prevDigit = digit;
			}

			return adjacent;
		}

		private static bool ValidatePassword2(int password)
		{
			int? prevDigit = null;

			var hasValidGroup = false;
			var groupSize = 1;
			foreach (var digit in password.ToString().Select(c => c - 48))
			{
				if (prevDigit != null)
				{
					if (digit < prevDigit)
						return false;

					if (!hasValidGroup)
					{
						if (digit == prevDigit)
						{
							groupSize++;
						}
						else
						{
							if (groupSize == 2)
								hasValidGroup = true;
							groupSize = 1;
						}
					}
				}
				prevDigit = digit;
			}

			return hasValidGroup || groupSize == 2;
		}
	}
}