using Autofac;
using ICP.Infrastructure.Abstractions;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Infrastructure.Core.Frameworks.Logging;

namespace ICP.Infrastructure.Core.Frameworks
{
    public class LogModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LogInterceptor>();

            builder.RegisterType<NLogLoggerFactory>()
                   .As<ILoggerFactory>();

            builder.RegisterGeneric(typeof(NLogLogger<>))
                   .As(typeof(ILogger<>));
        }
    }
}
