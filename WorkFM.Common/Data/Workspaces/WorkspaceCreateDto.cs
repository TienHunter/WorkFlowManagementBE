using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;

namespace WorkFM.Common.Data.Workspaces
{
    public class WorkspaceCreateDto
    {
        [Required]
        public string WorkspaceName { get; set; }

        public Guid ImageId { get; set; }

        [Required]
        public WorkspaceType Type { get; set; }

        public string? Description { get; set; }
    }
}
