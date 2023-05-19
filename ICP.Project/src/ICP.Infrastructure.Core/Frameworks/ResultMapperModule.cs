using Autofac;
using ICP.Infrastructure.Abstractions.ResultMapper;
using ICP.Infrastructure.Core.Frameworks.ResultMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks
{
    public class ResultMapperModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ResultMapperSource>()
                   .As<IResultMapperSource>();

            builder.Register(context =>
                   {
                       using (var source = context.Resolve<IResultMapperSource>())
                       {
                           var data = source.GetResults();
                           var resultMapper = new ResultMapper.ResultMapper();
                           resultMapper.Init(data);

                           return resultMapper;
                       }
                   })
                   .SingleInstance()
                   .AutoActivate();
        }
    }
}
