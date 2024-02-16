using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ICP.Batch.InvoiceCarrierDetail.Commands;

namespace ICP.Batch.InvoiceCarrierDetail
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
                    var command1 = services.Resolve<InvoiceCarrierDetailCommand>();

                    command1.exec();
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
