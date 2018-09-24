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


            /*
                  .method public hidebysig instance int32  Add(int32 a,
                                                 int32 b) cil managed
                {
                  // Code size       19 (0x13)
                  .maxstack  3
                  .locals init (int32 V_0)
                  IL_0000:  nop
                  IL_0001:  ldarg.0
                  IL_0002:  ldfld      class ProxiesAndInterceptors.CalcService ProxiesAndInterceptors.Class1::calc
                  IL_0007:  ldarg.1
                  IL_0008:  ldarg.2
                  IL_0009:  callvirt   instance int32 ProxiesAndInterceptors.CalcService::Add(int32,
                                                                                              int32)
                  IL_000e:  stloc.0
                  IL_000f:  br.s       IL_0011
                  IL_0011:  ldloc.0
                  IL_0012:  ret
                } // end of method Class1::Add
            */



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
