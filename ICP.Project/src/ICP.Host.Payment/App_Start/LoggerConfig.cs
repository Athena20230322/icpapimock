using ICP.Infrastructure.Core.Frameworks.Logging;
using System;
using System.IO;

namespace ICP.Host.Payment.App_Start
{
    public class LoggerConfig
    {
        public static void Register()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");

            NLogLogger.SetConfig(Path.Combine(path, "nlog.config"));
        }
    }
}