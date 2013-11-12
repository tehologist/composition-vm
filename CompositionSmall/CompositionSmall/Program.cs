
using System;

namespace CompositionSmall
{
	class Program {
		public static void Main(string[] args) {
			int config = -1;
			if(args.Length == 0)
				return;
			if(args[0].Contains(".cmp"))
				config = 0;
			if(args[0].Contains(".img"))
				config = 1;
			if(config == 0) {
				CVMCompiler compiler = new CVMCompiler();
				string codeMain = System.IO.File.OpenText(args[0]).ReadToEnd();
				compiler.image[4] = compiler.ParseCode(5,codeMain);
				string codeBoot = "(MAIN) DO HLT";
				compiler.ParseCode(0,codeBoot);
				var binWriter = new System.IO.BinaryWriter(
					System.IO.File.OpenWrite(args[0].Replace(".cmp",".img")));
				for(var ndx = 0;ndx < compiler.image[4];ndx++)
					binWriter.Write(compiler.image[ndx]);
			}
			if(config == 1) {
				CVM vm = new CVM();
				var binReader = new System.IO.BinaryReader(
					System.IO.File.OpenRead(args[0]));
				int ndx=0;
				while(true) {
					try {
						vm.SetMemory(ndx++,binReader.ReadInt32());
					}
					catch { break; }
				}
			    vm.SetPC(0);
				while(vm.DO_OP((CVM.OPCODES)vm.RUN()) != (int)CVM.OPCODES.HLT);
			}
			Console.ReadKey(true);
		}
	}
}