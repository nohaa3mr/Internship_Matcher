using InternshipMatcher.Application.Interfaces;
using InternshipMatcher.Application.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternshipMatcher.API.Controllers
{
    [Route("api/AI")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IAIService _service;

        public AIController(IAIService service)
        {
            _service = service;
        }

        [HttpPost("match-score")]
        public async Task<IActionResult> GetMatchScore([FromBody] MatchRequest request)
        {
            var result = await _service.GetMatchScoreAsync(request.Skills, request.Description);
            return Ok(result);
        }
    }
    
}
