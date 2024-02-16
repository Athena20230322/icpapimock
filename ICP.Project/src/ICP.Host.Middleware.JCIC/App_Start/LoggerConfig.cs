using ICP.Infrastructure.Core.Frameworks.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.JCIC.App_Start
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