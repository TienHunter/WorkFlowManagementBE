using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkFM.BL.Services.Auth;
using WorkFM.Common.Models.Auth;

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
        public async Task<IActionResult> Login([FromBody] AuthRequest accountLogin)
        {
            var res = await _authBL.Login(accountLogin.Username, accountLogin.Password);
            return StatusCode(StatusCodes.Status200OK, res);
        }
    }
}
