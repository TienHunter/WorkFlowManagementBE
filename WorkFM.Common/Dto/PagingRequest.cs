using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Dto
{
    public class PagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string KeySearch { get; set; }
        public string ColumnsFilter { get; set; }
    }
}
