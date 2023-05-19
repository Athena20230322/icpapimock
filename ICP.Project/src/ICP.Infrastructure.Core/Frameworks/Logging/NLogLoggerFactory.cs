using ICP.Infrastructure.Abstractions.Logging;
using System;

namespace ICP.Infrastructure.Core.Frameworks.Logging
{
    public class NLogLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger()
        {
            return new NLogLogger();
        }

        public ILogger CreateLogger(Type type)
        {
            return new NLogLogger(type);
        }

        public ILogger CreateLogger<T>()
        {
            return new NLogLogger<T>();
        }

        public ILogger CreateLogger(string name)
        {
            return new NLogLogger(name);
        }
    }
}
