using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Checklists;
using WorkFM.Common.Data.Jobs;
using WorkFM.Common.Dto;
using WorkFM.Common.Exceptions;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Repos.Checklists;

namespace WorkFM.BL.Services.Checklists
{
    public class ChecklistBL : BaseBL<Checklist, Checklist>, IChecklistBL
    {
        private readonly IChecklistDL _checklistDL;

        public ChecklistBL(IServiceProvider serviceProvider, IChecklistDL checklistDL) : base(serviceProvider, checklistDL)
        {
            _checklistDL = checklistDL;
        }

        public async Task<ServiceResponse> CreateAsync(ChecklistCreateDto checklistCreateDto)
        {
            var checklist = _mapper.Map<Checklist>(checklistCreateDto);
            checklist.Jobs = new List<Job>();
            base.BeforeCreate<Checklist>(ref checklist);
            var res = await _checklistDL.CreateAsync(checklist);
            if(res == 0)
            {
                throw new BaseException
                {
                    ErrorMessage="Create checklist failure"
                };
            }
            return new ServiceResponse
            {
                Data = checklist
            };
        }
    }
}
