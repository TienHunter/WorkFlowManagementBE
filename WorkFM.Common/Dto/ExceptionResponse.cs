using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Dto
{
    public class ExceptionResponse
    {
        public string Code { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }
    }
}
