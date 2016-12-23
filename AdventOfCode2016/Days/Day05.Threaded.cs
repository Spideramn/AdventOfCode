using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2016.Days
{
	public partial class Day05
	{
		private class Day05Threaded
		{
			private readonly string _input;
			private static readonly object _mutex = new object();

			public Day05Threaded(string input)
			{
				_input = input;
			}
			
			public object RunPart1()
			{
				// reset params
				var latest = 0;
				var batchSize = 100000;
				var result = new ConcurrentDictionary<int, char>();

				// create threads
				var tf = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.LongRunning);
				var tasks = Enumerable.Range(0, Environment.ProcessorCount).Select(t => tf.StartNew(() =>
					{
						using (var md5 = MD5.Create())
						{
							while (result.Count < 8)
							{
								int start;
								lock (_mutex)
								{
									start = latest;
									latest += batchSize;
								}
								for (var i = start; i < start + batchSize; i++)
								{
									var b = md5.ComputeHash(Encoding.Default.GetBytes(_input + i));
									if (b[0] == 0x00 && b[1] == 0x00 && b[2] <= 0x0F)
										result.TryAdd(i, b[2].ToString("X2")[1]);
								}
							}
						}
					}
				)).ToArray();
				Task.WaitAll(tasks);
				return string.Concat(result.OrderBy(pair => pair.Key).Select(pair => pair.Value).Take(8));
			}
			
			public object RunPart2()
			{
				// reset params
				var latest = 0;
				var batchSize = 100000;

				// ugly...
				var result = new ConcurrentDictionary<int, ConcurrentDictionary<int, char>>(new List<KeyValuePair<int, ConcurrentDictionary<int, char>>>
				{
					new KeyValuePair<int, ConcurrentDictionary<int, char>>(0, new ConcurrentDictionary<int, char>()),
					new KeyValuePair<int, ConcurrentDictionary<int, char>>(1, new ConcurrentDictionary<int, char>()),
					new KeyValuePair<int, ConcurrentDictionary<int, char>>(2, new ConcurrentDictionary<int, char>()),
					new KeyValuePair<int, ConcurrentDictionary<int, char>>(3, new ConcurrentDictionary<int, char>()),
					new KeyValuePair<int, ConcurrentDictionary<int, char>>(4, new ConcurrentDictionary<int, char>()),
					new KeyValuePair<int, ConcurrentDictionary<int, char>>(5, new ConcurrentDictionary<int, char>()),
					new KeyValuePair<int, ConcurrentDictionary<int, char>>(6, new ConcurrentDictionary<int, char>()),
					new KeyValuePair<int, ConcurrentDictionary<int, char>>(7, new ConcurrentDictionary<int, char>())
				});
				
				// create threads
				var tf = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.LongRunning);
				var tasks = Enumerable.Range(0, Environment.ProcessorCount).Select(t => tf.StartNew(() =>
				{
					using (var md5 = MD5.Create())
					{
						while (!result.All(c => c.Value.Any()))
						{
							int start;
							lock (_mutex)
							{
								start = latest;
								latest += batchSize;
							}
							for (var i = start; i < start + batchSize; i++)
							{
								var b = md5.ComputeHash(Encoding.Default.GetBytes(_input + i));
								if (b[0] != 0 || b[1] != 0 || b[2] > 0x07)
									continue;
								result[b[2]].TryAdd(i, b[3].ToString("X2")[0]);
							}
						} 
					}
				}
				)).ToArray();
				Task.WaitAll(tasks);
				return string.Concat(result.Values.Select(c => c.OrderBy(pair => pair.Key).Select(pair => pair.Value).First()));
			}
		}
	}
}
