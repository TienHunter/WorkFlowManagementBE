using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Kanbans;

namespace WorkFM.BL.Services.Kanbans
{
    public class KanbanProfile:Profile
    {
        public KanbanProfile() {
            CreateMap<KanbanCreateDto, Kanban>();
            CreateMap<KanbanUpdateDto, Kanban>();
            CreateMap<KanbanDto,Kanban>().ReverseMap();
        }
    }
}
