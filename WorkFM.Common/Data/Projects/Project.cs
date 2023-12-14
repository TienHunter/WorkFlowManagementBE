using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Attributes;
using WorkFM.Common.Enums;
using WorkFM.Common.Models.Base;

namespace WorkFM.Common.Data.Projects
{
    [Table("project")]
    public class Project:BaseEntity, IsHasInfoCreate, IsHasInfoUpdate
    {
        public Guid UserId { get; set; }
        public Guid WorkspaceId { get; set; }
        public string ProjectName { get; set; }
        public ProjectType Type { get; set; }
        public Guid ImageId { get; set; }

        [IgnoreProp]
        public UserRole UserRole { get; set; }
        [IgnoreProp]
        public bool IsFavorite { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
