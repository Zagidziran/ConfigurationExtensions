namespace Zagidziran.ConfigurationExtensions.Transformations
{
    using System.Collections.Generic;
    using Zagidziran.ConfigurationExtensions.Substitutions;

    internal interface ITransformer
    {
        ITransformationResult Transform(Substitution substitution, Dictionary<string, string> configuration);
    }
}
