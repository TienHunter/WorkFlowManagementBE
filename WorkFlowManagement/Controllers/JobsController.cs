using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorkFM.BL.Services.Jobs;
using WorkFM.Common.Data.Jobs;

namespace WorkFM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobBL _jobBL;
        public JobsController(IJobBL jobBL)
        {
            _jobBL = jobBL;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(JobCreateDto jobCreateDto)
        {
            var res = await _jobBL.CraeteAsync(jobCreateDto);
            return Ok(res);
        }

        [HttpPut("update-finished/{id}/{status}")]
        public async Task<IActionResult> UpdateStatus([FromRoute][Required] Guid id, [FromRoute][Required] bool status)
        {
            await _jobBL.UpdateStatusAsync(id, status);
            return Ok();
        }
        [HttpPut("move")]
        public async Task<IActionResult> Move([FromBody][Required] JobMoveDto jobMoveDto)
        {
            _ = await _jobBL.MoveAsync(jobMoveDto);
            return Ok();
        }
    }
}
