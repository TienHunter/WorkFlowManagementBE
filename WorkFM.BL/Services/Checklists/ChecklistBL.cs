using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Data.Checklists;
using WorkFM.Common.Data.Jobs;
using WorkFM.Common.Dto;
using WorkFM.Common.Exceptions;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Repos.Cards;
using WorkFM.DL.Repos.Checklists;

namespace WorkFM.BL.Services.Checklists
{
    public class ChecklistBL : BaseBL<Checklist, Checklist>, IChecklistBL
    {
        private readonly IChecklistDL _checklistDL;
        private readonly ICardDL _cardDL;

        public ChecklistBL(IServiceProvider serviceProvider, IChecklistDL checklistDL) : base(serviceProvider, checklistDL)
        {
            _checklistDL = checklistDL;
            _cardDL = serviceProvider.GetService(typeof(ICardDL)) as ICardDL;
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

        public async Task<ServiceResponse> MoveAsync(ChecklistMoveDto checklistMoveDto)
        {
            // check exit 
            var checklistExist = await _checklistDL.GetByIdAsync(checklistMoveDto.Id) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found checklist"
            };
            // check permission

            // check card exist
            var cardExist = await _cardDL.GetByIdAsync(checklistMoveDto.CardId) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found card"
            };

            // check thẻ thuộc dự án nào có quyền truy cập không

            // check permission ...

            var fieldUpdate = "CardId,SortOrder";
            // update cardExsit
            checklistExist.CardId = checklistMoveDto.CardId;
            checklistExist.SortOrder = checklistMoveDto.SortOrder;

            // update
            await _checklistDL.UpdateAsync(checklistExist, fieldUpdate);

            return new ServiceResponse();
        }
    }
}
