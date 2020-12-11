using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Days
{
	internal class Day04 : Day
	{
		public override int DayNumber => 4;

		public override object RunPart1()
		{
			var validPassportCount = 0;
			var passport = new HashSet<string> { "cid" };
			foreach (var line in GetInputLines())
			{
				// empty line, end of passport
				if (string.IsNullOrWhiteSpace(line))
				{
					// validate current password
					if (passport.Count == 8)
						validPassportCount++;
					
					// create a new passport
					passport = new HashSet<string>{ "cid" };
					continue;
				}
				
				// fill passport
				foreach (var key in line.Split(' ', ':').Where((key, index) => index % 2 == 0)) // alle odd parts are the keys
					passport.Add(key);
			}

			// validate last passport
			if (passport.Count == 8)
				validPassportCount++;
			
			return validPassportCount;
		}

		public override object RunPart2()
		{
			var validPassportCount = 0;
			var passport = new Dictionary<string, string> {{ "cid", "" }};
			foreach (var line in GetInputLines())
			{
				// empty line, end of passport
				if (string.IsNullOrWhiteSpace(line))
				{
					// validate current password
					if (ValidatePassport(passport))
						validPassportCount++;
					
					// create a new passport
					passport = new Dictionary<string, string> { { "cid", "" } };
					continue;
				}

				// fill passport
				var parts = line.Split(' ', ':');
				for (var i = 0; i < parts.Length; i++)
				{
					var key = parts[i++];
					var value = parts[i];
					passport[key] = value;
				}
			}

			// validate last passport
			if (ValidatePassport(passport))
				validPassportCount++;
			return validPassportCount;
		}

		private static bool ValidatePassport(Dictionary<string,string> passport)
		{
			if (passport.Count != 8)
				return false;
			
			foreach (var kv in passport)
			{
				switch (kv.Key)
				{
					case "byr":
						// four digits; at least 1920 and at most 2002.
						if (!int.TryParse(kv.Value, out var byr) || byr < 1920 || byr > 2002)
							return false;
						break;

					case "iyr":
						// four digits; at least 2010 and at most 2020.
						if (!int.TryParse(kv.Value, out var iyr) || iyr < 2010 || iyr > 2020)
							return false;
						break;

					case "eyr":
						// four digits; at least 2020 and at most 2030.
						if (!int.TryParse(kv.Value, out var eyr) || eyr < 2020 || eyr > 2030)
							return false;
						break;

					case "hgt": // (Height) - a number followed by either cm or in: 
						// If cm, the number must be at least 150 and at most 193.
						// If in, the number must be at least 59 and at most 76.
						if (!int.TryParse(kv.Value[..^2], out var value)) 
							return false;
						switch (kv.Value[^2..])
						{
							case "cm":
								if (value < 150 || value > 193)
									return false;
								break;

							case "in":
								if (value < 59 || value > 76)
									return false;
								break;

							default:
								return false;
						}

						break;

					case "hcl": // (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
						if (!Regex.IsMatch(kv.Value, "^#[0-9a-f]{6}$", RegexOptions.Compiled))
							return false;
						break;

					case "ecl": // (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
						if (!new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(kv.Value))
							return false;
						break;

					case "pid": // (Passport ID) - a nine-digit number, including leading zeroes.
						if (kv.Value.Length != 9 || !kv.Value.All(char.IsDigit))
							return false;
						break;

					case "cid": // (Country ID) - ignored, missing or not.

						break;

					default: // unknown key
						return false;
				}
			}

			return true;
		}
	}
}