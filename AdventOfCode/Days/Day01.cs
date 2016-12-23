using System.Linq;

namespace AdventOfCode.Days
{
	public class Day01 : Day
	{
		public Day01(string input)
			:base(1, input)
		{
		}

		public override object RunPart1()
		{
			var input = Resources.InputDay1;
			var up = input.Count(c => c == '(');
			var down = input.Count(c => c == ')');
			return (up - down);
		}

		public override object RunPart2()
		{
			var input = Resources.InputDay1;
			var floor = 0;
			for (var i = 0; i < input.Length; i++ )
			{
				var chr = input[i];
				if (chr == '(')
					floor++;
				if (chr == ')')
					floor--;
				
				if (floor == -1)
					return (i+1);
			}
			return -1;
		}
	}
}
