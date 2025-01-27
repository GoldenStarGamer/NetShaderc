using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

// Preety Pointers
using RawCompiler = nint;
using PtrInt = nint; // int*
using PtrUInt = nint; // unsigned int*
using RawCompilationResult = nint;
using RawCompileOptions = nint;
using RawUserData = nint;
using Size = nuint;

namespace NetShaderc
{
	// Usage examples:
	//
	// Aggressively release RawCompiler resources, but spend time in initialization
	// for each new use.
	//      shaderc_RawCompiler_t RawCompiler = shaderc_RawCompiler_initialize();
	//      shaderc_compilation_result_t result = shaderc_compile_into_spv(
	//          RawCompiler, "#version 450\nvoid main() {}", 27,
	//          shaderc_glsl_vertex_shader, "main.vert", "main", nullptr);
	//      // Do stuff with compilation results.
	//      shaderc_result_release(result);
	//      shaderc_RawCompiler_release(RawCompiler);
	//
	// Keep the RawCompiler object around for a long time, but pay for extra space
	// occupied.
	//      shaderc_RawCompiler_t RawCompiler = shaderc_RawCompiler_initialize();
	//      // On the same, other or multiple simultaneous threads.
	//      shaderc_compilation_result_t result = shaderc_compile_into_spv(
	//          RawCompiler, "#version 450\nvoid main() {}", 27,
	//          shaderc_glsl_vertex_shader, "main.vert", "main", nullptr);
	//      // Do stuff with compilation results.
	//      shaderc_result_release(result);
	//      // Once no more compilations are to happen.
	//      shaderc_RawCompiler_release(RawCompiler);

	/// <summary>
	/// Shaderc .NET Bindings
	/// Internal Raw Functions
	/// </summary>
	/// 
	internal static partial class Shaderc
	{

		// An opaque handle to an object that manages all RawCompiler state.


		// Returns a RawCompiler that can be used to compile modules.
		// A return of NULL indicates that there was an error initializing the RawCompiler.
		// Any function operating on RawCompiler must offer the basic
		// thread-safety guarantee.
		// [http://herbsutter.com/2014/01/13/gotw-95-solution-thread-safety-and-synchronization/]
		// That is: concurrent invocation of these functions on DIFFERENT objects needs
		// no synchronization; concurrent invocation of these functions on the SAME
		// object requires synchronization IF AND ONLY IF some of them take a non-const
		// argument.

		[LibraryImport("shaderc_shared")]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial RawCompiler shaderc_compiler_initialize();

		// Releases the resources held by the RawCompiler.
		// After this call it is invalid to make any future calls to functions
		// involving this RawCompiler.
		[LibraryImport("shaderc_shared")]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compiler_release(RawCompiler RawCompiler);



		// Returns a default-initialized RawCompileOptions that can be used
		// to modify the functionality of a compiled module.
		// A return of NULL indicates that there was an error initializing the options.
		// Any function operating on RawCompileOptions must offer the
		// basic thread-safety guarantee.

		[LibraryImport("shaderc_shared")]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial RawCompileOptions shaderc_compile_options_initialize();

		// Returns a copy of the given RawCompileOptions.
		// If NULL is passed as the parameter the call is the same as
		// shaderc_compile_options_init.
		[LibraryImport("shaderc_shared")]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial RawCompileOptions shaderc_compile_options_clone(RawCompileOptions options);

		// Releases the compilation options. It is invalid to use the given
		// RawCompileOptions object in any future calls. It is safe to pass
		// NULL to this function, and doing such will have no effect.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_release(RawCompileOptions options);

		// Adds a predefined macro to the compilation options. This has the same
		// effect as passing -Dname=value to the command-line RawCompiler.  If value
		// is NULL, it has the same effect as passing -Dname to the command-line
		// RawCompiler. If a macro definition with the same name has previously been
		// added, the value is replaced with the new value. The macro name and
		// value are passed in with char pointers, which point to their data, and
		// the lengths of their data. The strings that the name and value pointers
		// point to must remain valid for the duration of the call, but can be
		// modified or deleted after this function has returned. In case of adding
		// a valueless macro, the value argument should be a null pointer or the
		// value_length should be 0u.

		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_add_macro_definition(RawCompileOptions options, string name, Size name_length, string value, Size value_length);

		// Sets the source language.  The default is GLSL.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_source_language(RawCompileOptions options, SourceLanguage lang);

		// Sets the RawCompiler mode to generate debug information in the output.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_generate_debug_info(RawCompileOptions options);

		// Sets the RawCompiler optimization level to the given level. Only the last one
		// takes effect if multiple calls of this function exist.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_optimization_level(RawCompileOptions options, OptimizationLevel level);

		// Forces the GLSL language version and profile to a given pair. The version
		// number is the same as would appear in the #version annotation in the source.
		// Version and profile specified here overrides the #version annotation in the
		// source. Use profile: 'shaderc_profile_none' for GLSL versions that do not
		// define profiles, e.g. versions below 150.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_forced_version_profile(RawCompileOptions options, int version, Profile profile);

		/// <summary>
		/// Sets includer callback functions.
		/// </summary>
		/// <param name="options"></param>
		/// <param name="resolver"></param>
		/// <param name="result_releaser"></param>
		/// <param name="user_data"></param>
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_include_callbacks(RawCompileOptions options, Options.IncludeResolve.IncludeResolveFn resolver, Options.IncludeResolve.IncludeResultReleaseFn result_releaser, RawUserData user_data);

		/// <summary>
		/// Sets the RawCompiler mode to suppress warnings, overriding warnings-as-errors
		/// mode. When both suppress-warnings and warnings-as-errors modes are
		/// turned on, warning messages will be inhibited, and will not be emitted
		/// as error messages.
		/// </summary>
		/// <param name="options"></param>
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_suppress_warnings(RawCompileOptions options);

		// Sets the target shader environment, affecting which warnings or errors will
		// be issued.  The version will be for distinguishing between different versions
		// of the target environment.  The version value should be either 0 or
		// a value listed in shaderc_env_version.  The 0 value maps to Vulkan 1.0 if
		// |target| is Vulkan, and it maps to OpenGL 4.5 if |target| is OpenGL.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_target_env(RawCompileOptions options, TargetEnv target, uint version);

		// Sets the target SPIR-V version. The generated module will use this version
		// of SPIR-V.  Each target environment determines what versions of SPIR-V
		// it can consume.  Defaults to the highest version of SPIR-V 1.0 which is
		// required to be supported by the target environment.  E.g. Default to SPIR-V
		// 1.0 for Vulkan 1.0 and SPIR-V 1.3 for Vulkan 1.1.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_target_spirv(RawCompileOptions options, SpirvVersion version);

		// Sets the RawCompiler mode to treat all warnings as errors. Note the
		// suppress-warnings mode overrides this option, i.e. if both
		// warning-as-errors and suppress-warnings modes are set, warnings will not
		// be emitted as error messages.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_warnings_as_errors(RawCompileOptions options);

		// Sets a resource limit.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_limit(RawCompileOptions options, Limit limit, int value);

		// Sets whether the RawCompiler should automatically assign bindings to uniforms
		// that aren't already explicitly bound in the shader source.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_auto_bind_uniforms(RawCompileOptions options, [MarshalAs(UnmanagedType.Bool)] bool auto_bind);

		// Sets whether the RawCompiler should automatically remove sampler variables
		// and convert image variables to combined image-sampler variables.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_auto_combined_image_sampler(RawCompileOptions options, [MarshalAs(UnmanagedType.Bool)] bool upgrade);

		// Sets whether the RawCompiler should use HLSL IO mapping rules for bindings.
		// Defaults to false.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_hlsl_io_mapping(RawCompileOptions options, [MarshalAs(UnmanagedType.Bool)] bool hlsl_iomap);

		// Sets whether the RawCompiler should determine block member offsets using HLSL
		// packing rules instead of standard GLSL rules.  Defaults to false.  Only
		// affects GLSL compilation.  HLSL rules are always used when compiling HLSL.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_hlsl_offsets(RawCompileOptions options, [MarshalAs(UnmanagedType.Bool)] bool hlsl_offsets);

		// Sets the base binding number used for for a uniform resource type when
		// automatically assigning bindings.  For GLSL compilation, sets the lowest
		// automatically assigned number.  For HLSL compilation, the regsiter number
		// assigned to the resource is added to this specified base.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_binding_base(RawCompileOptions options, UniformKind kind, uint baseh); // it was base but base is a keyword in C#, also added an h specifically bc g sucks

		// Like shaderc_compile_options_set_binding_base, but only takes effect when
		// compiling a given shader stage.  The stage is assumed to be one of vertex,
		// fragment, tessellation evaluation, tesselation control, geometry, or compute.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_binding_base_for_stage(RawCompileOptions options, ShaderKind shader_kind, UniformKind kind, uint baseh);

		// Sets whether the RawCompiler should preserve all bindings, even when those
		// bindings are not used.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_preserve_bindings(RawCompileOptions options, [MarshalAs(UnmanagedType.Bool)] bool preserve_bindings);

		// Sets whether the RawCompiler should automatically assign locations to
		// uniform variables that don't have explicit locations in the shader source.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_auto_map_locations(RawCompileOptions options, [MarshalAs(UnmanagedType.Bool)] bool auto_map);

		// Sets a descriptor set and binding for an HLSL register in the given stage.
		// This method keeps a copy of the string data.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage(RawCompileOptions options, ShaderKind shader_kind, string reg, string set, string binding);

		// Like shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage,
		// but affects all shader stages.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_hlsl_register_set_and_binding(RawCompileOptions options, string reg, string set, string binding);

		// Sets whether the RawCompiler should enable extension
		// SPV_GOOGLE_hlsl_functionality1.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_hlsl_functionality1(RawCompileOptions options, [MarshalAs(UnmanagedType.Bool)] bool enable);

		// Sets whether 16-bit types are supported in HLSL or not.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_hlsl_16bit_types(RawCompileOptions options, [MarshalAs(UnmanagedType.Bool)] bool enable);

		// Enables or disables relaxed Vulkan rules.
		//
		// This allows most OpenGL shaders to compile under Vulkan semantics.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_vulkan_rules_relaxed(RawCompileOptions options, [MarshalAs(UnmanagedType.Bool)] bool enable);

		// Sets whether the RawCompiler should invert position.Y output in vertex shader.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_invert_y(RawCompileOptions options, [MarshalAs(UnmanagedType.Bool)] bool enable);

		// Sets whether the RawCompiler generates code for max and min builtins which,
		// if given a NaN operand, will return the other operand. Similarly, the clamp
		// builtin will favour the non-NaN operands, as if clamp were implemented
		// as a composition of max and min.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_compile_options_set_nan_clamp(RawCompileOptions options, [MarshalAs(UnmanagedType.Bool)] bool enable);

		// Takes a GLSL source string and the associated shader kind, input file
		// name, compiles it according to the given additional_options. If the shader
		// kind is not set to a specified kind, but shaderc_glslc_infer_from_source,
		// the RawCompiler will try to deduce the shader kind from the source
		// string and a failure in deducing will generate an error. Currently only
		// #pragma annotation is supported. If the shader kind is set to one of the
		// default shader kinds, the RawCompiler will fall back to the default shader
		// kind in case it failed to deduce the shader kind from source string.
		// The input_file_name is a null-termintated string. It is used as a tag to
		// identify the source string in cases like emitting error messages. It
		// doesn't have to be a 'file name'.
		// The source string will be compiled into SPIR-V binary and a
		// shaderc_compilation_result will be returned to hold the results.
		// The entry_point_name null-terminated string defines the name of the entry
		// point to associate with this GLSL source. If the additional_options
		// parameter is not null, then the compilation is modified by any options
		// present.  May be safely called from multiple threads without explicit
		// synchronization. If there was failure in allocating the RawCompiler object,
		// null will be returned.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
		[UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
		internal static partial RawCompilationResult shaderc_compile_into_spv(RawCompiler RawCompiler, string source_text, Size source_text_size, ShaderKind shader_kind, string input_file_name, string entry_point_name, RawCompileOptions additional_options);

		// Like shaderc_compile_into_spv, but the result contains SPIR-V assembly text
		// instead of a SPIR-V binary module.  The SPIR-V assembly syntax is as defined
		// by the SPIRV-Tools open source project.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
		[UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
		internal static partial RawCompilationResult shaderc_compile_into_spv_assembly(RawCompiler RawCompiler, string source_text, Size source_text_size, ShaderKind shader_kind, string input_file_name, string entry_point_name, RawCompileOptions additional_options);

		// Like shaderc_compile_into_spv, but the result contains preprocessed source
		// code instead of a SPIR-V binary module
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
		[UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
		internal static partial RawCompilationResult shaderc_compile_into_preprocessed_text(RawCompiler RawCompiler, string source_text, Size source_text_size, ShaderKind shader_kind, string input_file_name, string entry_point_name, RawCompileOptions additional_options);

		// Takes an assembly string of the format defined in the SPIRV-Tools project
		// (https://github.com/KhronosGroup/SPIRV-Tools/blob/master/syntax.md),
		// assembles it into SPIR-V binary and a shaderc_compilation_result will be
		// returned to hold the results.
		// The assembling will pick options suitable for assembling specified in the
		// additional_options parameter.
		// May be safely called from multiple threads without explicit synchronization.
		// If there was failure in allocating the RawCompiler object, null will be
		// returned.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
		[UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
		internal static partial RawCompilationResult shaderc_assemble_into_spv(RawCompiler RawCompiler, string source_assembly, Size source_assembly_size, RawCompileOptions additional_options);

		// The following functions, operating on RawCompilationResult object,
		// offer only the basic thread-safety guarantee.

		// Releases the resources held by the result object. It is invalid to use the
		// result object for any further operations.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_result_release(RawCompilationResult result);

		// Returns the number of bytes of the compilation output data in a result
		// object.
		[LibraryImport("shaderc_shared")]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial Size shaderc_result_get_length(RawCompilationResult result);

		// Returns the number of warnings generated during the compilation.
		[LibraryImport("shaderc_shared")]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial Size shaderc_result_get_num_warnings(RawCompilationResult result);

		// Returns the number of errors generated during the compilation.
		[LibraryImport("shaderc_shared")]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial Size shaderc_result_get_num_errors(RawCompilationResult result);

		// Returns the compilation status, indicating whether the compilation succeeded,
		// or failed due to some reasons, like invalid shader stage or compilation
		// errors.
		[LibraryImport("shaderc_shared")]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial CompilationStatus shaderc_result_get_compilation_status(RawCompilationResult result);

		// Returns a pointer to the start of the compilation output data bytes, either
		// SPIR-V binary or char string. When the source string is compiled into SPIR-V
		// binary, this is guaranteed to be castable to a uint32_t*. If the result
		// contains assembly text or preprocessed source text, the pointer will point to
		// the resulting array of characters.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial nint shaderc_result_get_bytes(RawCompilationResult result);

		// Returns a null-terminated string that contains any error messages generated
		// during the compilation.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial string shaderc_result_get_error_message(RawCompilationResult result);

		// Provides the version & revision of the SPIR-V which will be produced
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Utf8)]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		internal static partial void shaderc_get_spv_version(PtrUInt version, PtrUInt revision);

		// Parses the version and profile from a given null-terminated string
		// containing both version and profile, like: '450core'. Returns false if
		// the string can not be parsed. Returns true when the parsing succeeds. The
		// parsed version and profile are returned through arguments.
		[LibraryImport("shaderc_shared", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
		[UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static partial bool shaderc_parse_version_profile(string str, PtrInt version, nint profile);

		

	}
}
