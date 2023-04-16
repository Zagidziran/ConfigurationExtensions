namespace Zagidziran.ConfigurationExtensions.Transformations.Code
{
    using System;
    using System.Reflection;

    internal class CodeGenerationConfiguration
    {
        public Assembly[] Referencies { get; set; } = Array.Empty<Assembly>();
    }
}
