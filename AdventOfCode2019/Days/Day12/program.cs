using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2019.Days.Day12
{
	internal class Program : Day
	{
		public override int DayNumber => 12;

		public override object RunPart1()
		{
			var moons = new List<Moon>();
			foreach (var line in GetInputLines())
			{
				var parts = line.Trim('<', '>').Split(',');
				var x = int.Parse(parts[0].Split('=')[1]);
				var y = int.Parse(parts[1].Split('=')[1]);
				var z = int.Parse(parts[2].Split('=')[1]);
				moons.Add(new Moon(new Vector3(x, y, z)));
			}

			//Output(moons, 0);

			for (var step = 1; step <= 1000; step++)
			{
				// apply gravity
				foreach (var moon in moons)
				foreach (var otherMoon in moons)
					moon.ApplyGravity(otherMoon);
				
				// apply velocity
				foreach (var moon in moons)
					moon.ApplyVelocity();

				//if(step%10==0)
				//	Output(moons, step);
			}

			return moons.Sum(moon => moon.Energy);
		}

		public override object RunPart2()
		{
			return base.RunPart2();
		}

		private static void Output(IEnumerable<Moon> moons, long step)
		{
			Console.WriteLine($"After {step} steps");
			foreach (var moon in moons)
				Console.WriteLine($"pos=<x={moon.Position.X,3}, y={moon.Position.Y,3}, z={moon.Position.Z,3}>, vel=<x={moon.Velocity.X,3}, y={moon.Velocity.Y,3}, z={moon.Velocity.Z,3}>");
			Console.WriteLine();
		}


		private class Moon
		{
			public Moon(Vector3 position)
			{
				Start = position;
				Position = position;
				Velocity = Vector3.Zero;
			}

			public Vector3 Start { get; }
			public Vector3 Position { get; private set; }
			public Vector3 Velocity { get; private set; }
			public int Energy
			{
				get
				{
					var pos = Vector3.Abs(Position);
					var vel = Vector3.Abs(Velocity);
					return (int) (pos.X + pos.Y + pos.Z) * (int) (vel.X + vel.Y + vel.Z);
				}
			}

			public void ApplyGravity(Moon otherMoon)
			{
				if (otherMoon == this) return;
				int x = 0, y = 0, z = 0;
				if (Position.X < otherMoon.Position.X) x = 1;
				else if (Position.X > otherMoon.Position.X) x = -1;

				if (Position.Y < otherMoon.Position.Y) y = 1;
				else if (Position.Y > otherMoon.Position.Y) y = -1;

				if (Position.Z < otherMoon.Position.Z) z = 1;
				else if (Position.Z > otherMoon.Position.Z) z = -1;

				Velocity += new Vector3(x, y, z);
			}

			public void ApplyVelocity()
			{
				Position += Velocity;
			}
		}
	}
}