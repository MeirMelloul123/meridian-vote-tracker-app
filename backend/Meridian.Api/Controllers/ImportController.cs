using Meridian.Api.Dto;
using Meridian.Api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meridian.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly ImportService _importService;
        public ImportController(ImportService importService)
        {
            _importService = importService;
        }
        [HttpPost]
        public async Task<IActionResult> ImportFile([FromBody] FileRequestDto fileRequest)
        {    var users = await _importService.ImportFile(fileRequest);
            return Ok(users);

        }
    }
}
