using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using ICP.Batch.AppRssPush;
using ICP.Batch.AppRssPush.APP_Start;

namespace ICP.Batch.AppRssPush
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        static void Main()
        {
            //if (!Environment.UserInteractive)
                // running as service
                try
                {
                    using (var service = new ProgramService())
                    {
                        LoggerConfig.Register();
                        ServiceBase.Run(service);
                    }
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("AppRssPush", ex.ToString(), EventLogEntryType.Error);
                }

                /* else
                 {
                     // running as console app
                     using (var service = new ProgramService())
                     {
                         service.StartTimer();
     
                         Console.WriteLine("Press any key to stop...");
                         Console.ReadKey(true);
     
                         service.StopTimer();
                     }
                 }*/

        }

    }
}
