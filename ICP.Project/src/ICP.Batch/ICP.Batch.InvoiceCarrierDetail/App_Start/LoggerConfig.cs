using System;
using System.IO;
using ICP.Infrastructure.Core.Frameworks.Logging;

namespace ICP.Batch.InvoiceCarrierDetail
{
    public class LoggerConfig
    {
        public static void Register()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            NLogLogger.SetConfig(Path.Combine(path, "App_Data", "nlog.config"));
        }
    }
}