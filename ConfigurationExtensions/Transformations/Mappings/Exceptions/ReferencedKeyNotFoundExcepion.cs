namespace Zagidziran.ConfigurationExtensions.Transformations.Mappings.Exceptions
{
    using System;

    public class ReferencedKeyNotFoundExcepion : Exception
    {
        public ReferencedKeyNotFoundExcepion(string referencedKey)
            : base($"Referenced key {referencedKey} is not found.")
        {
            ReferencedKey = referencedKey;
        }

        public string ReferencedKey { get; }
    }
}
