using System;
using System.Linq;

namespace AdventOfCode2020.Days
{
	internal class Day02 : Day
	{
		public override int DayNumber => 2;

		public override object RunPart1()
		{
			var validPasswords = 0;
			foreach (var line in GetInputLines())
			{
				var parts = line.Split(new[] {'-', ':', ' '}, StringSplitOptions.RemoveEmptyEntries);
				var min = int.Parse(parts[0]);
				var max = int.Parse(parts[1]);
				var character = parts[2][0];
				var password = parts[3].TrimStart(' ');

				var charCount = password.Count(c => c == character);
				if (charCount >= min && charCount <= max)
					validPasswords++;
			}
			return validPasswords;
		}

		public override object RunPart2()
		{
			var validPasswords = 0;
			foreach (var line in GetInputLines())
			{
				var parts = line.Split(new[] { '-', ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				var posA = int.Parse(parts[0]);
				var posB = int.Parse(parts[1]);
				var character = parts[2][0];
				var password = parts[3].TrimStart(' ');

				if ((password[posA - 1] == character && password[posB - 1] != character) ||
				    (password[posA - 1] != character && password[posB - 1] == character))
					validPasswords++;
			}
			return validPasswords;
		}
	}
}