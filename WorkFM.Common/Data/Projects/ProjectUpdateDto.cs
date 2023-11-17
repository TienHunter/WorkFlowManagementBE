using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;

namespace WorkFM.Common.Data.Projects
{
    public class ProjectUpdateDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public ProjectType Type { get; set; }
        public Guid ImageId { get; set; }
    }
}
