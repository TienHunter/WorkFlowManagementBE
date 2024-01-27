using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Data.Jobs
{
    public class JobMoveDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid ChecklistId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double SortOrder { get; set; }
    }
}
