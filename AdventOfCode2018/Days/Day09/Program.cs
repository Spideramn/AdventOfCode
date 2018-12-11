using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Days.Day09
{
	internal class Program : Day
	{
		public override int DayNumber => 9;

		public override object RunPart1()
		{
			var parts = GetInputString().Split(' ', StringSplitOptions.RemoveEmptyEntries);
			var playerCount = int.Parse(parts[0]);
			var highestMarble = int.Parse(parts[6]);

			return PlayGame(playerCount, highestMarble).Max();
		}

		public override object RunPart2()
		{
			var parts = GetInputString().Split(' ', StringSplitOptions.RemoveEmptyEntries);
			var playerCount = int.Parse(parts[0]);
			var highestMarble = long.Parse(parts[6]);

			return PlayGame(playerCount, highestMarble * 100).Max();
		}

		private static long[] PlayGame(int playerCount, long highestMarble)
		{
			var players = new long[playerCount];
			var currentPlayer = 0;

			var circle = new LinkedList<long>();
			var currentMarble = circle.AddLast(0);
			
			for (var marble = 1L; marble <= highestMarble; marble++)
			{
				if (++currentPlayer >= playerCount)
					currentPlayer = 0;

				if (marble % 23 == 0)
				{
					currentMarble = currentMarble.PreviousOrLast(6);
					var marbleToRemove = currentMarble.PreviousOrLast();
					players[currentPlayer] += marble + marbleToRemove.Value;
					circle.Remove(marbleToRemove);
				}
				else
				{
					currentMarble = circle.AddAfter(currentMarble.NextOrFirst(), marble);
				}
			}
			return players;
		}
	}

	internal static class Extensions
	{
		public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> node, int count)
		{
			for (var i = 0; i < count; i++)
				node = node.Next ?? node.List.First;
			return node;
		}
		public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> node)
		{
			return node.Next ?? node.List.First;
		}
		public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> node, int count)
		{
			for (var i = 0; i < count; i++)
				node = node.Previous ?? node.List.Last;
			return node;
		}
		public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> node)
		{
			return node.Previous ?? node.List.Last;
		}
	}
}