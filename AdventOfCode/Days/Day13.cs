using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Lib;

namespace AdventOfCode.Days
{
	public class Day13 : Day
	{
		public Day13(string input)
			: base(13, input)
		{
		}

		public override object RunPart1()
		{
			var lines = Input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
			var happiness = new Dictionary<string, Dictionary<string, int>>();
			foreach (var line in lines)
			{
				var parts = line.Split(' ');
				var name = parts[0];
				var nextTo = parts[10].TrimEnd('.');
				var level = int.Parse(parts[3]);
				if (parts[2] == "lose")
					level *= -1;

				if(!happiness.ContainsKey(name))
					happiness.Add(name, new Dictionary<string, int>());
				happiness[name].Add(nextTo, level);
			}

			var maxH = 0;
			var names = happiness.Keys.ToList();
			foreach(var perm in names.GetPermutations())
			{
				var h = 0;
				for (var i = 0; i < perm.Length; i++)
				{
					var left = i + 1;
					if (left == perm.Length)
						left = 0;
					h += happiness[perm[i]][perm[left]];

					var right = i - 1;
					if (right == -1)
						right = perm.Length - 1;
					h += happiness[perm[i]][perm[right]];
				}
				if(h > maxH)
					maxH = h;
			}
			
			return maxH;
		}

		public override object RunPart2()
		{
			var lines = Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			var happiness = new Dictionary<string, Dictionary<string, int>>();
			foreach (var line in lines)
			{
				var parts = line.Split(' ');
				var name = parts[0];
				var nextTo = parts[10].TrimEnd('.');
				var level = int.Parse(parts[3]);
				if (parts[2] == "lose")
					level *= -1;

				if (!happiness.ContainsKey(name))
					happiness.Add(name, new Dictionary<string, int> {{"Mark", 0}});
				happiness[name].Add(nextTo, level);
			}
			happiness["Mark"] = happiness.Keys.ToDictionary(k => k, v => 0);

			var maxH = 0;
			var names = happiness.Keys.ToList();
			foreach (var perm in names.GetPermutations())
			{
				var h = 0;
				for (var i = 0; i < perm.Length; i++)
				{
					var left = i + 1;
					if (left == perm.Length)
						left = 0;
					h += happiness[perm[i]][perm[left]];

					var right = i - 1;
					if (right == -1)
						right = perm.Length - 1;
					h += happiness[perm[i]][perm[right]];
				}
				if (h > maxH)
					maxH = h;
			}

			return maxH;
		}

	}
}
