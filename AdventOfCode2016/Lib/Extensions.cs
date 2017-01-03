using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2016.Lib
{
	internal static class Extensions
	{
		public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> node)
		{
			return node.Next ?? node.List.First;
		}

		public static IEnumerable<T[]> GetPermutations<T>(this IList<T> list, int? length = null)
		{
			var l = length.GetValueOrDefault(list.Count);
			if (l == 1)
				return list.Select(t => new[] { t });

			return GetPermutations(list, l - 1)
				.SelectMany(
					t => list.Where(e => !t.Contains(e)).ToArray(),
					(t1, t2) => t1.Concat(new[] { t2 }).ToArray()
				);
		}

		/// <summary>
		/// Gets all combinations (of a given size) of a given list, either with or without reptitions.
		/// </summary>
		/// <typeparam name="T">The type of the elements in the list.</typeparam>
		/// <param name="list">The list of which to get combinations.</param>
		/// <param name="resultSize">The number of elements in each resulting combination; or <see langword="null"/> to get
		/// premutations of the same size as <paramref name="list"/>.</param>
		/// <param name="withRepetition"><see langword="true"/> to get combinations with reptition of elements;
		/// <see langword="false"/> to get combinations without reptition of elements.</param>
		/// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="resultSize"/> is less than zero.</exception>
		/// <remarks>
		/// The algorithm performs combinations in-place. <paramref name="list"/> is however not changed.
		/// </remarks>
		// http://stackoverflow.com/a/15103722
		public static IEnumerable<T[]> GetCombinations<T>(this IList<T> list, int? resultSize = null, bool withRepetition = false)
		{
			if (list == null)
				throw new ArgumentNullException(nameof(list));
			if (resultSize.HasValue && resultSize.Value <= 0)
				throw new ArgumentException("Value must be greater than zero, if specified.", nameof(resultSize));

			var result = new T[resultSize ?? list.Count];
			var indices = new int[result.Length];
			for (var i = 0; i < indices.Length; i++)
				indices[i] = withRepetition ? -1 : indices.Length - i - 2;

			var curIndex = 0;
			while (curIndex != -1)
			{
				indices[curIndex]++;
				if (indices[curIndex] == (curIndex == 0 ? list.Count : indices[curIndex - 1] + (withRepetition ? 1 : 0)))
				{
					indices[curIndex] = withRepetition ? -1 : indices.Length - curIndex - 2;
					curIndex--;
				}
				else
				{
					result[curIndex] = list[indices[curIndex]];
					if (curIndex < indices.Length - 1)
						curIndex++;
					else
					{	
						var ret = new T[result.Length];
						Array.Copy(result, ret, result.Length);
						yield return ret;
					}
				}
			}
		}
	}

}
