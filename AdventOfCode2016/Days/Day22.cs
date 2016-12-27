using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AdventOfCode2016.Lib;

namespace AdventOfCode2016.Days
{
	public class Day22 : Day
	{
		public Day22(string input)
			: base(22, input)
		{
		}

		public override object RunPart1()
		{
			//var nodes = Input.Split('\n').Skip(2).Select(l => new PcNode(l)).ToList();
			//var comb = nodes.GetPermutations(2);
			//return comb.Count(c => PcNode.Viable(c[0], c[1]));
			return Input
				.Split('\n')
				.Skip(2)
				.Select(l => new PcNode(l))
				.ToList()
				.GetPermutations(2)
				.Count(c => PcNode.Viable(c[0], c[1]));
		}

		public override object RunPart2()
		{
			var emptyNode = Point.Empty;
			var grid = new Dictionary<int, Dictionary<int, PcNode>>();
			foreach (var line in Input.Split('\n').Skip(2))
			{
				var node = new PcNode(line);
				if (!grid.ContainsKey(node.Y))
					grid.Add(node.Y, new Dictionary<int, PcNode>());
				grid[node.Y][node.X] = node;
				if (node.Used == 0)
					emptyNode = node.Location;
			}

			// flag node containg data we need
			var locationContainingData = new Point(grid[0].Count - 1, 0);
			
			// draw grid
			//DrawGrid(false, grid, locationContainingData, emptyNode);

			var steps = 0;
			while (locationContainingData != Point.Empty)
			{
				// make sure empty node is at the left of the data we need.
				var emptyNodeDestination = new Point(locationContainingData.X-1, locationContainingData.Y);

				// find path for empty node
				var pathFinder = new PathFinder(grid, locationContainingData);
				var path = pathFinder.FindPath(emptyNode, emptyNodeDestination);
				//foreach (var step in path)
				//	DrawGrid(true, grid, locationContainingData, step);
				steps += path.Count;

				// put data in empty node
				emptyNode = locationContainingData;
				locationContainingData = emptyNodeDestination;
				steps++;

				//DrawGrid(true, grid, locationContainingData, emptyNode);
			}

			return steps;
		}
		
		private static void DrawGrid(bool redraw, Dictionary<int, Dictionary<int, PcNode>> grid, Point locationContainingData, Point emptyNode)
		{
			if (redraw)
			{
				Thread.Sleep(75);
				Console.SetCursorPosition(0, Console.CursorTop - grid.Count - 2);
			}

			var emptyNodeSize = grid[emptyNode.Y][emptyNode.X].Size;

			var s = new StringBuilder();
			s.Append('╔');
			s.Append('═', grid[0].Count);
			s.AppendLine("╗");
			for (var y = 0; y < grid.Count; y++)
			{
				s.Append("║");
				for (var x = 0; x < grid[y].Count; x++)
				{
					var node = grid[y][x];

					// output!
					if (y == 0 && x == 0)
						s.Append('X');

					// containing data we need
					else if (node.Location == locationContainingData)
						s.Append('G');

					// empty node
					else if (node.Location == emptyNode)
						s.Append('_');

					// node containing more data than empty node
					else if (node.Used > emptyNodeSize)
						s.Append('#');

					else
						s.Append('.');
				}
				s.AppendLine("║");
			}
			s.Append('╚');
			s.Append('═', grid[0].Count);
			s.Append('╝');
			Console.WriteLine(s.ToString());
		}

		private class PcNode
		{
			public PcNode(string line)
			{
				var parts = line.Trim().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				var location = parts[0].Substring(15).Split('-');
				Location = new Point(int.Parse(location[0].Substring(1)), int.Parse(location[1].Substring(1)));
				Size = int.Parse(parts[1].Trim('T'));
				Used = int.Parse(parts[2].Trim('T'));
			}

			public static bool Viable(PcNode a, PcNode b)
			{
				// Node A is not empty
				if (a.Used == 0)
					return false;
				// Nodes A and B are not the same node.
				if (a == b)
					return false;

				// The data on node A (its Used) would fit on node B (its Avail).
				if (a.Used > b.Available)
					return false;

				return true;
			}
			
			public Point Location { get; }
			public int Size { get; }
			public int Used { get; set; }
			public int Available => Size - Used;
			public int X => Location.X;
			public int Y => Location.Y;
		}
		
		private class PathFinder
		{
			private readonly Dictionary<int, Dictionary<int, PcNode>> _grid;
			private readonly Point _locationContainingData;
			private Dictionary<Point, Node> _nodes;
			private Node _startNode;
			private Node _endNode;

			public PathFinder(Dictionary<int, Dictionary<int, PcNode>> grid, Point locationContainingData)
			{
				_grid = grid;
				_locationContainingData = locationContainingData;
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
					if (location.Y >= _grid.Count || location.X >= _grid[location.Y].Count)
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
				if (location == _locationContainingData)
					return false;

				var x = location.X;
				var y = location.Y;
				if (_grid[y][x].Used > _grid[_startNode.Location.Y][_startNode.Location.X].Size)
					return false;
				return true;
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
