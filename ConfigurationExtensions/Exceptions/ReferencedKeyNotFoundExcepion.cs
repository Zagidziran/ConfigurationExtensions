namespace Zagidziran.ConfigurationExtensions.Exceptions
{
    using System;

    public class ReferencedKeyNotFoundExcepion : Exception
    {
        public ReferencedKeyNotFoundExcepion(string sourceConfigurationKey, string referencedKey, string reason)
            : base ($"Failed to transform {sourceConfigurationKey}. Reason {reason}.")
        { 
            this.SourceConfigurationKey = sourceConfigurationKey;
            this.ReferencedKey = referencedKey;
            this.Reason = reason;
        }

        public string SourceConfigurationKey { get; }

        public string ReferencedKey { get; }

        public string Reason { get; }
    }
}
