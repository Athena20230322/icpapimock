using Autofac;
using ICP.Batch.AccountLink.App_Start;
using ICP.Batch.AccountLink.Commands;
using ICP.Library.Models.AccountLinkApi.Enums;
using System;
using System.Linq;

namespace ICP.Batch.AccountLink
{
    class Program
    {
        static void Main(string[] args)
        {
            string sBankCode = string.Empty;
            if (args.Count() != 1)
            {
                while (string.IsNullOrEmpty(sBankCode))
                {
                    Console.WriteLine("請輸入銀行代碼.");
                    sBankCode = Console.ReadLine();
                }
            }
            else
            {
                sBankCode = args[0].ToString();
            }

            int bankCode = Int32.Parse(sBankCode);
            BankType bankType = (BankType)bankCode;

            LoggerConfig.Register();

            using (IContainer container = AutofacConfig.Register())
            {
                using (var services = container.BeginLifetimeScope())
                {
                    var command1 = services.Resolve<ACLinkCommand>();
                    command1.CreateBank(bankType);
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
