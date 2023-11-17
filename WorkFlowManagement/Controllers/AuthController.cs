using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkFM.BL.Services.Auth;
using WorkFM.Common.Data.Users;

namespace WorkFM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBL _authBL ;
        public AuthController(IAuthBL authBL)
        {
            _authBL = authBL;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin accountLogin)
        {
            var res = await _authBL.Login(accountLogin);
            return StatusCode(StatusCodes.Status200OK, res);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
        {
            var res = await _authBL.Register(userRegister);
            return StatusCode(StatusCodes.Status200OK, res);
        }
    }
}
