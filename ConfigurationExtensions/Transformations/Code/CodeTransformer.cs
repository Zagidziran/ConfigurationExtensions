namespace Zagidziran.ConfigurationExtensions.Transformations.Code
{
    using System.Collections.Generic;
    using Zagidziran.ConfigurationExtensions.Exceptions;
    using Zagidziran.ConfigurationExtensions.Substitutions;
    using Zagidziran.ConfigurationExtensions.Transformations;

    internal class CodeTransformer : ITransformer
    {
        private readonly CodeGenerationConfiguration configuration;

        public CodeTransformer(CodeGenerationConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ITransformationResult Transform(Substitution substitution, Dictionary<string, string> configuration)
        {
            var rawTransformer = CodeParser.Parse(substitution.Body, this.configuration.Referencies);
            var data = rawTransformer.Transform(configuration);

            switch (data)
            {
                case null:
                    return new NullTransformationResult();

                case string stringResult:
                    return new StringTransformationResult(stringResult);

                case IReadOnlyDictionary<string, string> dictionaryResult:
                    return new DictionaryTransformationResult(dictionaryResult);

                default:
                    throw new TransformationResultNotSupportedException(data.GetType());
            }
        }
    }
}
