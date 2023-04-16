namespace Zagidziran.ConfigurationExtensions.Exceptions
{
    using Microsoft.CodeAnalysis;
    using System;
    using System.Collections.Immutable;

    internal class ErrorCompilingTransformationCode : Exception
    {
        public ErrorCompilingTransformationCode(string code, ImmutableArray<Diagnostic> diagnostics)
            : base($"Error compiling code. See {nameof(Diagnostics)} property.")
        {
            this.Code = code;
            this.Diagnostics = diagnostics;
        }

        public string Code { get; }
        
        public ImmutableArray<Diagnostic> Diagnostics { get; }
    }
}
