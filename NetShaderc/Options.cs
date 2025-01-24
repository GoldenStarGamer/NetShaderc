using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NetShaderc
{
	public class Options
	{
		internal class OptHndl : IDisposable
		{
			public OptHndl(Options opt)
			{
				
			}

			public static implicit operator nint(OptHndl obj)
			{
				return nint.Zero; // placeholder, if you use this you will fuck shit up and I will fuck your ass
			}

			void IDisposable.Dispose()
			{
				
			}
		}
		


		public Options() { Handle = Shaderc.shaderc_compile_options_initialize(); }
		public Options(Options opt) { Handle = Shaderc.shaderc_compile_options_clone(opt.Handle); }

	}
}
