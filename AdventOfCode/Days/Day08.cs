using System.Text.RegularExpressions;

namespace AdventOfCode.Days
{
	public class Day08 : Day
	{
		public Day08(string input)
			:base(8, input)
		{
		}

		public override object RunPart1()
		{
			var codeCharCount = 0;
			var memCharCount = 0;
			var regex = new Regex(@"\\x([0-9A-F]{2})", RegexOptions.IgnoreCase);
			foreach(var l in Resources.InputDay8.Split('\n'))
			{
				var line = l.Trim();
				codeCharCount += line.Length;
				
				// replace escape chars to single char
				var memLine = regex.Replace(line, "%")
					.Replace(@"\\", "%")
					.Replace(@"\""", "%");

				var memLength = memLine.Length - 2; // minus de 2 omringende " chars
				memCharCount += memLength;
			}

			return codeCharCount - memCharCount;
		}

		public override object RunPart2()
		{
			var originalCharCount = 0;
			var encodedCharCount = 0;

			foreach (var l in Resources.InputDay8.Split('\n'))
			{
				var line = l.Trim();
				originalCharCount += line.Length;

				var encodedLine = "\"";
				foreach(var c in line)
				{
					if (c == '\\')
						encodedLine += "\\\\";
					else if (c == '"')
						encodedLine += "\\\"";
					else
						encodedLine += c;
				}
				encodedLine += "\"";
				encodedCharCount += encodedLine.Length;
			}

			return encodedCharCount - originalCharCount;
		}
	}
}
