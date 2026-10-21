using Meridian.Api.Data;
using Meridian.Api.Dto;
using Meridian.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace Meridian.Api.Service
{
    public class UsersService
    {
        private readonly MeridianContext _context;
        public UsersService(MeridianContext context)
        {
            _context = context;
        }
        public async Task<List<UsersDto>> GetUsers()
        {
            var users = await _context.AppUsers.Select(u => new UsersDto
            {
                Id=u.Id,
                Name = u.Name,
                Role = u.Role.ToString()
            }).ToListAsync();
            return users;
        }
    }
}
