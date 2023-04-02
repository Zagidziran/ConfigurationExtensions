namespace Zagidziran.ConfigurationExtensions
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationManagerExtensions
    {
        public static ConfigurationManager Transform(this ConfigurationManager configurationManager)
        {
            ((IConfigurationBuilder)configurationManager).Add(new TransformationsConfigurationSource(configurationManager));

            return configurationManager;
        }
    }
}
