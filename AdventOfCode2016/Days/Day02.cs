using System;
using System.Linq;

namespace AdventOfCode2016.Days
{
	public class Day02 : Day
	{
		public Day02(string input)
			: base(02, input)
		{

		}

		public override object RunPart1()
		{
			var code = "";
			int x = 1, y = 1;
			foreach (var line in Input.Split('\n').Select(l => l.Trim()))
			{
				foreach (var c in line)
				{
					switch (c)
					{
						case 'U':
							y = Math.Max(y - 1, 0);
							break;
						case 'D':
							y = Math.Min(y + 1, 2);
							break;
						case 'L':
							x = Math.Max(x - 1, 0);
							break;
						case 'R':
							x = Math.Min(x + 1, 2);
							break;
					}
				}
				code += 1 + y*3 + x;
			}
			return code;
		}

		public override object RunPart2()
		{
			var code = "";
			int x = 0, y = 2;
			var keypad = new [,]
			{
				{' ',' ','1',' ',' '},
				{' ','2','3','4',' '},
				{'5','6','7','8','9'},
				{' ','A','B','C',' '},
				{' ',' ','D',' ',' '}
			};
			foreach (var line in Input.Split('\n').Select(l => l.Trim()))
			{
				foreach (var c in line)
				{
					switch (c)
					{
						case 'U':
							if (y - 1 >= 0 && keypad[x,y-1] != ' ')
								y--;
							break;
						case 'D':
							if (y + 1 <= 4 && keypad[x, y+1] != ' ')
								y++;
							break;
						case 'L':
							if (x - 1 >= 0 && keypad[x-1, y] != ' ')
								x--;
							break;
						case 'R':
							if (x + 1 <= 4 && keypad[x+1, y] != ' ')
								x++;
							break;
					}
				}
				code += keypad[y,x];
			}
			return code;
		}
	}
}