using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days.Day21
{
	internal class Program : Day
	{
		public override int DayNumber => 21;

		public override object RunPart1()
		{
			unchecked
			{
				var r3 = 0;
				var r2 = r3 | 65536;
				r3 = 14070682;

				Line8:
				var r1 = r2 & 255;
				r3 = r3 + r1;
				r3 = r3 & 16777215;
				r3 = r3 * 65899;
				r3 = r3 & 16777215;
				if (256 > r2)
					return r3;
				r1 = 0;

				Line18:
				var r4 = r1 + 1;
				r4 = r4 * 256;
				if (r4 > r2)
					goto Line26;
				r1 = r1 + 1;
				goto Line18;

				Line26:
				r2 = r1;
				goto Line8;
			}
		}
		
		public override object RunPart2()
		{
			var seen = new HashSet<int>();
			unchecked
			{
				var r3 = 0;

				Line6:
				var r2 = r3 | 65536;
				r3 = 14070682;

				Line8:
				r3 = r3 + (r2 & 255);
				r3 = r3 & 16777215;
				r3 = r3 * 65899;
				r3 = r3 & 16777215;

				if (r2 < 256)
				{
					if (!seen.Add(r3))
						return seen.Last();
					goto Line6;
				}
				r2 = r2 / 256;
				goto Line8;
			}
		}
	}
}