using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Library.Commands.App_Start;
using ICP.Library.Repositories.App_Start;
using ICP.Library.Services.App_Start;
using ICP.Modules.Api.Payment.Interface;
using ICP.Modules.Api.Payment.Models.Payment;
using ICP.Modules.Api.Payment.Services;
using ICP.Modules.Api.Payment.Services.PaymentType;
using ICP.Modules.Api.Payment.Services.TradeMode;
using System.Linq;

namespace ICP.Modules.Api.Payment.App_Start
{
    public class PaymentApiModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string sNamespace = ThisAssembly.GetName().Name;

            builder.RegisterControllers(ThisAssembly);

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith($"{sNamespace}.Commands") ||
                               x.FullName.StartsWith($"{sNamespace}.Services"))
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith($"{sNamespace}.Repositories"))
                   .InstancePerLifetimeScope()
                   .EnableClassInterceptors()
                   .InterceptedBy(typeof(DbProxyInterceptor));

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith("ICP.Modules.Api.Payment.Factory"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<TRANSACTION_ICASHService>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.TRANSACTION_ICASH)
                   .As<IPaymentType>();

            builder.RegisterType<TRANSACTION_ICASHService>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.TRANSFER_ICASH)
                   .As<IPaymentType>();

            builder.RegisterType<TRANSACTION_ICASHService>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.WITHDRAWAL_ICASH)
                   .As<IPaymentType>();

            builder.RegisterType<TRANSACTION_ICASHService>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.ADJUST_ICASH)
                   .As<IPaymentType>();

            builder.RegisterType<ACCOUNTLINKService>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.ACCOUNTLINK)
                   .As<IPaymentType>();

            builder.RegisterType<ATMService>()
                  .WithMetadata(nameof(ePaymentType), ePaymentType.ATM)
                  .As<IPaymentType>();

            builder.RegisterType<CASHService>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.CASH)
                   .As<IPaymentType>();

            builder.RegisterType<INVOICEService>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.INVOICE)
                   .As<IPaymentType>();

            builder.RegisterType<TransactionService>()
                   .WithMetadata(nameof(eTradeMode), eTradeMode.Transaction)
                   .As<ITradeMode>();

            builder.RegisterType<TopupService>()
                   .WithMetadata(nameof(eTradeMode), eTradeMode.Topup)
                   .As<ITradeMode>();

            builder.RegisterType<TransferService>()
                   .WithMetadata(nameof(eTradeMode), eTradeMode.Transfer)
                   .As<ITradeMode>();

            builder.RegisterType<WithdrawalService>()
                   .WithMetadata(nameof(eTradeMode), eTradeMode.Withdrawal)
                   .As<ITradeMode>();

            builder.RegisterModule<CommandLibraryModule>();
            builder.RegisterModule<ServiceLibraryModule>();
            builder.RegisterModule<RepositoryLibraryModule>();
        }
    }
}
