using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days.Day08
{
	internal class Program : Day
	{
		public override int DayNumber => 8;

		public override object RunPart1()
		{
			var input = GetInputString();
			return GetNodeList(input).SelectMany(node => node.MetaData).Sum();
		}

		public override object RunPart2()
		{
			var input = GetInputString();
			return GetNodeList(input).First().Value;
		}
		
		private static List<Node> GetNodeList(string input)
		{
			Node workingNode = null;
			var nodeList = new List<Node>();
			var numbers = new Queue<int>(input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
			while (numbers.TryDequeue(out var number))
			{
				var childNodeCount = number;
				var metaDataCount = numbers.Dequeue();

				workingNode = new Node(childNodeCount, metaDataCount, workingNode);
				nodeList.Add(workingNode);

				while (workingNode.Closed)
				{
					for (var i = 0; i < workingNode.MetaDataCount; i++)
						workingNode.MetaData.Add(numbers.Dequeue());

					if (workingNode.ParentNode == null)
						break;

					workingNode.ParentNode.ChildNodes.Add(workingNode);
					workingNode = workingNode.ParentNode;
				}
			}
			return nodeList;
		}
		
		private class Node
		{
			public Node(int childNodeCount, int metaDataCount, Node parentNode)
			{
				ChildNodeCount = childNodeCount;
				MetaDataCount = metaDataCount;
				ParentNode = parentNode;
			}

			public int MetaDataCount { get; }
			public int ChildNodeCount { get; }
			public List<Node> ChildNodes { get; } = new List<Node>();
			public List<int> MetaData { get; } = new List<int>();

			public Node ParentNode { get; }

			public bool Closed => ChildNodeCount == ChildNodes.Count;

			public int Value
			{
				get
				{
					if (ChildNodeCount == 0)
						return MetaData.Sum();

					var value = 0;
					foreach (var entry in MetaData)
					{
						if (entry > 0 && entry <= ChildNodes.Count)
							value += ChildNodes[entry - 1].Value;
					}
					return value;
				}
			}
		}
	}
}