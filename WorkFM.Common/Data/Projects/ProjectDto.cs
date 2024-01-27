using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;

namespace WorkFM.Common.Data.Projects
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid WorkspaceId { get; set; }
        public string ProjectName { get; set; }
        public ProjectType Type { get; set; }
        public string ImageUrl { get; set; }
        public UserRole UserRole { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
