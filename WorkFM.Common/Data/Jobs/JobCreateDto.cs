using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Data.Jobs
{
    public class JobCreateDto
    {
        public Guid ChecklistId { get; set; }
        public Guid CardId { get; set; }
        public string JobName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int Status { get; set; }
        public string? ExtraProperty { get; set; }
        public double SortOrder { get; set; }
    }
}
