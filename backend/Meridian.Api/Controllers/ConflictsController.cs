using Meridian.Api.Dto;
using Meridian.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace Meridian.Api.Controllers
{
    [ApiController]
    [Route("api/conflicts")]
    public class ConflictsController : ControllerBase
    {
        private readonly ConflictService _conflictService;

        public ConflictsController(ConflictService conflictService)
        {
            _conflictService = conflictService;
        }

        // GET /api/conflicts
        [HttpGet]
        public async Task<IActionResult> GetPending()
        {
            var conflicts = await _conflictService.GetPendingAsync();
            return Ok(conflicts);
        }
        [HttpPost("decision")]
        public async Task<IActionResult> Decide([FromBody] DecisionRequest? request)
        {
            var ok = await _conflictService.DecideAsync(request);
            if (!ok) return Conflict("This conflict was already decided.");
            return Ok();
        }
    }
}