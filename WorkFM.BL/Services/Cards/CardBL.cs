using Newtonsoft.Json;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Data.Tags;
using WorkFM.Common.Dto;
using WorkFM.Common.Exceptions;
using WorkFM.DL.Repos.Bases;
using WorkFM.DL.Repos.Cards;
using WorkFM.DL.Repos.Kanbans;
using WorkFM.DL.Repos.UserProjects;

namespace WorkFM.BL.Services.Cards
{
    public class CardBL : BaseBL<CardDto, Card>, ICardBL
    {
        private readonly ICardDL _cardDL;
        private readonly IKanbanDL _kanbanDL;
        private readonly IUserProjectDL _userProjectDL;
        public CardBL(IServiceProvider serviceProvider, ICardDL cardDL) : base(serviceProvider, cardDL)
        {
            _cardDL = cardDL;
            _userProjectDL = serviceProvider.GetService(typeof(IUserProjectDL)) as IUserProjectDL;
            _kanbanDL = serviceProvider.GetService(typeof(IKanbanDL)) as IKanbanDL;
        }

        public async Task<ServiceResponse> CreateAsync(CardCreateDto cardCreateDto)
        {
            var card = _mapper.Map<Card>(cardCreateDto);
            card.Id = _systenService.NewGuid();

            // check user in project

            card.UserId = _contextData.UserId;

            base.BeforeCreate<Card>(ref card);

            var res = await _cardDL.CreateAsync(card);
            if(res == 0)
            {
                throw new BaseException
                {
                    ErrorMessage = "Create card failure"
                };
            }

            return new ServiceResponse
            {
                Data = card
            };

        }

        public async Task<ServiceResponse> EditTitleAsync(CardUpdateDto cardUpdateDto)
        {
            // check exit 
            var cardExist = await _cardDL.GetByIdAsync(cardUpdateDto.Id) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found card"
            };
            var kanbanExist = await _kanbanDL.GetByIdAsync(cardUpdateDto.KanbanId) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found kanban"
            };
            // check permission
            var userProject = await _userProjectDL.GetByProjectIdAndUserIdAsync(kanbanExist.ProjectId,_contextData.UserId) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.Forbidden,
                ErrorMessage = "You not permission"
            };

            cardExist.Title = cardUpdateDto.Title;

            var res = await _cardDL.UpdateAsync(cardExist, "Title");
            if(res == 0)
            {
                throw new BaseException
                {
                    ErrorMessage = "Edit title card failure"
                };
            }

            return new ServiceResponse();
        }

        public async Task<ServiceResponse> GetById(Guid id)
        {
            // lay thong tin the
            var card = await _cardDL.GetByIdAsync(id) ?? throw new BaseException
            {
                StatusCode= System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found card"
            };
            var cardDto = _mapper.Map<CardDto>(card);
            // lay danh sach thanh vien
            // lay danh sach cong viec
            // lay danh sach tep dinh kem
            // lay lich su
            return new ServiceResponse()
            {
                Data = cardDto,
            };

        }

        public async Task MoveCardAsync(CardMoveDto cardMoveDto)
        {
            // check exit 
            var cardExist = await _cardDL.GetByIdAsync(cardMoveDto.Id) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage="Not found card"
            };
            // check permission
            var kanbanExist = await _kanbanDL.GetByIdAsync(cardMoveDto.KanbanId) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found kanban"
            };

            // check thẻ thuộc dự án nào có quyền truy cập không

            // check permission ...

            var fieldUpdate = "KanbanId,SortOrder";
            // update cardExsit
            cardExist.KanbanId = cardMoveDto.KanbanId;
            cardExist.SortOrder = cardMoveDto.SortOrder;

            // update
            await _cardDL.UpdateAsync(cardExist, fieldUpdate);

        }

        /// <summary>
        /// update description of card
        /// </summary>
        /// <param name="id"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        /// <exception cref="BaseException"></exception>
        public async Task UpdateDescriptionAsync(Guid id, string desc)
        {
            var cardExist = await _cardDL.GetByIdAsync(id) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found card"
            };

            cardExist.Description = desc;
            var fields = "Description";
            await _cardDL.UpdateAsync(cardExist, fields);
        }

        /// <summary>
        /// create, update, delete tag in card
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public async Task UpdateTagsAsync(Guid id, List<Tag> tags)
        {
            var cardExist = await _cardDL.GetByIdAsync(id) ?? throw new BaseException
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                ErrorMessage = "Not found card"
            };

            cardExist.Tags = JsonConvert.SerializeObject(tags);

            var res = await _cardDL.UpdateAsync(cardExist, "Tags");
            if (res == 0) throw new BaseException
            {
                ErrorMessage = "Update tag failure"
            };
        }
    }
}
