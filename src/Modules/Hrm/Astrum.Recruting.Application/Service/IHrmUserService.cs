using Astrum.Recruting.Application.Shared;
using Astrum.Recruting.Domain.Aggregates;
using Astrum.Recruting.Domain.Aggregates.Candidate;
using Astrum.Recruting.Domain.Aggregates.Practice;
using Astrum.Recruting.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Recruting.Application.Service;

public interface IHrmUserService
{
    Task<HrmUser> CreateHrmUserFromPracticeRequest(PracticeRequest practiceRequest);
    Task<HrmUser?> GetHrmUserByTgLogin(string tgLogin);
}

public class HrmUserService : IHrmUserService
{
    private readonly RecrutingDbContext _context;

    public HrmUserService(RecrutingDbContext context)
    {
        _context = context;
    }


    public async Task<HrmUser> CreateHrmUserFromPracticeRequest(PracticeRequest practiceRequest)
    {
        var user = await _context.HrmUsers.FirstOrDefaultAsync(x => x.TgLogin.Equals(practiceRequest.TgLogin));
        if (user != null)
            return user;
        var fio = practiceRequest.Fio.Split(" ");
        user = new HrmUser
        {
            Name = fio[1],
            Srurname = fio[0],
            LastName = fio.Length == 3 ? fio[2] : null,
            DateCreated = practiceRequest.DateCreated,
            IsActive = true,
            ProfileId = null,
            Email = string.Empty,
            TgLogin = practiceRequest.TgLogin,
            Gender = Gender.None,
            Id = Guid.NewGuid()
        };
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<HrmUser?> GetHrmUserByTgLogin(string tgLogin)
    {
        tgLogin.AssertFound();
        return await _context.HrmUsers.FirstOrDefaultAsync(x => x.TgLogin.Equals(tgLogin));
    }

    public Task<HrmUser> CreateHrmUserFromVacancyRequest(CandidateResume practiceRequest)
    {
        throw new NotImplementedException();
    }
}