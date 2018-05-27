using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pld
{
    class StringUtil
    {
        public static int StringToInt(string str)
        {
            int result = 0;
            int.TryParse(str, out result);
            return result;
        }

        public static string IntToString(int value)
        {
            return value.ToString();
        }
    }
}
