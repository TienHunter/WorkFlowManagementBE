 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkFM.BL.Services.Kanbans;
using WorkFM.Common.Data.Kanbans;

namespace WorkFM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KanbansController : ControllerBase
    {
        private readonly IKanbanBL _kabanBL;
        public KanbansController(IKanbanBL kanbanBL) { 
            _kabanBL = kanbanBL;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateKanban([FromBody] KanbanCreateDto kanbanCreateDto)
        {
            var res = await _kabanBL.CreateAsync(kanbanCreateDto);
            return Ok(res);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetListByProjectId([FromRoute] Guid projectId)
        {
            var res = await _kabanBL.GetListByProjectIdAsync(projectId);
            return Ok(res);
        }

        [HttpPut("move")]
        public async Task<IActionResult> MoveKanban([FromBody] KanbanMoveDto kanbanMoveDto)
        {
             await _kabanBL.MoveAsync(kanbanMoveDto);
            return Ok();
        }
    }
}
