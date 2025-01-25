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
			var option = (opt == null ? null : opt.Handle);

			Result result;

			switch (type)
			{
				case CompileType.CodeToSPIRV:
					
					result = new(Shaderc.shaderc_compile_into_spv(handle, source, (nuint)source.Length, kind, file, entry, (option == null? nint.Zero : option)));
					break;

				case CompileType.CodeToAsm:
					result = new(Shaderc.shaderc_compile_into_spv_assembly(handle, source, (nuint)source.Length, kind, file, entry, (option == null ? nint.Zero : option)));
					break;

				case CompileType.CodeToPreprocessed:
					result = new(Shaderc.shaderc_compile_into_preprocessed_text(handle, source, (nuint)source.Length, kind, file, entry, (option == null ? nint.Zero : option)));
					break;

				case CompileType.AsmToSPIRV:
					result = new(Shaderc.shaderc_assemble_into_spv(handle, source, (nuint)source.Length, (option == null ? nint.Zero : option)));
					break;

				default:
					throw new NotSupportedException();
			}

			if (option != null) option.Dispose();
			return result;
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
