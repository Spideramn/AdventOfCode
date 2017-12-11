using System;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day10 : Day
	{
		public Day10(string input)
			: base(10, input)
		{
		}

		public override object RunPart1()
		{
			var curPos = 0;
			var skip = 0;
			var list = Enumerable.Range(0, 256).ToArray();
			foreach (var length in Input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))
			{
				var left = curPos;
				var right = curPos + length - 1;
				while (left < right)
				{
					var t = list[left % 256];
					list[left % 256] = list[right % 256];
					list[right % 256] = t;
					left++;
					right--;
				}
				curPos += length + skip++;
			}
			return list[0] * list[1];
		}

		public override object RunPart2()
		{
			var curPos = 0;
			var skip = 0;
			var list = Enumerable.Range(0, 256).ToArray();
			var input = Input.Select(c => (int)c).ToList();
			input.AddRange(new[] {17, 31, 73, 47, 23});
			for (var round = 0; round < 64; round++)
			{
				foreach (var length in input)
				{
					var left = curPos;
					var right = curPos + length - 1;
					while (left < right)
					{
						var t = list[left % 256];
						list[left % 256] = list[right % 256];
						list[right % 256] = t;
						left++;
						right--;
					}
					curPos += length + skip++;
				}
			}

			var hash = "";
			var temp = 0;
			for (var index = 0; index < list.Length; index++)
			{
				if (index != 0 && index % 16 == 0)
				{
					hash += temp.ToString("x2");
					temp = 0;
				}
				temp ^= list[index];
			}
			return hash + temp.ToString("x2"); 
		}
	}
}
