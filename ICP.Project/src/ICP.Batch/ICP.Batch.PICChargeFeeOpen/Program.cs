using Autofac;
using ICP.Batch.PICChargeFeeOpen.App_Start;
using ICP.Batch.PICChargeFeeOpen.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.PICChargeFeeOpen
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
                    var service = services.Resolve<PICChargeFeeOpenCommand>();
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
