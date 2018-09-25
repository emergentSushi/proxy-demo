using Autofac;
using ProxiesAndInterceptors;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ProxyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var typeSignature = "MyProxy";

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    null);

            var constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            foreach (var targetMethod in typeof(ICalcService).GetMethods())
            {
                MethodBuilder mb = tb.DefineMethod(targetMethod.Name, MethodAttributes.Public);
                mb.SetParameters(new Type[] { typeof(int), typeof(int) });
                mb.DefineParameter(0, ParameterAttributes.None, "a");
                mb.DefineParameter(1, ParameterAttributes.None, "b");
                ILGenerator il = mb.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Callvirt, targetMethod);


                il.Emit(OpCodes.Ret);
            }

            Type proxyType = tb.CreateType();

            var myObject = Activator.CreateInstance(proxyType);


            var methodInfo = proxyType.GetMethod("Add");

            var ret = methodInfo.Invoke(myObject, new object[] { 2, 1 });
            

            Console.ReadLine();
        }
    }
}
