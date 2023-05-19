using Autofac;
using ICP.Batch.MailSendReSend.App_Start;
using ICP.Batch.MailSendReSend.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.MailSendReSend
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
                    var service = services.Resolve<ProgramCommand>();
                    service.Exec();
                }
            }
        }
    }
}
