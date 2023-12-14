using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Data.Kanbans
{
    public class KanbanMoveDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public double SortOrder { get; set; }
    }
}
