# Simple .NET Shaderc Bindings  

## Usage:  

### Simple SPIR-V compilation:  

Given the files `vertex.vert` and `fragment.frag` you can compile them by simply using:
```
var compiler = new Compiler();

var vert = File.ReadAllText("vertex.vert");
var frag = File.ReadAllText("fragment.frag");

var vertres = compiler.Compile(vert, CompileType.CodeToSPIRV, ShaderKind.VertexShader);
var fragres = compiler.Compile(frag, CompileType.CodeToSPIRV, ShaderKind.FragmentShader);
```

To display Errors you can just:
```
if (vertres.Status != CompilationStatus.Success)
	Console.WriteLine(vertres.ErrorMessage);

if (fragres.Status != CompilationStatus.Success)
	Console.WriteLine(fragres.ErrorMessage);
```

To write the result to a file just:
```
if (vertres.Code is not null) File.WriteAllBytes("vertex.spv", vertres.Code);
if (fragres.Code is not null) File.WriteAllBytes("fragment.spv", fragres.Code);
```
