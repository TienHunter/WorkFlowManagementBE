using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Attributes;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Data.Kanbans
{
    [Table("kanban")]
    public class Kanban:BaseEntity, IsHasInfoCreate, IsHasInfoUpdate
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string? ExtraProperty { get; set; }
        public double SortOrder { get; set; }
        [IgnoreProp]
        public List<Card> Cards { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
