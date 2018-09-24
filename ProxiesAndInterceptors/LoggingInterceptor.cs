using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProxiesAndInterceptors
{
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.Write($"{invocation.Method} {string.Join(", ", invocation.Arguments)} ");

            invocation.Proceed();

            Console.WriteLine($"returns:{invocation.ReturnValue}");
        }
    }
}
