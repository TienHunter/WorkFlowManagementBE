using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkFM.BL.Services.Users;

namespace WorkFM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserBL _userBL;

        public UsersController(IUserBL userBL)
        {
            _userBL = userBL;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var res = await _userBL.GetAllAsync();

            return StatusCode(StatusCodes.Status200OK, res);
        }
    }
}
