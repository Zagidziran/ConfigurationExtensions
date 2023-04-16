namespace Zagidziran.ConfigurationExtensions.Transformations
{
    using System.Collections.Generic;

    public class DictionaryTransformationResult : ITransformationResult
    {
        public DictionaryTransformationResult(IReadOnlyDictionary<string, string> data)
        {
            Data = data;
        }

        public IReadOnlyDictionary<string, string> Data { get; }
    }
}
