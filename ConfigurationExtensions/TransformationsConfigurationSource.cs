namespace Zagidziran.ConfigurationExtensions
{
    using Microsoft.Extensions.Configuration;
    
    internal sealed class TransformationsConfigurationSource : IConfigurationSource
    {
        private readonly ConfigurationManager configurationManager;

        public TransformationsConfigurationSource(ConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new TransformationsConfigurationProvider(
                configurationManager.AsEnumerable());
        }
    }
}