using System;
using Autofac;
using System;


namespace ICP.Batch.EInvoiceAwardNotify
{
    using App_Start;
    using Commands;

    class Program
    {
        static void Main(string[] args)
        {
            LoggerConfig.Register();

            using (IContainer container = AutofacConfig.Register())
            {
                using (var services = container.BeginLifetimeScope())
                {
                    var command1 = services.Resolve<ProgramCommand>();
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
