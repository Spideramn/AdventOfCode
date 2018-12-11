using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.Days.Day07
{
	internal class Program : Day
	{
		public override int DayNumber => 7;

		public override object RunPart1()
		{
			// fill step requirements
			var steps = new SortedDictionary<char, HashSet<char>>();
			foreach (var line in GetInputLines())
			{
				var requires = line.Substring(5, 1)[0];
				var step = line.Substring(36, 1)[0];
				if (steps.ContainsKey(step))
					steps[step].Add(requires);
				else
					steps[step] = new HashSet<char> {requires};
				if (!steps.ContainsKey(requires))
					steps[requires] = new HashSet<char>();
			}

			var stepsCompleted = new StringBuilder(steps.Count);
			while (steps.Any())
			{
				var step = steps.First(kv => !kv.Value.Any()).Key;
				steps.Remove(step);
				stepsCompleted.Append(step);
				foreach (var step2 in steps)
					step2.Value.Remove(step);
			}

			return stepsCompleted.ToString();
		}
		
		public override object RunPart2()
		{
			// fill step requirements
			var steps = new SortedDictionary<char, HashSet<char>>();
			foreach (var line in GetInputLines())
			{
				var requires = line.Substring(5, 1)[0];
				var step = line.Substring(36, 1)[0];
				if (steps.ContainsKey(step))
					steps[step].Add(requires);
				else
					steps[step] = new HashSet<char> {requires};
				if (!steps.ContainsKey(requires))
					steps[requires] = new HashSet<char>();
			}

			var totalTime = 0;
			var stepTime = 60;
			var workers = new[]
			{
				new Worker(),
				new Worker(),
				new Worker(),
				new Worker(),
				new Worker()
			};

			// do the work
			while (true)
			{
				// assign workers
				var availableWorkers = workers.Where(worker => worker.WorkingOn == '.').ToList();
				if (availableWorkers.Any())
				{
					var availableSteps = steps.Where(kv => kv.Value != null && !kv.Value.Any()).Take(availableWorkers.Count).ToList();
					if (availableSteps.Any())
					{
						for (var i = 0; i < Math.Min(availableSteps.Count, availableWorkers.Count); i++)
						{
							var availableStep = availableSteps[i].Key;
							steps[availableStep] = null; // null means, beeing worked on

							// assign work to  worker
							var worker = availableWorkers[i];
							worker.AvailableIn = availableStep + stepTime - 'A' + 1; //so 'A' => 1
							worker.WorkingOn = availableStep;
						}
					}
				}

				// nothing more to do? 
				if (!steps.Any())
					break;

				// move forward in time
				var delta = workers.Where(w => w.AvailableIn > 0).Min(w => w.AvailableIn);
				foreach (var worker in workers.Where(worker => worker.AvailableIn > 0))
				{
					worker.AvailableIn -= delta;

					// handle finished workers
					if (worker.AvailableIn == 0 && worker.WorkingOn != '.')
					{
						// remove step 
						steps.Remove(worker.WorkingOn);
						foreach (var step2 in steps)
							step2.Value?.Remove(worker.WorkingOn);
						worker.WorkingOn = '.';
					}
				}
				totalTime += delta;
			}

			return totalTime;
		}

		private class Worker
		{
			public int AvailableIn { get; set; }
			public char WorkingOn { get; set; } = '.';
		}
	}
}