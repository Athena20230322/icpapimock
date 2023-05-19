using System;
using System.IO;

namespace ICP.Batch.BankTranfserQuery.App_Start
{
    using Infrastructure.Core.Frameworks.Logging;

    public static class LoggerConfig
    {
        public static void Register()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            NLogLogger.SetConfig(Path.Combine(path, "App_Data", "nlog.config"));
        }
    }
}
