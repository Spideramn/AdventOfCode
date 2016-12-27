using System;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.Serialization;

namespace AdventOfCode2016.Days
{
	public class Day10 : Day
	{
		public Day10(string input)
			: base(10, input)
		{
		}

		public override object RunPart1()
		{
			var bots = new Dictionary<int, Bot>();
			var outputs = new Dictionary<int, Output>();

			foreach (var instruction in Input.Split('\n').Select(l => l.Trim()).OrderBy(l=>l))
			{
				var parts = instruction.Split(' ');
				if (parts[0] == "value")
				{
					var value = int.Parse(parts[1]);
					var botId = int.Parse(parts[5]);
					if (!bots.ContainsKey(botId)) bots.Add(botId, new Bot(botId));
					try
					{
						bots[botId].Set(value, true);
					}
					catch (BotFoundException e)
					{
						return e.Output.Id;
					}
				}
				else if(parts[0] == "bot")
				{
					var botId = int.Parse(parts[1]);
					if (!bots.ContainsKey(botId)) bots.Add(botId, new Bot(botId));
					
					var lowId = int.Parse(parts[6]);
					if (parts[5] == "bot")
					{
						if(!bots.ContainsKey(lowId)) bots.Add(lowId, new Bot(lowId));
						bots[botId].Low = bots[lowId];
					}
					else if (parts[5] == "output")
					{
						if (!outputs.ContainsKey(lowId)) outputs.Add(lowId, new Output(lowId));
						bots[botId].Low = outputs[lowId];
					}

					var highId = int.Parse(parts[11]);
					if (parts[10] == "bot")
					{
						if (!bots.ContainsKey(highId)) bots.Add(highId, new Bot(highId));
						bots[botId].High = bots[highId];
					}
					else if (parts[10] == "output")
					{
						if (!outputs.ContainsKey(highId)) outputs.Add(highId, new Output(highId));
						bots[botId].High = outputs[highId];
					}
				}
			}

			return null;
		}

		private class Output
		{
			public int Id { get; }
			public int? Value { get; private set; }

			public Output(int id)
			{
				Id = id;
			}
			
			public virtual void Set(int value, bool isPart1)
			{
				Value = value;
			}

			public override string ToString()
			{
				return $"{Id} => {Value}";
			}
		}
		
		private class Bot : Output
		{
			public Bot(int id) : base(id)
			{
			}

			public override void Set(int value, bool isPart1)
			{
				if (Value.HasValue)
				{
					if (isPart1 && ((value == 17 && Value == 61) || (Value == 17 && value == 61)))
						throw new BotFoundException(this);

					Low?.Set(Math.Min(Value.Value, value), isPart1);
					High?.Set(Math.Max(Value.Value, value), isPart1);
				}
				base.Set(value, isPart1);
			}

			public Output Low { private get; set; }
			public Output High { private get; set; }
		}

		private class BotFoundException : Exception
		{
			public readonly Output Output;

			public BotFoundException(Output output)
			{
				Output = output;
			}
		}

		public override object RunPart2()
		{
			var bots = new Dictionary<int, Bot>();
			var outputs = new Dictionary<int, Output>();

			foreach (var instruction in Input.Split('\n').Select(l => l.Trim()).OrderBy(l=>l))
			{
				var parts = instruction.Split(' ');
				if (parts[0] == "value")
				{
					var value = int.Parse(parts[1]);
					var botId = int.Parse(parts[5]);
					if (!bots.ContainsKey(botId)) bots.Add(botId, new Bot(botId));
					bots[botId].Set(value, false);
				}
				else if (parts[0] == "bot")
				{
					var botId = int.Parse(parts[1]);
					if (!bots.ContainsKey(botId)) bots.Add(botId, new Bot(botId));

					var lowId = int.Parse(parts[6]);
					if (parts[5] == "bot")
					{
						if (!bots.ContainsKey(lowId)) bots.Add(lowId, new Bot(lowId));
						bots[botId].Low = bots[lowId];
					}
					else if (parts[5] == "output")
					{
						if (!outputs.ContainsKey(lowId)) outputs.Add(lowId, new Output(lowId));
						bots[botId].Low = outputs[lowId];
					}

					var highId = int.Parse(parts[11]);
					if (parts[10] == "bot")
					{
						if (!bots.ContainsKey(highId)) bots.Add(highId, new Bot(highId));
						bots[botId].High = bots[highId];
					}
					else if (parts[10] == "output")
					{
						if (!outputs.ContainsKey(highId)) outputs.Add(highId, new Output(highId));
						bots[botId].High = outputs[highId];
					}
				}
			}
			return outputs[0].Value*outputs[1].Value*outputs[2].Value;
		}
	}
}