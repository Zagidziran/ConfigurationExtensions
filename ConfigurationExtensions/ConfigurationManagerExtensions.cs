namespace Zagidziran.ConfigurationExtensions
{
    using Microsoft.Extensions.Configuration;
    using System.Reflection;
    using Zagidziran.ConfigurationExtensions.Transformations.Code;

    public static class ConfigurationManagerExtensions
    {
        public static ConfigurationManager Transform(this ConfigurationManager configurationManager)
        {
            var configuration = new CodeGenerationConfiguration();
            configuration.Referencies = new[] { Assembly.GetCallingAssembly() };
            
            ((IConfigurationBuilder)configurationManager)
                .Add(new TransformationsConfigurationSource(configurationManager, configuration));

            return configurationManager;
        }
    }
}
