using System.Runtime.InteropServices;
using Compiler = nint;
using CompileOptions = nint;

namespace NetShaderc
{
	
	

	/// <summary>
	/// Shaderc .NET Bindings
	/// </summary>
	/// /// <example>
	///	Aggressively release compiler resources, but spend time in initialization
	/// for each new use.
	/// <code>
	/// Compiler compiler = CompilerInitialize();
	/// shaderc_compilation_result_t result = shaderc_compile_into_spv(
	///     compiler, "#version 450\nvoid main() {}", 27,
	///     GLSLvertex_shader, "main.vert", "main", nullptr);
	/// // Do stuff with compilation results.
	/// shaderc_result_release(result);
	/// shaderc_compiler_release(compiler);
	/// Compiler compiler = CompilerInitialize();
	/// // On the same, other or multiple simultaneous threads.
	/// shaderc_compilation_result_t result = shaderc_compile_into_spv(
	///     compiler, "#version 450\nvoid main() {}", 27,
	///     GLSLvertex_shader, "main.vert", "main", nullptr);
	/// // Do stuff with compilation results.
	/// shaderc_result_release(result);
	/// // Once no more compilations are to happen.
	/// shaderc_compiler_release(compiler);
	/// </code>
	/// 
	///  Keep the compiler object around for a long time, but pay for extra space
	/// occupied.
	/// <code>
	/// </code>
	/// </example>
	public class Shaderc
	{
		
		public enum TargetEnv : uint
		{
			/// <summary>
			/// SPIR-V under Vulkan semantics
			/// </summary>
			Vulkan,

			/// <summary>
			/// SPIR-V under OpenGL semantics
			/// </summary>
			OpenGL,

			/// <summary>
			/// SPIR-V under OpenGL semantics,
			/// including compatibility profile
			/// functions
			/// </summary>
			/// <note>
			/// SPIR-V code generation is not supported for shaders under OpenGL
			/// compatibility profile.
			/// </note>
			OpenGLCompat,

			/// <summary>
			/// SPIR-V under WebGPU semantics
			/// </summary>
			[Obsolete]
			WebGPU,

			Default = Vulkan
		}

		public enum EnvVersion : uint
		{
			// For Vulkan, use Vulkan's mapping of version numbers to integers.
			Vulkan1_0 = ((1u << 22)),
			Vulkan1_1 = ((1u << 22) | (1 << 12)),
			Vulkan1_2 = ((1u << 22) | (2 << 12)),
			Vulkan1_3 = ((1u << 22) | (3 << 12)),
			Vulkan1_4 = ((1u << 22) | (4 << 12)),

			// For OpenGL, use the number from #version in shaders.
			OpenGL4_5 = 450,

			[Obsolete("WebGPU env never defined versions")]
			WebGPU,
		}

		/// <summary>
		/// The known versions of SPIR-V.
		/// </summary>
		public enum SpirvVersion : uint
		{
			// Use the values used for word 1 of a SPIR-V binary:
			// - bits 24 to 31: zero
			// - bits 16 to 23: major version number
			// - bits 8 to 15: minor version number
			// - bits 0 to 7: zero
			SPIRV1_0 = 0x010000u,
			SPIRV1_1 = 0x010100u,
			SPIRV1_2 = 0x010200u,
			SPIRV1_3 = 0x010300u,
			SPIRV1_4 = 0x010400u,
			SPIRV1_5 = 0x010500u,
			SPIRV1_6 = 0x010600u
		}

		/// <summary>
		/// Indicate the status of a compilation.
		/// </summary>
		enum CompilationStatus : uint
		{
			Success = 0,
			/// <summary>
			/// error stage deduction
			/// </summary>
			InvalidStage = 1,
			CompilationError = 2,
			/// <summary>
			/// unexpected failure
			/// </summary>
			InternalError = 3,
			NullResultObject = 4,
			InvalidAssembly = 5,
			ValidationError = 6,
			TransformationError = 7,
			ConfigurationError = 8,
		}

		// Source language kind.
		enum SourceLanguage: uint
		{
			GLSL,
			HLSL,
		}

		enum ShaderKind : uint
		{
			// Forced shader kinds. These shader kinds force the compiler to compile the
			// source code as the specified kind of shader.
			VertexShader,
			FragmentShader,
			ComputeShader,
			GeometryShader,
			TessControlShader,
			TessEvaluationShader,

			GLSLVertexShader = VertexShader,
			GLSLFragmentShader = FragmentShader,
			GLSLComputeShader = ComputeShader,
			GLSLGeometryShader = GeometryShader,
			GLSLTessControlShader = TessControlShader,
			GLSLTessEvaluationShader = TessEvaluationShader,

			// Deduce the shader kind from #pragma annotation in the source code. Compiler
			// will emit error if #pragma annotation is not found.
			GLSLInferFromSource,
			// Default shader kinds. Compiler will fall back to compile the source code as
			// the specified kind of shader when #pragma annotation is not found in the
			// source code.
			GLSLDefaultVertexShader,
			GLSLDefaultFragmentShader,
			GLSLDefaultComputeShader,
			GLSLDefaultGeometryShader,
			GLSLDefaultTessControlShader,
			GLSLDefaultTessEvaluationShader,
			SPIRVAssembly,
			RayGenShader,
			AnyHitShader,
			ClosestHitShader,
			MissShader,
			IntersectionShader,
			CallableShader,
			GLSLRayGenShader = RayGenShader,
			GLSLAnyHitShader = AnyHitShader,
			GLSLClosestHitShader = ClosestHitShader,
			GLSLMissShader = MissShader,
			GLSLIntersectionShader = IntersectionShader,
			GLSLCallableShader = CallableShader,
			GLSLDefaultRayGenShader,
			GLSLDefaultAnyHitShader,
			GLSLDefaultClosestHitShader,
			GLSLDefaultMissShader,
			GLSLDefaultIntersectionShader,
			GLSLDefaultCallableShader,
			TaskShader,
			MeshShader,
			GLSLTaskShader = TaskShader,
			GLSLMeshShader = MeshShader,
			GLSLDefaultTaskShader,
			GLSLDefaultMeshShader,
		}

		enum Profile : uint
		{
			None,					// Used if and only if GLSL version did not specify
									// profiles.
			Core,
			[Obsolete("Disabled. This generates an error")]
			Compatibility,
			ES,
		}

		/// <summary>
		/// Optimization level.
		/// </summary>
		enum OptimizationLevel : uint
		{
			/// <summary>
			/// no optimization
			/// </summary>
			Zero,
			/// <summary>
			/// optimize towards reducing code size
			/// </summary>
			Size,
			/// <summary>
			/// optimize towards performance
			/// </summary>
			Performance,
		}

// Resource limits.
		enum Limit : uint
		{
			Lights,
			ClipPlanes,
			TextureUnits,
			TextureCoords,
			VertexAttribs,
			VertexUniformComponents,
			VaryingFloats,
			VertexTextureImageUnits,
			CombinedTextureImageUnits,
			TextureImageUnits,
			FragmentUniformComponents,
			DrawBuffers,
			VertexUniformVectors,
			VaryingVectors,
			FragmentUniformVectors,
			VertexOutputVectors,
			FragmentInputVectors,
			MinProgramTexelOffset,
			MaxProgramTexelOffset,
			ClipDistances,
			ComputeWorkGroupCountX,
			ComputeWorkGroupCountY,
			ComputeWorkGroupCountZ,
			ComputeWorkGroupSizeX,
			ComputeWorkGroupSizeY,
			ComputeWorkGroupSizeZ,
			ComputeUniformComponents,
			ComputeTextureImageUnits,
			ComputeImageUniforms,
			ComputeAtomicCounters,
			ComputeAtomicCounterBuffers,
			VaryingComponents,
			VertexOutputComponents,
			GeometryInputComponents,
			GeometryOutputComponents,
			FragmentInputComponents,
			ImageUnits,
			CombinedImageUnitsAndFragmentOutputs,
			CombinedShaderOutputResources,
			ImageSamples,
			VertexImageUniforms,
			TessControlImageUniforms,
			TessEvaluationImageUniforms,
			GeometryImageUniforms,
			FragmentImageUniforms,
			CombinedImageUniforms,
			GeometryTextureImageUnits,
			GeometryOutputVertices,
			GeometryTotalOutputComponents,
			GeometryUniformComponents,
			GeometryVaryingComponents,
			TessControlInputComponents,
			TessControlOutputComponents,
			TessControlTextureImageUnits,
			TessControlUniformComponents,
			TessControlTotalOutputComponents,
			TessEvaluationInputComponents,
			TessEvaluationOutputComponents,
			TessEvaluationTextureImageUnits,
			TessEvaluationUniformComponents,
			TessPatchComponents,
			PatchVertices,
			TessGenLevel,
			Viewports,
			VertexAtomicCounters,
			TessControlAtomicCounters,
			TessEvaluationAtomicCounters,
			GeometryAtomicCounters,
			FragmentAtomicCounters,
			CombinedAtomicCounters,
			AtomicCounterBindings,
			VertexAtomicCounterBuffers,
			TessControlAtomicCounterBuffers,
			TessEvaluationAtomicCounterBuffers,
			GeometryAtomicCounterBuffers,
			FragmentAtomicCounterBuffers,
			CombinedAtomicCounterBuffers,
			AtomicCounterBufferSize,
			TransformFeedbackBuffers,
			TransformFeedbackInterleavedComponents,
			CullDistances,
			CombinedClipAndCullDistances,
			Samples,
			MeshOutputVerticesNV,
			MeshOutputPrimitivesNV,
			MeshWorkGroupSizeXNV,
			MeshWorkGroupSizeYNV,
			MeshWorkGroupSizeZNV,
			TaskWorkGroupSizeXNV,
			TaskWorkGroupSizeYNV,
			TaskWorkGroupSizeZNV,
			MeshViewCountNV,
			MeshOutputVerticesExt,
			MeshOutputPrimitivesExt,
			MeshWorkGroupSizeXExt,
			MeshWorkGroupSizeYExt,
			MeshWorkGroupSizeZExt,
			TaskWorkGroupSizeXExt,
			TaskWorkGroupSizeYExt,
			TaskWorkGroupSizeZExt,
			MeshViewCountExt,
			DualSourceDrawBuffersExt,
		}

		/// <summary>
		/// Uniform resource kinds.
		///	In Vulkan, uniform resources are bound to the pipeline via descriptors
		/// with numbered bindings and sets.
		/// </summary>
		enum UniformKind : uint
		{
			/// <summary>
			/// Image and image buffer.
			/// </summary>
			Image,
			/// <summary>
			/// Pure sampler.
			/// </summary>
			Sampler,
			/// <summary>
			/// Sampled texture in GLSL, and Shader Resource View in HLSL.
			/// </summary>
			Texture,
			/// <summary>
			/// Uniform Buffer Object (UBO) in GLSL.  Cbuffer in HLSL.
			/// </summary>
			Buffer,
			/// <summary>
			/// Shader Storage Buffer Object (SSBO) in GLSL.
			/// </summary>
			StorageBuffer,
			/// <summary>
			/// Unordered Access View, in HLSL.  (Writable storage image or storage
			/// buffer.)
			/// </summary>
			UnorderedAccessView,
		}

		// An opaque handle to an object that manages all compiler state.


		// Returns a Compiler that can be used to compile modules.
		// A return of NULL indicates that there was an error initializing the compiler.
		// Any function operating on Compiler must offer the basic
		// thread-safety guarantee.
		// [http://herbsutter.com/2014/01/13/gotw-95-solution-thread-safety-and-synchronization/]
		// That is: concurrent invocation of these functions on DIFFERENT objects needs
		// no synchronization; concurrent invocation of these functions on the SAME
		// object requires synchronization IF AND ONLY IF some of them take a non-const
		// argument.

		[DllImport("shaderc_shared", CallingConvention = CallingConvention.Cdecl, EntryPoint = "shaderc_compiler_initialize")]
		static extern Compiler Initialize();

		/// <summary>
		/// Releases the resources held by the Compiler.
		/// </summary>
		/// <param name=""></param>
		// After this call it is invalid to make any future calls to functions
		// involving this Compiler.
		[DllImport("shaderc_shared", CallingConvention = CallingConvention.Cdecl, EntryPoint = "shaderc_compiler_release")]
		static extern void Release(Compiler compiler);

		// Returns a default-initialized CompileOptions that can be used
		// to modify the functionality of a compiled module.
		// A return of NULL indicates that there was an error initializing the options.
		// Any function operating on CompileOptions must offer the
		// basic thread-safety guarantee.

		[DllImport("shaderc_shared", CallingConvention = CallingConvention.Cdecl, EntryPoint = "shaderc_compile_options_initialize")]
		static extern CompileOptions shaderc_compile_options_initialize();

		// Returns a copy of the given CompileOptions.
		// If NULL is passed as the parameter the call is the same as
		// shaderc_compile_options_init.
		[DllImport("shaderc_shared", CallingConvention= CallingConvention.Cdecl, EntryPoint = "shaderc_compile_options_clone")]
		static extern CompileOptions shaderc_compile_options_clone(CompileOptions options);

		// Releases the compilation options. It is invalid to use the given
		// CompileOptions object in any future calls. It is safe to pass
		// NULL to this function, and doing such will have no effect.
		[DllImport("shaderc_shared", CallingConvention = CallingConvention.Cdecl, EntryPoint = "shaderc_compile_options_release")]
		static extern void shaderc_compile_options_release(CompileOptions options);

		// Adds a predefined macro to the compilation options. This has the same
		// effect as passing -Dname=value to the command-line compiler.  If value
		// is NULL, it has the same effect as passing -Dname to the command-line
		// compiler. If a macro definition with the same name has previously been
		// added, the value is replaced with the new value. The macro name and
		// value are passed in with char pointers, which point to their data, and
		// the lengths of their data. The strings that the name and value pointers
		// point to must remain valid for the duration of the call, but can be
		// modified or deleted after this function has returned. In case of adding
		// a valueless macro, the value argument should be a null pointer or the
		// value_length should be 0u.
		static extern void shaderc_compile_options_add_macro_definition(
			CompileOptions options, const char* name, size_t name_length,
	const char* value, size_t value_length);

// Sets the source language.  The default is GLSL.
static extern void shaderc_compile_options_set_source_language(
	CompileOptions options, SourceLanguage lang);

		// Sets the compiler mode to generate debug information in the output.
		static extern void shaderc_compile_options_set_generate_debug_info(
			CompileOptions options);

		// Sets the compiler optimization level to the given level. Only the last one
		// takes effect if multiple calls of this function exist.
		static extern void shaderc_compile_options_set_optimization_level(
			CompileOptions options, shaderc_optimization_level level);

		// Forces the GLSL language version and profile to a given pair. The version
		// number is the same as would appear in the #version annotation in the source.
		// Version and profile specified here overrides the #version annotation in the
		// source. Use profile: 'shaderc_profile_none' for GLSL versions that do not
		// define profiles, e.g. versions below 150.
		static extern void shaderc_compile_options_set_forced_version_profile(
			CompileOptions options, int version, shaderc_profile profile);

		// Source text inclusion via #include is supported with a pair of callbacks
		// to an "includer" on the client side.  The first callback processes an
		// inclusion request, and returns an include result.  The includer owns
		// the contents of the result, and those contents must remain valid until the
		// second callback is invoked to release the result.  Both callbacks take a
		// user_data argument to specify the client context.
		// To return an error, set the source_name to an empty string and put your
		// error message in content.

		// An include result.
		typedef struct shaderc_include_result
		{
			// The name of the source file.  The name should be fully resolved
			// in the sense that it should be a unique name in the context of the
			// includer.  For example, if the includer maps source names to files in
			// a filesystem, then this name should be the absolute path of the file.
			// For a failed inclusion, this string is empty.
			const char* source_name;
			size_t source_name_length;
			// The text contents of the source file in the normal case.
			// For a failed inclusion, this contains the error message.
			const char* content;
			size_t content_length;
			// User data to be passed along with this request.
			void* user_data;
		}
		shaderc_include_result;

// The kinds of include requests.
enum shaderc_include_type
		{
			shaderc_include_type_relative,  // E.g. #include "source"
			shaderc_include_type_standard   // E.g. #include <source>
		};

		// An includer callback type for mapping an #include request to an include
		// result.  The user_data parameter specifies the client context.  The
		// requested_source parameter specifies the name of the source being requested.
		// The type parameter specifies the kind of inclusion request being made.
		// The requesting_source parameter specifies the name of the source containing
		// the #include request.  The includer owns the result object and its contents,
		// and both must remain valid until the release callback is called on the result
		// object.
		typedef shaderc_include_result* (* shaderc_include_resolve_fn) (
			void* user_data, const char* requested_source, int type,

	const char* requesting_source, size_t include_depth);

// An includer callback type for destroying an include result.
typedef void (* shaderc_include_result_release_fn) (
	void* user_data, shaderc_include_result* include_result);

// Sets includer callback functions.
static extern void shaderc_compile_options_set_include_callbacks(
	CompileOptions options, shaderc_include_resolve_fn resolver,
	shaderc_include_result_release_fn result_releaser, void* user_data);

		// Sets the compiler mode to suppress warnings, overriding warnings-as-errors
		// mode. When both suppress-warnings and warnings-as-errors modes are
		// turned on, warning messages will be inhibited, and will not be emitted
		// as error messages.
		static extern void shaderc_compile_options_set_suppress_warnings(
			CompileOptions options);

		// Sets the target shader environment, affecting which warnings or errors will
		// be issued.  The version will be for distinguishing between different versions
		// of the target environment.  The version value should be either 0 or
		// a value listed in shaderc_env_version.  The 0 value maps to Vulkan 1.0 if
		// |target| is Vulkan, and it maps to OpenGL 4.5 if |target| is OpenGL.
		static extern void shaderc_compile_options_set_target_env(
			CompileOptions options,
			shaderc_target_env target,
			uint32_t version);

		// Sets the target SPIR-V version. The generated module will use this version
		// of SPIR-V.  Each target environment determines what versions of SPIR-V
		// it can consume.  Defaults to the highest version of SPIR-V 1.0 which is
		// required to be supported by the target environment.  E.g. Default to SPIR-V
		// 1.0 for Vulkan 1.0 and SPIR-V 1.3 for Vulkan 1.1.
		static extern void shaderc_compile_options_set_target_spirv(
			CompileOptions options, shaderc_spirv_version version);

		// Sets the compiler mode to treat all warnings as errors. Note the
		// suppress-warnings mode overrides this option, i.e. if both
		// warning-as-errors and suppress-warnings modes are set, warnings will not
		// be emitted as error messages.
		static extern void shaderc_compile_options_set_warnings_as_errors(
			CompileOptions options);

		// Sets a resource limit.
		static extern void shaderc_compile_options_set_limit(
			CompileOptions options, shaderc_limit limit, int value);

		// Sets whether the compiler should automatically assign bindings to uniforms
		// that aren't already explicitly bound in the shader source.
		static extern void shaderc_compile_options_set_auto_bind_uniforms(
			CompileOptions options, bool auto_bind);

		// Sets whether the compiler should automatically remove sampler variables
		// and convert image variables to combined image-sampler variables.
		static extern void shaderc_compile_options_set_auto_combined_image_sampler(
			CompileOptions options, bool upgrade);

		// Sets whether the compiler should use HLSL IO mapping rules for bindings.
		// Defaults to false.
		static extern void shaderc_compile_options_set_hlsl_io_mapping(
			CompileOptions options, bool hlsl_iomap);

		// Sets whether the compiler should determine block member offsets using HLSL
		// packing rules instead of standard GLSL rules.  Defaults to false.  Only
		// affects GLSL compilation.  HLSL rules are always used when compiling HLSL.
		static extern void shaderc_compile_options_set_hlsl_offsets(
			CompileOptions options, bool hlsl_offsets);

		// Sets the base binding number used for for a uniform resource type when
		// automatically assigning bindings.  For GLSL compilation, sets the lowest
		// automatically assigned number.  For HLSL compilation, the regsiter number
		// assigned to the resource is added to this specified base.
		static extern void shaderc_compile_options_set_binding_base(
			CompileOptions options,
			shaderc_uniform_kind kind,
			uint32_t base);

		// Like shaderc_compile_options_set_binding_base, but only takes effect when
		// compiling a given shader stage.  The stage is assumed to be one of vertex,
		// fragment, tessellation evaluation, tesselation control, geometry, or compute.
		static extern void shaderc_compile_options_set_binding_base_for_stage(
			CompileOptions options, ShaderKind shader_kind,
			shaderc_uniform_kind kind, uint32_t base);

		// Sets whether the compiler should preserve all bindings, even when those
		// bindings are not used.
		static extern void shaderc_compile_options_set_preserve_bindings(
			CompileOptions options, bool preserve_bindings);

		// Sets whether the compiler should automatically assign locations to
		// uniform variables that don't have explicit locations in the shader source.
		static extern void shaderc_compile_options_set_auto_map_locations(
			CompileOptions options, bool auto_map);

		// Sets a descriptor set and binding for an HLSL register in the given stage.
		// This method keeps a copy of the string data.
		static extern void shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage(
			CompileOptions options, ShaderKind shader_kind,

	const char* reg, const char* set, const char* binding);

		// Like shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage,
		// but affects all shader stages.
		static extern void shaderc_compile_options_set_hlsl_register_set_and_binding(
			CompileOptions options, const char* reg, const char* set,

	const char* binding);

		// Sets whether the compiler should enable extension
		// SPV_GOOGLE_hlsl_functionality1.
		static extern void shaderc_compile_options_set_hlsl_functionality1(
			CompileOptions options, bool enable);

		// Sets whether 16-bit types are supported in HLSL or not.
		static extern void shaderc_compile_options_set_hlsl_16bit_types(
			CompileOptions options, bool enable);

		// Enables or disables relaxed Vulkan rules.
		//
		// This allows most OpenGL shaders to compile under Vulkan semantics.
		static extern void shaderc_compile_options_set_vulkan_rules_relaxed(
			CompileOptions options, bool enable);

		// Sets whether the compiler should invert position.Y output in vertex shader.
		static extern void shaderc_compile_options_set_invert_y(
			CompileOptions options, bool enable);

		// Sets whether the compiler generates code for max and min builtins which,
		// if given a NaN operand, will return the other operand. Similarly, the clamp
		// builtin will favour the non-NaN operands, as if clamp were implemented
		// as a composition of max and min.
		static extern void shaderc_compile_options_set_nan_clamp(
			CompileOptions options, bool enable);

		// An opaque handle to the results of a call to any shaderc_compile_into_*()
		// function.
		typedef struct shaderc_compilation_result* shaderc_compilation_result_t;

// Takes a GLSL source string and the associated shader kind, input file
// name, compiles it according to the given additional_options. If the shader
// kind is not set to a specified kind, but shaderc_glslc_infer_from_source,
// the compiler will try to deduce the shader kind from the source
// string and a failure in deducing will generate an error. Currently only
// #pragma annotation is supported. If the shader kind is set to one of the
// default shader kinds, the compiler will fall back to the default shader
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
// synchronization. If there was failure in allocating the compiler object,
// null will be returned.
static extern shaderc_compilation_result_t shaderc_compile_into_spv(

	const Compiler compiler, const char* source_text,
	size_t source_text_size, ShaderKind shader_kind,

	const char* input_file_name, const char* entry_point_name,

	const CompileOptions additional_options);

		// Like shaderc_compile_into_spv, but the result contains SPIR-V assembly text
		// instead of a SPIR-V binary module.  The SPIR-V assembly syntax is as defined
		// by the SPIRV-Tools open source project.
		static extern shaderc_compilation_result_t shaderc_compile_into_spv_assembly(

	const Compiler compiler, const char* source_text,
	size_t source_text_size, ShaderKind shader_kind,

	const char* input_file_name, const char* entry_point_name,

	const CompileOptions additional_options);

		// Like shaderc_compile_into_spv, but the result contains preprocessed source
		// code instead of a SPIR-V binary module
		static extern shaderc_compilation_result_t shaderc_compile_into_preprocessed_text(

	const Compiler compiler, const char* source_text,
	size_t source_text_size, ShaderKind shader_kind,

	const char* input_file_name, const char* entry_point_name,

	const CompileOptions additional_options);

		// Takes an assembly string of the format defined in the SPIRV-Tools project
		// (https://github.com/KhronosGroup/SPIRV-Tools/blob/master/syntax.md),
		// assembles it into SPIR-V binary and a shaderc_compilation_result will be
		// returned to hold the results.
		// The assembling will pick options suitable for assembling specified in the
		// additional_options parameter.
		// May be safely called from multiple threads without explicit synchronization.
		// If there was failure in allocating the compiler object, null will be
		// returned.
		static extern shaderc_compilation_result_t shaderc_assemble_into_spv(

	const Compiler compiler, const char* source_assembly,
	size_t source_assembly_size,
	const CompileOptions additional_options);

		// The following functions, operating on shaderc_compilation_result_t objects,
		// offer only the basic thread-safety guarantee.

		// Releases the resources held by the result object. It is invalid to use the
		// result object for any further operations.
		static extern void shaderc_result_release(shaderc_compilation_result_t result);

		// Returns the number of bytes of the compilation output data in a result
		// object.
		static extern size_t shaderc_result_get_length(const shaderc_compilation_result_t result);

		// Returns the number of warnings generated during the compilation.
		static extern size_t shaderc_result_get_num_warnings(

	const shaderc_compilation_result_t result);

		// Returns the number of errors generated during the compilation.
		static extern size_t shaderc_result_get_num_errors(const shaderc_compilation_result_t result);

		// Returns the compilation status, indicating whether the compilation succeeded,
		// or failed due to some reasons, like invalid shader stage or compilation
		// errors.
		static extern shaderc_compilation_status shaderc_result_get_compilation_status(

	const shaderc_compilation_result_t);

		// Returns a pointer to the start of the compilation output data bytes, either
		// SPIR-V binary or char string. When the source string is compiled into SPIR-V
		// binary, this is guaranteed to be castable to a uint32_t*. If the result
		// contains assembly text or preprocessed source text, the pointer will point to
		// the resulting array of characters.
		static extern const char* shaderc_result_get_bytes(const shaderc_compilation_result_t result);

		// Returns a null-terminated string that contains any error messages generated
		// during the compilation.
		static extern const char* shaderc_result_get_error_message(

	const shaderc_compilation_result_t result);

		// Provides the version & revision of the SPIR-V which will be produced
		static extern void shaderc_get_spv_version(unsigned int* version, unsigned int* revision);

		// Parses the version and profile from a given null-terminated string
		// containing both version and profile, like: '450core'. Returns false if
		// the string can not be parsed. Returns true when the parsing succeeds. The
		// parsed version and profile are returned through arguments.
		static extern bool shaderc_parse_version_profile(const char* str, int* version,
										   shaderc_profile* profile);
	}
}
