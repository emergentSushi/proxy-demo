using System;
using System.Collections.Generic;
using System.Text;

namespace ProxiesAndInterceptors
{
    public class Class1
    {
        public CalcService calc = new CalcService();

        public int Add(int a, int b)
        {
            return calc.Add(a, b);
        }
    }
}
