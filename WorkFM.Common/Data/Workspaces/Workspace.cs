using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Data.Workspaces
{
    /// <summary>
    /// lớp thể hiện không gian làm việc
    /// </summary>
    [Table("workspace")]
    public class Workspace : BaseEntity, IsHasInfoCreate,IsHasInfoUpdate
    {
        public Guid UserId { get; set; }
        public string WorkspaceName { get; set; }
        public string ImageUrl { get; set; }
        public WorkspaceType Type { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

    }
}
