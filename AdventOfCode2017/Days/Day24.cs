using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Days
{
	internal class Day24 : Day
	{
		public Day24(string input)
			: base(24, input)
		{
		}

		public override object RunPart1()
		{
			var components = Input
				.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split('/'))
				.Select(parts => new Component(int.Parse(parts[0]), int.Parse(parts[1])))
				.ToList();
			
			var maxStrength = 0;
			var chain = new[] {new ComponentChain(components)};
			do
			{
				maxStrength = Math.Max(maxStrength, chain.Max(c => c.Strength));
				chain = chain.AsParallel().SelectMany(c => c.GetOptions()).ToArray();
			} while (chain.Any());
			return maxStrength;
		}

		public override object RunPart2()
		{
			var components = Input
				.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split('/'))
				.Select(parts => new Component(int.Parse(parts[0]), int.Parse(parts[1])))
				.ToList();

			var maxLength = 0;
			var maxStrength = 0;
			var chains = new Queue<ComponentChain>();
			chains.Enqueue(new ComponentChain(components));

			while (chains.Any())
			{
				var chain = chains.Dequeue();
				if (chain.Length == maxLength)
				{
					maxStrength = Math.Max(maxStrength, chain.Strength);
				}
				else if (chain.Length > maxLength)
				{
					maxStrength = chain.Strength;
					maxLength = chain.Length;
				}
				foreach (var subChain in chain.GetOptions())
					chains.Enqueue(subChain);
			}
			return maxStrength;
		}
		
		private class ComponentChain
		{
			private List<Component> Available { get; }
			private int Right { get; }

			public int Strength { get; }
			public int Length { get; }
			
			public ComponentChain(List<Component> available, int strength = 0, int length = 0, int right = 0)
			{
				Available = available;
				Right = right;
				Strength = strength;
				Length = length;
			}

			public IEnumerable<ComponentChain> GetOptions()
			{
				foreach (var option in Available.Where(c => Right == c.Left || Right == c.Right))
				{
					yield return new ComponentChain(
						Available.Where(c => c.Left != option.Left || c.Right != option.Right).ToList(),
						Strength + option.Left + option.Right,
						Length + 1,
						option.Left != Right ? option.Left : option.Right
					);
				}
			}
		}

		private struct Component
		{
			public readonly int Left;
			public readonly int Right;
			
			public Component(int left, int right)
			{
				Left = left;
				Right = right;
			}
		}
	}
}
