using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorkFM.BL.Services.Cards;
using WorkFM.Common.Data.Cards;
using WorkFM.Common.Data.Tags;

namespace WorkFM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        public ICardBL _cardBL;
        public CardsController(ICardBL cardBL) {
            _cardBL = cardBL;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var res = await _cardBL.GetById(id);
            return Ok(res);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CardCreateDto cardCreateDto)
        {
            var res = await _cardBL.CreateAsync(cardCreateDto);
            return Ok(res);
        }

        [HttpPut("edit-title")]
        public async Task<IActionResult> EditTile([FromBody] CardUpdateDto cardUpdateDto )
        {
            var res = await _cardBL.EditTitleAsync(cardUpdateDto);
            return Ok(res);
        }

        [HttpPut("move")]
        public async Task<IActionResult> MoveCard([FromBody] CardMoveDto cardMoveDto)
        {
            await _cardBL.MoveCardAsync(cardMoveDto);
            return Ok();
        }

        [HttpPut("{id}/tag")]
        public async Task<IActionResult> UpdateTags([FromRoute][Required] Guid id, [FromBody] List<Tag> tags )
        {
            await _cardBL.UpdateTagsAsync(id, tags);
            return Ok();
        }

        [HttpPut("{id}/description")]
        public async Task<IActionResult> UpdateDescription([FromRoute] Guid id, [FromBody] CardDescDto cardDescDto)
        {
            await _cardBL.UpdateDescriptionAsync(id, cardDescDto.Description);
            return Ok();
        }


    }
}
