using Autofac;
using ICP.Infrastructure.Abstractions;
using ICP.Infrastructure.Abstractions.EmailSender;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Frameworks.EmailSender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks
{
    public class EmailSenderModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((context) =>
            {
                var smtpClient = new SmtpClient();
#if DEBUG
                smtpClient.Host = "192.168.151.2";
#endif
                var logger = context.Resolve<ILogger<SmtpEmailSender>>();
                return new SmtpEmailSender("GW-ProjectDev@ecpay.com.tw", smtpClient, logger);
            })
            .As<IEmailSender>();

            // other sender
        }
    }
}
