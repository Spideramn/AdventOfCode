using System.Collections.Generic;
using System.Linq;
using AdventOfCode2016.Lib;

namespace AdventOfCode2016.Days
{
	public class Day19 : Day
	{
		public Day19(string input)
			: base(19, input)
		{
		}

		public override object RunPart1()
		{
			var elfCount = int.Parse(Input);
			var elves = Enumerable.Range(1, elfCount).Select(e => new Elf(e)).ToArray();
			while (elves.Length != 1)
			{
				for (var i = 0; i < elves.Length; i++)
				{
					if (elves[i].Presents == 0)
						continue;

					var from = i + 1;
					if (from == elves.Length)
						from = 0;

					elves[i].Presents += elves[from].Presents;
					elves[from].Presents = 0;
				}
				elves = elves.Where(e => e.Presents != 0).ToArray();
			}
			return elves.Single().Number;
		}

		public override object RunPart2()
		{
			var elfCount = int.Parse(Input);

			var elves = new LinkedList<Elf>();
			var elf = elves.AddLast(new Elf(1));
			LinkedListNode<Elf> elfAcross = null;
			for(var i=2; i<=elfCount;i++)
			{
				var e = elves.AddLast(new Elf(i));
				if (i == (elfCount/2) + 1)
					elfAcross = e;
			}
			if (elfAcross == null) return 1;


			var count = 0;
			while (elf != elf.NextOrFirst())
			{
				var del = elfAcross;

				if(count%2==0)
					elfAcross = elfAcross.NextOrFirst().NextOrFirst();
				else
					elfAcross = elfAcross.NextOrFirst();

				del.List.Remove(del);

				elf = elf.NextOrFirst();
				count++;
			}
			return elf.Value.Number;
		}
		
		private class Elf
		{
			public Elf(int number)
			{
				Number = number;
				Presents = 1;
			}

			public int Number { get; }
			public int Presents { get; set; }
		}
	}
}