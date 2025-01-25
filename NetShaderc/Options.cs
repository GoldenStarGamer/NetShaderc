using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

			public OptHndl(Options opt) // for some stupid reason when you set the value of a certain optional option you can't change it back
			{                           // so we create new ones every time, don't blame me, blame google

				handle = Shaderc.shaderc_compile_options_initialize();

				if (opt.Macros != null)
				{
					foreach (var macro in opt.Macros)
					{
						Shaderc.shaderc_compile_options_add_macro_definition(this, macro.Key, (uint)macro.Key.Length, macro.Value, (uint)macro.Value.Length);
					}
				}

				Shaderc.shaderc_compile_options_set_source_language(this, opt.Language);

				if (opt.DebugInfo) Shaderc.shaderc_compile_options_set_generate_debug_info(this);

				if (opt.Optimization != null) Shaderc.shaderc_compile_options_set_optimization_level(handle, (OptimizationLevel)opt.Optimization);

				if (opt.ForcedVersionProfile != null) Shaderc.shaderc_compile_options_set_forced_version_profile(this, ((KeyValuePair<int, Profile>)opt.ForcedVersionProfile).Key, ((KeyValuePair<int, Profile>)opt.ForcedVersionProfile).Value);

				if (opt.Resolve != null) Shaderc.shaderc_compile_options_set_include_callbacks(this, opt.Resolve.Value.Resolve, opt.Resolve.Value.Release, opt.Resolve.Value.UserData);

				if(opt.SuppressWarnings) Shaderc.shaderc_compile_options_set_suppress_warnings(this);
			}

			public static implicit operator nint(OptHndl obj)
			{
				return obj.handle;
			}

			public void Dispose()
			{
				if (!disp)
				{
					Shaderc.shaderc_compile_options_release(this);
					disp = true;
				}
			}
			~OptHndl()
			{
				Dispose();
			}
		}

		public struct IncludeResolve
		{
			/// <summary>
			/// An includer callback type for mapping an #include request to an include
			/// result.  The user_data parameter specifies the client context.  The
			/// requested_source parameter specifies the name of the source being requested.
			/// The type parameter specifies the kind of inclusion request being made.
			/// The requesting_source parameter specifies the name of the source containing
			/// the #include request.  The includer owns the result object and its contents,
			/// and both must remain valid until the release callback is called on the result
			/// object.
			/// </summary>
			/// <param name="userData">Pointer to user data (context)</param>
			/// <param name="requestedSource">Pointer to requested source string</param>
			/// <param name="type">Integer specifying the inclusion type</param>
			/// <param name="requestingSource">Pointer to requesting source string</param>
			/// <param name="includeDepth">Depth of the include</param>
			/// <returns></returns>
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate nint IncludeResolveFn(nint userData, string requestedSource, int type, string requestingSource, nuint includeDepth);

			/// <summary>
			/// An includer callback type for destroying an include result.
			/// </summary>
			/// <param name="userData">Pointer to user data</param>
			/// <param name="includeResult">Pointer to ShadercIncludeResult</param>
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void IncludeResultReleaseFn(nint userData, nint includeResult);

			public IncludeResolveFn Resolve;
			public IncludeResultReleaseFn Release;
			public nint UserData;
		}

		public Dictionary<string, string>? Macros;

		public SourceLanguage Language = SourceLanguage.GLSL; // Shaderc specifies that GLSL is the default so me do that

		public bool DebugInfo = false; // funny enough you can't set this off in the original version, stupid design

		public OptimizationLevel? Optimization; // not sure what the default is so nullable it is

		public KeyValuePair<int, Profile>? ForcedVersionProfile; // KeyValuePair because it takes 2 values

		public IncludeResolve? Resolve;

		public bool SuppressWarnings;

		internal OptHndl Handle { get { return new OptHndl(this); } }
	}
}
