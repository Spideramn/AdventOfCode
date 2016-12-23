using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
	public class Day15 : Day
	{
		public Day15(string input)
			: base(15, input)
		{
		}

		public override object RunPart1()
		{
			var ingredients = Input
				.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Replace(",", "").Split(' '))
				.Select(parts => new Ingredient(parts[0].TrimEnd(':'), int.Parse(parts[2]), int.Parse(parts[4]), int.Parse(parts[6]), int.Parse(parts[8]), int.Parse(parts[10])))
				.ToList();

			var maxScore = 0L;
			foreach(var ditribution in Distributions(100, new int[ingredients.Count]))
			{
				if (ditribution.Contains(0)) 
					continue;
				var capacity = 0L;
				var durability = 0L;
				var flavor = 0L;
				var texture = 0L;
				for (var i = 0; i < ingredients.Count; i++)
				{
					var spoons = ditribution[i];
					var ingredient = ingredients[i];
					capacity += ingredient.Capacity * spoons;
					durability += ingredient.Durability * spoons;
					flavor += ingredient.Flavor * spoons;
					texture += ingredient.Texture * spoons;
				}

				var score = Math.Max(0, capacity) * Math.Max(0, durability) * Math.Max(0, flavor) * Math.Max(0, texture);
				maxScore = Math.Max(maxScore, score);
			}
			return maxScore;
		}

		public override object RunPart2()
		{
			var ingredients = Input
				.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Replace(",", "").Split(' '))
				.Select(parts => new Ingredient(parts[0].TrimEnd(':'), int.Parse(parts[2]), int.Parse(parts[4]), int.Parse(parts[6]), int.Parse(parts[8]), int.Parse(parts[10])))
				.ToList();

			var maxScore = 0L;
			foreach (var ints in Distributions(100, new int[ingredients.Count]))
			{
				if (ints.Contains(0))
					continue;
				
				var capacity = 0L;
				var durability = 0L;
				var flavor = 0L;
				var texture = 0L;
				var calories = 0L;
				for (var i = 0; i < ingredients.Count; i++)
				{
					var spoons = ints[i];
					var ingredient = ingredients[i];
					capacity += ingredient.Capacity*spoons;
					durability += ingredient.Durability*spoons;
					flavor += ingredient.Flavor*spoons;
					texture += ingredient.Texture*spoons;
					calories += ingredient.Calories*spoons;
				}
				if (calories == 500)
				{
					var score = Math.Max(0, capacity)*Math.Max(0, durability)*Math.Max(0, flavor)*Math.Max(0, texture);
					maxScore = Math.Max(maxScore, score);
				}
			}
			return maxScore;
		}


		private class Ingredient
		{
			public Ingredient(string name, int capacity, int durability, int flavor, int texture, int calories)
			{
				Name = name;
				Capacity = capacity;
				Durability = durability;
				Flavor = flavor;
				Texture = texture;
				Calories = calories;
			}

			public string Name { get; private set; }
			public int Capacity  { get; private set; }
			public int Durability { get; private set; }
			public int Flavor { get; private set; }
			public int Texture { get; private set; }
			public int Calories { get; private set; }
		}


		
		// based on http://stackoverflow.com/a/26642148
		static IEnumerable<int[]> Distributions(int maxValue,  int[] values, int currentValueIndex=0)
		{
			if (currentValueIndex == values.Length - 1)
			{
				values[currentValueIndex] = maxValue;
				yield return values;
				yield break; // prevent index out of bounds
			}

			values[currentValueIndex] = 0;
			while (values[currentValueIndex] <= maxValue)
			{
				foreach (var ditribution in Distributions(maxValue - values[currentValueIndex], values, currentValueIndex + 1))
					yield return ditribution;
				values[currentValueIndex]++;
			}
		}
	}
}