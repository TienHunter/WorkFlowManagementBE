using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Attributes;
using WorkFM.Common.Data.Jobs;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Data.Checklists
{
    public class Checklist:BaseEntity
    {
        public Guid CardId { get; set; }
        public string ChecklistName { get; set; }
        public double SortOrder { get; set; }

        [IgnoreProp]
        public List<Job> Jobs { get; set; } = new List<Job>();
    }
}
