using System;
using System.Collections.Generic;
using System.Text;

namespace ProxiesAndInterceptors
{
    public interface ICalcService
    {
        int Add(int a, int b);

        int Sub(int a, int b);
    }
}
