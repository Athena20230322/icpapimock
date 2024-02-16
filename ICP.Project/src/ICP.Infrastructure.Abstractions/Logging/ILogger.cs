using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Abstractions.Logging
{
    public interface ILogger
    {
        void Info(string msg, params object[] args);

        void Info(Exception ex, string msg, params object[] args);

        void Error(string msg, params object[] args);

        void Error(Exception ex, string msg, params object[] args);

        void Warning(string msg, params object[] args);

        void Warning(Exception ex, string msg, params object[] args);

        void Trace(string msg, params object[] args);

        void Trace(Exception ex, string msg, params object[] args);

        void Fatal(string msg, params object[] args);

        void Fatal(Exception ex, string msg, params object[] args);

        void Debug(string msg, params object[] args);

        void Debug(Exception ex, string msg, params object[] args);

        void SetCustomVariables(string key, string value);

        void RemoveCustomVariables(string key);
    }

    public interface ILogger<T> : ILogger
    {

    }
}
