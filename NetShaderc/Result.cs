using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetShaderc
{
	public class Result
	{
		readonly nint Handle;
		internal Result(nint handle)
		{
			Handle = handle;
		}

		public int Length { get { return (int)Shaderc.shaderc_result_get_length(Handle); } }
		public byte[]? Code 
		{
			get 
			{
				nint ptr = Shaderc.shaderc_result_get_bytes(Handle);
				if(ptr == 0) return null;
				var ass = new byte[Length];
				Marshal.Copy(ptr, ass, 0, Length);
				return ass;
			}
		}
		public uint Warnings
		{
			get
			{
				return (uint)Shaderc.shaderc_result_get_num_warnings(Handle);
			}
		}
		public uint Errors
		{
			get
			{
				return (uint)Shaderc.shaderc_result_get_num_errors(Handle);
			}
		}
		public CompilationStatus Status
		{
			get
			{
				return Shaderc.shaderc_result_get_compilation_status(Handle);
			}
		}
		public string ErrorMessage
		{
			get
			{
				return Shaderc.shaderc_result_get_error_message(Handle);
			}
		}

		~Result()
		{
			Shaderc.shaderc_result_release(Handle);
		}

	}
}
