using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace AdventOfCode2016.Days
{
	public class Day08 : Day
	{
		public Day08(string input)
			: base(8, input)
		{
		}

		public override object RunPart1()
		{
			var screen = new bool[6,50];
			var redraw = false;
			foreach (var line in Input.Split('\n').Select(l => l.Trim().Split(' ')))
			{
				switch (line[0])
				{
					case "rect":
						var parts = line[1].Split('x');
						Rect(screen, int.Parse(parts[0]), int.Parse(parts[1]));
					break;
					case "rotate":
						Rotate(screen, line[1] == "column", int.Parse(line[2].Substring(2)), int.Parse(line[4]));
						break;
				}
				Show(screen, redraw);
				redraw = true;
			}
			return screen.Cast<bool>().Count(c => c);
		}

		private static void Rotate(bool[,] screen, bool column, int pos, int distance)
		{
			for (var d = 0; d < distance; d++)
			{
				if (column)
				{
					var m = screen.GetLength(0);
					var latest = screen[m - 1, pos]; // onderste
					for (var x = 0; x < m; x++)
					{
						var last = screen[x, pos];
						screen[x, pos] = latest;
						latest = last;
					}
				}
				else
				{
					var m = screen.GetLength(1);
					var latest = screen[pos, m - 1]; // onderste
					for (var x = 0; x < m; x++)
					{
						var last = screen[pos,x];
						screen[pos, x] = latest;
						latest = last;
					}
				}
			}
		}
		private static void Rect(bool[,] screen, int width, int height)
		{
			for (var y = 0; y < width; y++)
				for (var x = 0; x < height; x++)
					screen[x, y] = true;
		}
		private static void Show(bool[,] screen, bool redraw)
		{
			if (redraw)
			{
				Thread.Sleep(25);
				Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - screen.GetLength(0)-2);
			}

			var s = new StringBuilder();
			s.Append('╔');
			s.Append('═', screen.GetLength(1));
			s.AppendLine("╗");
			for (var x = 0; x < screen.GetLength(0); x++)
			{
				s.Append("║");
				for (var y = 0; y < screen.GetLength(1); y++)
					s.Append(screen[x, y] ? '█' : '░');
				s.AppendLine("║");
			}
			s.Append('╚');
			s.Append('═', screen.GetLength(1));
			s.Append('╝');
			Console.WriteLine(s.ToString());
		}







		public override object RunPart2()
		{
			var screen = new bool[6, 50];
			foreach (var line in Input.Split('\n').Select(l => l.Trim().Split(' ')))
			{
				switch (line[0])
				{
					case "rect":
						var parts = line[1].Split('x');
						Rect(screen, int.Parse(parts[0]), int.Parse(parts[1]));
						break;
					case "rotate":
						Rotate(screen, line[1] == "column", int.Parse(line[2].Substring(2)), int.Parse(line[4]));
						break;
				}
			}
			Show(screen, false);
			return null;
		}
	}
}