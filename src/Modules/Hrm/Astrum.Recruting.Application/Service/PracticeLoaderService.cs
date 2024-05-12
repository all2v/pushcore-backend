using Astrum.Recruting.Domain.Aggregates.Practice;
using Astrum.Recruting.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Recruting.Application.Service;

public class PracticeLoaderService
{
    private readonly RecrutingDbContext _context;
    private readonly PracticeDataCollectorService _practiceDataCollectorService;
    private readonly IHrmUserService _hrmUserService;
    private readonly IPracticeVacancyService _practiceVacancyService;

    public PracticeLoaderService(RecrutingDbContext context, PracticeDataCollectorService practiceDataCollectorService,
        IHrmUserService hrmUserService, IPracticeVacancyService practiceVacancyService)
    {
        _context = context;
        _practiceDataCollectorService = practiceDataCollectorService;
        _hrmUserService = hrmUserService;
        _practiceVacancyService = practiceVacancyService;
    }

    public async Task<IEnumerable<PracticeVacancy>> CollectNewPracticeDirections()
    {
        //получаем активные вакансии
        var existedVacancies = await _context.PracticeVacancies
            .Where(x => x.IsActive == true)
            .ToListAsync();
        //смотрим есть ли направления у кого истек дедлайн и если да деактивируем их
        if (existedVacancies.Any(x => DateTimeOffset.UtcNow.Date >= x.ExpiredAt.Date))
        {
            await _practiceVacancyService.SetPracticesWithDeadlineNotActive(existedVacancies);
        }

        var vacancies = await _practiceDataCollectorService.GetPracticeVacancies();
        var directions = vacancies.Select(x => x.Name).ToList();

        //выбираем активные вакансии
        var existDirections = existedVacancies.Where(x => x.IsActive).Select(x => x.Name).ToList();
        var newPracticeDirections = new List<PracticeVacancy>();
        foreach (var direction in directions.Except(existDirections))
        {
            var newVacancy = vacancies.First(x => x.Name.Equals(direction));
            newPracticeDirections.Add(newVacancy);
            await _practiceVacancyService.AddPracticeVacancy(newVacancy);
        }

        return newPracticeDirections;
    }

    public async Task<Dictionary<string, List<PracticeRequest>>> GetGroupedPersonsRequests()
    {
        //получаем заявки на практику
        var practiceRequests = await _practiceDataCollectorService.GetPracticeRequests();

        //группируем заявки по персоне
        var requestByPerson = practiceRequests
            .GroupBy(x => x.TgLogin)
            .ToDictionary(x => x.Key, x => x.Select(x => x).ToList());

        return requestByPerson;
    }

    public async Task ProcessPersonRequests(Dictionary<string, List<PracticeRequest>> requestByPerson)
    {
        foreach (var personRequest in requestByPerson)
        {
            //получаем ранние заявки по персоне
            var dbRequest = await
                _context.PracticeRequests
                    .Where(x => x.TgLogin.Equals(personRequest.Key))
                    .ToDictionaryAsync(x => (x.TgLogin, x.PracticeVacancyId, x.CreatedAt), x => x);

            //если в системе его нет, то создаем аккаунт иначе получаем его
            var hrmUser = await _hrmUserService.GetHrmUserByTgLogin(personRequest.Key)
                          ?? await _hrmUserService.CreateHrmUserFromPracticeRequest(personRequest.Value.Last());

            //если заявок нет, то добавляем все пришедшие
            if (dbRequest.Count == 0)
            {
                personRequest.Value.ForEach(x => x.HrmUserId = hrmUser.Id);
                await _context.AddRangeAsync(personRequest.Value);
            }
            else
            {
                //иначе группируем их по направлениям и дате, и проверяем на наличие, если заявки еще нет в базе то добавляем иначе пропускаем
                var tmp = personRequest
                    .Value.ToDictionary(x =>
                    (
                        x.TgLogin, x.PracticeVacancyId, x.CreatedAt
                    ), x => x);

                var newRequests = new List<PracticeRequest>();
                foreach (var i in tmp)
                {
                    if (!dbRequest.ContainsKey(i.Key))
                        newRequests.Add(i.Value);
                }

                newRequests.ForEach(x => x.HrmUserId = hrmUser.Id);

                await _context.AddRangeAsync(newRequests);
            }
        }
    }
}