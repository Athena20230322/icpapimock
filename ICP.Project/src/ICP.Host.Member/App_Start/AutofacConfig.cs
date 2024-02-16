﻿using Autofac;
using Autofac.Integration.Mvc;
using ICP.Infrastructure.Core.Frameworks;
using ICP.Infrastructure.Core.Web.Frameworks;
using ICP.Modules.Api.Authorization.App_Start;
using ICP.Modules.Api.Member.App_Start;
using ICP.Modules.Mvc.Authorization.App_Start;
using ICP.Modules.Mvc.Member.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ICP.Host.Member.App_Start
{
    public static class AutofacConfig
    {
        /// <summary>
        /// 註冊DI注入物件資料
        /// </summary>
        public static void Register()
        {
            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            // 註冊 Controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // 攔截Attribute注入屬性
            builder.RegisterFilterProvider();

            // 載入模組
            builder.RegisterModule<DefaultHostModule>();
            builder.RegisterModule<AuthorizationApiModule>();
            builder.RegisterModule<AuthorizationMvcModule>();
            builder.RegisterModule<MemberApiModule>();
            builder.RegisterModule<MemberMvcModule>();

            // 建立容器
            IContainer container = builder.Build();

            // 解析容器內的型別
            AutofacDependencyResolver resolver = new AutofacDependencyResolver(container);

            // 建立相依解析器
            DependencyResolver.SetResolver(resolver);
        }
    }
}