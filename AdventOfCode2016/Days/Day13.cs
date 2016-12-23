using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2016.Days
{
	public class Day13 : Day
	{
		public Day13(string input)
			: base(13, input)
		{
		}

		#region Part 1
		public override object RunPart1()
		{
			var pathfinder = new PathFinderPart1(new Point(1, 1), new Point(31, 39), int.Parse(Input));
			var path = pathfinder.FindPath();
			return path.Count;
		}
		
		private class PathFinderPart1
		{
			private readonly Dictionary<Point, Node> _nodes = new Dictionary<Point, Node>();
			private readonly Node _startNode;
			private readonly Node _endNode;
			private readonly int _favoriteNumber;

			public PathFinderPart1(Point startLocation, Point endLocation, int favoriteNumber)
			{
				_favoriteNumber = favoriteNumber;
				_startNode = new Node(startLocation, true, endLocation);
				_startNode.State = NodeState.Open;
				_nodes.Add(_startNode.Location, _startNode);

				_endNode = new Node(endLocation, true, endLocation);
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
				var nextNodes = GetAdjacentWalkableNodes(currentNode).OrderBy(n => n.F).ToList();
				foreach (var nextNode in nextNodes)
				{
					// Check whether the end node has been reached
					if (nextNode.Location == _endNode.Location)
						return true;
				}

				var node = _nodes.Values.Where(n => n.State != NodeState.Closed && n.IsWalkable && n.ParentNode != null).OrderBy(n => n.F).First();
				if (node.Location == _endNode.Location)
					return true;

				// If not, check the next set of nodes
				if (Search(node)) // Note: Recurses back into Search(Node)
					return true;

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
					// Stay within the grid's boundaries
					if (location.X < 0 || location.Y < 0)
						continue;

					Node node;
					if (!_nodes.ContainsKey(location))
					{
						node = new Node(location, IsWalkable(location), _endNode.Location);
						_nodes.Add(node.Location, node);
					}
					else
					{
						node = _nodes[location];
					}

					// Ignore non-walkable nodes
					if (!node.IsWalkable)
						continue;
					
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

			private static IEnumerable<Point> GetAdjacentLocations(Point fromLocation)
			{
				yield return new Point(fromLocation.X - 1, fromLocation.Y);
				yield return new Point(fromLocation.X, fromLocation.Y + 1);
				yield return new Point(fromLocation.X + 1, fromLocation.Y);
				yield return new Point(fromLocation.X, fromLocation.Y - 1);
			}

			private bool IsWalkable(Point location)
			{
				var x = location.X;
				var y = location.Y;
				var n = x * x + 3 * x + 2 * x * y + y + y * y + _favoriteNumber;
				var count = 0;
				while (n > 0)
				{
					n = n & (n - 1);
					count++;
				}
				return count % 2 != 1;
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
				public bool IsWalkable { get; set; }

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
				/// <param name="isWalkable">True if the node can be traversed, false if the node is a wall</param>
				/// <param name="endLocation">The location of the destination node</param>
				public Node(Point location, bool isWalkable, Point endLocation)
				{
					Location = location;
					State = NodeState.Untested;
					IsWalkable = isWalkable;
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

		#endregion

		#region Part 2

		public override object RunPart2()
		{
			var pathfinder = new PathFinderPart2(int.Parse(Input));
			return pathfinder.Run(new Point(1, 1));
		}

		private class PathFinderPart2
		{
			private readonly Dictionary<Point, Node> _nodes = new Dictionary<Point, Node>();
			private readonly Queue<Node> _openNodes = new Queue<Node>();
			private readonly int _favoriteNumber;
			public PathFinderPart2(int favoriteNumber)
			{
				_favoriteNumber = favoriteNumber;
			}

			
			public int Run(Point start)
			{
				_nodes.Clear();
				_openNodes.Clear();
				_nodes.Add(start, new Node(start));
				_openNodes.Enqueue(_nodes[start]);
				while (_openNodes.Count > 0)
				{
					var node = _openNodes.Dequeue();
					if (node.Distance >= 50) continue;

					foreach (var location in AdjecentLocations(node.Location))
					{
						if (location.X < 0 || location.Y < 0)
							continue;

						if (IsWall(location))
							continue;

						// get node form list
						Node newNode;
						if (_nodes.ContainsKey(location)) // eerder gehad
						{
							newNode = _nodes[location];

							// korter? nog maals proberen!
							if (newNode.Distance > node.Distance + 1)
							{
								newNode.Distance = node.Distance + 1;
								_openNodes.Enqueue(newNode);
							}
						}
						else
						{
							// nieuwe node
							newNode = new Node(location, node.Distance + 1);
							_nodes.Add(location, newNode);
							_openNodes.Enqueue(newNode);
						}
					}
				}
				return _nodes.Count;
			}

			private IEnumerable<Point> AdjecentLocations(Point location)
			{
				yield return new Point(location.X, location.Y - 1);
				yield return new Point(location.X, location.Y + 1);
				yield return new Point(location.X - 1, location.Y);
				yield return new Point(location.X + 1, location.Y);
			}

			private bool IsWall(Point location)
			{
				var x = location.X;
				var y = location.Y;
				var n = x * x + 3 * x + 2 * x * y + y + y * y + _favoriteNumber;
				var count = 0;
				while (n > 0)
				{
					n = n & (n - 1);
					count++;
				}
				return count % 2 == 1;
			}

			private class Node
			{
				public Node(Point location, int distance = 0)
				{
					Location = location;
					Distance = distance;
				}

				public Point Location { get; } // locatie van de Node
				public int Distance { get; set; } // afstand van start tot hier
			}
		}
		
		#endregion
	}
}