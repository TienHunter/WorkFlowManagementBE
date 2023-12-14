using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Checklists;
using WorkFM.Common.Dto;

namespace WorkFM.BL.Services.Checklists
{
    public interface IChecklistBL: IBaseBL<Checklist, Checklist>
    {
        public Task<ServiceResponse> CreateAsync(ChecklistCreateDto checklistCreateDto);
    }
}
