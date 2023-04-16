namespace Zagidziran.ConfigurationExtensions.Transformations.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Zagidziran.ConfigurationExtensions.Substitutions;
    using Zagidziran.ConfigurationExtensions.Transformations;
    using Zagidziran.ConfigurationExtensions.Transformations.Mappings.Exceptions;

    internal class MappingTransformer : ITransformer
    {
        public ITransformationResult Transform(Substitution substitution, Dictionary<string, string> configuration)
        {
            var key = substitution.Body;
            var nestedKeys = configuration
                .Where(k => k.Key.StartsWith(key + ":"))
                .ToList();

            if (!nestedKeys.Any() && !configuration.ContainsKey(key))
            {
                throw new ReferencedKeyNotFoundExcepion(substitution.Body);
            }

            if (nestedKeys.Any())
            {
                var resultData = new Dictionary<string, string>();
                foreach (var nestedKey in nestedKeys)
                {
                    resultData[nestedKey.Key[(key.Length + 1)..]] = nestedKey.Value;
                }

                if (configuration.ContainsKey(key))
                {
                    resultData[string.Empty] = configuration[key];
                }

                return new DictionaryTransformationResult(resultData);
            }

            return new StringTransformationResult(configuration[key]);
        }
    }
}
