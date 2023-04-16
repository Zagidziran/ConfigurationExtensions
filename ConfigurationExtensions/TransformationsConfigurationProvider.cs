namespace Zagidziran.ConfigurationExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Zagidziran.ConfigurationExtensions.Exceptions;
    using Zagidziran.ConfigurationExtensions.Substitutions;
    using Zagidziran.ConfigurationExtensions.Transformations;
    using Zagidziran.ConfigurationExtensions.Transformations.Code;
    using Zagidziran.ConfigurationExtensions.Transformations.Mappings;

    internal sealed class TransformationsConfigurationProvider : ConfigurationProvider
    {
        private readonly Dictionary<string, string> originalConfiguration;

        private readonly ITransformer mappingTransformer = new MappingTransformer();

        private readonly ITransformer codeTransformer;

        public TransformationsConfigurationProvider(
            CodeGenerationConfiguration codeGenerationConfiguration,
            IEnumerable<KeyValuePair<string, string>> originalConfiguration)
        {
            this.originalConfiguration = new Dictionary<string, string>(originalConfiguration);
            this.codeTransformer = new CodeTransformer(codeGenerationConfiguration);
        }

        public override void Load()
        {
            foreach (var configurationItem in this.originalConfiguration)
            {
                if (configurationItem.Value == null)
                {
                    continue;
                }

                var substitutions =
                    SubstitutionLocator.FindSubstitutions(configurationItem.Value)
                    .ToList();

                if (substitutions.Count == 1 && configurationItem.Value == substitutions[0].Definition)
                {
                    var transformationResult = this.Transform(substitutions[0]);
                    switch (transformationResult)
                    {
                        case StringTransformationResult str:
                            this.Data[configurationItem.Key] = str.Data;
                            break;

                        case NullTransformationResult _:
                            this.Data[configurationItem.Key] = null;
                            break;

                        case DictionaryTransformationResult dictionary:
                            foreach (var entry in dictionary.Data)
                            {
                                var resultingKey = entry.Key == string.Empty
                                    ? configurationItem.Key
                                    : configurationItem.Key + ":" + entry.Key;

                                this.Data[resultingKey] = entry.Value;
                            }
                            break;
                    }
                }
                else if (substitutions.Count > 0)
                {
                    var stringBuilder = new StringBuilder();
                    var currentPosition = 0;

                    foreach (var substitution in substitutions)
                    {
                        var transformationResult = this.Transform(substitution);

                        if (transformationResult is not StringTransformationResult stringResult)
                        {
                            throw new TransformationResultNotSupportedException(transformationResult);
                        }

                        stringBuilder.Append(configurationItem.Value.Substring(currentPosition, substitution.Index));
                        stringBuilder.Append(stringResult.Data);
                        
                        currentPosition = substitution.Index + substitution.Length;
                    }

                    this.Data[configurationItem.Key] = stringBuilder.ToString();
                }
            }
        }

        private ITransformationResult Transform(Substitution substitution)
        {
            var transformer = substitution.Kind switch
            {
                SubstitutionKind.Mapping => this.mappingTransformer,
                SubstitutionKind.Code => this.codeTransformer,
                _ => throw new ApplicationException("It should not happen!")
            };

            return transformer.Transform(substitution, this.originalConfiguration);
        }
    }
}