using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Days.Day07
{
	internal class Program : Day
	{
		public override int DayNumber => 7;

		public override object RunPart1()
		{
			var code = GetInputString().Split(',').Select(int.Parse).ToArray();
			return GetPermutations(new[] {0, 1, 2, 3, 4})
				.Select(perm => CalculateThrustersValue1(code, perm[0], perm[1], perm[2], perm[3], perm[4]))
				.Max();
		}

		public override object RunPart2()
		{
			var code = GetInputString().Split(',').Select(int.Parse).ToArray();
			return GetPermutations(new[] {5, 6, 7, 8, 9})
				.Select(perm => CalculateThrustersValue2(code, perm[0], perm[1], perm[2], perm[3], perm[4]))
				.Max();
		}
		
		private static int CalculateThrustersValue1(int[] code, int phaseA, int phaseB, int phaseC, int phaseD, int phaseE)
		{
			var signal = 0;
			signal = new Intcode(code, phaseA, signal).Run().First();
			signal = new Intcode(code, phaseB, signal).Run().First();
			signal = new Intcode(code, phaseC, signal).Run().First();
			signal = new Intcode(code, phaseD, signal).Run().First();
			return new Intcode(code, phaseE, signal).Run().First();
		}
		
		private static int CalculateThrustersValue2(int[] code, int phaseA, int phaseB, int phaseC, int phaseD, int phaseE)
		{
			var ampA = new Intcode(code, phaseA);
			var ampB = new Intcode(code, phaseB);
			var ampC = new Intcode(code, phaseC);
			var ampD = new Intcode(code, phaseD);
			var ampE = new Intcode(code, phaseE);

			using (var enumA = ampA.Run().GetEnumerator())
			using (var enumB = ampB.Run().GetEnumerator())
			using (var enumC = ampC.Run().GetEnumerator())
			using (var enumD = ampD.Run().GetEnumerator())
			using (var enumE = ampE.Run().GetEnumerator())
			{
				var signal = 0;
				while (true)
				{
					ampA.AddInput(signal);
					if (!enumA.MoveNext()) break;
					
					ampB.AddInput(enumA.Current);
					if (!enumB.MoveNext()) break;
					
					ampC.AddInput(enumB.Current);
					if (!enumC.MoveNext()) break;
					
					ampD.AddInput(enumC.Current);
					if (!enumD.MoveNext()) break;
					
					ampE.AddInput(enumD.Current);
					if (!enumE.MoveNext()) break;

					signal = enumE.Current;
				}
				return signal;
			}
		}

		// source: https://stackoverflow.com/a/10629938/1251423
		private static IEnumerable<T[]> GetPermutations<T>(IList<T> list, int? length = null)
		{
			var l = length.GetValueOrDefault(list.Count);
			if (l == 1) return list.Select(t => new[] { t });

			return GetPermutations(list, l - 1)
				.SelectMany(
					t => list.Where(e => !t.Contains(e)).ToArray(),
					(t1, t2) => t1.Concat(new[] { t2 }).ToArray()
				);
		}
	}
}