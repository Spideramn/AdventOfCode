using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2016.Days
{
	public class Day04 : Day
	{
		public Day04(string input)
			: base(4, input)
		{
		}

		public override object RunPart1()
		{
			return Input
				.Split('\n')
				.Select(l => Regex.Match(l.Trim(), @"^([a-z\-]*)-([0-9]*)\[([a-z]*)\]$").Groups)
				.Select(g => new
				{
					encryptedName = g[1].Value,
					sectorId = int.Parse(g[2].Value),
					checksum = g[3].Value
				})
				.Where(t => t.checksum == string.Concat(t.encryptedName
					            .Replace("-", "")
					            .GroupBy(c => c)
					            .Select(x => new {c = x.Key, count = x.Count()})
					            .OrderByDescending(r => r.count)
					            .ThenBy(r => r.c)
					            .Select(r => r.c)
								.Take(5))
				)
				.Sum(t => t.sectorId);
		}

		public override object RunPart2()
		{
			return Input
				.Split('\n')
				.Select(l => Regex.Match(l.Trim(), @"^([a-z\-]*)-([0-9]*)\[([a-z]*)\]$").Groups)
				.Select(g => new
				{
					encryptedName = g[1].Value,
					sectorId = int.Parse(g[2].Value),
					checksum = g[3].Value
				})
				.Where(t => t.checksum == string.Concat(t.encryptedName
					            .Replace("-", "")
					            .GroupBy(c => c)
					            .Select(x => new {c = x.Key, count = x.Count()})
					            .OrderByDescending(r => r.count)
					            .ThenBy(r => r.c)
					            .Select(r => r.c)
								.Take(5))
				)
				.Select(t => new
				{
					decryptedName = t.encryptedName.Aggregate("", (d, c) => c != '-' ? d + (char) ('a' + (c - 'a' + t.sectorId)%26) : d + ' '),
					t.sectorId
				})
				.FirstOrDefault(d => d.decryptedName.Contains("north"))?.sectorId;
		}
	}
}