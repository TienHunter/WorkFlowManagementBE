using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Enums
{
    public enum ServiceResponseCode
    {
        Success = 0,
        Warning=1,
        UsernameTaken=2,
        EmailTaken=3,
        Error = 99,
        Exception=999,
    }
}
