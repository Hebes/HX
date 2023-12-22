using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class ExpansionEnum
    {
        public static int ToInt(this System.Enum e)
        {
            return e.GetHashCode();
        }
    }
}
