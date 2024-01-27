using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Data.Workspaces
{
    public class WorkspaceDto : BaseEntity
    {
        public Guid UserId { get; set; }
        public string WorkspaceName { get; set; }
        public string ImageUrl { get; set; }
        public WorkspaceType Type { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
    }
}
