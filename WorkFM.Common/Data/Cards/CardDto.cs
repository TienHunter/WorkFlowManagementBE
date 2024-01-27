using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Attributes;
using WorkFM.Common.Data.Checklists;
using WorkFM.Common.Data.Files;
using WorkFM.Common.Data.Tags;
using WorkFM.Common.Data.Users;
using WorkFM.Common.Models.Base;
using WorkFM.Common.Models.Users;

namespace WorkFM.Common.Data.Cards
{
    public class CardDto:BaseEntity
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
        public List<Tag> Tags { get; set; }

        public List<Checklist> Checklists { get; set; }
        public List<FileEntity> Attachments { get; set; }
        // public List<Member> Members { get; set; }


    }
}
