using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorkFM.BL.Services.Projects;
using WorkFM.Common.Data.Projects;
using WorkFM.Common.Data.Workspaces;

namespace WorkFM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectBL _projectBL;

        public ProjectsController(IProjectBL projectBL)
        {
            _projectBL = projectBL;
        }

        /// <summary>
        /// crate 1 project
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ProjectCreateDto projectCreateDto)
        {
            var res = await _projectBL.CreateAsync(projectCreateDto);
            return Ok(res);
        }

        /// <summary>
        /// update 1 porject by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workspace"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateWorkspace([FromRoute] Guid id, [FromBody] ProjectUpdateDto projectUpdateDto)
        {
            var res = await _projectBL.UpdateAsync(id, projectUpdateDto);
            return Ok(res);
        }

        [HttpGet("workspace")]
        public async Task<IActionResult> GetProjectsInWorkspace([FromQuery][Required] Guid id)
        {
            var res = await _projectBL.GetProjectsInWorkspaceAsync(id);
            return Ok(res);
        }

    }
}
