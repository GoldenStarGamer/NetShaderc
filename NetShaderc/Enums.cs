using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetShaderc
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
		[Obsolete("it says so on the shaderc.h file")]
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
	public enum CompilationStatus : uint
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
	public enum SourceLanguage : uint
	{
		GLSL,
		HLSL,
	}

	public enum ShaderKind : uint
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

	public enum Profile : uint
	{
		None,                   // Used if and only if GLSL version did not specify
								// profiles.
		Core,
		[Obsolete("Disabled. This generates an error")]
		Compatibility,
		ES,
	}

	/// <summary>
	/// Optimization level.
	/// </summary>
	public enum OptimizationLevel : uint
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
	public enum Limit : uint
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
	public enum UniformKind : uint
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



	/// <summary>
	/// The kinds of include requests.
	/// </summary>
	public enum IncludeType : uint
	{
		Relative,  // E.g. #include "source"
		Standard   // E.g. #include <source>
	};

	public enum CompileType
	{
		CodeToSPIRV,
		CodeToAsm,
		CodeToPreprocessed,
		AsmToSPIRV,
	}
}
