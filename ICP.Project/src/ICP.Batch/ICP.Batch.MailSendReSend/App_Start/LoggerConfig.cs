using ICP.Infrastructure.Core.Frameworks.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.MailSendReSend.App_Start
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
