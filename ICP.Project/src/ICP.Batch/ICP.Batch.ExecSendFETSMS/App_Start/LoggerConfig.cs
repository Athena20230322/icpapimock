using ICP.Infrastructure.Core.Frameworks.Logging;
using System;
using System.IO;

namespace ICP.Batch.ExecSendFETSMS.App_Start
{
    public static class LoggerConfig
    {
        public static void Register()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            NLogLogger.SetConfig(Path.Combine(path, "nlog.config"));
        }
    }
}
