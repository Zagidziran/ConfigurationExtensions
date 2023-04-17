namespace Zagidziran.ConfigurationExtensions.Transformations.Code
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Text;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Loader;
    using System.Text;
    using Zagidziran.ConfigurationExtensions.Exceptions;

    internal static class CodeParser
    {
        private const string TransformMethodSignature = @"
        public object? Transform(IReadOnlyDictionary<string, string> Configuration)
        {";

        public static IRawCodeTransformer Parse(string code, params Assembly[] referenceRoots)
        {
            var appendedReferenceRoots = referenceRoots
                .Append(Assembly.GetExecutingAssembly())
                .Append(typeof(object).Assembly);
            var className = $"cl{Guid.NewGuid():N}";
            var formattedCode = FormatCode(code, className);
            var compilation = CompileCode(formattedCode, BuildReferencies(appendedReferenceRoots));
            var il = new MemoryStream();
            var emitResult = compilation.Emit(il);
            if (!emitResult.Success)
            {
                throw new ErrorCompilingTransformationCode(code, formattedCode, emitResult.Diagnostics);
            }

            il.Seek(0, SeekOrigin.Begin);
            var assembly = AssemblyLoadContext.Default.LoadFromStream(il);
            var type = assembly.GetType(className);
            var instance = Activator.CreateInstance(type);

            // TODO: Add assembly unload
            return (IRawCodeTransformer)instance;
        }

        private static string FormatCode(string code, string className)
        {

            var lines = code.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var codeBuilder = new StringBuilder(@"using System.Collections.Generic;
                using Zagidziran.ConfigurationExtensions.Transformations.Code;");

            var usingLinesNumber = 0;

            // moving usings
            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].TrimStart().StartsWith("using")
                    // Passing whitspeces lines between usings.
                    // Leading empty lines does not matter.
                    && !string.IsNullOrWhiteSpace(lines[i]))
                {
                    break;
                }
                codeBuilder.AppendLine(lines[i]);
                usingLinesNumber++;
            }

            codeBuilder.AppendLine($"internal class {className} : IRawCodeTransformer {{");
            codeBuilder.AppendLine(TransformMethodSignature);
            codeBuilder.AppendLine(string.Join("\r\n", lines.Skip(usingLinesNumber)));
            codeBuilder.AppendLine("}}");

            return codeBuilder.ToString();
        }

        private static IEnumerable<MetadataReference> BuildReferencies(IEnumerable<Assembly> referencesRoots)
        {
            return referencesRoots
                .SelectMany(root => root.GetReferencedAssemblies())
                .Distinct()
                .Select(assemblyName => Assembly.Load(assemblyName))
                .Concat(referencesRoots)
                .Select(assembly => assembly.Location)
                .Select(location => (MetadataReference)MetadataReference.CreateFromFile(location));
        }

        private static CSharpCompilation CompileCode(string sourceCode, IEnumerable<MetadataReference> referencies)
        {
            var codeString = SourceText.From(sourceCode);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);
            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);

            var compilationOptions = new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Release,
                assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default,
                nullableContextOptions: NullableContextOptions.Enable);

            return CSharpCompilation.Create(
                Guid.NewGuid().ToString(),
                new[] { parsedSyntaxTree },
                references: referencies,
                options: compilationOptions);
        }
    }
}
