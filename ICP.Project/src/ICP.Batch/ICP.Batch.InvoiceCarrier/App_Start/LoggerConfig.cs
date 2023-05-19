using System;
using System.IO;
using ICP.Infrastructure.Core.Frameworks.Logging;

namespace ICP.Batch.InvoiceCarrier.App_Start
{
    public static class LoggerConfig
    {
        public static void Register()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            NLogLogger.SetConfig(Path.Combine(path, "App_Data", "nlog.config"));
        }
    }
}