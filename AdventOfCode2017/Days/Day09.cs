namespace AdventOfCode2017.Days
{
	internal class Day09 : Day
	{
		public Day09(string input)
			: base(09, input)
		{
		}

		public override object RunPart1()
		{
			var skipNext = false;
			var inGarbage = false;
			var totalScore = 0;
			var groupScore = 0;
			foreach (var c in Input)
			{
				if (skipNext)
				{
					skipNext = false;
					continue;
				}
				switch (c)
				{
					case '<':
						inGarbage = true;
						break;

					case '>':
						inGarbage = false;
						break;

					case '!':
						skipNext = true;
						break;

					case '{':
						if (!inGarbage)
							groupScore++;
						break;

					case '}':
						if (!inGarbage)
						{
							totalScore += groupScore;
							groupScore--;
						}
						break;
				}
			}
			return totalScore;
		}

		public override object RunPart2()
		{
			var skipNext = false;
			var inGarbage = false;
			var garbage = 0;
			foreach (var c in Input)
			{
				if (skipNext)
				{
					skipNext = false;
					continue;
				}
				switch (c)
				{
					case '<':
						if (inGarbage)
							garbage++;
						inGarbage = true;
						break;

					case '>':
						inGarbage = false;
						break;

					case '!':
						skipNext = true;
						break;

					default:
						if (inGarbage)
							garbage++;
						break;
				}
			}
			return garbage;
		}
	}
}