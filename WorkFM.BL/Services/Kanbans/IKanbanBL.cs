using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Kanbans;
using WorkFM.Common.Dto;

namespace WorkFM.BL.Services.Kanbans
{
    public interface IKanbanBL:IBaseBL<KanbanDto,Kanban>
    {
        public Task<ServiceResponse> CreateAsync(KanbanCreateDto kanbanCreateDto);
        public Task<ServiceResponse> GetListByProjectIdAsync(Guid projectId);
        public Task MoveAsync(KanbanMoveDto kanbanMoveDto);
    }
}
