using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventOfCode.Days
{
	public class Day12 : Day
	{
		public Day12(string input)
			: base(12, input)
		{
		}

		public override object RunPart1()
		{
			var count = 0L;
			using (var stringReader = new StringReader(Input))
			using(var jsonReader = new JsonTextReader(stringReader))
			{
				while (jsonReader.Read())
				{
					if (jsonReader.TokenType == JsonToken.Integer)
						count += (long)jsonReader.Value;
				}
			}
			return count;
		}

		public override object RunPart2()
		{
			var jToken = JToken.Parse(Input);
			return ParseJToken(jToken);
		}


		private int ParseJToken(JToken jToken)
		{
			var count = 0;
			switch (jToken.Type)
			{
				case JTokenType.Object:
					if (jToken.Children<JProperty>().All(t => t.Value.Type != JTokenType.String || (string)t.Value != "red"))
						count += jToken.Sum(token => ParseJToken(token));
					break;

				case JTokenType.Property:
				case JTokenType.Array:
					count += jToken.Sum(token => ParseJToken(token));
					break;

				case JTokenType.Integer:
					count += jToken.Value<int>();
					break;
			}
			return count;
		}
	}
}
