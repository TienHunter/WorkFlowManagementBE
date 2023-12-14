using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Attributes;
using WorkFM.Common.Data.Checklists;
using WorkFM.Common.Data.Tags;
using WorkFM.Common.Data.Users;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Data.Cards
{
    [Table("card")]
    public class Card:BaseEntity, IsHasInfoCreate, IsHasInfoUpdate
    {
        public string Title { get; set; }
        public Guid KanbanId { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool IsFinished { get; set; }
        public int RemindType { get; set; }
        public double SortOrder { get; set; }
        public string Tags { get; set; }
        [IgnoreProp]
        public List<Checklist> Checklists { get; set; }
        [IgnoreProp]
        public List<Member> Members { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
