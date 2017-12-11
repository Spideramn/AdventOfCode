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
			foreach (var length in Input.Split(',').Select(int.Parse))
			{
				var left = curPos;
				var right = curPos + length - 1;
				while (left < right)
				{
					var tLeft = list[left % 256];
					list[left % 256] = list[right % 256];
					list[right % 256] = tLeft;
					left++;
					right--;
				}
				curPos = (curPos + length + skip++) % 256;
			}
			return list[0] * list[1];
		}

		public override object RunPart2()
		{
			var curPos = 0;
			var skip = 0;
			var list = Enumerable.Range(0, 256).ToArray();
			var input = Input.Select(c => (int)c).Concat(new[] { 17, 31, 73, 47, 23 }).ToArray();
			for (var round = 0; round < 64; round++)
			{
				foreach (var length in input)
				{
					var left = curPos;
					var right = curPos + length - 1;
					while (left < right)
					{
						var tLeft = list[left % 256];
						list[left % 256] = list[right % 256];
						list[right % 256] = tLeft;
						left++;
						right--;
					}
					curPos = (curPos + length + skip++) % 256;
				}
			}

			var hash = "";
			for (var index = 0; index < list.Length; index += 16)
			{
				var temp = 0;
				for (var i = 0; i < 16; i++)
					temp ^= list[index + i];
				hash += temp.ToString("x2");
			}
			return hash;
		}
	}
}
