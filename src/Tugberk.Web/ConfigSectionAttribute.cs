using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tugberk.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ConfigSectionAttribute : Attribute
    {
        public ConfigSectionAttribute(string sectionName)
        {
            SectionName = sectionName ?? throw new ArgumentNullException(nameof(sectionName));
        }
        
        public string SectionName { get; }
        
        public static IEnumerable<TypeInfo> GetAllAppliedTypes(IEnumerable<Assembly> assemblies) => 
            assemblies.SelectMany(assembly => assembly
                .DefinedTypes.Where(type => type.GetCustomAttributes(typeof(ConfigSectionAttribute), true).Any()));
    }
}
