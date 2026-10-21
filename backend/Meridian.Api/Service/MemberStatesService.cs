using Meridian.Api.Data;
using Meridian.Api.Dto;
using Meridian.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace Meridian.Api.Service
{
    public class MemberStatesService
    {
        private readonly MeridianContext _context;
        public MemberStatesService(MeridianContext context)
        {
            _context = context;
        }
        public async Task<List<MemberStateDto>> GetAllMemberStates()
        {
            var memberStates = await _context.MemberStates.OrderBy(m => m.Name).Select(u => new MemberStateDto
            {
                Code = u.Code,
                Name = u.Name,
                Periods = u.MembershipPeriods.OrderBy(p => p.MemberFrom).Select(p => new MembershipPeriodDto
                {
                    MemberFrom = p.MemberFrom,
                    MemberTo = p.MemberTo
                }).ToList()
            }).ToListAsync();
            return memberStates;
        }
    }
}
