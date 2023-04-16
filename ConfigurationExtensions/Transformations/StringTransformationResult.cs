namespace Zagidziran.ConfigurationExtensions.Transformations
{
    public class StringTransformationResult : ITransformationResult
    {
        public StringTransformationResult(string data)
        {
            Data = data;
        }

        public string Data { get; }
    }
}
