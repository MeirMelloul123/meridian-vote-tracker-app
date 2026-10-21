using Meridian.Api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meridian.Api.Controllers
{
    [Route("api/member-states")]
    [ApiController]
    public class MemberStatesController : ControllerBase
    {
        private readonly MemberStatesService _memberStatesService;
        public MemberStatesController(MemberStatesService memberStatesService)
        {
            _memberStatesService = memberStatesService;
        }
        public async Task<IActionResult> GetAllMemberStates()
        {    var memberStates = await _memberStatesService.GetAllMemberStates();
            return Ok(memberStates);
        }
    }
}
