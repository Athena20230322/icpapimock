using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.ExecSendMistake
{
    using Autofac;
    using ICP.Batch.ExecSendMistake.App_Start;
    using ICP.Batch.ExecSendMistake.Commands;
    using System.Timers;

    public partial class ProgramService : ServiceBase
    {
        private Timer timer;
        private IContainer _container = null;

        public ProgramService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //使用多執行緒(每兩秒執行一次)
            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(ShortSmsSubmitMistake);
            timer.Interval = 2000;
            timer.Start();
        }

        private void ShortSmsSubmitMistake(object sender, ElapsedEventArgs e)
        {
            LoggerConfig.Register();
            _container = AutofacConfig.Register();

            using (var scop = _container.BeginLifetimeScope())
            {
                var command = scop.Resolve<SMSCommand>();
                command.Start();
            }
        }

        protected override void OnStop()
        {
            _container.Dispose();
            _container = null;

            timer.Stop();
            timer = null;
        }
    }
}
