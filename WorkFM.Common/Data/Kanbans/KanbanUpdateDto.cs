using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFM.Common.Data.Kanbans
{
    public class KanbanUpdateDto
    {
        [Required(ErrorMessage ="Id is required")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "project is required")]
        public Guid ProjectId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string? ExtraProperty { get; set; }
        [Required(ErrorMessage = "sort order is required")]
        public double SortOrder { get; set; }
    }
}
