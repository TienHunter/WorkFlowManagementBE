using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using WorkFM.BL.Services.Workspaces;
using WorkFM.Common.Data.Workspaces;

namespace WorkFM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkspacesController : ControllerBase
    {
        private readonly IWorkspaceBL _workspaceBL;
        public WorkspacesController(IWorkspaceBL workspaceBL)
        {
           _workspaceBL = workspaceBL;
        }

        /// <summary>
        /// tạo mới 1 không gian làm việc
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateWorkspace([FromBody] WorkspaceCreateDto workspace)
        {
            var res = await _workspaceBL.CreateWorkspaceAsync(workspace);
            return Ok(res);
        }

        /// <summary>
        /// cập nhật không gian làm việc
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workspace"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateWorkspace([FromRoute] Guid id, [FromBody] WorkspaceUpdateDto workspace)
        {
            var res = await _workspaceBL.UpdateWorkspaceAsync(id,workspace);
            return Ok(res);
        }

        /// <summary>
        /// get all workspace user has joined
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var res = await _workspaceBL.GetAllAsync();
            return Ok(res);
        }

        /// <summary>
        /// get workspace by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var res = await _workspaceBL.GetByIdAsync(id);
            return Ok(res);
        }

    }
}
