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

        // POST: /api/auth/register
        // 201 Created + AuthResponseDto (inkl token)
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            var res = await _auth.RegisterAsync(dto);

            // 201 Created. Vi har ikke et "GetUserById" endpoint, så vi kan bare pege på login-route,
            // eller returnere Created uden Location. CreatedAtAction er stadig fint i skoleprojekter.
            return CreatedAtAction(nameof(Login), new { }, res);
        }

        // POST: /api/auth/login
        // 200 OK + AuthResponseDto (inkl token)
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            var res = await _auth.LoginAsync(dto);
            return Ok(res);
        }
    }
}
