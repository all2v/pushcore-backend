using System.ComponentModel.DataAnnotations;
using Astrum.Recruting.Application.Shared;
using Astrum.Recruting.Domain.Aggregates.Board;
using Astrum.Recruting.Domain.Aggregates.Candidate;
using Astrum.Recruting.Domain.Aggregates.Practice;
using Astrum.Recruting.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Recruting.Application.Service;

public interface ICandidateService
{
    Task<List<CandidateInfoModel>> GetAllCandidates();
    Task<CandidateDetailModel> GetDetailCandidate(Guid candidateId);
    Task<List<UserCommentModel>> GetCandidateComments(Guid candidateId);
    Task<CandidateDetailModel> CreateCandidate(CandidateCreateModel model);
    Task CreateComment(CreateCommentRequest commentRequest);
    Task<CandidateDetailModel> AddToPracticeVacancy(PracticeRequestCreate practiceCandidate);
}

public class CandidateService : ICandidateService
{
    private readonly RecrutingDbContext _context;

    public CandidateService(RecrutingDbContext context)
    {
        _context = context;
    }

    public async Task<List<CandidateInfoModel>> GetAllCandidates()
    {
        var hrmUsers = await _context
            .HrmUsers
            .Where(x => x.IsActive)
            .Select(x => new
            {
                x.Name,
                x.Srurname,
                x.LastName,
                x.TgLogin,
                x.Email,
                x.Id,
                WorkDates = x.CandidateResumes
                    .SelectMany(x => x.JobExperiences)
                    .Select(x => new { x.StartAt, x.EndAt }).ToList()
            })
            .AsNoTracking()
            .ToListAsync();

        return hrmUsers.Select(hrmUser => new CandidateInfoModel
            {
                Email = hrmUser.Email,
                Surname = hrmUser.Srurname,
                FirstName = hrmUser.Name,
                TgLogn = hrmUser.TgLogin,
                SecondName = hrmUser.LastName,
                HrmUserId = hrmUser.Id,
                WorkYears = hrmUser.WorkDates.Count == 0
                    ? 0d
                    : (hrmUser.WorkDates.Max(x => x.EndAt) - hrmUser.WorkDates.Min(x => x.StartAt)).TotalDays
            })
            .ToList();
    }

    public async Task<CandidateDetailModel> GetDetailCandidate(Guid candidateId)
    {
        var hrmUser = await _context
            .HrmUsers
            .Where(x => x.Id == candidateId)
            .Select(x => new
            {
                x.Name,
                x.Srurname,
                x.LastName,
                x.TgLogin,
                x.Email,
                x.Id,
                WorkDates = x.CandidateResumes
                    .SelectMany(x => x.JobExperiences)
                    .Select(x => new { x.StartAt, x.EndAt }).ToList(),
                LastResume = x.CandidateResumes.OrderBy(c => c.CreatedAt).LastOrDefault(),
                x.UserComments
            })
            .AsNoTracking()
            .FirstOrDefaultAsync();
        var practiceSkills = await _context.PracticeRequests.Where(x => x.HrmUserId == hrmUser.Id)
            .SelectMany(x => x.PracticeVacancy.PracticeSkills)
            .ToListAsync();

        var practiceRequests = await _context.PracticeRequests.Where(x => x.HrmUserId == hrmUser.Id)
            .GroupBy(x => x.PracticeVacancyId)
            .Select(x => new
            {
                VacancyId = x.Key,
                TaskUrl = x.First().TaskUrl,
                VacancyName = x.First().PracticeVacancy.Name
            })
            .ToListAsync();

        return new CandidateDetailModel
        {
            Email = hrmUser.Email,
            Surname = hrmUser.Srurname,
            FirstName = hrmUser.Name,
            TgLogn = hrmUser.TgLogin,
            SecondName = hrmUser.LastName,
            HrmUserId = hrmUser.Id,
            WorkYears = hrmUser.WorkDates.Count == 0
                ? 0d
                : (hrmUser.WorkDates.Max(x => x.EndAt) - hrmUser.WorkDates.Min(x => x.StartAt)).TotalDays / 365,

            Salary = hrmUser?.LastResume?.Salary,

            PracticeCandidates = practiceRequests
                .Select(x => new PracticeCandidate
                {
                    HrmUserId = hrmUser.Id,
                    PracticeDirectionId = x.VacancyId,
                    TaskUrl = x.TaskUrl,
                    VacancyName = x.VacancyName,
                    TgLogin = hrmUser.TgLogin,
                    Fio = $"{hrmUser.Srurname} {hrmUser.Name} {hrmUser.LastName}"
                })
                .ToList(),
            Comments = hrmUser.UserComments.Select(x => new CommentCommonModel
            {
                VacancyId = x.VacancyId,
                Comment = x.Comment,
                StageId = x.StageId,
                Id = x.Id,
                CreatedAt = x.CreatedAt
            }).ToList()
        };
    }

    public Task<List<UserCommentModel>> GetCandidateComments(Guid candidateId)
    {
        throw new NotImplementedException();
    }

    public async Task<CandidateDetailModel> CreateCandidate(CandidateCreateModel model)
    {
        var practiceVacancy = await _context.PracticeVacancies.FirstOrDefaultAsync(x => x.Id == model.VacancyId);
        var vacancy = await _context.Vacancies.FirstOrDefaultAsync(x => x.Id == model.VacancyId);
        if (practiceVacancy == null && vacancy == null)
            throw new ArgumentException("Вакансии не существует");

        var newUser = new HrmUser
        {
            Id = Guid.NewGuid(),
            Name = model.FirstName,
            Srurname = model.Surname,
            LastName = model.SecondName,
            Email = model.Email,
            Gender = model.Gender,
            IsActive = true,
            TgLogin = model.TgLogn,
        };
        return await GetDetailCandidate(newUser.Id);
    }

    public async Task CreateComment(CreateCommentRequest commentRequest)
    {
        var candidate = await _context.HrmUsers.FirstOrDefaultAsync(x => x.Id == commentRequest.CandidateId);
        candidate.AssertFound();
        var comment = new UserComment
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            HrmUserId = candidate.Id,
            HrmUser = candidate,
            Comment = commentRequest.Comment
        };
        await _context.UserComments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<CandidateDetailModel> AddToPracticeVacancy(PracticeRequestCreate practiceRequestCreate)
    {
        var vacancy =
            await _context.PracticeVacancies.FirstOrDefaultAsync(x => x.Id == practiceRequestCreate.PracticeVacancyId);
        var candidate = await _context.HrmUsers.FirstOrDefaultAsync(x => x.Id == practiceRequestCreate.HrmUserId);

        vacancy.AssertFound();
        candidate.AssertFound();

        if (!vacancy.IsActive) throw new ArgumentException("Вакансия не активна");

        var startStage = await _context.KanbanBoards
            .Where(x => x.VacancyId == vacancy.Id)
            .SelectMany(x => x.Stages)
            .Where(x => x.Position == 0)
            .Include(x => x.StageCards)
            .FirstOrDefaultAsync();

        var stageCard = new StageCard
        {
            Id = Guid.NewGuid(),
            HrmUserId = candidate.Id,
            StageId = startStage.Id,
            Stage = startStage,
            DateCreated = DateTimeOffset.UtcNow,
            HrmUser = candidate,
        };
        var practiceRequest = new PracticeRequest
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            HrmUserId = practiceRequestCreate.HrmUserId,
            TaskUrl = practiceRequestCreate.TaskUrl,
            Fio = $"{candidate.Srurname} {candidate.Name} {candidate.LastName}",
            About = practiceRequestCreate.About,
            TgLogin = practiceRequestCreate.TgLogin,
            PracticeVacancyId = vacancy.Id,
            PracticeVacancy = vacancy
        };

        await _context.AddAsync(stageCard);
        startStage.StageCards.Add(stageCard);
        await _context.AddAsync(practiceRequest);
        
        await _context.SaveChangesAsync();
        return await GetDetailCandidate(practiceRequestCreate.HrmUserId);
    }
}

public class CandidateDetailModel
{
    public Guid HrmUserId { get; set; }
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string Surname { get; set; } = null!;
    public string? PhotoUrl { get; set; }
    public string Email { get; set; }
    public string TgLogn { get; set; }
    public string? SecondName { get; set; }
    public double? Salary { get; set; }
    public double WorkYears { get; set; }
    public CandidateEventModel Event { get; set; }
    [Required] public List<PracticeCandidate> PracticeCandidates { get; set; }
    [Required] public List<CommentCommonModel> Comments { get; set; }
}

public class CommentCommonModel
{
    public Guid Id { get; set; }
    public string Comment { get; set; }
    public Guid? StageId { get; set; }
    public Guid? VacancyId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class CreateCommentRequest
{
    public Guid CandidateId { get; set; }
    public string Comment { get; set; }
}

public class CandidateCreateModel
{
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string Surname { get; set; } = null!;
    public string? SecondName { get; set; }
    public string? Email { get; set; }
    public string? TgLogn { get; set; }

    public Guid VacancyId { get; set; }
    public Gender Gender { get; set; }

    public PracticeRequestCreate PracticeRequest { get; set; }
}

public class VacancyRequestCreate
{
    public bool IsActive { get; set; }
    public string About { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public double Salary { get; set; }

    public HrmUser HrmUser { get; set; }
    public double? WorkYears { get; set; }

    public List<JobExperience> JobExperiences { get; set; }
}

public class PracticeRequestCreate
{
    public string TgLogin { get; set; }
    public string TaskUrl { get; set; }
    public bool IsApproved { get; set; }
    public string About { get; set; }
    public Guid HrmUserId { get; set; }
    public Guid PracticeVacancyId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}