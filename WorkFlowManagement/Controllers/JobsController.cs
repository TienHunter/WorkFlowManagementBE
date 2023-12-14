using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
