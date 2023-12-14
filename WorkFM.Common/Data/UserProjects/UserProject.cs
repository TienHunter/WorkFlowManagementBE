using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Data.UserProjects
{
    [Table("user_project")]
    public class UserProject:BaseEntity,IsHasInfoCreate, IsHasInfoUpdate
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public UserRole UserRole { get; set; } = UserRole.Member;
        public bool IsFavorite { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
