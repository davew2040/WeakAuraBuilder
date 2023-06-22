using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakAuraManager
{
    public static class Utilities
    {
        public static string FormatFromDictionary(this string formatString, Dictionary<string, string> valueDict)
        {
            StringBuilder newFormatString = new StringBuilder(formatString);
            foreach (var tuple in valueDict)
            {
                newFormatString = newFormatString.Replace(tuple.Key, tuple.Value);
            }
            return newFormatString.ToString();
        }

        public static string FormatFilenameDate(DateTime dt)
        {
            return dt.ToString("yyyy_MM_dd__HH_mm_ss");
        }
    }
}
