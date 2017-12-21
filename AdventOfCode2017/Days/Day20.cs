using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2017.Days
{
	internal class Day20 : Day
	{
		public Day20(string input)
			: base(20, input)
		{
		}

		public override object RunPart1()
		{
			var slowestIndex = 0;
			var slowestAcceleration = int.MaxValue;
			var slowestSpeed = int.MaxValue;
			var lowestdistance = int.MaxValue;
			var split = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
			for (var index = 0; index < split.Length; index++)
			{
				var line = split[index];
				var parts = line.Split(", ");

				var partsD = parts[0].Split(new[] { '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);
				var distance = Math.Abs(int.Parse(partsD[1])) + Math.Abs(int.Parse(partsD[2])) + Math.Abs(int.Parse(partsD[3]));
				var partsS = parts[1].Split(new[] {'<', '>', ','}, StringSplitOptions.RemoveEmptyEntries);
				var speed = Math.Abs(int.Parse(partsS[1])) + Math.Abs(int.Parse(partsS[2])) + Math.Abs(int.Parse(partsS[3]));
				var partsA = parts[2].Split(new[] { '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);
				var acceleration = Math.Abs(int.Parse(partsA[1])) + Math.Abs(int.Parse(partsA[2])) + Math.Abs(int.Parse(partsA[3]));

				if (acceleration < slowestAcceleration)
				{
					slowestIndex = index;
					slowestSpeed = speed;
					slowestAcceleration = acceleration;
					lowestdistance = distance;
				}
				else if (acceleration == slowestAcceleration)
				{
					if (speed < slowestSpeed)
					{
						slowestSpeed = speed;
						slowestAcceleration = acceleration;
						slowestIndex = index;
						lowestdistance = distance;
					}
					else if (speed == slowestSpeed)
					{
						if (distance < lowestdistance)
						{
							slowestSpeed = speed;
							slowestAcceleration = acceleration;
							slowestIndex = index;
							lowestdistance = distance;
						}
						else if (distance == lowestdistance)
						{
							// duplicate?
							throw new Exception("Duplicate particle found...");
						}
					}
				}
			}
			return slowestIndex;
		}

		public override object RunPart2()
		{
			var particles = new List<Particle>();
			foreach (var line in Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
			{
				var parts = line.Split(", ");
				var partsP = parts[0].Split(new[] { '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries); 
				var partsV = parts[1].Split(new[] { '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);
				var partsA = parts[2].Split(new[] { '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);
				var position = new Vector3(int.Parse(partsP[1]), int.Parse(partsP[2]), int.Parse(partsP[3]));
				var velocity = new Vector3(int.Parse(partsV[1]), int.Parse(partsV[2]), int.Parse(partsV[3]));
				var acceleration = new Vector3(int.Parse(partsA[1]), int.Parse(partsA[2]), int.Parse(partsA[3]));
				particles.Add(new Particle(position, velocity, acceleration));
			}

			// brute force 1000 iterations. Would be better to determine if the particles left will ever collide..
			for (var i = 0; i < 1000; i++)
			{
				particles.ForEach(p => p.Tick());
				var collisions = particles.GroupBy(p => p.Position).Where(g => g.Skip(1).Any()).SelectMany(p => p);
				particles = particles.Except(collisions).ToList();
			}
			return particles.Count;
		}

		private class Particle
		{
			private readonly Vector3 _acceleration;
			private Vector3 _velocity;
			
			public Particle(Vector3 position, Vector3 velocity, Vector3 acceleration)
			{
				Position = position;
				_velocity = velocity;
				_acceleration = acceleration;
			}

			public Vector3 Position { get; private set; }
			
			public void Tick()
			{
				_velocity += _acceleration;
				Position += _velocity;
			}
		}
	}
}