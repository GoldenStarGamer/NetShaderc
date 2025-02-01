using NetShaderc;

namespace Tests
{
	[TestClass]
	public sealed class Test
	{
		[TestMethod]
		public void SimpleCompilation()
		{
			var compiler = new Compiler();

			var vert = File.ReadAllText("vertex.vert");
			var frag = File.ReadAllText("fragment.frag");

			var vertres = compiler.Compile(vert, CompileType.CodeToSPIRV, ShaderKind.VertexShader);
			var fragres = compiler.Compile(frag, CompileType.CodeToSPIRV, ShaderKind.FragmentShader);

			if (vertres.Status != CompilationStatus.Success)
				Console.WriteLine(vertres.ErrorMessage);

			if (fragres.Status != CompilationStatus.Success)
				Console.WriteLine(fragres.ErrorMessage);
			
			if (vertres.Code is not null) File.WriteAllBytes("vertex.spv", vertres.Code);
			if (fragres.Code is not null) File.WriteAllBytes("fragment.spv", fragres.Code);
		}
	}
}
