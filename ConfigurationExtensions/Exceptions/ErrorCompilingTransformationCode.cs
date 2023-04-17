namespace Zagidziran.ConfigurationExtensions.Exceptions
{
    using Microsoft.CodeAnalysis;
    using System;
    using System.Collections.Immutable;

    internal class ErrorCompilingTransformationCode : Exception
    {
        public ErrorCompilingTransformationCode(string code, string formattedCode, ImmutableArray<Diagnostic> diagnostics)
            : base($"Error compiling code.\r\n{code}\r\n{formattedCode}\r\n{FormatDiagnostics(diagnostics)}")
        {
            this.Code = code;
            this.FormattedCode = formattedCode;
            this.Diagnostics = diagnostics;
        }

        public string Code { get; }

        public string FormattedCode { get; }

        public ImmutableArray<Diagnostic> Diagnostics { get; }

        private static string FormatDiagnostics(ImmutableArray<Diagnostic> diagnostics)
        { 
            return string.Join(Environment.NewLine, diagnostics);
        }
    }
}
