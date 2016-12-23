using System;
using System.Text;

namespace AdventOfCode2016.Days
{
	public class Day16 : Day
	{
		public Day16(string input)
			: base(16, input)
		{
		}

		public override object RunPart1()
		{
			return Run(272);
		}

		public override object RunPart2()
		{
			return Run(35651584);
		}

		private string Run(int neededLength)
		{
			var state = Input;
			// stage 1
			while (state.Length < neededLength)
			{
				// reverse
				var b = state.ToCharArray();
				Array.Reverse(b);
				for (var i = 0; i < b.Length; i++)
					b[i] = (b[i] == '1') ? '0' : '1';

				state = string.Concat(state, '0', new string(b));
			}
			state = state.Substring(0, neededLength);


			// stage 2
			var checksum = state;
			do
			{
				var sb = new StringBuilder();
				for (var i = 0; i < checksum.Length; i += 2)
				{
					if (checksum[i] == checksum[i + 1])
						sb.Append('1');
					else
						sb.Append('0');
				}
				checksum = sb.ToString();
			} while (checksum.Length % 2 == 0); // checksum needs t0 be odd

			return checksum;
		}
	}
}