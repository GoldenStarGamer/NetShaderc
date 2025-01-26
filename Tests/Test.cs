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

			File.WriteAllBytes("vertex.spv", vertres.Code);
			File.WriteAllBytes("fragment.spv", fragres.Code);
		}
	}
}
