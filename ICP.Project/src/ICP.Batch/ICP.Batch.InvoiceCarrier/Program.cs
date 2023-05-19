using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ICP.Batch.InvoiceCarrier.App_Start;
using ICP.Batch.InvoiceCarrier.Commands;

namespace ICP.Batch.InvoiceCarrier
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
                    var command1 = services.Resolve<InvoiceCarrierCommand>();

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