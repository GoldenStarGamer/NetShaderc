using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetShaderc
{
	public class Compiler : IDisposable
	{
		nint handle;
		bool disp;
		public Compiler()
		{
			handle = Shaderc.shaderc_compiler_initialize();
		}

		public Result Compile(string source, CompileType type, ShaderKind kind, string file = "", string entry = "main", Options? opt = null)
		{
			if (disp) throw new ObjectDisposedException($"Compiler {nameof(Compile)} has been disposed");
			switch (type)
			{
				case CompileType.CodeToSPIRV:
					return new(Shaderc.shaderc_compile_into_spv(handle, source, (nuint)source.Length, kind, file, entry, (opt == null ? nint.Zero : opt.Handle)));
				case CompileType.CodeToAsm:
					return new(Shaderc.shaderc_compile_into_spv_assembly(handle, source, (nuint)source.Length, kind, file, entry, (opt == null ? nint.Zero : opt.Handle)));
				case CompileType.CodeToPreprocessed:
					return new(Shaderc.shaderc_compile_into_preprocessed_text(handle, source, (nuint)source.Length, kind, file, entry, (opt == null ? nint.Zero : opt.Handle)));
				case CompileType.AsmToSPIRV:
					return new(Shaderc.shaderc_assemble_into_spv(handle, source, (nuint)source.Length, nint.Zero));
				default:
					throw new NotSupportedException();
			}
			
		}

		public void Dispose()
		{
			if (!disp)
			{
				disp = true;
				Shaderc.shaderc_compiler_release(handle);
				handle = nint.Zero;
			}
		}

		~Compiler()
		{
			// making sure
			Dispose();
		}
	}
}
