using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using System.Text;
using System.Text.Json;

const string CSHARP_FILE_EXT = "*.cs";
var directory = ParseDirectoryFromArg(args[0]);
string[] files = Directory.GetFiles(directory, CSHARP_FILE_EXT);

foreach (var filePath in files)
{
    var fileText = File.ReadAllText(filePath, Encoding.UTF8); ;
    List<string> methodNames = GetMethodNames(fileText);

    if (!methodNames.Any())
        continue;

    var methodBodys = methodNames.ToDictionary(
        methodName => methodName, 
        methodName => GetMethodBody(fileText, methodName)
    );

    string json = JsonSerializer.Serialize(methodBodys, new JsonSerializerOptions { 
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    });
    
    var jsonFileName = $"{Path.GetFileName(filePath)}.json";
    var fqOutputFileName = $"{directory}\\{jsonFileName}";

    File.WriteAllText(fqOutputFileName, json);
}

static List<string> GetMethodNames(string sourceCode)
{
    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
    CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();

    List<string> methodNames = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
        .Select(m => m.Identifier.ValueText).ToList();
        //.Select(m => m.Identifier.ValueText).ToList();

    return methodNames;
}

static string GetMethodBody(string sourceCode, string methodName)
{
    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
    CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();

    MethodDeclarationSyntax method = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                                          .FirstOrDefault(m => m.Identifier.ValueText == methodName);

    return method?.Body?.ToString();
}

static string ParseDirectoryFromArg(string? arg)
    => arg ?? Assembly.GetExecutingAssembly().Location;