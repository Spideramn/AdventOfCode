using System.Linq;

namespace AdventOfCode2016.Days
{
	public class Day06 : Day
	{
		public Day06(string input)
			: base(6, input)
		{
		}

		public override object RunPart1()
		{
			var message = "";
			var data = Input.Split('\n').Select(l => l.Trim()).ToArray();
			for (var i = 0; i < data[0].Length; i++)
			{
				message += data.Select(line => line[i])
					.GroupBy(c => c)
					.OrderByDescending(r => r.Count())
					.Select(r => r.Key)
					.First();
			}
			return message;
		}

		public override object RunPart2()
		{
			var message = "";
			var data = Input.Split('\n').Select(l => l.Trim()).ToArray();
			for (var i = 0; i < data[0].Length; i++)
			{
				message += data.Select(line => line[i])
					.GroupBy(c => c)
					.OrderBy(r => r.Count())
					.Select(r => r.Key)
					.First();
			}
			return message;
		}
	}
}