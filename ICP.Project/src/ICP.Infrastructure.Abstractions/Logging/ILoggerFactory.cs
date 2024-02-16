using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Abstractions.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger();

        ILogger CreateLogger(string name);

        ILogger CreateLogger(Type type);

        ILogger CreateLogger<T>();
    }
}
