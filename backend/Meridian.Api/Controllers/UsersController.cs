using Meridian.Api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meridian.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;
        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }
        public async Task<IActionResult> GetAllUsers()
        {    var users = await _usersService.GetUsers();
            return Ok(users);

        }
    }
}
