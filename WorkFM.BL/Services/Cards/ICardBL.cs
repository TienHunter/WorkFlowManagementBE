using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFM.BL.Services.Bases;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Data.Tags;
using WorkFM.Common.Dto;

namespace WorkFM.BL.Services.Cards
{
    public interface ICardBL:IBaseBL<CardDto,Card>
    {
        public Task<ServiceResponse> GetById(Guid id);
        public Task<ServiceResponse> CreateAsync(CardCreateDto cardCreateDto);
        public Task<ServiceResponse> EditTitleAsync(CardUpdateDto cardUpdateDto);
        public Task MoveCardAsync(CardMoveDto cardMoveDto);

        /// <summary>
        /// create, update, delete tag in card
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public Task UpdateTagsAsync(Guid id, List<Tag> tags);

        /// <summary>
        /// update desc of card
        /// </summary>
        /// <param name="id"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public Task UpdateDescriptionAsync(Guid id, string desc);
    }
}
