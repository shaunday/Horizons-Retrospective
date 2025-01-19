using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Common.Extenders
{
    public static class DoubleExtenders
    {
        public static string ToF2String(this double value)
        {
            return value.ToString("F2");
        }
    }
}
