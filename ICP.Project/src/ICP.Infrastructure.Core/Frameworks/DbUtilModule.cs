using Autofac;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks
{
    public class DbUtilModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbConnectionPool>()
                   .As<IDbConnectionPool>()
                   // 同一個生命週期共用已開啟的 DB 連線
                   .InstancePerLifetimeScope();
        }
    }
}
