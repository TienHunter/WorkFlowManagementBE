using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WorkFM.Common.Utils
{
    public static class Helper
    {
        public static string HandleSQLColumn(string columns)
        {
            return Regex.Replace(columns, "[^0-9a-zA-Z,_]", "");
        }
    }
}
