using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Projects;

namespace WorkFM.BL.Services.Projects
{
    public class ProjectProfile:Profile
    {
        public ProjectProfile()
        {
            CreateMap<ProjectCreateDto, Project>();
            CreateMap<ProjectUpdateDto, Project>();
            CreateMap<Project, ProjectDto>().ReverseMap();
        }
    }
}
