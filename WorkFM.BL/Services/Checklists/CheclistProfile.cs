using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.Common.Data.Checklists;

namespace WorkFM.BL.Services.Checklists
{
    public class CheclistProfile:Profile
    {
        public CheclistProfile() {
            CreateMap<ChecklistCreateDto, Checklist>();
        }
    }
}
