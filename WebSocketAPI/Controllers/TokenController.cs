using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebSocketAPI.TokenService;

namespace WebSocketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("CreateToken")]
        public IActionResult CreateToken()
        {
            var token = _tokenService.GenerateAcsessToken("test");
            Console.WriteLine(token);
            return Ok(new { accsess_token = token });
        }
        [Authorize(Roles = "test")]
        [HttpGet("get")]
        public IActionResult GetToken()
        {
            return Ok("asdasdasdasdasd");
        }
    }
}
