using Autofac;
using System;

namespace ProxiesAndInterceptors
{
    class Program
    {
        static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            cb.RegisterAssemblyModules(typeof(Program).Assembly);
            var container = cb.Build();











            var calcService = container.Resolve<ICalcService>();

            Console.WriteLine("2 + 3 = " + calcService.Add(2, 3));

            Console.ReadLine();
        }
    }
}
