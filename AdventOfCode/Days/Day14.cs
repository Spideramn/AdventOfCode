using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
	public class Day14 : Day
	{
		public Day14(string input)
			: base(14, input)
		{
		}

		public override object RunPart1()
		{
			var totalSeconds = 2503;
			var reindeers = new Dictionary<string, int>();
			foreach(var line in Input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries))
			{
				var parts = line.Split(' ');
				var speed = int.Parse(parts[3]);
				var s1 = int.Parse(parts[6]);
				var s2 = int.Parse(parts[13]);
				
				var timeFullCycle = (totalSeconds/(s1 + s2));
				var timeRemaining = (totalSeconds%(s1 + s2));
				var baseDistance = timeFullCycle * (speed*s1);
				
				if (timeRemaining >= s1)
					baseDistance += speed * s1;
				else
					baseDistance += speed*timeRemaining;
				reindeers[parts[0]] = baseDistance;
			}
			return reindeers.Values.Max();
		}

		public override object RunPart2()
		{
			var totalSeconds = 2503;
			var reindeers = Input
				.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split(' '))
				.Select(parts => new Reindeer(int.Parse(parts[3]), int.Parse(parts[6]), int.Parse(parts[13])))
				.ToList();
			for (var currentSecond = 0; currentSecond < totalSeconds; currentSecond++)
			{
				foreach(var reindeer in reindeers)
					reindeer.Tick(currentSecond);

				var maxDistance = reindeers.Max(r => r.Distance);
				foreach(var reindeer in reindeers.Where(r => r.Distance == maxDistance))
					reindeer.Points++;
			}
			var winning = reindeers.OrderByDescending(r => r.Points).First().Points;
			return winning;
		}


		private class Reindeer 
		{
			private readonly int _speed;
			private readonly int _s1;
			private readonly int _s2;

			public Reindeer(int speed, int s1, int s2)
			{
				_speed = speed;
				_s2 = s2;
				_s1 = s1;
			}
			public int Distance { get; private set; }
			public int Points { get; set; }

			public void Tick(int second)
			{
				if (second % (_s1 + _s2) < _s1)
					Distance += _speed;
			}
		}
	}
}
