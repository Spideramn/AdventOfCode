using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AdventOfCode2020.Days
{
	internal class Day08 : Day
	{
		public override int DayNumber => 8;

		public override object RunPart1()
		{
			var instructionPointer = 0;
			var accumulator = 0;
			var instructions = GetInputLines().Select(l => new Instruction(l)).ToArray();

			while (true)
			{
				var instruction = instructions[instructionPointer];
				if (instruction.Counter == 1) break;

				switch (instruction.Operation)
				{
					case "nop":
						break;
					case "jmp":
						instructionPointer += instruction.Argument - 1;
						break;
					case "acc":
						accumulator += instruction.Argument;
						break;
				}

				instructionPointer++;
				instruction.Counter++;
			}

			return accumulator;
		}

		public override object RunPart2()
		{
			var instructions = GetInputLines()
				.Select(l => new Instruction(l)).ToArray();
			
			var programList = new Queue<Program>();
			programList.Enqueue(new Program(instructions, 0, 0));

			while (programList.TryDequeue(out var program))
			{
				var instruction = program.Instructions[program.InstructionPointer];
				if (instruction.Counter == 1) 
					continue; // second time here, do not enqueue again.
				
				switch (instruction.Operation)
				{
					case "nop":
					{
						if (!program.IsCloned)
						{
							var newProgram = program.Clone();
							newProgram.Instructions[newProgram.InstructionPointer].Operation = "jmp";
							programList.Enqueue(newProgram);
						}

						break;
					}

					case "jmp":
					{
						if (!program.IsCloned)
						{
							var newProgram = program.Clone();
							newProgram.Instructions[newProgram.InstructionPointer].Operation = "nop";
							programList.Enqueue(newProgram);
						}

						program.InstructionPointer += instruction.Argument - 1;
						break;
					}
					case "acc":
						program.Accumulator += instruction.Argument;
						break;
				}
				program.InstructionPointer++;
				if (program.InstructionPointer < 0 || program.InstructionPointer >= program.Instructions.Length)
					return program.Accumulator;

				instruction.Counter++;

				programList.Enqueue(program);
			}

			//return accumulator;
			return "NOT FOUND";
		}


		private class Instruction
		{
			public Instruction(string line)
			{
				var parts = line.Split(' ', 2);
				Operation = parts[0];
				Argument = int.Parse(parts[1]);
				Counter = 0;
			}

			public Instruction(Instruction line)
			{
				Operation = line.Operation;
				Argument = line.Argument;
				Counter = line.Counter;
			}

			public string Operation { get; set; }
			public int Argument { get; }
			public int Counter { get; set; }
		}

		private class Program
		{
			public bool IsCloned { get; private set; }
			public int InstructionPointer { get; set; }
		
			public Instruction[] Instructions { get; }
			
			public int Accumulator { get; set; }

			public Program(Instruction[] instructions, int pointer, int accumulator)
			{
				Instructions = instructions;
				InstructionPointer = pointer;
				Accumulator = accumulator;
			}


			public Program Clone()
			{
				return new Program(Instructions.Select(i => new Instruction(i)).ToArray(), InstructionPointer, Accumulator) {IsCloned = true};
			}
		}
	}
}