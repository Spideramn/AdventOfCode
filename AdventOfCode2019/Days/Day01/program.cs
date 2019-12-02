using System.Linq;

namespace AdventOfCode2019.Days.Day01
{
	internal class Program : Day
	{
		public override int DayNumber => 1;

		public override object RunPart1()
		{
			return GetInputLines()
				.Select(int.Parse)
				.Select(FuelRequired)
				.Sum();
		}

		public override object RunPart2()
		{
			return GetInputLines()
				.Select(int.Parse)
				.Select(mass =>
				{
					var fuel = FuelRequired(mass);
					var totalFuel = fuel;
					while ((fuel = FuelRequired(fuel)) > 0)
						totalFuel += fuel;
					return totalFuel;
				})
				.Sum();
		}

		private static int FuelRequired(int mass)
		{
			return (mass / 3) - 2;
		}
	}
}