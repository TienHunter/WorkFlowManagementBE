using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Data.Kanbans;
using WorkFM.Common.Dto;
using WorkFM.Common.Exceptions;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Repos.Kanbans;
using WorkFM.DL.Repos.UserProjects;

namespace WorkFM.BL.Services.Kanbans
{
    public class KanbanBL : BaseBL<KanbanDto, Kanban>, IKanbanBL
    {
        private readonly IKanbanDL _kanbanDL;
        private readonly IUserProjectDL _userProjectDL;
        public KanbanBL(IServiceProvider serviceProvider, IKanbanDL kabanDL) : base(serviceProvider, kabanDL)
        {
            _kanbanDL = kabanDL;
            _userProjectDL = serviceProvider.GetService(typeof(IUserProjectDL)) as IUserProjectDL;
        }

        public async Task<ServiceResponse> CreateAsync(KanbanCreateDto kanbanCreateDto)
        {
            // check permission
            //var parameters = new Dictionary<string, object>
            //{
            //    {"@UserId", _contextData.UserId },
            //    {"@ProjectId", kanbanCreateDto.ProjectId }
            //};
            var userProject = await _userProjectDL.GetByProjectIdAndUserIdAsync(kanbanCreateDto.ProjectId, _contextData.UserId);
            if (userProject == null || userProject.UserRole == Common.Enums.UserRole.Member)
            {
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ErrorMessage = "You not permission"
                };
            }

            var kanban = _mapper.Map<Kanban>(kanbanCreateDto);
            base.BeforeCreate<Kanban>(ref kanban);
            kanban.UserId = _contextData.UserId;

            var res = await _kanbanDL.CreateAsync(kanban);
            if (res == 0)
            {
                throw new BaseException { ErrorMessage = "Create kanban failure" };
            }

            return new ServiceResponse
            {
                Data = kanban,
            };
        }

        public async Task<ServiceResponse> GetListByProjectIdAsync(Guid projectId)
        {
            // check permission

            var res = await _kanbanDL.GetListByProjectIdAsync(projectId);
            var kanbanList = _mapper.Map<List<KanbanDto>>(res);

            foreach (var k in kanbanList)
            {
                foreach (var c in k.Cards)
                {
                    c.Tags = c.Tags.Where(t=>t.IsUsed).ToList();
                }
            }


            return new ServiceResponse { Data = kanbanList, };
        }

        public async Task MoveAsync(KanbanMoveDto kanbanMoveDto)
        {
            // check exist
            var kanbanExist = await _kanbanDL.GetByIdAsync(kanbanMoveDto.Id) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found kanban"
            };

            // check permission ...

            var fieldUpdate = "SortOrder";
            // update cardExsit
            kanbanExist.SortOrder = kanbanMoveDto.SortOrder;

            // update
            await _kanbanDL.UpdateAsync(kanbanExist, fieldUpdate);
        }
    }
}
