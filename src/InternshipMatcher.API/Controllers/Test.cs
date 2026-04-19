using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternshipMatcher.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping() => Ok("API is working!");

        [HttpGet("ping-auth")]
        [Authorize]
        public IActionResult PingAuth() => Ok($"Hello {User.Identity!.Name}, you are authenticated!");
    }
}
