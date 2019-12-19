using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
	internal class Intcode
	{
		private long _pointer;
		private long _relativeBase;
		private bool _isRunning;
		private readonly Dictionary<long, long> _memory;
		private readonly Queue<long> _inputs = new Queue<long>();

		public Intcode(IEnumerable<long> code, params long[] initialInput)
		{
			_memory = code.Select((v, k) => new {k, v}).ToDictionary(arg => (long) arg.k, arg => (long) arg.v);
			AddInput(initialInput);
		}

		public Intcode AddInput(params long[] input)
		{
			foreach (var i in input)
				_inputs.Enqueue(i);
			return this;
		}
		
		public IEnumerable<long> Run()
		{
			if (_isRunning)
				throw new Exception("Allready running!");
			_isRunning = true;

			while (true)
			{
				var instruction = GetValue(_pointer++);
				var opCode = instruction % 100;
				switch (opCode)
				{
					// add -> p3 = p1 + p2
					case 1:
						{
							var value1 = GetParamValueAndIncreasePointer(1, instruction);
							var value2 = GetParamValueAndIncreasePointer(2, instruction);
							SetParamValueAndIncreasePointer(3, instruction, value1 + value2);
							break;
						}

					// multiply -> p3 = p1 * p2
					case 2:
						{
							var value1 = GetParamValueAndIncreasePointer(1, instruction);
							var value2 = GetParamValueAndIncreasePointer(2, instruction);
							SetParamValueAndIncreasePointer(3, instruction, value1 * value2);
							break;
						}

					// set -> p1 = input
					case 3:
						{
							SetParamValueAndIncreasePointer(1, instruction, _inputs.Dequeue());
							break;
						}

					// output -> output p1
					case 4:
						{
							var value1 = GetParamValueAndIncreasePointer(1, instruction);
							yield return value1;
							break;
						}

					// jump-if-true ->  if p1 != 0 then pointer = p2
					case 5:
						{
							var value1 = GetParamValueAndIncreasePointer(1, instruction);
							var value2 = GetParamValueAndIncreasePointer(2, instruction);
							if (value1 != 0) _pointer = value2;
							break;
						}

					// jump-if-false ->  if p1 == 0 then pointer = p2
					case 6:
						{
							var value1 = GetParamValueAndIncreasePointer(1, instruction);
							var value2 = GetParamValueAndIncreasePointer(2, instruction);
							if (value1 == 0) _pointer = value2;
							break;
						}

					// less than -> if p1 < p2 then p3 = 1 else p3 = 0
					case 7:
						{
							var value1 = GetParamValueAndIncreasePointer(1, instruction);
							var value2 = GetParamValueAndIncreasePointer(2, instruction);
							SetParamValueAndIncreasePointer(3, instruction, value1 < value2 ? 1 : 0);
							break;
						}

					// equals -> if p1 == p2 then p3 = 1 else p3 = 0
					case 8:
						{
							var value1 = GetParamValueAndIncreasePointer(1, instruction);
							var value2 = GetParamValueAndIncreasePointer(2, instruction);
							SetParamValueAndIncreasePointer(3, instruction, value1 == value2 ? 1 : 0);
							break;
						}

					// adjust relative base -> relativeBase += p1
					case 9:
						{
							var value1 = GetParamValueAndIncreasePointer(1, instruction);
							_relativeBase += value1;
							break;
						}

					//halt
					case 99:
						_isRunning = false;
						yield break;

					default:
						_isRunning = false;
						throw new Exception($"Unknown opcode: {opCode}");
				}
			}
		}

		public long GetValue(long position)
		{
			return _memory.TryGetValue(position, out var value) ? value : 0;
		}
		public Intcode SetValue(long position, long value)
		{
			_memory[position] = value;
			return this;
		}

		private long GetParamValueAndIncreasePointer(int paramIndex, long instruction)
		{
			var param = GetValue(_pointer++);
			var mode = (instruction / (int)Math.Pow(10, 1 + paramIndex)) % 10;
			switch (mode)
			{
				case 0: // position mode
					return GetValue(param);

				case 1: // immediate mode
					return param;

				case 2: // relative mode
					return GetValue(_relativeBase + param);

				default:
					throw new Exception($"Unknown param mode: {mode}");
			}
		}
		private void SetParamValueAndIncreasePointer(int paramIndex, long instruction, long value)
		{
			var param = GetValue(_pointer++);
			var mode = (instruction / (int)Math.Pow(10, 1 + paramIndex)) % 10;
			switch (mode)
			{
				case 0: // position mode
					_memory[param] = value;
					return;

				case 1: // immediate mode
					throw new Exception("Writing is not allowed in immediate mode");

				case 2: // relative mode
					_memory[_relativeBase + param] = value;
					return;

				default:
					throw new Exception($"Unknown param mode: {mode}");
			}
		}
	}
}