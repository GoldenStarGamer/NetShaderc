using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
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
			readonly nint handle;

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

				if (opt.Optimization.HasValue) Shaderc.shaderc_compile_options_set_optimization_level(handle, opt.Optimization.Value);

				if (opt.ForcedVersionProfile.HasValue) Shaderc.shaderc_compile_options_set_forced_version_profile(this, opt.ForcedVersionProfile.Value.Key, opt.ForcedVersionProfile.Value.Value);

				if (opt.Resolve.HasValue) Shaderc.shaderc_compile_options_set_include_callbacks(this, opt.Resolve.Value.Resolve, opt.Resolve.Value.Release, opt.Resolve.Value.UserData);

				if (opt.SuppressWarnings) Shaderc.shaderc_compile_options_set_suppress_warnings(this);

				if (opt.TargetEnv.HasValue) Shaderc.shaderc_compile_options_set_target_env(this, opt.TargetEnv.Value.Key, opt.TargetEnv.Value.Value);

				if (opt.TargetSpirvVersion.HasValue) Shaderc.shaderc_compile_options_set_target_spirv(this, opt.TargetSpirvVersion.Value);

				if (opt.WarningsAsErrors) Shaderc.shaderc_compile_options_set_warnings_as_errors(this);

				if (opt.Limits != null) foreach (var limit in opt.Limits) Shaderc.shaderc_compile_options_set_limit(this, limit.Key, limit.Value);

				if (opt.AutoBindUniforms) Shaderc.shaderc_compile_options_set_auto_bind_uniforms(this, true);

				if (opt.AutoCombinedImageSampler) Shaderc.shaderc_compile_options_set_auto_combined_image_sampler(this, true);

				if (opt.HLSLIOMapping) Shaderc.shaderc_compile_options_set_hlsl_io_mapping(this, true);

				if (opt.HLSLOffsets) Shaderc.shaderc_compile_options_set_hlsl_offsets(this, true);

				if (opt.BindingBase != null) foreach (var binding in opt.BindingBase) Shaderc.shaderc_compile_options_set_binding_base(this, binding.Key, binding.Value);

				if (opt.BindingBaseForStage != null) foreach (var stage in opt.BindingBaseForStage) foreach (var binding in stage.Value) Shaderc.shaderc_compile_options_set_binding_base_for_stage(this, stage.Key, binding.Key, binding.Value);

				if (opt.PreserveBindings) Shaderc.shaderc_compile_options_set_preserve_bindings(this, true);

				if (opt.AutoMapLocations) Shaderc.shaderc_compile_options_set_auto_map_locations(this, true);

				if (opt.RegisterSetAndBindingForStage != null) foreach (var rsbs in opt.RegisterSetAndBindingForStage) Shaderc.shaderc_compile_options_set_hlsl_register_set_and_binding_for_stage(this, rsbs.Key, rsbs.Value.reg, rsbs.Value.set, rsbs.Value.binding);

				if (opt.RegisterSetAndBinding != null) Shaderc.shaderc_compile_options_set_hlsl_register_set_and_binding(this, opt.RegisterSetAndBinding.Value.reg, opt.RegisterSetAndBinding.Value.set, opt.RegisterSetAndBinding.Value.binding);

				if (opt.HLSLFunctionality1) Shaderc.shaderc_compile_options_set_hlsl_functionality1(this, true);

				if (opt.HLSL16BitTypes) Shaderc.shaderc_compile_options_set_hlsl_16bit_types(this, true);

				if (opt.RelaxedVulkanRules) Shaderc.shaderc_compile_options_set_vulkan_rules_relaxed(handle, true);

				if (opt.InvertY) Shaderc.shaderc_compile_options_set_invert_y(handle, true);

				if (opt.NANClamp) Shaderc.shaderc_compile_options_set_nan_clamp(handle, true);
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
					GC.SuppressFinalize(this);
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

		public struct RegisterSetBinding
		{
			public string reg, set, binding;
		}

		public Dictionary<string, string>? Macros;

		public SourceLanguage Language = SourceLanguage.GLSL; // Shaderc specifies that GLSL is the default so me do that

		public bool DebugInfo = false; // funny enough you can't set this off in the original version, stupid design

		public OptimizationLevel? Optimization; // not sure what the default is so nullable it is

		public KeyValuePair<int, Profile>? ForcedVersionProfile; // KeyValuePair because it takes 2 values

		public IncludeResolve? Resolve;

		public bool SuppressWarnings;

		public KeyValuePair<TargetEnv, uint>? TargetEnv;

		public SpirvVersion? TargetSpirvVersion;

		public bool WarningsAsErrors;

		public Dictionary<Limit, int>? Limits;

		public bool AutoBindUniforms = false;
		
		public bool AutoCombinedImageSampler = false;

		public bool HLSLIOMapping = false;

		public bool HLSLOffsets = false;

		public Dictionary<UniformKind, uint>? BindingBase;

		public Dictionary<ShaderKind, Dictionary<UniformKind, uint>>? BindingBaseForStage;

		public bool PreserveBindings = false;

		public bool AutoMapLocations = false;

		public Dictionary<ShaderKind, RegisterSetBinding>? RegisterSetAndBindingForStage;

		public RegisterSetBinding? RegisterSetAndBinding;

		public bool HLSLFunctionality1 = false;

		public bool HLSL16BitTypes = false;

		public bool RelaxedVulkanRules = false;

		public bool InvertY = false;

		public bool NANClamp = false;

		internal OptHndl Handle { get { return new OptHndl(this); } }
	}
}
