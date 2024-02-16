using Autofac;
using System;

namespace ICP.Batch.Demo
{
    using App_Start;
    using Commands;

    class Program
    {
        static void Main(string[] args)
        {
            LoggerConfig.Register();

            IContainer container = AutofacConfig.Register();

            using (var services = container.BeginLifetimeScope())
            {
                var service = services.Resolve<DemoBatchCommand>();
                Console.WriteLine($"結果：{service.Test()}");
            }

            container.Dispose();
            container = null;

            Console.ReadKey();
        }
    }
}
