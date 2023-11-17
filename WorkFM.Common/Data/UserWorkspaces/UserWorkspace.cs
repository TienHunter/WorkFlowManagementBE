using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Data.UserWorkspaces
{
    [Table("user_workspace")]
    public class UserWorkspace:BaseEntity, IsHasInfoCreate, IsHasInfoUpdate
    {
        public Guid UserId { get; set; }
        public Guid WorkspaceId { get; set; }
        public UserRole UserRole { get; set; } = UserRole.Member;

        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
