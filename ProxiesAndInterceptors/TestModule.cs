using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Autofac.Extras.DynamicProxy;

namespace ProxiesAndInterceptors
{
    public class TestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CalcService>()
                .As<ICalcService>()
                .EnableInterfaceInterceptors();

            builder.RegisterType<LoggingInterceptor>().Named<IInterceptor>("logging");

            base.Load(builder);
        }
    }
}
