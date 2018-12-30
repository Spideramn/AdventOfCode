using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.Days.Day15
{
	internal class Program : Day
	{
		public override int DayNumber => 15;

		public override object RunPart1()
		{
			var units = new UnitList();
			var y = 0;
			foreach (var line in GetInputLines())
			{
				for (var x = 0; x < line.Length; x++)
				{
					switch (line[x])
					{
						case '#':
							units.Walls.Add(new Point(x, y));
							break;
						case 'G':
							units.Add(new Unit(UnitType.Goblin, x, y));
							break;
						case 'E':
							units.Add(new Unit(UnitType.Elf, x, y));
							break;
					}
				}
				y++;
			}

			var cursorPosition = new Point(Console.CursorLeft, Console.CursorTop);
			var round = 0;
			DrawGrid(units, round);
			while (true)
			{
				foreach (var unit in units.OrderedUnits)
				{
					if (!unit.IsAlive)
						continue;

					// check for targets!
					if (!units.HasTargets)
					{
						DrawGrid(units, round, cursorPosition);

						var hpSum = units.AliveUnits.Sum(u => u.Hp);
						Console.WriteLine($"{round} * {hpSum}");
						return round * hpSum;
					}

						unit.MoveAndAttack(units);
				}

				round++;
				DrawGrid(units, round, cursorPosition);
				//Console.ReadKey();
			}
		}


		public override object RunPart2()
		{
			var elfAttack = 3;
			while (true)
			{
				elfAttack++;
				var elfDied = false;
				var units = new UnitList();
				var y = 0;
				foreach (var line in GetInputLines())
				{
					for (var x = 0; x < line.Length; x++)
					{
						switch (line[x])
						{
							case '#':
								units.Walls.Add(new Point(x, y));
								break;
							case 'G':
								units.Add(new Unit(UnitType.Goblin, x, y));
								break;
							case 'E':
								units.Add(new Unit(UnitType.Elf, x, y, elfAttack));
								break;
						}
					}
					y++;
				}

				var cursorPosition = new Point(Console.CursorLeft, Console.CursorTop);
				var round = 0;
				DrawGrid(units, round, null, elfAttack);
				while (!elfDied)
				{
					foreach (var unit in units.OrderedUnits)
					{
						if (!unit.IsAlive)
							continue;

						// check for targets!
						if (!units.HasTargets)
						{
							DrawGrid(units, round, cursorPosition, elfAttack);

							var hpSum = units.AliveUnits.Sum(u => u.Hp);
							Console.WriteLine($"{round} * {hpSum}");
							return round * hpSum;
						}

						var unitAttacked = unit.MoveAndAttack(units);
						if (unitAttacked != null && unitAttacked.UnitType == UnitType.Elf && !unitAttacked.IsAlive)
						{
							// elf died...
							elfDied = true;
							break;
						}
					}

					round++;
					DrawGrid(units, round, cursorPosition, elfAttack);
					//Console.ReadKey();
				}
			}
		}


		private void DrawGrid(UnitList units, int round, Point? cursor = null, int? elfAttack = null)
		{
			if (cursor.HasValue)
				Console.SetCursorPosition(cursor.Value.X, cursor.Value.Y);
			Console.WriteLine($"Round: {round} | Elf attack power: {elfAttack.GetValueOrDefault(3)}");

			var aliveUnits = units.AliveUnits
				.OrderBy(u => u.Position, units)
				.Select((unit, index) => (unit, index)).ToList();

			var maxX = units.Walls.Max(p => p.X);
			var maxY = units.Walls.Max(p => p.Y);
			for (var y = 0; y <= maxY; y++)
			{
				var unitsOnLine = new List<string>();
				for (var x = 0; x <= maxX; x++)
				{
					var point = new Point(x,y);
					if (units.Walls.Contains(point))
					{
						Console.Write('#');
					}
					else
					{
						var unit = aliveUnits.FirstOrDefault(u => u.unit.Position == point);
						if (unit.unit != null)
						{
							var unitType = unit.unit.UnitType == UnitType.Goblin ? 'G' : 'E';
							Console.Write(unitType);
							unitsOnLine.Add($"{unitType} ({unit.unit.Hp})");
						}
						else
						{
							Console.Write('.');
						}
					}
				}
				Console.Write("   " + string.Join(", ", unitsOnLine));
				Console.WriteLine(new string(' ', Console.BufferWidth - Console.CursorLeft - 1));
			}
			Console.WriteLine();
		}


		private class Unit
		{
			private Point _position;

			public UnitType UnitType { get; }
			public int Hp { get; private set; } = 200;
			public int AttackPower { get; }
			public Point Position => _position;
			public bool IsAlive => Hp > 0;

			public Point[] AdjacentPoints => new[] {
					new Point(_position.X, _position.Y-1),
					new Point(_position.X-1, _position.Y),
					new Point(_position.X+1, _position.Y),
					new Point(_position.X, _position.Y+1)
				};

			public Unit(UnitType unitType, int x, int y, int attackPower = 3)
			{
				UnitType = unitType;
				_position = new Point(x, y);
				AttackPower = attackPower;
			}

			public Unit MoveAndAttack(UnitList units)
			{
				// move
				var enemyUnitInRange = units.EnemyInRange(this);
				if (enemyUnitInRange == null)
				{
					var closedPoints = units.ClosedPoints();
					
					// get distance to each point
					var enemies = units.GetOpenEnemyPointsInrange(this).ToList();

					var paths = enemies.Select(p => PathFinder.FindPath(_position, p, closedPoints)) // get path of each point
						.Where(p => p != null && p.Any())
						.ToList(); // only "reachable" paths
					
					var pathsGrouped = paths.GroupBy(p => p.Count) // group by length of path
						.OrderBy(g => g.Key) // order by path length
						.FirstOrDefault(); // first group (shortest path)

					var point = pathsGrouped?
						.Select(p => (First:p[1], Last:p.Last())) // select first step in path
						.OrderBy(p => p.Last, units) // sort by 'reading order'
						.FirstOrDefault(); // new position!

					if (point.HasValue)
						_position = point.Value.First;

					// now in range?
					enemyUnitInRange = units.EnemyInRange(this);
				}

				// attack!
				if (enemyUnitInRange != null)
					enemyUnitInRange.Hp -= AttackPower;
				return enemyUnitInRange;
			}
		}
		private enum UnitType
		{
			Goblin,
			Elf
		}
		
		private class UnitList : List<Unit>, IComparer<Point>
		{
			public HashSet<Point> Walls = new HashSet<Point>();

			public bool HasTargets => this.GroupBy(u => u.UnitType).All(ug => ug.Any(u => u.IsAlive));
			public IEnumerable<Unit> AliveUnits => this.Where(u => u.IsAlive);
			public IEnumerable<Unit> OrderedUnits => this.OrderBy(u => u.Position, this);

			public HashSet<Point> ClosedPoints()
			{
				var closedPoints = new HashSet<Point>(Walls);
				foreach (var unit in AliveUnits)
					closedPoints.Add(unit.Position);
				return closedPoints;
			}

			public Unit EnemyInRange(Unit unit)
			{
				return this
					.Where(u => u.IsAlive &&
								u.UnitType != unit.UnitType &&
								unit.AdjacentPoints.Contains(u.Position))
					.OrderBy(u => u.Hp)
					.ThenBy(u => u.Position, this)
					.FirstOrDefault();				
			}

			public IEnumerable<Point> GetOpenEnemyPointsInrange(Unit unit)
			{
				var closedPoints = ClosedPoints();
				return this
					.Where(u => u.IsAlive && u.UnitType != unit.UnitType)
					.SelectMany(u => u.AdjacentPoints)
					.Where(p => !closedPoints.Contains(p))
					.Distinct();
			}

			public int Compare(Point first, Point second)
			{
				if (first.Y == second.Y)
					return first.X - second.X;
				return first.Y - second.Y;
			}
		}

		private static class PathFinder
		{
			public static List<Point> FindPath(Point start, Point finish, HashSet<Point> closedPoints)
			{
				var parent = new Dictionary<Point, Point?>();
				var queue = new Queue<Point>();
				var visited = new List<Point>();

				queue.Enqueue(start);
				parent.Add(start, null);

				while (queue.Count != 0)
				{
					var c = queue.Dequeue();

					visited.Add(c);

					if (c == finish)
						break;

					var adjacent = new[] {
						new Point(c.X, c.Y-1),
						new Point(c.X-1, c.Y),
						new Point(c.X+1, c.Y),
						new Point(c.X, c.Y+1)
					};

					foreach (var near in adjacent)
					{
						if (near != null)
						{
							if (closedPoints.Contains(near))
								continue;
							if (!visited.Contains(near))
							{
								parent.Add(near, c);
								visited.Add(near);
								queue.Enqueue(near);
							}
						}
					}
				}

				var path = new List<Point>();

				if (parent.ContainsKey(finish))
				{
					Point? backTrack = finish;
					do
					{
						path.Add(backTrack.Value);
						backTrack = parent[backTrack.Value];
					}
					while (backTrack != null);
					path.Reverse();
				}

				return path;
			}
		}
	}
}