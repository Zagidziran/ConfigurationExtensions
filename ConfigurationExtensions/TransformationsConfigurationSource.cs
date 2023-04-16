namespace Zagidziran.ConfigurationExtensions
{
    using Microsoft.Extensions.Configuration;
    using System.Linq;
    using Zagidziran.ConfigurationExtensions.Transformations.Code;

    internal sealed class TransformationsConfigurationSource : IConfigurationSource
    {
        private readonly ConfigurationManager configurationManager;

        private readonly CodeGenerationConfiguration codeGenerationConfiguration;

        public TransformationsConfigurationSource(
            ConfigurationManager configurationManager,
            CodeGenerationConfiguration codeGenerationConfiguration)
        {
            this.configurationManager = configurationManager;
            this.codeGenerationConfiguration = codeGenerationConfiguration;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new TransformationsConfigurationProvider(
                this.codeGenerationConfiguration,
                configurationManager.AsEnumerable());
        }
    }
}