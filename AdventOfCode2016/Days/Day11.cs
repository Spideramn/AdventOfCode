using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode2016.Lib;

namespace AdventOfCode2016.Days
{
	public class Day11 : Day
	{
		public Day11(string input)
			: base(11, input)//"The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.\r\nThe second floor contains a hydrogen generator.\r\nThe third floor contains a lithium generator.\r\nThe fourth floor contains nothing relevant.")
		{
		}

		public override object RunPart1()
		{
			var isotopes = new Dictionary<char, string>();
			var matches = Regex.Matches(Input, @"([a-z]*)\sgenerator");
			for(var i = 0; i< matches.Count; i++)
				isotopes.Add((char)(i+65), matches[i].Groups[1].Value);
			
			var sb = new StringBuilder("1");			
			foreach (var line in Input.Split('\n').Select(l => l.Trim()))
			{
				var generators = Regex.Matches(line, @"([a-z]*)\sgenerator")
					.Cast<Match>()
					.Select(m => m.Groups[1].Value)
					.ToList();
				var microchips = Regex.Matches(line, @"([a-z]*)\-compatible")
					.Cast<Match>()
					.Select(m => m.Groups[1].Value)
					.ToList();
				foreach(var isotope in isotopes)
				{
					sb.Append(generators.Contains(isotope.Value) ? isotope.Key : '.');
					sb.Append(microchips.Contains(isotope.Value) ? isotope.Key : '.');
				}
			}
			var layout = sb.ToString();
			PrintState(layout);
			Console.WriteLine();

			var solver = new Solver();
			return solver.Solve(layout);
		}
		public override object RunPart2()
		{
			var isotopes = new Dictionary<char, string>();
			var matches = Regex.Matches(Input, @"([a-z]*)\sgenerator");
			int i;
			for (i = 0; i < matches.Count; i++)
				isotopes.Add((char)(i + 65), matches[i].Groups[1].Value);
			isotopes.Add((char)(i++ + 65), "elerium");
			isotopes.Add((char)(i++ + 65), "dilithium");

			var first = true;
			var sb = new StringBuilder("1");
			foreach (var line in Input.Split('\n').Select(l => l.Trim()))
			{
				var generators = Regex.Matches(line, @"([a-z]*)\sgenerator")
					.Cast<Match>()
					.Select(m => m.Groups[1].Value)
					.ToList();
				var microchips = Regex.Matches(line, @"([a-z]*)\-compatible")
					.Cast<Match>()
					.Select(m => m.Groups[1].Value)
					.ToList();

				if (first)
				{
					generators.AddRange(new [] { "elerium", "dilithium" });
					microchips.AddRange(new[] { "elerium", "dilithium" });
				}

				foreach (var isotope in isotopes)
				{
					sb.Append(generators.Contains(isotope.Value) ? isotope.Key : '.');
					sb.Append(microchips.Contains(isotope.Value) ? isotope.Key : '.');
				}
				first = false;
			}
			var layout = sb.ToString();
			PrintState(layout);
			Console.WriteLine();

			var solver = new Solver();
			return solver.Solve(layout);
		}


		private class Solver
		{
			private Dictionary<string, Node> _nodes;
			private List<Node> _openNodes;
			public int Solve(string startState)
			{
				_nodes = new Dictionary<string, Node>();
				_openNodes = new List<Node>();

				var startNode = new Node(startState, 0);
				_nodes.Add(startNode.Layout, startNode);
				_openNodes.Add(startNode);

				var currentNode = startNode;
				while (currentNode != null)
				{
					// process node
					currentNode.State = NodeState.Closed;
					_openNodes.Remove(currentNode);

					// get possible moves
					foreach(var node in GetAdjacentNodes(currentNode))
					{
						if(node.IsFinish)
							return node.G;
					}

					currentNode = _openNodes.OrderBy(n => n.G).FirstOrDefault(); // speed this up. Make sure Node.F is a valid number. doesnt make sense now...
				}
				return -1;
			}

			private IEnumerable<Node> GetAdjacentNodes(Node currentNode)
			{
				foreach(var layout in AdjacentLayouts(currentNode))
				{
					if(!_nodes.ContainsKey(layout))
					{
						// new node
						var node = new Node(layout, currentNode.G+1);
						node.ParentNode = currentNode;
						_nodes.Add(node.Layout, node);
						_openNodes.Add(node);
						yield return node;
					}
					else
					{
						var node = _nodes[layout];
						if(node.State == NodeState.Open)
						{
							// open node, but going though here is shorter!
							if(currentNode.G+1 < node.G)
							{
								node.ParentNode = currentNode;
								node.G = currentNode.G + 1;
								yield return node;
							}
						}
					}
				}
			}
			private static IEnumerable<string> AdjacentLayouts(Node node)
			{
				var lineLength = (node.Layout.Length - 1) / Node.TotalFloors;
				
				var combinations = Enumerable.Range(0, lineLength).ToList().GetCombinations(2).ToList();
				combinations.AddRange(Enumerable.Range(0, lineLength).Select(i => new []{i}));

				var currentFloorIndex = 1+((node.Elevator-1)*lineLength);

				var otherFloors = new List<int>();
				// can we go up?
				if(node.Elevator < Node.TotalFloors)
					otherFloors.Add(node.Elevator + 1);

				// can we go down
				if (node.Elevator > 1)
				{
					// stop going down when all floors beneath are empty!
					var floorsBeneath = node.Layout.Substring(1, lineLength * (node.Elevator - 1));
					if (floorsBeneath.Any(c => c != '.'))
						otherFloors.Add(node.Elevator - 1);
				}
				
				// new floor locations
				foreach(var newFloor in otherFloors)
				{
					var newFloorIndex = 1+((newFloor-1)*lineLength);

					foreach(var combo in combinations)
					{
						var valid = true;

						// create new layout
						var layout = node.Layout.ToCharArray();
						layout[0] = (char)((newFloor)+'0');
						foreach(var c in combo)
						{
							if(layout[currentFloorIndex+c] == '.') // we cannot move empty nodes
							{
								valid = false;
								break;
							}
							
							layout[newFloorIndex+c] = layout[currentFloorIndex+c];
							layout[currentFloorIndex + c] = '.';
						}

						if (!valid)
							continue;

						// validate new floor layouts
						var l = new string(layout);
						foreach (var floor in new [] { newFloor-1 , node.Elevator-1 })
						{
							var fl = l.Substring(1 + (floor * lineLength), lineLength);

							// geen microchips, geen generator == OK
							if (fl.All(e => e == '.'))
								continue;

							var generators = fl.Where((e, i) => i % 2 == 0 && e != '.').ToList();
							var microchips = fl.Where((e, i) => i % 2 != 0 && e != '.').ToList();

							// no generators == OK
							// all microchips have a generator == ok
							if (!generators.Any() || microchips.All(m => generators.Contains(m)))
								continue;

							valid = false;
							break;
						}


						if(valid)
							yield return l;
					}
				}
			}

			private enum NodeState
			{
				Open,
				Closed
			}
			private class Node
			{
				public const int TotalFloors = 4;
				public Node(string layout, int g)
				{
					Layout = layout;
					State = NodeState.Open;
					G = g;
					
					Elevator = Layout[0] - '0';

					var lineLength = (Layout.Length - 1) / TotalFloors;
					Floors = new string[TotalFloors];
					for(var floor = 0; floor < TotalFloors; floor++)
						Floors[floor] = Layout.Substring(1+(floor*lineLength), lineLength);

					H = CalculateH();

					IsFinish = IsFinished();
				}
				public bool IsFinish { get; }
				public string Layout { get; }
				public int Elevator { get; }
				public string[] Floors { get; }

				// start to here (steps)
				public int G { get; set; }
				// from here to end (estimate)
				public int H { get; private set; }
				public int F => -H;
				public NodeState State { get; set; }

				public Node ParentNode {get;set;}
			
				private int CalculateH()
				{
					// estimate of how far we are away from the "finish"
					var floor4 = Floors[3].Count(c => c != '.');
					var floor3 = Floors[2].Count(c => c != '.');
					var floor2 = Floors[1].Count(c => c != '.');
					var floor1 = Floors[0].Count(c => c != '.');

					return floor4*5 + floor3*2 + floor2;//*2 + floor1;
				}

				private bool IsFinished()
				{
					// elevator at upper floor
					if(Elevator != TotalFloors)
						return false;
					
					// all elements at upper floor
					if(Floors[TotalFloors-1].Contains('.'))
						return false;

					return true;
				}
			}
		}

		private static void PrintState(string state)
		{
			var floors = 4;
			var elevator = state[0] - '0';
			var lineLength = (state.Length - 1) / floors;

			for(var floor = floors; floor > 0; floor--)
			{
				Console.Write("F" + floor + " ");
				Console.Write(elevator == floor ? 'E' : '.');
				Console.Write(" | ");
				for(var index = 1+((floor-1)*lineLength); index < 1+(floor*lineLength); index++)
				{
					if(state[index] == '.')
					{
						Console.Write(".  ");
					}
					else
					{
						Console.Write(state[index]);
						Console.Write(index%2==0 ? 'M' : 'G');
						Console.Write(' ');
					}
				}
				Console.WriteLine();
			}
		}
	}
}