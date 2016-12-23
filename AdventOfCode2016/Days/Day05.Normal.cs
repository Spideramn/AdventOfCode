using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode2016.Days
{
	public partial class Day05
	{
		private class Day05Normal
		{
			private readonly string _input;
			public Day05Normal(string input)
			{
				_input = input;
			}

			public object RunPart1()
			{
				var i = 0;
				var password = "";
				using (var md5 = MD5.Create())
				{
					while (password.Length < 8)
					{
						var b = md5.ComputeHash(Encoding.Default.GetBytes(_input + i++));
						if (b[0] == 0x00 && b[1] == 0x00 && b[2] <= 0x0F)
							password += b[2].ToString("X2")[1];
					}
				}
				return password;
			}

			public object RunPart2()
			{
				int i = 0, c = 0;
				var password = new char[8];
				using (var md5 = MD5.Create())
				{
					while (c < 8)
					{
						var b = md5.ComputeHash(Encoding.Default.GetBytes(_input + i++));
						if (b[0] != 0x00 || b[1] != 0x00 || b[2] > 0x07 || password[b[2]] != '\0')
							continue;
						password[b[2]] = b[3].ToString("X2")[0];
						c++;
					}
				}
				return string.Concat(password);
			}
		}
	}
}
