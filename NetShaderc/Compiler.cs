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
			ObjectDisposedException.ThrowIf(disp, this);
			var option = (opt?.Handle);
			Result result = type switch
			{
				CompileType.CodeToSPIRV => new(Shaderc.shaderc_compile_into_spv(handle, source, (nuint)source.Length, kind, file, entry, (option ?? nint.Zero))),
				CompileType.CodeToAsm => new(Shaderc.shaderc_compile_into_spv_assembly(handle, source, (nuint)source.Length, kind, file, entry, (option ?? nint.Zero))),
				CompileType.CodeToPreprocessed => new(Shaderc.shaderc_compile_into_preprocessed_text(handle, source, (nuint)source.Length, kind, file, entry, (option ?? nint.Zero))),
				CompileType.AsmToSPIRV => new(Shaderc.shaderc_assemble_into_spv(handle, source, (nuint)source.Length, (option ?? nint.Zero))),
				_ => throw new NotSupportedException(),
			};
			option?.Dispose();
			return result;
		}

		public void Dispose()
		{
			if (!disp)
			{
				Shaderc.shaderc_compiler_release(handle);
				handle = nint.Zero;
				GC.SuppressFinalize(this);
				disp = true;
			}
		}

		~Compiler()
		{
			// making sure
			Dispose();
		}
	}
}
