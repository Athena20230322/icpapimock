using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Integration.Mvc;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Library.Commands.App_Start;
using ICP.Library.Models.PaymentCenterApi.Enums;
using ICP.Library.Repositories.App_Start;
using ICP.Library.Services.App_Start;
using ICP.Modules.Api.PaymentCenter.Commands;
using ICP.Modules.Api.PaymentCenter.Enums;
using ICP.Modules.Api.PaymentCenter.Factory;
using ICP.Modules.Api.PaymentCenter.Factory.ATM;
using ICP.Modules.Api.PaymentCenter.Interface;
using ICP.Modules.Api.PaymentCenter.Interface.ATM;
using ICP.Modules.Api.PaymentCenter.Repositories.ATM;
using ICP.Modules.Api.PaymentCenter.Services;
using System.Linq;

namespace ICP.Modules.Api.PaymentCenter.App_Start
{
    public class PaymentCenterApiModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(ThisAssembly);

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith("ICP.Modules.Api.PaymentCenter.Commands") ||
                               x.FullName.StartsWith("ICP.Modules.Api.PaymentCenter.Services"))
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(x => x.FullName.StartsWith("ICP.Modules.Api.PaymentCenter.Repositories"))
                   .InstancePerLifetimeScope()
                   .EnableClassInterceptors()
                   .InterceptedBy(typeof(DbProxyInterceptor));

            builder.RegisterType<TradeCommandFactory>()
                   .As<ITradeCommandFactory>();

            builder.RegisterType<TransactionCommand>()
                  .As<ITradeCommand>()
                  .WithMetadata(nameof(eTradeMode), eTradeMode.Transaction);

            builder.RegisterType<TopupCommand>()
                  .As<ITradeCommand>()
                  .WithMetadata(nameof(eTradeMode), eTradeMode.Topup);

            builder.RegisterType<RefundCommand>()
                  .As<ITradeCommand>()
                  .WithMetadata(nameof(eTradeMode), eTradeMode.Refund);

            builder.RegisterType<ReversalCommand>()
                  .As<ITradeCommand>()
                  .WithMetadata(nameof(eTradeMode), eTradeMode.Reversal);

            builder.RegisterType<PaymentMethodFactory>()
                   .As<IPaymentMethodFactory>();

            builder.RegisterType<TransactionMethodFactory>()
                   .As<ITransactionMethodFactory>();

            builder.RegisterType<ATMServiceFactory>()
                   .As<IATMServiceFactory>();

            builder.RegisterType<FirstATMRepository>()
                  .As<IATMService>()
                  .WithMetadata(nameof(PaymentSubType_ATM), PaymentSubType_ATM.First);

            builder.RegisterType<ICashPaymentService>()
                   .As<IPaymentMethod>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.ICash);

            builder.RegisterType<AccountLinkPaymentService>()
                   .As<IPaymentMethod>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.AccountLink);

            builder.RegisterType<ATMPaymentService>()
                   .As<IPaymentMethod>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.ATM);

            builder.RegisterType<CashPaymentService>()
                   .As<IPaymentMethod>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.Cash);

            builder.RegisterType<InvoicePaymentService>()
                   .As<IPaymentMethod>()
                   .WithMetadata(nameof(ePaymentType), ePaymentType.Invoice);

            builder.RegisterType<TransactionService>()
                   .As<ITransactionMethod>()
                   .WithMetadata(nameof(eTradeMode), eTradeMode.Transaction);

            builder.RegisterType<TopupService>()
                   .As<ITransactionMethod>()
                   .WithMetadata(nameof(eTradeMode), eTradeMode.Topup);

            builder.RegisterType<TransferService>()
                   .As<ITransactionMethod>()
                   .WithMetadata(nameof(eTradeMode), eTradeMode.Transfer);

            builder.RegisterType<WithdrawalService>()
                   .As<ITransactionMethod>()
                   .WithMetadata(nameof(eTradeMode), eTradeMode.Withdrawal);

            //builder.RegisterType<RefundService>()
            //       .As<ITransactionMethod>()
            //       .WithMetadata(nameof(eTradeMode), eTradeMode.Allocate);

            //builder.RegisterType<RefundService>()
            //       .As<ITransactionMethod>()
            //       .WithMetadata(nameof(eTradeMode), eTradeMode.Adjust);

            builder.RegisterModule<CommandLibraryModule>();
            builder.RegisterModule<ServiceLibraryModule>();
            builder.RegisterModule<RepositoryLibraryModule>();
        }
    }
}
