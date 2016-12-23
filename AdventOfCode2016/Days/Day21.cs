using System;
using System.Linq;

namespace AdventOfCode2016.Days
{
	public class Day21 : Day
	{
		public Day21(string input)
			: base(21, input)
		{
		}

		public override object RunPart1()
		{
			var password = "abcdefgh".ToCharArray();
			foreach (var line in Input.Split('\n').Select(l => l.Trim().Split(' ')))
			{
				switch (line[0])
				{
					case "swap":
					{
						switch (line[1])
						{
							case "position":
								var x = int.Parse(line[2]);
								var y = int.Parse(line[5]);
								var c = password[y];
								password[y] = password[x];
								password[x] = c;
								break;
							case "letter":
								var a = line[2][0];
								var b = line[5][0];
								for (var i = 0; i < password.Length; i++)
								{
									if (password[i] == a)
										password[i] = b;
									else if (password[i] == b)
										password[i] = a;
								}
								break;
						}
					}
						break;
					case "rotate":
					{
						switch (line[1])
						{
							case "right":
								for (var i = 0; i < int.Parse(line[2]); i++)
								{
										var last = password[password.Length - 1];
										Array.Copy(password, 0, password, 1, password.Length - 1);
										password[0] = last;
										
								}
								break;
							case "left":
								for (var i = 0; i < int.Parse(line[2]); i++)
								{
										var first = password[0];
										Array.Copy(password, 1, password, 0, password.Length - 1);
										password[password.Length - 1] = first;
									}
								break;
							case "based":
								var pos = Array.IndexOf(password, line[6][0]);
								var times = 1 + pos;
								if (pos >= 4) times++;
								for (var i = 0; i < times; i++)
								{
										var last = password[password.Length - 1];
										Array.Copy(password, 0, password, 1, password.Length - 1);
										password[0] = last;
									}
								break;
						}
					}
						break;
					case "reverse":
					{
						var x = int.Parse(line[2]);
						var y = int.Parse(line[4]);
						var np = new char[password.Length];
						for (var i = 0; i < password.Length; i++)
						{
							if (i < x || i > y)
								np[i] = password[i];
							else
								np[i] = password[y - (i - x)];
						}
						password = np;
					}
						break;
					case "move":
					{
						// Ugly...
						var x = int.Parse(line[2]);
						var y = int.Parse(line[5]);
						var np = new char[password.Length];
						for (var i = 0; i < password.Length; i++)
						{
							if (i == y)
							{
								np[i] = password[x];
								if (x > y)
									i++;
								else
									continue;
							}

							if (x > y)
							{
								if (i > y && i <= x)
									np[i] = password[i - 1];
								else
									np[i] = password[i];
							}
							else
							{
								if (i >= x && i < y)
									np[i] = password[i + 1];
								else
									np[i] = password[i];
							}
						}
						password = np;
					}
						break;
				}
				
			}
			return new string(password);
		}

		public override object RunPart2()
		{
			var password = "fbgdceah".ToCharArray();

			foreach (var line in Input.Split('\n').Select(l => l.Trim().Split(' ')).Reverse())
			{
				switch (line[0])
				{
					case "swap":
						{
							switch (line[1])
							{
								case "position":
									var x = int.Parse(line[2]);
									var y = int.Parse(line[5]);
									var c = password[y];
									password[y] = password[x];
									password[x] = c;
									break;
								case "letter":
									var a = line[2][0];
									var b = line[5][0];
									for (var i = 0; i < password.Length; i++)
									{
										if (password[i] == a)
											password[i] = b;
										else if (password[i] == b)
											password[i] = a;
									}
									break;
							}
						}
						break;
					case "rotate":
						{
							switch (line[1])
							{
								case "right":
									for (var i = 0; i < int.Parse(line[2]); i++)
									{
										var first = password[0];
										Array.Copy(password, 1, password, 0, password.Length - 1);
										password[password.Length - 1] = first;
									}
									break;
								case "left":
									for (var i = 0; i < int.Parse(line[2]); i++)
									{
										var last = password[password.Length - 1];
										Array.Copy(password, 0, password, 1, password.Length - 1);
										password[0] = last;
									}
									break;
								case "based":
/*
	pos	  newpos
	  0      1
      1      3
      2      5
      3      7
      4      2
      5      4
      6      6
      7      0
*/
									var a = new [] { 7, 0, 4, 1, 5, 2, 6, 3 };
									var pos = a[Array.IndexOf(password, line[6][0])];
									var times = 1 + pos;
									if (pos >= 4) times++;
									for (var i = 0; i < times; i++)
									{
										var first = password[0];
										Array.Copy(password, 1, password, 0, password.Length - 1);
										password[password.Length - 1] = first;
									}
									break;
							}
						}
						break;
					case "reverse":
						{
							var x = int.Parse(line[2]);
							var y = int.Parse(line[4]);
							var np = new char[password.Length];
							for (var i = 0; i < password.Length; i++)
							{
								if (i < x || i > y)
									np[i] = password[i];
								else
									np[i] = password[y - (i - x)];
							}
							password = np;
						}
						break;
					case "move":
						{
							// Ugly...
							var x = int.Parse(line[5]);
							var y = int.Parse(line[2]);
							var np = new char[password.Length];
							for (var i = 0; i < password.Length; i++)
							{
								if (i == y)
								{
									np[i] = password[x];
									if (x > y)
										i++;
									else
										continue;
								}

								if (x > y)
								{
									if (i > y && i <= x)
										np[i] = password[i - 1];
									else
										np[i] = password[i];
								}
								else
								{
									if (i >= x && i < y)
										np[i] = password[i + 1];
									else
										np[i] = password[i];
								}
							}
							password = np;
						}
						break;
				}
			}
			return new string(password);
		}
	}
}