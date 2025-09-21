using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KvizAPI.Application.DTO;
using KvizAPI.Domain.Interfaces;

namespace KvizAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResultDto>> Register([FromBody] RegisterRequestDto request)
        {
            if (request == null) return BadRequest();
            var result = await _authService.RegisterAsync(request);
            if (!result.Success)
            {
                if (result.Error == "User already exists")
                {
                    return Conflict(result);
                }
                return BadRequest(result);
            }
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginRequestDto request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            var result = await _authService.LoginAsync(request);
            if (!result.Success)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
    }
}
