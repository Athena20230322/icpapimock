using AutoMapper;
using System;
using System.Linq;

namespace ICP.Infrastructure.Core.Frameworks
{
    public static class AutoMapperConfig
    {
        public static void Register()
        {
            Mapper.Initialize(config =>
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                Type tpeProfile = typeof(Profile);
                var profiles = assemblies
                                    .SelectMany(x => x.DefinedTypes)
                                    .Where(x => x.FullName.StartsWith("ICP.") &&
                                                x.IsSubclassOf(tpeProfile));
                config.AddProfiles(profiles);
            });
        }
    }
}
