using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2016.Lib;

namespace AdventOfCode2016.Days
{
	public class Day24 : Day
	{
		public Day24(string input)
			: base(24, input)
		{
		}

		public override object RunPart1()
		{
			var grid = Input.Split('\n').Select(l=>l.Trim().ToCharArray()).ToArray();
			var destinations = new Dictionary<char, Point>();
			for(var y=0; y < grid.Length; y++)
			{
				for(var x=0; x < grid[y].Length; x++)
				{
					var c = grid[y][x];
					if(c != '.' && c != '#') 
						destinations.Add(c, new Point(x,y));
				}
			}

			var start = destinations['0'];
			destinations.Remove('0');

			var pathCache = new Dictionary<Point,Dictionary<Point,int>>();
			var pathFinder = new PathFinder(grid);
			var shortestRoute = int.MaxValue;
			foreach(var route in destinations.Keys.ToList().GetPermutations())
			{
				var routeLength = 0;
				var from = start;
				foreach(var c in route)
				{
					var to = destinations[c];
					if(pathCache.ContainsKey(from) && pathCache[from].ContainsKey(to))
					{
						routeLength += pathCache[from][to];
					}
					else
					{
						var path = pathFinder.FindPath(from, to);
						routeLength += path.Count;

						// fill pathCache
						if(!pathCache.ContainsKey(from))
							pathCache[from] = new Dictionary<Point, int>();
						if(!pathCache.ContainsKey(to))
							pathCache[to] = new Dictionary<Point, int>();
						pathCache[from][to] = path.Count;
						pathCache[to][from] = path.Count;
					}
					if(routeLength > shortestRoute)
						break; 
					from = to;
				}
				if(routeLength < shortestRoute)
					shortestRoute = routeLength;
			}
			
			return shortestRoute;
		}

		public override object RunPart2()
		{
			var grid = Input.Split('\n').Select(l=>l.Trim().ToCharArray()).ToArray();
			var destinations = new Dictionary<char, Point>();
			for(var y=0; y < grid.Length; y++)
			{
				for(var x=0; x < grid[y].Length; x++)
				{
					var c = grid[y][x];
					if(c != '.' && c != '#') 
						destinations.Add(c, new Point(x,y));
				}
			}

			var start = destinations['0'];
			var pathCache = new Dictionary<Point,Dictionary<Point,int>>();
			var pathFinder = new PathFinder(grid);
			var shortestRoute = int.MaxValue;
			foreach(var route in destinations.Keys.Except(new[]{'0'}).ToList().GetPermutations())
			{
				var routeLength = 0;
				var from = start;
				var r = new List<char>(route) {'0'};
				foreach(var c in r)
				{
					var to = destinations[c];
					if(pathCache.ContainsKey(from) && pathCache[from].ContainsKey(to))
					{
						routeLength += pathCache[from][to];
					}
					else
					{
						var path = pathFinder.FindPath(from, to);
						routeLength += path.Count;

						// fill pathCache
						if(!pathCache.ContainsKey(from))
							pathCache[from] = new Dictionary<Point, int>();
						if(!pathCache.ContainsKey(to))
							pathCache[to] = new Dictionary<Point, int>();
						pathCache[from][to] = path.Count;
						pathCache[to][from] = path.Count;
					}
					if(routeLength > shortestRoute)
						break; 
					from = to;
				}
				if(routeLength < shortestRoute)
					shortestRoute = routeLength;
			}
			
			return shortestRoute;
		}

		private class PathFinder
		{
			private readonly char[][] _grid;
			private Dictionary<Point, Node> _nodes;
			private Node _startNode;
			private Node _endNode;

			public PathFinder(char[][] grid)
			{
				_grid = grid;				
			}

			/// <summary>
			/// Attempts to find a path from the start location to the end location based on the supplied SearchParameters
			/// </summary>
			/// <returns>A List of Points representing the path. If no path was found, the returned list is empty.</returns>
			public List<Point> FindPath(Point startLocation, Point endLocation)
			{
				_nodes = new Dictionary<Point, Node>();

				_startNode = new Node(startLocation, true, endLocation);
				_startNode.State = NodeState.Open;
				_nodes.Add(_startNode.Location, _startNode);

				_endNode = new Node(endLocation, true, endLocation);
				_nodes.Add(_endNode.Location, _endNode);

				// The start node is the first entry in the 'open' list
				var path = new List<Point>();

				var currentNode = _startNode;
				while (currentNode.Location != _endNode.Location)
				{
					if (Search(currentNode))
						break; // end node found
					
					currentNode = _nodes.Values.Where(n => n.State != NodeState.Closed && n.IsWalkable && n.ParentNode != null).OrderBy(n => n.F).FirstOrDefault(); // need to speed this up!
					if (currentNode == null) // no path!
						return path;
				}
				
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
					if (location.Y >= _grid.Length || location.X >= _grid[location.Y].Length)
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
							var traversalCost = Node.GetTraversalCost(node.Location, fromNode.Location);   // ???
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
				yield return new Point(fromLocation.X - 1, fromLocation.Y);
				yield return new Point(fromLocation.X, fromLocation.Y + 1);
				yield return new Point(fromLocation.X + 1, fromLocation.Y);
				yield return new Point(fromLocation.X, fromLocation.Y - 1);
			}

			private bool IsWalkable(Point location)
			{
				var x = location.X;
				var y = location.Y;
				return _grid[y][x] != '#';
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
					return $"{Location.X}, {Location.Y}: {State} {IsWalkable}";
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