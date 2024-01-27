using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkFM.BL.Services.Checklists;
using WorkFM.Common.Data.Checklists;

namespace WorkFM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistsController : ControllerBase
    {
        private readonly IChecklistBL _checklistBL;
        public ChecklistsController(IChecklistBL checklistBL)
        {
            _checklistBL = checklistBL;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ChecklistCreateDto checklistCreateDto)
        {
            var res = await _checklistBL.CreateAsync(checklistCreateDto);
            return Ok(res);
        }

        [HttpPut("move")]
        public async Task<IActionResult> Move(ChecklistMoveDto checklistMoveDto)
        {
            var res = await _checklistBL.MoveAsync(checklistMoveDto);
            return Ok(res);
        }

    }
}
