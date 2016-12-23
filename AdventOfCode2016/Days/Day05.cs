using System.Diagnostics;

namespace AdventOfCode2016.Days
{
	public partial class Day05 : Day
	{
		private readonly Day05Threaded _threaded;
		private readonly Day05Normal _normal;

		public Day05(string input)
			: base(5, input)
		{
			_threaded = new Day05Threaded(Input);
			_normal = new Day05Normal(Input);
		}

		public override object RunPart1()
		{
			var result = "";
			var stopwatch = Stopwatch.StartNew();
			result += string.Format("\r\nNormal   [{1:#00.0000}] {0}", _normal.RunPart1(), stopwatch.Elapsed.TotalSeconds);
			stopwatch = Stopwatch.StartNew();
			result += string.Format("\r\nThreaded [{1:#00.0000}] {0}", _threaded.RunPart1(), stopwatch.Elapsed.TotalSeconds); 
			return result;
		}

		public override object RunPart2()
		{
			var result = "";
			var stopwatch = Stopwatch.StartNew();
			result += string.Format("\r\nNormal   [{1:#00.0000}] {0}", _normal.RunPart2(), stopwatch.Elapsed.TotalSeconds); 
			stopwatch = Stopwatch.StartNew();
			result += string.Format("\r\nThreaded [{1:#00.0000}] {0}", _threaded.RunPart2(), stopwatch.Elapsed.TotalSeconds);
			return result;
		}
	}
}