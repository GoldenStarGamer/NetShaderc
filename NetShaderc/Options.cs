using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NetShaderc
{
	public class Options
	{
		internal class OptHndl : IDisposable
		{
			nint handle;

			bool disp;

			public OptHndl(Options opt)
			{
				handle = Shaderc.shaderc_compile_options_initialize();
			}

			public static implicit operator nint(OptHndl obj)
			{
				return obj.handle; // placeholder,, if you use this you will fuck shit up and I will fuck your ass
			}

			public void Dispose()
			{
				if (!disp)
				{
					Shaderc.shaderc_compile_options_release(this);
				}
			}
			~OptHndl()
			{
				Dispose();
			}
		}



		public Options() { }

	}
}
