using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;

namespace WorkFM.Common.Data.Workspaces
{
    public class WorkspaceUpdateDto
    {
        public Guid Id { get; set; }
        public string WorkspaceName { get; set; }
        public Guid ImageId { get; set; }
        public WorkspaceType Type { get; set; }
        public string Description { get; set; }
    }
}
