using System.IO;
using System.Reflection;

namespace AdventOfCode2016.Lib
{
	internal static class ResourceManager
	{
		public static string GetString(string resourceName)
		{
			using (var stream = Assembly.GetManifestResourceStream("AdventOfCode2016.Resources." + resourceName + ".txt"))
			{
				if (stream == null)
					return null;
				using (var reader = new StreamReader(stream))
					return reader.ReadToEnd();
			}
		}

		private static Assembly _assembly;
		private static Assembly Assembly => _assembly ?? (_assembly = typeof(ResourceManager).GetTypeInfo().Assembly);
	}
}