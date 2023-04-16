namespace Zagidziran.ConfigurationExtensions.Transformations.Code
{
    using System.Collections.Generic;

    // Could be I robot sequel
    public interface IRawCodeTransformer
    {
        object? Transform(IReadOnlyDictionary<string, string> configuration);
    }
}
