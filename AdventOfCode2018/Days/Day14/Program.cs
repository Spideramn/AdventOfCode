using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.Days.Day14
{
	internal class Program : Day
	{
		public override int DayNumber => 14;

		public override object RunPart1()
		{
			var afterRecipes = int.Parse(GetInputString());
			var table = new StringBuilder(afterRecipes + 11);
			table.Append("37");
			var elf1 = 0;
			var elf2 = 1;
			while (table.Length < afterRecipes + 10)
			{
				var score1 = table[elf1] - '0';
				var score2 = table[elf2] - '0';
				table.Append(score1 + score2);
				elf1 = (elf1 + 1 + score1) % table.Length;
				elf2 = (elf2 + 1 + score2) % table.Length;
			}
			return table.ToString().Substring(afterRecipes, 10);
		}
		public override object RunPart2()
		{
			var searchString = GetInputString();
			var search = searchString.Select(c => c - '0').ToArray();
			var table = new List<int> { 3, 7 };
			var elf1 = 0;
			var elf2 = 1;
			while (true)
			{
				
				var score1 = table[elf1];
				var score2 = table[elf2];
				var append = score1 + score2;
				if (append > 9)
				{
					table.Add(append / 10);
					table.Add(append % 10);
				}
				else
				{
					table.Add(append);
				}

				if (table.Count > search.Length)
				{
					var lastDigits = table.GetRange(table.Count - search.Length, search.Length);
					if (Enumerable.SequenceEqual(lastDigits, search))
						return new string(table.Select(c => (char)(c + '0')).ToArray()).IndexOf(searchString);
				}


				elf1 = (elf1 + 1 + score1) % table.Count;
				elf2 = (elf2 + 1 + score2) % table.Count;
			}
		}
	}
}