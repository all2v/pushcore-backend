using System.ComponentModel.DataAnnotations;
using Astrum.Recruting.Application.Shared;
using Astrum.Recruting.Domain.Aggregates.Vacancy;
using Astrum.Recruting.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Recruting.Application.Service;

public interface IVacancyService
{
    public Task<CreateVacancyRequest> CreateOrUpdateVacancy(Guid userId, CreateVacancyRequest vacancyRequest);
    public Task<CreateVacancyRequest> MoveVacancyToArchive(Guid vacancyId);
    public Task<CreateVacancyRequest> MoveVacancyToActive(Guid vacancyId, DateTimeOffset newDeadline);
    public Task<DetailVacancyResponse> GetDetailVacancyInfo(Guid vacancyId);
    public Task<List<VacancyCommonModel>> GetAllVacancies(bool isActive);
    public Task<List<IdNameModel<long>>> GetRoles();
    public Task<List<IdNameModel<long>>> GetWorkTypes();
    public Task<List<IdNameModel<long>>> GetVacancyTypes();
}

public class VacancyService : IVacancyService
{
    private readonly RecrutingDbContext _context;

    public VacancyService(RecrutingDbContext context)
    {
        this._context = context;
    }

    // TODO добавить потом отправку на hh.ru
    public async Task<CreateVacancyRequest> CreateOrUpdateVacancy(Guid userId, CreateVacancyRequest vacancyRequest)
    {
        var area = await _context.VacancyAreas.FirstOrDefaultAsync(x => x.Id == vacancyRequest.AreaId);
        var roles = await _context.VacancyRoles.Where(x => vacancyRequest.RoleIds.Contains(x.Id)).ToListAsync();
        var workTypes = await _context.WorkTypes.Where(x => vacancyRequest.WorkTypeIds.Contains(x.Id)).ToListAsync();
        var vacancyTypes = await _context.VacancyTypes.Where(x => vacancyRequest.VacancyTypeIds.Contains(x.Id))
            .ToListAsync();
        Vacancy vacancy;
        if (vacancyRequest.Id.HasValue)
        {
            vacancy = await _context.Vacancies.FirstOrDefaultAsync(x => x.Id == vacancyRequest.Id);
            if (vacancy == null)
                throw new ArgumentException("Вакансии не существует");

            vacancy.CreatedAt = DateTimeOffset.UtcNow;
            vacancy.Name = vacancyRequest.Name;
            vacancy.Description = vacancyRequest.Description;
            vacancy.City = "Екатеринбург";
            vacancy.Salary = vacancyRequest.Salary;
            vacancy.IsActive = true;
            vacancy.ExpiredAt = vacancyRequest.Deadline;
            vacancy.VacancyAreas = new List<VacancyArea> { area };
            vacancy.VacancyRoles = roles;
            vacancy.WorkTypes = workTypes;
            vacancy.VacancyTypes = vacancyTypes;
            vacancy.IsActive = vacancyRequest.IsActive;
        }
        else
        {
            vacancy = new Vacancy
            {
                CreatedAt = DateTimeOffset.UtcNow,
                Name = vacancyRequest.Name,
                Description = vacancyRequest.Description,
                City = "Екатеринбург",
                Salary = vacancyRequest.Salary,
                IsActive = true,
                ExpiredAt = vacancyRequest.Deadline,
                VacancyAreas = new List<VacancyArea> { area },
                VacancyRoles = roles,
                WorkTypes = workTypes,
                VacancyTypes = vacancyTypes,
            };
            await _context.AddAsync(vacancy);
        }

        await _context.SaveChangesAsync();
        vacancyRequest.Id = vacancy.Id;
        return vacancyRequest;
    }

    public async Task<CreateVacancyRequest> MoveVacancyToArchive(Guid vacancyId)
    {
        var vacancy = await _context.Vacancies
            .Include(x => x.VacancyRoles)
            .Include(x => x.VacancyTypes)
            .Include(x => x.WorkTypes)
            .Include(x => x.VacancyAreas)
            .FirstOrDefaultAsync(x => x.Id == vacancyId);

        vacancy.AssertFound();
        vacancy.IsActive = false;
        await _context.SaveChangesAsync();
        return new CreateVacancyRequest
        {
            Name = vacancy.Name,
            Description = vacancy.Description,
            Salary = vacancy.Salary,
            IsActive = false,
            Deadline = vacancy.ExpiredAt,
            AreaId = vacancy.VacancyAreas.First().Id,
            RoleIds = vacancy.VacancyRoles.Select(x => x.Id).ToList(),
            WorkTypeIds = vacancy.WorkTypes.Select(x => x.Id).ToList(),
            VacancyTypeIds = vacancy.VacancyTypes.Select(x => x.Id).ToList()
        };
    }

    public async Task<CreateVacancyRequest> MoveVacancyToActive(Guid vacancyId, DateTimeOffset newDeadline)
    {
        var vacancy = await _context.Vacancies
            .Include(x => x.VacancyRoles)
            .Include(x => x.VacancyTypes)
            .Include(x => x.WorkTypes)
            .Include(x => x.VacancyAreas)
            .FirstOrDefaultAsync(x => x.Id == vacancyId);

        vacancy.AssertFound();
        vacancy.IsActive = true;
        vacancy.ExpiredAt = newDeadline;
        await _context.SaveChangesAsync();
        return new CreateVacancyRequest
        {
            Name = vacancy.Name,
            Description = vacancy.Description,
            Salary = vacancy.Salary,
            IsActive = false,
            Deadline = vacancy.ExpiredAt,
            AreaId = vacancy.VacancyAreas.First().Id,
            RoleIds = vacancy.VacancyRoles.Select(x => x.Id).ToList(),
            WorkTypeIds = vacancy.WorkTypes.Select(x => x.Id).ToList(),
            VacancyTypeIds = vacancy.VacancyTypes.Select(x => x.Id).ToList()
        };
    }

    public Task<DetailVacancyResponse> GetDetailVacancyInfo(Guid vacancyId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<VacancyCommonModel>> GetAllVacancies(bool isActive)
    {
        return await _context.Vacancies
            .Where(x => x.IsActive == isActive)
            .Select(x => new VacancyCommonModel
            {
                Title = x.Name,
                Deadline = x.ExpiredAt,
                CreatedAt = x.CreatedAt,
                CandidateCount = 0,
            }).ToListAsync();
    }

    public async Task<List<IdNameModel<long>>> GetRoles()
    {
        return await _context.VacancyRoles.Select(x => new IdNameModel<long>
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
    }

    public async Task<List<IdNameModel<long>>> GetWorkTypes()
    {
        return await _context.WorkTypes.Select(x => new IdNameModel<long>
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
    }

    public async Task<List<IdNameModel<long>>> GetVacancyTypes()
    {
        return await _context.VacancyTypes.Select(x => new IdNameModel<long>
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
    }
}

public class VacancyCommonModel
{
    public string Title { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset Deadline { get; set; }
    public int CandidateCount { get; set; }
}

public class VacancyResponse
{
    public Guid Id { get; set; }
    [Required] public IdNameModel<Guid> Area { get; set; }
    [Required] public IdNameModel<Guid> BillingType { get; set; }
    [Required] public IdNameModel<Guid> Type { get; set; }
    [Required] public string Name { get; set; }
    [Required] public int Requests { get; set; }
    [Required] public DateTimeOffset CreatedAt { get; set; }
    [Required] public DateTimeOffset Deadline { get; set; }
}

public class VacancyRequestResponse
{
    public string Title { get; set; }
    public string Id { get; set; }
    public int Age { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Gender { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public double Salary { get; set; }
}

public class DetailVacancyResponse
{
    public Guid Id { get; set; }
    [Required] public IdNameModel<Guid> Area { get; set; }
    [Required] public IdNameModel<Guid> BillingType { get; set; }
    [Required] public IdNameModel<Guid> Type { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Description { get; set; }
    [Required] public int Requests { get; set; }
    [Required] public DateTimeOffset CreatedAt { get; set; }
    [Required] public DateTimeOffset Deadline { get; set; }
    public List<VacancyResponse> Responses { get; set; }
}

public class IdNameModel<T>
{
    public T Id { get; set; }
    public string Name { get; set; }
}

public class CreateVacancyRequest
{
    public Guid? Id { get; set; }
    [Required] public List<string> ProffestionalRoles { get; set; }
    [Required] public long AreaId { get; set; }
    [Required] public string BillingType { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string TypeId { get; set; }
    [Required] public double Salary { get; set; }
    [Required] public DateTimeOffset Deadline { get; set; }
    [Required] public List<long> RoleIds { get; set; }
    [Required] public List<long> WorkTypeIds { get; set; }
    [Required] public List<long> VacancyTypeIds { get; set; }
    [Required] public bool IsActive { get; set; }
}

public class DetailVacancyModel
{
    public Guid Id { get; set; }
    [Required] public List<string> ProffestionalRoles { get; set; }
    [Required] public long AreaId { get; set; }
    [Required] public string BillingType { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string TypeId { get; set; }
    [Required] public double Salary { get; set; }
    [Required] public DateTimeOffset Deadline { get; set; }
    [Required] public List<long> RoleIds { get; set; }
    [Required] public List<long> WorkTypeIds { get; set; }
    [Required] public List<long> VacancyTypeIds { get; set; }
    [Required] public bool IsActive { get; set; }
}