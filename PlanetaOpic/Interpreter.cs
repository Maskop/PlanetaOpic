using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetaOpic {
    internal class Interpreter {
        public string programPath { get; private set; }
        public uint cells { get; private set; }
        public char[] memory { get; private set; }
        /// <summary>
        /// Pointer to the current memory cell.
        /// </summary>
        private uint memoryPointer { get; set; }
        /// <summary>
        /// Pointer to the current instruction.
        /// </summary>
        private uint instructionPointer { get; set; }
        private char[,] commands { get; set; }

        public Interpreter(string path, uint cells) {
            this.programPath = path;
            this.cells = cells;
            this.memory = new char[cells];
        }

        public void Run() {
            memoryPointer = 0;
            commands = LoadProgram(programPath);
            instructionPointer = 0;
            ExecuteProgram();
        }

        private char[,] LoadProgram(string path) {
            char[] program = System.IO.File.ReadAllText(path).ToCharArray();
            List<char> relevantChars = new List<char>();
            foreach(char c in program) {
                if (c == '.'||c == '?'||c == '!') {
                    relevantChars.Add(c);
                }
            }
            char[,] commands = new char[relevantChars.Count/2, 2];
            for (int i = 0; i < commands.GetLength(0); i++) {
                commands[i, 0] = relevantChars[0];
                relevantChars.RemoveAt(0);
                commands[i, 1] = relevantChars[0];
                relevantChars.RemoveAt(0);
            }
            return commands;
        }
        
        private void ExecuteProgram() {
            uint loopCounter = 0;
            while (instructionPointer < commands.GetLength(0)) {
                switch (commands[instructionPointer, 0], commands[instructionPointer, 1]) {
                    case ('.', '?'):
                        memoryPointer++;
                        if (memoryPointer >= cells) {
                            //Console.WriteLine("Memory overflow");
                            throw new IndexOutOfRangeException("Memory overflow");
                        }
                        break;
                    case ('?', '.'):
                        memoryPointer--;
                        if (memoryPointer >= cells) {
                            //Console.WriteLine("Memory underflow");
                            throw new IndexOutOfRangeException("Memory underflow");
                        }
                        break;
                    case ('.', '.'):
                        memory[memoryPointer]++;
                        break;
                    case ('!', '!'):
                        memory[memoryPointer]--;
                        break;
                    case ('!', '.'):
                        Console.Write(memory[memoryPointer]);
                        break;
                    case ('.', '!'):
                        memory[memoryPointer] = (char)Console.Read();
                        break;
                    case ('!', '?'):
                        if (memory[memoryPointer] == 0) {
                            loopCounter = 1;
                            while (loopCounter > 0) {
                                instructionPointer++;
                                if (instructionPointer >= commands.GetLength(0)) {
                                    //Console.WriteLine("Unclosed cycle");
                                    throw new IndexOutOfRangeException("Unclosed cycle");
                                } else if (instructionPointer < 0) {
                                    //Console.WriteLine("Unopened cycle");
                                    throw new IndexOutOfRangeException("Unopened cycle");
                                }
                                if (commands[instructionPointer, 0] == '!' && commands[instructionPointer, 1] == '?') {
                                    loopCounter++;
                                } else if (commands[instructionPointer, 0] == '?' && commands[instructionPointer, 1] == '!') {
                                    loopCounter--;
                                }
                            }
                        }
                        break;
                    case ('?', '!'):
                        if (memory[memoryPointer] != 0) {
                            loopCounter = 1;
                            while (loopCounter > 0) {
                                instructionPointer--;
                                if (instructionPointer < 0) {
                                    //Console.WriteLine("Unopened cycle");
                                    throw new IndexOutOfRangeException("Unopened cycle");
                                } else if (instructionPointer >= commands.GetLength(0)) {
                                    //Console.WriteLine("Unclosed cycle");
                                    throw new IndexOutOfRangeException("Unclosed cycle");
                                }
                                if (commands[instructionPointer, 0] == '?' && commands[instructionPointer, 1] == '!') {
                                    loopCounter++;
                                } else if (commands[instructionPointer, 0] == '!' && commands[instructionPointer, 1] == '?') {
                                    loopCounter--;
                                }
                            }
                        }
                        break;
                }
                instructionPointer++;
            }
        }
    }
}
