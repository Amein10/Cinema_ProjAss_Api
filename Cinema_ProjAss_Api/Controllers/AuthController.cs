using Microsoft.AspNetCore.Http;
using Cinema_ProjAss_Application.DTOs.Auth;
using Cinema_ProjAss_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cinema_ProjAss_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            var res = await _auth.RegisterAsync(dto);
            return Ok(res);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            var res = await _auth.LoginAsync(dto);
            return Ok(res);
        }
    }
}
