using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2016.Days
{
	public class Day07 : Day
	{
		public Day07(string input)
			: base(7, input)
		{
		}

		public override object RunPart1()
		{
			return Input
				.Split('\n')
				.Select(l => l.Trim())
				.Where(IpSupportsTls)
				.Count();
		}
		private static bool IpSupportsTls(string ip)
		{
			var parts = ip.Split('[', ']')
				.Select((item, index) => new {item, index})
				.GroupBy(x => x.index%2 == 0)
				.ToDictionary(g => g.Key, g => g.Select(p => p.item));
			return !parts[false].Any(HasAbba) && parts[true].Any(HasAbba);
		}
		private static bool HasAbba(string s)
		{
			if (s.Length < 4)
				return false;
			for (var i = 0; i < s.Length - 3; i++)
			{
				if (s[i] != s[i + 1] && s[i] == s[i + 3] && s[i + 1] == s[i + 2])
					return true;
			}
			return false;
		}


		public override object RunPart2()
		{
			return Input
				.Split('\n')
				.Select(l => l.Trim())
				.Where(IpSupportsSsl)
				.Count();
		}
		private static bool IpSupportsSsl(string ip)
		{
			var parts = ip.Split('[', ']')
				.Select((item, index) => new { item, index })
				.GroupBy(x => x.index % 2 == 0)
				.ToDictionary(g => g.Key, g => g.Select(p => p.item));
			return parts[true].Any(superNet => SelectAbas(superNet).Any(aba => parts[false].Any(p => HasBab(p, aba))));
		}
		private static IEnumerable<string >SelectAbas(string s)
		{
			if (s.Length < 3) yield break;
			for (var i = 0; i < s.Length - 2; i++)
			{
				if (s[i] == s[i + 2] && s[i] != s[i + 1])
					yield return string.Concat(s[i],s[i+1],s[i+2]);
			}
		}
		private static bool HasBab(string s, string aba)
		{
			return s.Length >= 3 && aba.Length == 3 && s.Contains(string.Concat(aba[1], aba[0], aba[1]));
		}
	}
}