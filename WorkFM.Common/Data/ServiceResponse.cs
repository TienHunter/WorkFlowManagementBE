using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;

namespace WorkFM.Common.Models
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public ServiceResponseCode Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

    }
}
