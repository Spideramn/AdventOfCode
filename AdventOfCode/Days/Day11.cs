using AdventOfCode.Lib;

namespace AdventOfCode.Days
{
	public class Day11 : Day
	{
		public Day11(string input) :
			base(11, input)
		{
		}

		public override object RunPart1()
		{
			var password = new IncrementalString(Input, 'a', 'z', new[] { 'i', 'o', 'l' });
			do
			{
				password++;
			} while (!IsValidPassword(password));
			return password;
		}
		public override object RunPart2()
		{
			var password = (IncrementalString)RunPart1();
			do
			{
				password++;
			} while (!IsValidPassword(password));
			return password;
		}

		private static bool IsValidPassword(string password)
		{
			// min 3 lang
			if (password.Length < 3)
				return false;

			// Passwords may not contain the letters i, o, or l, as these letters can be mistaken for other characters and are therefore confusing.
			//if(new[] { 'i', 'o', 'l'}.Any(password.Contains)) // zit al in incrementalstring :)
			//	return false;

			// Passwords must contain at least *two different*, *non-overlapping* pairs of letters, like aa, bb, or zz.
			var isValidCounter = 0;
			var lastChar = password[0];
			for (var i = 1; i < password.Length; i++)
			{
				if (password[i] == lastChar)
				{
					if(++isValidCounter ==2)
						break;
					// skip current char for next check
					if (++i >= password.Length)
						continue;
				}
				lastChar = password[i];
			}
			if (isValidCounter != 2)
				return false;

			// Passwords must include one increasing straight of at least three letters, like abc, bcd, cde, and so on, up to xyz. They cannot skip letters; abd doesn't count.
			for (var i = 0; i < password.Length-2; i++)
			{
				var c1 = password[i];
				var c2 = password[i+1];
				var c3 = password[i+2];
				if (c2 == c1 + 1 && c3 == c2 + 1)
					return true;
			}
			return false;
		}
	}
}