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
    }
}