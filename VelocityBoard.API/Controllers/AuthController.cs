using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VelocityBoard.Application.DTOs;
using VelocityBoard.Application.Interfaces;

namespace VelocityBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var token = _authService.Login(loginDto);

            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new { token });
        }

    }
}
