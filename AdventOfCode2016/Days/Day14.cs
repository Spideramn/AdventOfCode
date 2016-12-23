using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016.Days
{
	public class Day14 : Day
	{
		public Day14(string input)
			: base(14, input)
		{
		}

		public override object RunPart1()
		{
			return Run(0); 
		}

		public override object RunPart2()
		{
			return Run(2016);
		}

		private int Run(int stretch)
		{
			var keysFound = 0;
			var index = -1; // start at 0
			var salt = Input;
			var hashes = new MaxDictionary<int, string>(1000);
			using (var md5 = MD5.Create())
			{
				while (keysFound < 64)
				{
					index++;
					if (!hashes.ContainsKey(index))
						hashes.Add(index, GetHash(md5, salt + index, stretch));
					var hash = hashes[index];

					var c = HashChars(hash, 3);
					if (c != null)
					{
						// check next 1000 hashes for 5!
						for (var i = 1; i <= 1000; i++)
						{
							var index2 = index + i;
							if (!hashes.ContainsKey(index2))
								hashes.Add(index2, GetHash(md5, salt + index2, stretch));
							var hash2 = hashes[index2];
							var c2 = HashChars(hash2, 5);
							if (c2.HasValue)
							{
								if (c2.Value == c.Value)
								{
									keysFound++;
									Console.WriteLine($"Found key {keysFound} at index {index}");
									break;
								}
							}
						}
					}
				}
			}
			// Console.WriteLine(index);
			return index;
		}
		private static char? HashChars(string s, int count)
		{
			var lastChar = '?';
			var lastCount = 1;
			foreach (var c in s)
			{
				if (c == lastChar)
				{
					lastCount++;
					if (lastCount == count)
						return c;
				}
				else
				{
					lastChar = c;
					lastCount = 1;
				}
			}
			return null;
		}
		private static string GetHash(HashAlgorithm md5, string s, int stretch)
		{
			for (var i = 0; i <= stretch; i++)
				s = BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(s))).Replace("-", "").ToLower();
			return s;
		}
		public class MaxDictionary<T, T2> : Dictionary<T, T2>
		{
			private readonly object _syncObject = new object();

			public int Size { get; }

			public MaxDictionary(int size)
			{
				Size = size;
			}

			public new void Add(T index, T2 obj)
			{
				base.Add(index, obj);
				lock (_syncObject)
				{
					while (Count > Size)
						Remove(Keys.Min());
				}
			}
		}
	}
}