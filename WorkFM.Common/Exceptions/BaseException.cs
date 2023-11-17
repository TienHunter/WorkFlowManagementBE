using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Exceptions
{
    public class BaseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
        public string Code { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }
    }
}
