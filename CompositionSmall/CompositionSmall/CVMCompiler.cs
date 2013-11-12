
using System;
using System.Collections.Generic;

namespace CompositionSmall
{
	public class CVMCompiler {
		Dictionary<string, CVM.OPCODES> ops;
		Dictionary<string, int> labels;
		public int[] image;
		
		public CVMCompiler() {
			image = new int[1024];
			labels = new Dictionary<string, int>();
			ops = new Dictionary<string, CVM.OPCODES>();
			ops.Add("LIT",CVM.OPCODES.LIT);
			ops.Add("HLT",CVM.OPCODES.HLT);
			ops.Add("NOP",CVM.OPCODES.NOP);
			ops.Add("LT",CVM.OPCODES.LT);
			ops.Add("EQ",CVM.OPCODES.EQ);
			ops.Add("GT",CVM.OPCODES.GT);
			ops.Add("AND",CVM.OPCODES.AND);
			ops.Add("OR",CVM.OPCODES.OR);
			ops.Add("XOR",CVM.OPCODES.XOR);
			ops.Add("NOT",CVM.OPCODES.NOT);
			ops.Add("ADD",CVM.OPCODES.ADD);
			ops.Add("SUB",CVM.OPCODES.SUB);
			ops.Add("MULT",CVM.OPCODES.MULT);
			ops.Add("DIVMOD",CVM.OPCODES.DIVMOD);
			ops.Add("SHL",CVM.OPCODES.SHL);
			ops.Add("SHR",CVM.OPCODES.SHR);
			ops.Add("LOAD",CVM.OPCODES.LOAD);
			ops.Add("STORE",CVM.OPCODES.STORE);
			ops.Add("PUSH",CVM.OPCODES.PUSH);
			ops.Add("POP",CVM.OPCODES.POP);
			ops.Add("DROP",CVM.OPCODES.DROP);
			ops.Add("DUP",CVM.OPCODES.DUP);
			ops.Add("SWAP",CVM.OPCODES.SWAP);
			ops.Add("OVER",CVM.OPCODES.OVER);
			ops.Add("ROT",CVM.OPCODES.ROT);
			ops.Add("GETC",CVM.OPCODES.GETC);
			ops.Add("PUTC",CVM.OPCODES.PUTC);
			ops.Add("DO",CVM.OPCODES.DO);
			ops.Add("RETURN",CVM.OPCODES.RETURN);
			ops.Add("ZRETURN",CVM.OPCODES.ZRETURN);
			ops.Add("GOTO",CVM.OPCODES.GOTO);
			ops.Add("IF",CVM.OPCODES.IF);
		}
		
		public int ParseCode(int loc, string code)
		{
			string[] tokens = 
				code.Replace("\n"," ").Replace("\r"," ").Split(
					new char[]{' '});
			foreach(var a in tokens) {
				var b = a;
				if(b == "")
					continue;
				if(b[0] == '(') {
					b = a.Replace("(","").Replace(")","");
					loc = CompileOP(loc,CVM.OPCODES.LIT);
				}
				if(ops.ContainsKey(b.ToUpper())) {
				   	loc = CompileOP(loc,ops[b.ToUpper()]);
				   }
				else if(labels.ContainsKey(b.ToUpper())) {
					loc = CompileLIT(loc, labels[b.ToUpper()]);
				   }
				else if(b[0] == ':') {
					labels.Add(b.Replace(":",""),loc);
				}
				else {
				   	short tmpVal;
					if(short.TryParse(b,out tmpVal)) {
						loc = CompileLIT(loc,tmpVal);
					}
			    }
		    }
			return loc;
		}
		
		public int CompileLIT(int loc, int num) {
			image[loc++] = num;
			return loc;
		}
		
		public int CompileOP(int loc, CVM.OPCODES op) {
			image[loc++] = (int)op;
			return loc;
		}
	}
}
