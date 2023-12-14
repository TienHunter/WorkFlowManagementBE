using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Utils
{
    public class SystemService : ISystenService
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }

        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }

        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}
