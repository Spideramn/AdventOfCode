using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2019.Days.Day13
{
	internal class Program : Day
	{
		public override int DayNumber => 13;

		public override object RunPart1()
		{
			//Console.Clear();
			var screen = new Dictionary<Point, int>();
			var code = GetInputString().Split(',').Select(long.Parse).ToArray();
			var program = new Intcode(code);
			foreach (var chunk in program.Run().Chunk(3))
			{
				var point = new Point((int) chunk[0], (int) chunk[1]);
				screen[point] = (int) chunk[2];
				
				//Console.SetCursorPosition(point.X, point.Y);
				//Console.Write((int)chunk[2]);
			}
			//Console.WriteLine();
			return screen.Values.Count(t => t == 2);
		}
		
		public override object RunPart2()
		{
			//Console.Clear();
			var chars = new []
			{
				' ', // empty
				'=', // wall
				'#', // block
				'_', // paddle
				'O'  // ball
			};

			var score = 0L;
			var paddle = Point.Empty;
			var ball = Point.Empty;
			var code = GetInputString().Split(',').Select(long.Parse).ToArray();
			var program = new Intcode(code);
			program.SetValue(0, 2); // 2 coins
			foreach (var chunk in program.Run().Chunk(3))
			{
				if (chunk[0] == -1 && chunk[1] == 0)
				{
					score = chunk[2];
					//Console.SetCursorPosition(0,0);
					//Console.Write("Score: " + score);
					continue;
				}
				var point = new Point((int) chunk[0], (int) chunk[1]);
				var tile = (int) chunk[2];
				switch (tile)
				{
					case 3:
						paddle = point;
						break;
					case 4:
						ball = point;
						
						// provide input
						if (ball.X < paddle.X)
							program.AddInput(-1);
						else if (ball.X > paddle.X)
							program.AddInput(1);
						else
							program.AddInput(0);
						break;
				}

				//Console.SetCursorPosition(point.X, point.Y + 1);
				//Console.Write(chars[tile]);
			}
			//Console.Clear();
			return score;
		}
	}


	internal static class EnumerableExtensions
	{ 
		/// <summary>
		/// Take chunks of a sequence.
		/// </summary>
		/// <typeparam name="TValue">Value type</typeparam>
		/// <param name="values">The sequence.</param>
		/// <param name="chunkSize">Size of the chunk.</param>
		/// <returns></returns>
		public static IEnumerable<List<TValue>> Chunk<TValue>(this IEnumerable<TValue> values, int chunkSize)
		{
			using (var enumerator = values.GetEnumerator())
			{
				while (enumerator.MoveNext())
					yield return GetChunk(enumerator, chunkSize).ToList();
			}
		}
		private static IEnumerable<T> GetChunk<T>(IEnumerator<T> enumerator, int chunkSize)
		{
			do
			{
				yield return enumerator.Current;
			} while (--chunkSize > 0 && enumerator.MoveNext());
		}
	}
}