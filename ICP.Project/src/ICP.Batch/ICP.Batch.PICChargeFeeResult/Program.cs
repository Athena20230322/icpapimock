using Autofac;
using ICP.Batch.PICChargeFeeResult.App_Start;
using ICP.Batch.PICChargeFeeResult.Commands;
using System;

namespace ICP.Batch.PICChargeFeeResult
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggerConfig.Register();

            using (IContainer container = AutofacConfig.Register())
            {
                using (var services = container.BeginLifetimeScope())
                {
                    var service = services.Resolve<PICChargeFeeResultCommand>();
                    service.exec();
                }
            }

            //VS 偵錯
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("Press Any Key to Exit.");
                Console.ReadKey();
            }
        }
    }
}
