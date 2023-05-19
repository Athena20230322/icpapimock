using System;
using System.Diagnostics;
using System.IO.Ports;
using System.ServiceProcess;


namespace ICP.Batch.AppRssPush
{
    using System.Timers;
    using APP_Start;
    using Autofac;
    using Commands;

    public partial class ProgramService : ServiceBase
    {
        private Timer _timer;
        private IContainer _container = null;
        public StopBits _StopBits = 0;
        public ProgramService()
        {
            InitializeComponent();
        }

        public void StartTimer()
        {
            //使用多執行緒(每兩秒執行一次)
            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(AppRssPush);
            _timer.Interval = 2000;
            _timer.Start();
            
        }

        public void StopTimer()
        {
            _container.Dispose();
            _container = null;

            _timer.Stop();
            _timer = null;
        }

        private void AppRssPush(object sender, ElapsedEventArgs e)
        {
            if (_StopBits != StopBits.None)
            {
                try
                {
                    using (var container=AutofacConfig.Register())
                    {
                        using (var scop = container.BeginLifetimeScope())
                        {
                            var command = scop.Resolve<AppRssPushCommand>();
                            command.Start();
                        }  
                    }
                }
                catch (Exception exception)
                {
                    EventLog.WriteEntry("AppRssPush", exception.ToString(), EventLogEntryType.Error);
                    throw;
                }
            }
        }
        
        protected override void OnStart(string[] args)
        {
            //使用多執行緒(每兩秒執行一次)
            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(AppRssPush);
            _StopBits = StopBits.One;
            _timer.Interval = 2000;
            _timer.Start();
        }

        protected override void OnStop()
        {
            _StopBits = StopBits.None;
            _timer.Stop();
            _timer = null;
        }
    }
}
