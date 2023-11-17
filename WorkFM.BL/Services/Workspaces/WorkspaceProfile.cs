using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Workspaces;

namespace WorkFM.BL.Services.Workspaces
{
    public class WorkspaceProfile:Profile
    {
        public WorkspaceProfile()
        {
            CreateMap<WorkspaceCreateDto, Workspace>();
            CreateMap<WorkspaceUpdateDto, Workspace>();
            CreateMap<WorkspaceDto, Workspace>();
        }
    }
}
