using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib
{
	public class IncrementalString
	{
		private readonly char _start;
		private readonly char _end;
		private readonly char[] _forbiddenChars;
		private readonly List<char> _charList;

		public IncrementalString(string value, char start = 'a', char end = 'z', char[] forbiddenChars = null)
		{
			_start = start;
			_end = end;
			_forbiddenChars = forbiddenChars ?? new char[0];
			if (_forbiddenChars.Contains(start))
				throw new ArgumentException("Forbidden chars contains starting char", "forbiddenChars");
			if (_forbiddenChars.Contains(end))
				throw new ArgumentException("Forbidden chars contains ending char", "forbiddenChars");

			_charList = new List<char>(value.Reverse());
		}

		private IncrementalString Increment()
		{
			for (var i = 0; i < _charList.Count; i++)
			{
				if (_charList[i] + 1 > _end)
				{
					_charList[i] = _start;
					continue;
				}
				_charList[i]++;
				while (_forbiddenChars.Contains(_charList[i]))
					_charList[i]++;
				return this;
			}
			_charList.Add(_start);
			return this;
		}

		public static IncrementalString operator ++(IncrementalString incrementalString)
		{
			return incrementalString.Increment();
		}
		public static IncrementalString operator +(IncrementalString incrementalString, int incrementBy)
		{
			for (var i = 0; i < incrementBy; i++)
				incrementalString++;
			return incrementalString;
		}
		public static implicit operator string(IncrementalString incrementalString)
		{
			return new string(incrementalString._charList.AsEnumerable().Reverse().ToArray());
		}
		public static implicit operator IncrementalString(string s)
		{
			return new IncrementalString(s);
		}
		public override string ToString()
		{
			return this;
		}
	}
}
