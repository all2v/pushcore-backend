using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Astrum.Recruting.Application.Shared;
using Astrum.Recruting.Domain.Aggregates.Practice;
using Astrum.Recruting.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Recruting.Application.Service;

public interface IPracticeVacancyService
{
    Task<List<PracticeDirection>> GetPracticeDirections(bool isActive);
    Task<List<PracticeCandidate>> GetPracticeCandidatesByPracticeId(Guid practiceId);
    Task<List<Candidate>> GetAllCandidates();
    Task<PracticeCreateModel> CreateOrUpdatePractice(PracticeCreateModel practiceCreateModel);
    Task<List<SkillModel>> GetPracticeSkills();
    Task<SkillModel> CreateOrUpdateSkill(SkillModel skillModel);
    Task RemovePracticeSkills(Guid skillId);
    Task MovePracticeDirectionToArchive(Guid practiceId);
    Task SetPracticesWithDeadlineNotActive(List<PracticeVacancy> vacancies);
    Task<PracticeVacancy> AddPracticeVacancy(PracticeVacancy vacancy);
    Task<DetailPracticeModel> GetPracticeDetail(Guid practiceId);
}

public class PracticeVacancyService : IPracticeVacancyService
{
    private readonly RecrutingDbContext _context;

    public PracticeVacancyService(RecrutingDbContext context)
    {
        _context = context;
    }

    public async Task<List<PracticeDirection>> GetPracticeDirections(bool isActive)
    {
        return await _context.PracticeVacancies
            .Where(x => x.IsActive == isActive)
            .Select(x => new PracticeDirection
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                ExpiredAt = x.ExpiredAt,
                MaxParticipantsCount = x.MaxParticipantsCount,
                RequestsCount = x.PracticeRequests.Select(x => x.TgLogin).Distinct().Count(),
                ApprovedRequests = x.PracticeRequests.Count(x => x.IsApproved)
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PracticeCandidate>> GetPracticeCandidatesByPracticeId(Guid practiceId)
    {
        var requestsByPerson = await _context.PracticeRequests
            .Where(x => x.PracticeVacancyId == practiceId)
            .GroupBy(x => x.TgLogin)
            .Select(x => new
            {
                TgLogin = x.Key,
                Requests = x.Select(x => new
                {
                    x.PracticeVacancyId, x.Fio, x.TaskUrl, x.About, x.HrmUserId, x.CreatedAt,
                    VacancyName = x.PracticeVacancy.Name
                }).OrderBy(x => x.CreatedAt).ToList()
            })
            .AsNoTracking()
            .ToListAsync();

        var result = new List<PracticeCandidate>(requestsByPerson.Count);
        foreach (var request in requestsByPerson)
        {
            var actualRequest = request.Requests.First();
            result.Add(new PracticeCandidate
            {
                HrmUserId = actualRequest.HrmUserId,
                TgLogin = request.TgLogin,
                About = actualRequest.About,
                Fio = actualRequest.Fio,
                TaskUrl = actualRequest.TaskUrl,
                VacancyName = actualRequest.VacancyName,
                PracticeDirectionId = actualRequest.PracticeVacancyId
            });
        }

        return result;
    }

    public async Task<List<Candidate>> GetAllCandidates()
    {
        return await Task.FromResult(_context.PracticeRequests
            .GroupBy(x => new { x.TgLogin, x.PracticeVacancyId })
            .AsEnumerable()
            .Select(x =>
                new Candidate
                {
                    TgLogin = x.Key.TgLogin,
                    About = x.LastOrDefault()!.About,
                    Fio = x.LastOrDefault()!.Fio,
                    HrmUserId = x.LastOrDefault()!.HrmUserId
                }
            )
            .ToList());

        // var result = new List<PracticeCandidate>(requestsByPerson.Count);
        // foreach (var request in requestsByPerson)
        // {
        //     var actualRequest = request.Requests.First();
        //     result.Add(new PracticeCandidate
        //     {
        //         HrmUserId = actualRequest.HrmUserId,
        //         TgLogin = request.TgLogin,
        //         About = actualRequest.About,
        //         Fio = actualRequest.Fio,
        //         TaskUrl = actualRequest.TaskUrl,
        //         VacancyName = actualRequest.VacancyName,
        //         PracticeDirectionId = actualRequest.PracticeVacancyId
        //     });
        // }
        //
        // return result;
    }

    public async Task<PracticeCreateModel> CreateOrUpdatePractice(PracticeCreateModel practiceCreateModel)
    {
        var practice = await _context.PracticeVacancies.FirstOrDefaultAsync(x => x.Id == practiceCreateModel.Id);

        if (practice != null && practice.IsActive == false)
            throw new ArgumentException("Нельзя редактировать архивные вакансии");

        var skills = await _context.PracticeSkills
            .Where(x => practiceCreateModel.PracticeSkills.Contains(x.Id))
            .ToListAsync();

        if (practice == null)
        {
            practiceCreateModel.Id = Guid.NewGuid();
            practice = new PracticeVacancy
            {
                Name = practiceCreateModel.Name,
                CreatedAt = DateTimeOffset.UtcNow,
                ExpiredAt = practiceCreateModel.ExpiredAt,
                IsActive = practiceCreateModel.IsActive,
                PracticeSkills = skills,
                MaxParticipantsCount = practiceCreateModel.MaxParticipantsCount,
                Responsibilities = practiceCreateModel.Responsibilities.Select(x => new Responsibility { Name = x })
                    .ToList(),
                TaskUrl = practiceCreateModel.TaskUrl,
                Id = practiceCreateModel.Id
            };
            await _context.AddAsync(practice);
        }
        else
        {
            practice.Name = practiceCreateModel.Name;
            practice.CreatedAt = DateTimeOffset.UtcNow;
            practice.ExpiredAt = practiceCreateModel.ExpiredAt;
            practice.IsActive = practiceCreateModel.IsActive;
            practice.PracticeSkills = skills;
            practice.MaxParticipantsCount = practiceCreateModel.MaxParticipantsCount;
            practice.Responsibilities = practiceCreateModel.Responsibilities
                .Select(x => new Responsibility { Name = x })
                .ToList();
            practice.TaskUrl = practiceCreateModel.TaskUrl;
        }

        await _context.SaveChangesAsync();
        return practiceCreateModel;
    }

    public async Task<List<SkillModel>> GetPracticeSkills()
    {
        return await _context.PracticeSkills
            .Select(x => new SkillModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .Distinct()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<SkillModel> CreateOrUpdateSkill(SkillModel skillModel)
    {
        var skill = await _context.PracticeSkills.FirstOrDefaultAsync(x => x.Id == skillModel.Id);
        if (skill == null)
        {
            skillModel.Id = Guid.NewGuid();
            skill = new PracticeSkill
            {
                Id = skillModel.Id,
                Name = skillModel.Name
            };
            await _context.AddAsync(skill);
        }
        else
        {
            skill.Name = skillModel.Name;
        }

        await _context.SaveChangesAsync();
        return skillModel;
    }

    public async Task RemovePracticeSkills(Guid skillId)
    {
        var skill = await _context.PracticeSkills.FirstOrDefaultAsync(x => x.Id == skillId);
        skillId.AssertFound();
        _context.PracticeSkills.Remove(skill);
        await _context.SaveChangesAsync();
    }

    public async Task MovePracticeDirectionToArchive(Guid practiceId)
    {
        var practice = await _context.PracticeVacancies.FirstOrDefaultAsync(x => x.Id == practiceId);
        practice.AssertFound();
        practice.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public Task SetPracticesWithDeadlineNotActive(List<PracticeVacancy> vacancies)
    {
        var t = vacancies.Where(x => x.ExpiredAt.Date >= DateTimeOffset.UtcNow.Date);
        foreach (var i in t) i.IsActive = false;
        return Task.CompletedTask;
    }

    public async Task<PracticeVacancy> AddPracticeVacancy(PracticeVacancy newVacancy)
    {
        newVacancy.Id = Guid.NewGuid();
        newVacancy.CreatedAt = DateTimeOffset.UtcNow.Date;
        newVacancy.ExpiredAt = DateTimeOffset.UtcNow.AddMonths(3).Date;
        await _context.AddAsync(newVacancy);
        await _context.SaveChangesAsync();
        return newVacancy;
    }

    public async Task<DetailPracticeModel> GetPracticeDetail(Guid practiceId)
    {
        var practice = await _context.PracticeVacancies
            .Include(x => x.PracticeRequests)
            .Include(x => x.PracticeSkills)
            .Include(x => x.Responsibilities)
            .FirstOrDefaultAsync(x => x.Id == practiceId);
        practice.AssertFound();
        return new DetailPracticeModel
        {
            Id = practice.Id,
            Name = practice.Name,
            CreatedAt = practice.CreatedAt,
            ExpiredAt = practice.ExpiredAt,
            MaxParticipantsCount = practice.MaxParticipantsCount,
            RequestsCount = practice.PracticeRequests.Select(x => x.TgLogin).Distinct().Count(),
            ApprovedRequests = practice.PracticeRequests.Count(x => x.IsApproved),
            Responsibilities = practice.Responsibilities.Select(x => new SkillModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList(),
            Skills = practice.PracticeSkills.Select(x => new SkillModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList()
        };
    }
}

public class PracticeDirection
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiredAt { get; set; }
    public int MaxParticipantsCount { get; set; }
    public int ApprovedRequests { get; set; }
    public int RequestsCount { get; set; }
}

public class DetailPracticeModel : PracticeDirection
{
    public List<SkillModel> Responsibilities { get; set; }
    public List<SkillModel> Skills { get; set; }
}

public class Candidate
{
    [Required] public string Fio { get; set; }
    [Required] public string TgLogin { get; set; }
    public string? About { get; set; }
    public Guid HrmUserId { get; set; }
}

public class PracticeCandidate
{
    public string Fio { get; set; }
    public string TgLogin { get; set; }
    public string TaskUrl { get; set; }
    public string About { get; set; }
    public Guid HrmUserId { get; set; }
    public Guid PracticeDirectionId { get; set; }
    public string VacancyName { get; set; }
}

public class PracticeCreateModel
{
    public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public bool IsActive { get; set; }
    [Required] public DateTimeOffset ExpiredAt { get; set; }
    [Required] public int MaxParticipantsCount { get; set; }
    [Required] public string TaskUrl { get; set; }
    [Required] public List<Guid> PracticeSkills { get; set; }
    [Required] public List<string> Responsibilities { get; set; }
}

public class SkillModel
{
    public Guid Id { get; set; }
    [Required] public string Name { get; set; }
}