using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Lib
{
    public interface IDbLogger<T>
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(Exception ex,string message);
        void LogDebbug(string message);
    }
}
