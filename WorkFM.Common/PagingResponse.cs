using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common
{
    public class PagingResponse
    {
        public dynamic Data { get; set; }

        public int TotalRecords { get; set; }

        public int TotalRoots { get; set; }
    }
}
