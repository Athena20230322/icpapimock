using Autofac;
using System;

namespace ICP.Batch.OPUploadFileWrite
{
    using App_Start;
    using Commands;

    class Program
    {
        static void Main(string[] args)
        {
            DateTime? execDate = null;

            //外部參數
            if (args != null && args.Length > 0)
            {
                #region 執行日期
                if (!string.IsNullOrWhiteSpace(args[0]))
                {
                    execDate = Convert.ToDateTime(args[0]);
                }
                #endregion
            }

            //Log 設定
            LoggerConfig.Register();

            using (IContainer container = AutofacConfig.Register())
            {
                using (var services = container.BeginLifetimeScope())
                {
                    //會員狀態變更
                    var command1 = services.Resolve<MemberStatusChangeCommand>();
                    command1.exec(execDate);
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
