using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;


const string source = """
                      using System;
                      namespace ConsoleApp{
                          internal class Program{
                              static int Main(string[] args){
                                  int _gen_a = Int32.Parse(args[0]);
                                  int _gen_b = Int32.Parse(args[1]);
                                  int _gen_c = _gen_a + _gen_b;
                                  Console.WriteLine(string.Format("{0}+{1}={2}", _gen_a, _gen_b, _gen_c));
                                  return _gen_c;
                              }
                          }
                      }
                      """;

var syntaxTree = CSharpSyntaxTree.ParseText(source);



var compilation = CSharpCompilation.Create(
    "TestAssembly",
    new[] { syntaxTree },
    new[]
    {
        MetadataReference.CreateFromFile(From("System.Runtime.dll")),
        MetadataReference.CreateFromFile(From("System.Private.CoreLib.dll")),
        MetadataReference.CreateFromFile(From("System.Console.dll")),
        MetadataReference.CreateFromFile(From("System.Linq.dll")),
    },
    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

using (var dllStream = new MemoryStream())
using (var pdbStream = new MemoryStream())
{
    var emitResult = compilation.Emit(dllStream, pdbStream);
    if (!emitResult.Success)
    {
        var sb = new StringBuilder();
        Console.ForegroundColor = ConsoleColor.Red;
        sb.AppendLine("Compilation failed:");
        Console.ResetColor();


        //print the warings for each
        foreach (var emitResultDiagnostic in emitResult.Diagnostics)
        {
            sb.AppendLine(emitResultDiagnostic.ToString());
        }

        Console.WriteLine(sb.ToString());
        return;
    }
    
    //print the warnings
    foreach (var emitResultDiagnostic in emitResult.Diagnostics)
    {
        Console.WriteLine(emitResultDiagnostic.ToString());
    }
    
    if(emitResult.Diagnostics.IsEmpty) Console.WriteLine("No warnings");
    
    //write the assembly to disk
    var path = @"C:\Users\Hexma\Desktop\flowScript\code_gen_playtime\codegen\__testOutputs";
    var dllPath = Path.Combine(path, "assembly.dll");
    var pdbPath = Path.Combine(path, "assembly.pdb");
    File.WriteAllBytes(dllPath, dllStream.ToArray());
    File.WriteAllBytes(pdbPath, pdbStream.ToArray());
    
}

return;


static string From( string shortDllName )
{
    string dllString = AppContext.GetData( "TRUSTED_PLATFORM_ASSEMBLIES" ).ToString();
    var dlls = dllString.Split( ";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
    string dll = dlls.First( d => d.Contains( shortDllName, StringComparison.OrdinalIgnoreCase ) );
    return dll;
}