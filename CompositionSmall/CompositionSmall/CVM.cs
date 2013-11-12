
using System;

namespace CompositionSmall
{
	public struct Stack {
		private int[] vals;
		private int index, size;
		public Stack(int ssize) {
			size = ssize;
			vals = new int[size];
			index = -1;
		}
		public void Push(int val) { vals[++index] = val; }
		public int Pop() { return vals[index--]; }
	}
	
	public class CVM {
		Stack returnStack, dataStack;
		int [] memory;
		int memsize, PC, TOS, SOS, A;
		
		public CVM() {
			memsize = 64 * 1024;
			returnStack = new Stack(64);
			dataStack = new Stack(64);
			memory = new int[memsize];
		}
		
		public void External()
		{
			switch(dataStack.Pop())
			{
				case 1:
					dataStack.Push(Console.WindowHeight);
					break;
				case 2:
					dataStack.Push(Console.WindowWidth);
					break;
				case 3:
					Console.ForegroundColor = (ConsoleColor)dataStack.Pop();
					break;
				case 4:
					Console.BackgroundColor = (ConsoleColor)dataStack.Pop();
					break;
				case 5:
					dataStack.Push(Console.KeyAvailable ? -1 : 0);
					break;
				case 6:
					dataStack.Push((int)Console.ReadKey().KeyChar);
					break;
				case 7:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					Console.SetCursorPosition(SOS, TOS);
					break;
				case 8:
					Console.Write((char)dataStack.Pop());
					break;
				case 9:
					Console.Clear();
					break;
				case 10:
					Console.CursorVisible = dataStack.Pop() == -1 ? true : false;
					break;
				default:
					break;
			}
		}
		
		public enum OPCODES { NOP,RETURN,ZRETURN,GOTO,IF,DO,LIT,LT,EQ,GT,AND,
			OR,XOR,NOT,ADD,SUB,MULT,DIVMOD,SHL,SHR,LOAD,STORE,PUSH,POP,DROP,
			DUP,SWAP,OVER,ROT,EXT,HLT};
		
		public int DO_OP(OPCODES op) {
			switch(op) {
				case OPCODES.NOP:
					PC++;
					break;
				case OPCODES.EXT:
					External();
					PC++;
					break;
				case OPCODES.LIT:
					PC++;
					dataStack.Push(memory[PC]);
					PC++;
					break;
				case OPCODES.LT:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(SOS < TOS ? -1 : 0);
					PC++;
					break;
				case OPCODES.EQ:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(TOS == SOS ? -1 : 0);
					PC++;
					break;
				case OPCODES.GT:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(SOS > TOS ? -1 : 0);
					PC++;
					break;
				case OPCODES.AND:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(TOS & SOS);
					PC++;
					break;
				case OPCODES.OR:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(TOS | SOS);
					PC++;
					break;
				case OPCODES.XOR:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(TOS ^ SOS);
					PC++;
					break;
				case OPCODES.NOT:
					TOS = dataStack.Pop();
					dataStack.Push(~TOS);
					PC++;
					break;
				case OPCODES.ADD:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(TOS + SOS);
					PC++;
					break;
				case OPCODES.SUB:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(SOS - TOS);
					PC++;
					break;
				case OPCODES.MULT:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(TOS * SOS);
					PC++;
					break;
				case OPCODES.DIVMOD:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(SOS / TOS);
					dataStack.Push(SOS % TOS);
					PC++;
					break;
				case OPCODES.SHL:
					TOS = dataStack.Pop();
					dataStack.Push(TOS << 1);
					PC++;
					break;
				case OPCODES.SHR:
					TOS = dataStack.Pop();
					dataStack.Push(TOS >> 1);
					PC++;
					break;
				case OPCODES.LOAD:
					TOS = dataStack.Pop();
					dataStack.Push(memory[TOS]);
					PC++;
					break;
				case OPCODES.STORE:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					memory[TOS] = SOS;
					PC++;
					break;
				case OPCODES.PUSH:
					returnStack.Push(dataStack.Pop());
					PC++;
					break;
				case OPCODES.POP:
					dataStack.Push(returnStack.Pop());
					PC++;
					break;
				case OPCODES.DROP:
					dataStack.Pop();
					PC++;
					break;
				case OPCODES.DUP:
					TOS = dataStack.Pop();
					dataStack.Push(TOS);
					dataStack.Push(TOS);
					PC++;
					break;
				case OPCODES.SWAP:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(TOS);
					dataStack.Push(SOS);
					PC++;
					break;
				case OPCODES.OVER:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(SOS);
					dataStack.Push(TOS);
					dataStack.Push(SOS);
					PC++;
					break;
				case OPCODES.ROT:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					A = dataStack.Pop();
					dataStack.Push(SOS);
					dataStack.Push(TOS);
					dataStack.Push(A);
					PC++;
					break;
				case OPCODES.HLT:
					PC = -1;
					break;
				case OPCODES.IF:
					TOS = dataStack.Pop();
					SOS = dataStack.Pop();
					dataStack.Push(dataStack.Pop() == -1 ? SOS : TOS);
					PC++;
					break;
				case OPCODES.RETURN:
				rlabel:
					PC = returnStack.Pop();
					PC++;
					break;
				case OPCODES.ZRETURN:
					TOS = dataStack.Pop();
					if(TOS == 0)
						goto rlabel;
					else
						PC++;
					break;
				case OPCODES.GOTO:
					PC = dataStack.Pop();
					break;
				case OPCODES.DO:
					returnStack.Push(PC);
					PC = dataStack.Pop();
					break;
			default:
				break;
		    }
			return (int)op;
		}
		public void SetMemory(int element, int val) { memory[element] = val; }
		public void SetPC(int rPC) { PC = rPC; }
		public int GetPC() { return PC; }
		public int RUN() { return memory[PC]; }
	}
}
