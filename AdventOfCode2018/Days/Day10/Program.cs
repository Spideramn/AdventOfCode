using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2018.Days.Day10
{
	internal class Program : Day
	{
		public override int DayNumber => 10;

		public override object RunPart1()
		{
			var points = new ConcurrentBag<Vector2[]>();
			foreach (var line in GetInputLines())
			{
				var x1 = int.Parse(line.Substring(10, 6));
				var y1 = int.Parse(line.Substring(17, 7));

				var x2 = int.Parse(line.Substring(36, 2));
				var y2 = int.Parse(line.Substring(39, 3));

				points.Add(new []{ new Vector2(x1,y1), new Vector2(x2, y2) });
			}
			
			float minX;
			float minY;
			float maxX;
			float maxY;
			var lastSize = float.MaxValue;
			while (true)
			{
				minX = float.MaxValue;
				minY = float.MaxValue;
				maxX = float.MinValue;
				maxY = float.MinValue;
				foreach (var point in points.Select(b => b[0]))
				{
					minX = Math.Min(point.X, minX);
					minY = Math.Min(point.Y, minY);
					maxX = Math.Max(point.X, maxX);
					maxY = Math.Max(point.Y, maxY);
				}
				var size = (maxX - minX) * (maxY - minY);
				if (size > lastSize)
					break;
				lastSize = size;
				
				// perform step
				points.AsParallel().ForAll(vs => vs[0] += vs[1]);
			}

			// 1 step back
			points.AsParallel().ForAll(vs => vs[0] -= vs[1]);

			// draw
			var sb = new StringBuilder();
			for (var y = minY; y <= maxY; y++)
			{
				for (var x = minX; x <= maxX; x++)
				{
					var v = new Vector2(x, y);
					var hasPoint = points.Any(p => p[0].Equals(v));
					sb.Append(hasPoint ? '#' : ' ');
				}
				sb.AppendLine();
			}
			return sb.ToString();
		}

		public override object RunPart2()
		{
			var points = new ConcurrentBag<Vector2[]>();
			foreach (var line in GetInputLines())
			{
				var x1 = int.Parse(line.Substring(10, 6));
				var y1 = int.Parse(line.Substring(17, 7));

				var x2 = int.Parse(line.Substring(36, 2));
				var y2 = int.Parse(line.Substring(39, 3));

				points.Add(new[] { new Vector2(x1, y1), new Vector2(x2, y2) });
			}

			var lastSize = float.MaxValue;
			var seconds = 0;
			while (true)
			{
				var minX = float.MaxValue;
				var minY = float.MaxValue;
				var maxX = float.MinValue;
				var maxY = float.MinValue;
				foreach (var point in points.Select(b => b[0]))
				{
					minX = Math.Min(point.X, minX);
					minY = Math.Min(point.Y, minY);
					maxX = Math.Max(point.X, maxX);
					maxY = Math.Max(point.Y, maxY);
				}
				var size = (maxX - minX) * (maxY - minY);
				if (size > lastSize)
					break;
				lastSize = size;

				// perform step
				points.AsParallel().ForAll(vs => vs[0] += vs[1]);
				seconds++;
			}
			// 1 step back!
			return seconds-1;
		}
	}
}