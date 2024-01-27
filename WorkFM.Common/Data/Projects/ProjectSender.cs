using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Enums;

namespace WorkFM.Common.Data.Projects
{
    public class ProjectSender
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public ProjectType Type { get; set; }
        public string ImageUrl { get; set; }
    }
}
