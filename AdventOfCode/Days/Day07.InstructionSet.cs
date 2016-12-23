using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Days
{
	public partial class Day07
	{
		private class InstructionSet : Dictionary<string, Instruction>
		{
			public InstructionSet(string input)
			{
				foreach (var parts in input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Split(' ')))
				{
					switch (parts.Length)
					{
							// ASSIGN
						case 3:
							this[parts[2]] = new Instruction(Operation.Assign, parts[0]);
							break;
							// NOT!
						case 4:
							this[parts[3]] = new Instruction(Operation.Not, parts[1]);
							break;
							// AND / OR / LSHIFT / RSHIFT
						case 5:
							switch (parts[1])
							{
								case "AND":
									this[parts[4]] = new Instruction(Operation.And, parts[0], parts[2]);
									break;
								case "OR":
									this[parts[4]] = new Instruction(Operation.Or, parts[0], parts[2]);
									break;
								case "LSHIFT":
									this[parts[4]] = new Instruction(Operation.Lshift, parts[0], parts[2]);
									break;
								case "RSHIFT":
									this[parts[4]] = new Instruction(Operation.Rshift, parts[0], parts[2]);
									break;
							}
							break;
					}
				}
			}

			public void Reset()
			{
				foreach (var set in this)
					set.Value.Output = null;
			}

			public ushort GetOutput(string wire)
			{
				var instruction = this[wire];
				if (instruction.Output.HasValue)
					return instruction.Output.Value;

				ushort input1;
				ushort input2;
				switch (instruction.Operation)
				{
					case Operation.Assign:
						if (!ushort.TryParse(instruction.Input1, out input1))
							input1 = GetOutput(instruction.Input1);
						instruction.Output = input1;
						break;

					case Operation.Not:
						if (!ushort.TryParse(instruction.Input1, out input1))
							input1 = GetOutput(instruction.Input1);
						instruction.Output = (ushort) (~input1);
						break;

					case Operation.And:
						if (!ushort.TryParse(instruction.Input1, out input1))
							input1 = GetOutput(instruction.Input1);
						if (!ushort.TryParse(instruction.Input2, out input2))
							input2 = GetOutput(instruction.Input2);
						instruction.Output = (ushort) (input1 & input2);
						break;

					case Operation.Or:
						if (!ushort.TryParse(instruction.Input1, out input1))
							input1 = GetOutput(instruction.Input1);
						if (!ushort.TryParse(instruction.Input2, out input2))
							input2 = GetOutput(instruction.Input2);
						instruction.Output = (ushort) (input1 | input2);
						break;

					case Operation.Lshift:
						if (!ushort.TryParse(instruction.Input1, out input1))
							input1 = GetOutput(instruction.Input1);
						if (!ushort.TryParse(instruction.Input2, out input2))
							input2 = GetOutput(instruction.Input2);
						instruction.Output = (ushort) (input1 << input2);
						break;

					case Operation.Rshift:
						if (!ushort.TryParse(instruction.Input1, out input1))
							input1 = GetOutput(instruction.Input1);
						if (!ushort.TryParse(instruction.Input2, out input2))
							input2 = GetOutput(instruction.Input2);
						instruction.Output = (ushort) (input1 >> input2);
						break;

					default:
						throw new Exception("Unknown instruction");
				}
				return instruction.Output.Value;
			}
		}
	}
}
