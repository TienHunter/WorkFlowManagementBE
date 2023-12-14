using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Data.Cards
{
    public class CardMoveDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid KanbanId { get; set; }
        [Required]
        [Range(0,double.MaxValue)]
        public double SortOrder { get; set; }
    }
}
