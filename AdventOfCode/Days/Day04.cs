using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Days
{
	public class Day04 : Day
	{
		private readonly MD5 _md5;
		public Day04(string input)
			:base(4, input)
		{
			_md5 = MD5.Create();
		}

		
		public override object RunPart1()
		{
			var secret = Input;
			var i = 1;
			while (true)
			{
				var hash = _md5.ComputeHash(Encoding.ASCII.GetBytes(secret + i));
				if (hash[0] == 0 && hash[1] == 0 && hash[2] < 16)
					return i;
				i++;
			}
		}
		public override object RunPart2()
		{
			var secret = Input;
			var i = 1;
			while (true)
			{
				var hash = _md5.ComputeHash(Encoding.ASCII.GetBytes(secret + i));
				if (hash[0] == 0 && hash[1] == 0 && hash[2] == 0)
					return i;
				i++;
			}
		}
	}
}
