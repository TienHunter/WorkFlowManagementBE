using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;

namespace WorkFM.Common.Dto
{
    public class ServiceResponse
    {
        public bool Success { get; set; } = true;
        public ServiceResponseCode Code { get; set; } = ServiceResponseCode.Success;
        public string Message { get; set; } = "Oke";
        public object Data { get; set; }

    }
}
