using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.AuthTeenagers
{
    using App_Start;
    using Commands;
    using ICP.Infrastructure.Abstractions.Logging;

    class Program
    {
        static void Main(string[] args)
        {
            LoggerConfig.Register();

            using (IContainer container = AutofacConfig.Register())
            {
                using (var services = container.BeginLifetimeScope())
                {
                    var command1 = services.Resolve<AuthTeenagersCommand>();
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
