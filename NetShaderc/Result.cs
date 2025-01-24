using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetShaderc
{
	public class Result
	{

		internal Result(nint handle)
		{
			Length = (int)Shaderc.shaderc_result_get_length(handle);
			nint ptr = Shaderc.shaderc_result_get_bytes(handle);
			Code = new byte[Length];
			Marshal.Copy(ptr, Code, 0, Length);
			Warnings = (uint)Shaderc.shaderc_result_get_num_warnings(handle);
			Errors = (uint)Shaderc.shaderc_result_get_num_errors(handle);
			Status = Shaderc.shaderc_result_get_compilation_status(handle);
			ErrorMessage = Shaderc.shaderc_result_get_error_message(handle);
			Shaderc.shaderc_result_release(handle);
		}

		public int Length { get; private set; }
		public byte[] Code { get; private set; }
		public uint Warnings { get; private set; }
		public uint Errors { get; private set; }
		public CompilationStatus Status { get; private set; }
		public string ErrorMessage { get; private set; }

	}
}
