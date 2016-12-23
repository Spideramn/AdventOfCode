using System;
using System.Linq;

namespace AdventOfCode.Days
{
	public class Day06 : Day
	{
		public Day06(string input)
			:base(6, input)
		{
		}

		public override object RunPart1()
		{
			var lights = new bool[1000,1000];

			foreach (var l in Resources.InputDay6.Split('\n'))
			{
				var line = l.Trim();
				var parts = line.Split(' ');

				var action = Action.Unknown;
				var start = new string[2];
				var end = new string[2];

				if(parts[0] == "turn")
				{
					if(parts[1] == "on")
						action = Action.On;
					else if(parts[1] == "off")
						action = Action.Off;
					start = parts[2].Split(',');
					end = parts[4].Split(',');
				}
				else if(parts[0] == "toggle")
				{
					action = Action.Toggle;
					start = parts[1].Split(',');
					end = parts[3].Split(',');
				}

				var startX = int.Parse(start[0]);
				var startY = int.Parse(start[1]);
				var endX = int.Parse(end[0]);
				var endY = int.Parse(end[1]);
				
				for (var x = startX; x <= endX; x++ )
				{
					for (var y = startY; y <= endY; y++ )
					{
						switch (action)
						{
							case Action.On:
								lights[x,y] = true;
								break;
							case Action.Off:
								lights[x, y] = false;
								break;
							case Action.Toggle:
								lights[x, y] = !lights[x, y];
								break;
						}
					}
				}
			}
			return lights.Cast<bool>().Count(l => l);
		}

		public override object RunPart2()
		{
			var lights = new int[1000, 1000];

			foreach (var l in Resources.InputDay6.Split('\n'))
			{
				var line = l.Trim();
				var parts = line.Split(' ');

				var action = Action.Unknown;
				var start = new string[2];
				var end = new string[2];

				if (parts[0] == "turn")
				{
					if (parts[1] == "on")
						action = Action.On;
					else if (parts[1] == "off")
						action = Action.Off;
					start = parts[2].Split(',');
					end = parts[4].Split(',');
				}
				else if (parts[0] == "toggle")
				{
					action = Action.Toggle;
					start = parts[1].Split(',');
					end = parts[3].Split(',');
				}

				var startX = int.Parse(start[0]);
				var startY = int.Parse(start[1]);
				var endX = int.Parse(end[0]);
				var endY = int.Parse(end[1]);

				for (var x = startX; x <= endX; x++)
				{
					for (var y = startY; y <= endY; y++)
					{
						switch (action)
						{
							case Action.On:
								lights[x, y]++;
								break;
							case Action.Off:
								lights[x, y] = Math.Max(0, lights[x, y] - 1);
								break;
							case Action.Toggle:
								lights[x, y] += 2;
								break;
						}
					}
				}
			}

			return lights.Cast<int>().Sum();
		}

		private enum Action
		{
			Unknown,
			On,
			Off,
			Toggle
		}
	}
}
