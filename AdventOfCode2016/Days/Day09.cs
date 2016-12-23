namespace AdventOfCode2016.Days
{
	public class Day09 : Day
	{
		public Day09(string input)
			: base(9, input)
		{
		}

		public override object RunPart1()
		{
			var fileLength = 0L;
			for (var i = 0; i < Input.Length; i++)
			{
				// marker
				if (Input[i] == '(')
				{
					var marker = "";
					while (Input[++i] != ')')
						marker += Input[i];
					i++; // skip ')'

					var parts = marker.Split('x');
					var length = int.Parse(parts[0]);
					var times = int.Parse(parts[1]);
					fileLength += length * times;
					i += length - 1;
				}
				else
				{
					fileLength++;
				}
			}
			return fileLength;
		}

		public override object RunPart2()
		{
			return Decompress(Input);
		}

		private static long Decompress(string s)
		{
			long fileLength = 0;
			for (var i = 0; i < s.Length; i++)
			{
				if (s[i] == '(')
				{
					var marker = "";
					while (s[++i] != ')')
						marker += s[i];
					i++;

					var parts = marker.Split('x');
					var length = int.Parse(parts[0]);
					var times = int.Parse(parts[1]);
					fileLength += Decompress(s.Substring(i, length)) * times;
					i += length - 1;
				}
				else
				{
					fileLength++;
				}
			}
			return fileLength;
		}
	}
}