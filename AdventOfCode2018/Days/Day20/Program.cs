using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2018.Days.Day20
{
	internal class Program : Day
	{
		public override int DayNumber => 20;

		public override object RunPart1()
		{
			var input = new Queue<char>(GetInputString());
			var position = new Point(0, 0);
			var left = new Size(-1, 0);
			var right = new Size(1, 0);
			var up = new Size(0, -1);
			var down = new Size(0, 1);
			var map = new Dictionary<Point, HashSet<char>> { { position, new HashSet<char>() } };
			var backtracker = new Stack<Point>();
			while (input.TryDequeue(out var direction))
			{
				switch (direction)
				{
					case 'E':
						map[position].Add('E');
						position = Point.Add(position, right);
						if (map.ContainsKey(position))
							map[position].Add('W');
						else
							map[position] = new HashSet<char> { 'W' };
						break;
					case 'W':
						map[position].Add('W');
						position = Point.Add(position, left);
						if (map.ContainsKey(position))
							map[position].Add('E');
						else
							map[position] = new HashSet<char> { 'E' };
						break;
					case 'N':
						map[position].Add('N');
						position = Point.Add(position, up);
						if (map.ContainsKey(position))
							map[position].Add('S');
						else
							map[position] = new HashSet<char> { 'S' };
						break;
					case 'S':
						map[position].Add('S');
						position = Point.Add(position, down);
						if (map.ContainsKey(position))
							map[position].Add('N');
						else
							map[position] = new HashSet<char> { 'N' };
						break;
					case '(':
						backtracker.Push(position);
						break;
					case '|':
						if (input.Peek() == ')')
						{
							input.Dequeue();
							position = backtracker.Pop();
						}
						else
							position = backtracker.Peek();
						break;
					case ')':
						position = backtracker.Pop();
						break;
				}
			}
			//DrawMap(map);

			var longestPath = 0;
			var sortedPoints = map.Keys.OrderByDescending(p => Math.Abs(p.X) + Math.Abs(p.Y)).ToList();
			var roomsVisited = new HashSet<Point> { Point.Empty };

			foreach (var destination in sortedPoints)
			{
				if (roomsVisited.Contains(destination))
					continue;


				var path = new PathFinder(Point.Empty, destination, map).FindPath();

				foreach (var p in path)
					roomsVisited.Add(p);
				longestPath = Math.Max(longestPath, path.Count);
			}

			return longestPath;
		}

		public override object RunPart2()
		{
			var input = new Queue<char>(GetInputString());
			var position = new Point(0, 0);
			var left = new Size(-1, 0);
			var right = new Size(1, 0);
			var up = new Size(0, -1);
			var down = new Size(0, 1);
			var map = new Dictionary<Point, HashSet<char>> { { position, new HashSet<char>() } };
			var backtracker = new Stack<Point>();
			while (input.TryDequeue(out var direction))
			{
				switch (direction)
				{
					case 'E':
						map[position].Add('E');
						position = Point.Add(position, right);
						if (map.ContainsKey(position))
							map[position].Add('W');
						else
							map[position] = new HashSet<char> { 'W' };
						break;
					case 'W':
						map[position].Add('W');
						position = Point.Add(position, left);
						if (map.ContainsKey(position))
							map[position].Add('E');
						else
							map[position] = new HashSet<char> { 'E' };
						break;
					case 'N':
						map[position].Add('N');
						position = Point.Add(position, up);
						if (map.ContainsKey(position))
							map[position].Add('S');
						else
							map[position] = new HashSet<char> { 'S' };
						break;
					case 'S':
						map[position].Add('S');
						position = Point.Add(position, down);
						if (map.ContainsKey(position))
							map[position].Add('N');
						else
							map[position] = new HashSet<char> { 'N' };
						break;
					case '(':
						backtracker.Push(position);
						break;
					case '|':
						if (input.Peek() == ')')
						{
							input.Dequeue();
							position = backtracker.Pop();
						}
						else
							position = backtracker.Peek();
						break;
					case ')':
						position = backtracker.Pop();
						break;
				}
			}
			//DrawMap(map);

			return map.Keys
				.Except(new[] { Point.Empty })
				.AsParallel()
				.OrderByDescending(p => Math.Abs(p.X) + Math.Abs(p.Y))
				.Select(d => new PathFinder(Point.Empty, d, map).FindPath().Count)
				.Count(l => l >= 1000);

		}

		private static void DrawMap(Dictionary<Point, HashSet<char>> map)
		{
			var minX = map.Keys.Min(p => p.X);
			var maxX = map.Keys.Max(p => p.X);
			var minY = map.Keys.Min(p => p.Y);
			var maxY = map.Keys.Max(p => p.Y);
						
			Console.WriteLine(new string('#', ((maxX - minX + 1)*2) +1));
			for (var y = minY; y <= maxY; y++)
			{
				for (var x = minX; x <= maxX; x++)
				{
					var point = new Point(x,y);
					Console.Write(map.TryGetValue(point, out var room) && room.Contains('W') ? '|': '#');
					Console.Write(point.Equals(Point.Empty) ? 'X' : '.');
				}
				Console.WriteLine('#');
				for (var x = minX; x <= maxX; x++)
					Console.Write(map.TryGetValue(new Point(x, y), out var room) && room.Contains('S') ? "#-" : "##");
				Console.WriteLine('#');
			}
			Console.WriteLine();
		}

		private class PathFinder
		{
			private readonly Dictionary<Point, Node> _nodes = new Dictionary<Point, Node>();
			private readonly Node _startNode;
			private readonly Node _endNode;
			private readonly Dictionary<Point, HashSet<char>> _map;

			public PathFinder(Point startLocation, Point endLocation, Dictionary<Point, HashSet<char>> map)
			{
				_map = map;
				_startNode = new Node(startLocation, endLocation);
				_startNode.State = NodeState.Open;
				_nodes.Add(_startNode.Location, _startNode);

				_endNode = new Node(endLocation, endLocation);
				_nodes.Add(_endNode.Location, _endNode);
			}

			/// <summary>
			/// Attempts to find a path from the start location to the end location based on the supplied SearchParameters
			/// </summary>
			/// <returns>A List of Points representing the path. If no path was found, the returned list is empty.</returns>
			public List<Point> FindPath()
			{
				// The start node is the first entry in the 'open' list
				var path = new List<Point>();
				var success = Search(_startNode);
				if (!success)
					return path;

				// If a path was found, follow the parents from the end node to build a list of locations
				var node = _endNode;
				while (node.ParentNode != null)
				{
					path.Add(node.Location);
					node = node.ParentNode;
				}

				// Reverse the list so it's in the correct order when returned
				path.Reverse();

				return path;
			}

			/// <summary>
			/// Attempts to find a path to the destination node using <paramref name="currentNode"/> as the starting location
			/// </summary>
			/// <param name="currentNode">The node from which to find a path</param>
			/// <returns>True if a path to the destination has been found, otherwise false</returns>
			private bool Search(Node currentNode)
			{
				// Set the current node to Closed since it cannot be traversed more than once
				currentNode.State = NodeState.Closed;

				// Sort by F-value so that the shortest possible routes are considered first
				foreach (var nextNode in GetAdjacentWalkableNodes(currentNode).OrderBy(n => n.F))
				{
					// Check whether the end node has been reached
					if (nextNode.Location == _endNode.Location || Search(nextNode))
						return true;
				}

				// The method returns false if this path leads to be a dead end
				return false;
			}

			/// <summary>
			/// Returns any nodes that are adjacent to <paramref name="fromNode"/> and may be considered to form the next step in the path
			/// </summary>
			/// <param name="fromNode">The node from which to return the next possible nodes in the path</param>
			/// <returns>A list of next possible nodes in the path</returns>
			private IEnumerable<Node> GetAdjacentWalkableNodes(Node fromNode)
			{
				foreach (var location in GetAdjacentLocations(fromNode.Location))
				{
					Node node;
					if (!_nodes.ContainsKey(location))
					{
						node = new Node(location, _endNode.Location);
						_nodes.Add(node.Location, node);
					}
					else
					{
						node = _nodes[location];
					}
					
					switch (node.State)
					{
						case NodeState.Closed:
							continue;

						case NodeState.Open:
							// Already-open nodes are only added to the list if their G-value is lower going via this route.
							var traversalCost = Node.GetTraversalCost(node.Location, fromNode.Location);
							var gTemp = fromNode.G + traversalCost;
							if (gTemp < node.G)
							{
								node.ParentNode = fromNode;
								yield return node;
							}
							break;

						case NodeState.Untested:
							// If it's untested, set the parent and flag it as 'Open' for consideration
							node.ParentNode = fromNode;
							node.State = NodeState.Open;
							yield return node;
							break;

						default:
							throw new ArgumentOutOfRangeException(nameof(node.State));
					}
				}
			}

			private IEnumerable<Point> GetAdjacentLocations(Point fromLocation)
			{
				if (_map.TryGetValue(fromLocation, out var room))
				{
					if(room.Contains('W'))
						yield return new Point(fromLocation.X - 1, fromLocation.Y);
					if (room.Contains('S'))
						yield return new Point(fromLocation.X, fromLocation.Y + 1);
					if (room.Contains('E'))
						yield return new Point(fromLocation.X + 1, fromLocation.Y);
					if (room.Contains('N'))
						yield return new Point(fromLocation.X, fromLocation.Y - 1);
				}
			}


			private enum NodeState
			{
				Untested,
				Open,
				Closed
			}
			private class Node
			{
				private Node _parentNode;

				public Point Location { get; }

				/// <summary>
				/// Cost from start to here
				/// </summary>
				public float G { get; private set; }

				/// <summary>
				/// Estimated cost from here to end
				/// </summary>
				public float H { get; }

				/// <summary>
				/// Flags whether the node is open, closed or untested by the PathFinder
				/// </summary>
				public NodeState State { get; set; }

				/// <summary>
				/// Estimated total cost (F = G + H)
				/// </summary>
				public float F => G + H;

				/// <summary>
				/// Gets or sets the parent node. The start node's parent is always null.
				/// </summary>
				public Node ParentNode
				{
					get { return _parentNode; }
					set
					{
						// When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
						_parentNode = value;
						G = _parentNode.G + GetTraversalCost(Location, _parentNode.Location);
					}
				}

				/// <summary>
				/// Creates a new instance of Node.
				/// </summary>
				/// <param name="location">Location of the node</param>
				/// <param name="endLocation">The location of the destination node</param>
				public Node(Point location, Point endLocation)
				{
					Location = location;
					State = NodeState.Untested;
					H = GetTraversalCost(Location, endLocation);
					G = 0;
				}

				public override string ToString()
				{
					return $"{Location.X}, {Location.Y}: {State}";
				}

				/// <summary>
				/// Gets the distance between two points
				/// </summary>
				internal static float GetTraversalCost(Point location, Point otherLocation)
				{
					float deltaX = otherLocation.X - location.X;
					float deltaY = otherLocation.Y - location.Y;
					return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
				}
			}
		}
	}
}