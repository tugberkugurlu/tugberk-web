using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Tugberk.Web
{
    public static class ConfigurationExtensions
    {
        public static TConfigType GetConfigType<TConfigType>(this IConfiguration configuration)
        {   
            var configSectionAttribute = typeof(TConfigType).GetCustomAttribute<ConfigSectionAttribute>();
            var configInstance = Activator.CreateInstance(typeof(TConfigType));
            var section = configuration.GetSection(configSectionAttribute.SectionName);
            
            ConfigurationBinder.Bind(section, configInstance);
            
            return (TConfigType)configInstance;
        }
    }
}
