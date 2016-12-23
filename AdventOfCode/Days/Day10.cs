using System.Linq;
using System.Text;

namespace AdventOfCode.Days
{
	public class Day10 : Day
	{
		public Day10(string input)
			:base(10, input)
		{
		}

		public override object RunPart1()
		{
			var input = Resources.InputDay10;
			var output = input;
			for (var i = 0; i < 40; i++)
				output = LookAndSay(output);
			return output.Length;
		}

		public override object RunPart2()
		{
			var input = Resources.InputDay10;
			var output = input;
			for (var i = 0; i < 50; i++)
				output = LookAndSay(output);
			return output.Length;
		}


		private static string LookAndSay(string input)
		{
			var last = input.First();
			var count = 0;
			var result = new StringBuilder();
			foreach (var currentChar in input)
			{
				if (currentChar == last)
				{
					count++;
					continue;
				}
				result.Append(count);
				result.Append(last);

				last = currentChar;
				count = 1;
			}

			result.Append(count);
			result.Append(last);
			return result.ToString();
		}
	}
}
