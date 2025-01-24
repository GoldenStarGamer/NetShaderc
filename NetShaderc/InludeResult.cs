using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetShaderc
{
	public class IncludeResult
	{
		// Source text inclusion via #include is supported with a pair of callbacks
		// to an "includer" on the client side.  The first callback processes an
		// inclusion request, and returns an include result.  The includer owns
		// the contents of the result, and those contents must remain valid until the
		// second callback is invoked to release the result.  Both callbacks take a
		// user_data argument to specify the client context.
		// To return an error, set the source_name to an empty string and put your
		// error message in content.

		
		[StructLayout(LayoutKind.Sequential)]
		struct RawIncludeResult
		{
			/// <summary>
			/// Pointer to the name of the source file (null-terminated string)
			/// </summary>
			public string SourceName;

			/// <summary>
			/// Length of the source name
			/// </summary>
			public Size SourceNameLength;

			/// <summary>
			/// Pointer to the content of the source file (null-terminated string)
			/// </summary>
			public string Content;

			/// <summary>
			/// Length of the content
			/// </summary>
			public Size ContentLength;

			/// <summary>
			/// Pointer to user data
			/// </summary>
			public IntPtr UserData;
		}

	}
}
