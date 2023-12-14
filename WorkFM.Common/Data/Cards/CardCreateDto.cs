using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Data.Cards
{
    public class CardCreateDto
    {

        [Required]
        public string Title { get; set; }
        [Required]

        public Guid KanbanId { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool IsFinished { get; set; }
        public int RemindType { get; set; }
        public double SortOrder { get; set; }
    }
}
