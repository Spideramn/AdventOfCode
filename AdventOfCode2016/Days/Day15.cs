using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2016.Days
{
	public class Day15 : Day
	{
		public Day15(string input)
			: base(15, input)
		{
		}

		public override object RunPart1()
		{
			var time = 0;
			var disks = new List<Disk>();
			foreach (var line in Input.Split('\n').Select(l => l.Trim()))
				disks.Add(new Disk(line));

			while (disks.Any(disk => disk.Position(time) != 0))
				time++;

			return time;
		}

		public override object RunPart2()
		{
			var time = 0;
			var disks = new List<Disk>();
			foreach (var line in Input.Split('\n').Select(l => l.Trim()))
				disks.Add(new Disk(line));
			
			disks.Add(new Disk("Disc #7 has 11 positions; at time=0, it is at position 0."));
			while (disks.Any(disk => disk.Position(time) != 0))
				time++;

			return time;
		}

		public class Disk
		{
			private readonly int _positions;
			private readonly int _startPosition;
			private readonly int _index;

			public Disk(string line)
			{
				var parts = line.Split(' ');
				_index = int.Parse(parts[1].TrimStart('#'));
				_positions = int.Parse(parts[3]);
				_startPosition = int.Parse(parts[11].TrimEnd('.'));
			}

			public int Position(int time)
			{
				var totalTime = _startPosition + time + _index;
				return totalTime % _positions;
			}
		}

	}
}