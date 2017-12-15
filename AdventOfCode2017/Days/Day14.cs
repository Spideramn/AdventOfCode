using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day14 : Day
	{
		public Day14(string input)
			: base(14, input)
		{
		}

		public override object RunPart1()
		{
			var count = 0;
			for (var row = 0; row < 128; row++)
				count += Hash(Input + "-" + row).Take(128).Count(c => c == '1');
			return count;
		}

		public override object RunPart2()
		{
			var grid = new int?[128*128];
			for (var row = 0; row < 128; row++)
			{
				foreach (var column in Hash(Input + "-" + row).Take(128).Select((v, i) => (Index:i, Value:v)).Where(i => i.Value == '1').Select(i => i.Index))
					grid[row * 128 + column] = 0;
			}
			
			int index, groupIndex = 0;
			while ((index = Array.IndexOf(grid, 0)) != -1)
				UpdateNeighbours(grid, index, ++groupIndex);
			return groupIndex;
		}

		private static void UpdateNeighbours(int?[] grid, int index, int groupIndex)
		{
			if (!grid[index].HasValue || grid[index].Value == groupIndex)
				return;
			if(grid[index].Value > 0)
				throw new Exception("Allready in another group...?");

			grid[index] = groupIndex;

			var row = index / 128;
			var column = index % 128;
			
			// check left
			if (column > 0)
				UpdateNeighbours(grid, (row * 128) + (column - 1), groupIndex);
			// right
			if (column < 127)
				UpdateNeighbours(grid, (row * 128) + (column + 1), groupIndex);
			// down
			if (row < 127)
				UpdateNeighbours(grid, ((row+1) * 128) + (column), groupIndex);
			// up
			if (row > 0)
				UpdateNeighbours(grid, ((row-1) * 128) + (column), groupIndex);
		}

		private static IEnumerable<char> Hash(string rowHash)
		{
			var curPos = 0;
			var skip = 0;
			var list = Enumerable.Range(0, 256).ToArray();
			var input = rowHash.Select(c => (int)c).Concat(new[] { 17, 31, 73, 47, 23 }).ToArray();
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
				hash += temp.ToString("X2");
			}

			return hash.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')).SelectMany(c => c);
		}
	}
}
