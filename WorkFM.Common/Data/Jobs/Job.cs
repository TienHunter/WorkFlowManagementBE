using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Data.Jobs
{
    public class Job:BaseEntity, IsHasInfoCreate, IsHasInfoUpdate
    {
        public Guid ChecklistId { get; set; }
        public Guid CardId { get; set; }
        public string JobName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool IsFinished { get; set; } = false;
        public int Status { get; set; } = 0;
        public string ExtraProperty { get; set; }
        public double SortOrder { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
