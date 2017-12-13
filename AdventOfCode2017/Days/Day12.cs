using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day12 : Day
	{
		public Day12(string input)
			: base(12, input)
		{
		}

		public override object RunPart1()
		{
			var pipes = new Dictionary<int, int[]>();
			foreach (var line in Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
			{
				var parts = line.Split("<->", StringSplitOptions.RemoveEmptyEntries);
				pipes.Add(int.Parse(parts[0]), parts[1].Split(',').Select(int.Parse).ToArray());
			}

			var checkedPipes = new HashSet<int>();
			var q = new Queue<int[]>(new[] {pipes[0]});
			while (q.Any())
			{
				foreach (var pName in q.Dequeue())
				{
					if(checkedPipes.Add(pName))
						q.Enqueue(pipes[pName]);
				}
			}
			return checkedPipes.Count;
		}
		

		public override object RunPart2()
		{
			var pipes = new Dictionary<int, int[]>();
			foreach (var line in Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
			{
				var parts = line.Split("<->", StringSplitOptions.RemoveEmptyEntries);
				pipes.Add(int.Parse(parts[0]), parts[1].Split(',').Select(int.Parse).ToArray());
			}

			var groupCount = 0;
			var pipeNames = pipes.Keys.ToHashSet();
			while (pipeNames.Any())
			{
				var pipesInGroup = new HashSet<int>();
				var q = new Queue<int[]>(new[] {pipes[pipeNames.First()]});
				while (q.Any())
				{
					foreach (var pName in q.Dequeue())
					{
						if (pipesInGroup.Add(pName))
						{
							pipeNames.Remove(pName);
							q.Enqueue(pipes[pName]);
						}
					}
				}
				groupCount++;
			}
			return groupCount;
		}
	}
}
