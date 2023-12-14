using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Lib
{
    public static class WMSJsonConvert
    {
        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T DeserializeObject<T>(string json) {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
