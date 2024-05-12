using System.Globalization;
using Astrum.Recruting.Domain.Aggregates.Practice;
using Astrum.Recruting.Persistence;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Recruting.Application.Service;

public class PracticeDataCollectorService
{
    private readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    private const string ApplicationName = "astrumpractice";
    private const string spreadsheetId = "1CAGxEbf3SYbLrMCsACtwEe9xPo1oEgcucXvD6xcuueM";
    private readonly SheetsService _service;
    private readonly RecrutingDbContext _context;

    public PracticeDataCollectorService(RecrutingDbContext context)
    {
        _context = context;
        GoogleCredential credential;

        using (var stream =
               new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
        }

        _service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }

    public async Task<IEnumerable<PracticeVacancy>> GetPracticeVacancies()
    {
        const string range = "'Лист1'!A2:E";
        var request = _service.Spreadsheets.Values.Get(spreadsheetId, range);
        var response = (await request.ExecuteAsync())
            .Values.Where(x => x.Count == 5);

        return response
            .Select(x => new PracticeVacancy
            {
                Name = x[0].ToString(),
                MaxParticipantsCount = int.Parse(x[1].ToString()),
                IsActive = true,
                PracticeSkills = x[2].ToString().Split(",").Select(skill => new PracticeSkill
                {
                    Name = skill,
                }).ToList(),
                Responsibilities = x[3].ToString().Split(",").Select(skill => new Responsibility
                {
                    Name = skill,
                }).ToList(),
                TaskUrl = x[4].ToString()
            })
            .ToList();
    }

    public async Task<IEnumerable<PracticeRequest>> GetPracticeRequests()
    {
        const string range = "Youngling!A:F";
        var request = _service.Spreadsheets.Values.Get(spreadsheetId, range);
        var response = (await request.ExecuteAsync())
            .Values
            .Where(x => x.Count > 0)
            .Select(x => new
            {
                CreatedAt = DateTime.Parse(x[0].ToString()),
                Fio = x[1].ToString(),
                TgLink = x[2].ToString(),
                DirectionName = x[3].ToString(),
                ResultUrl = x[4].ToString(),
                About = x[5].ToString(),
            })
            .ToList();

        var practiceDirections = response
            .Select(x => x.DirectionName)
            .Distinct()
            .ToList();
        var practiceVacancies = await _context.PracticeVacancies
            .Where(x => practiceDirections.Contains(x.Name))
            .ToListAsync();

        return response
            .Select(x => new PracticeRequest
            {
                About = x.About,
                Fio = x.Fio,
                TaskUrl = x.ResultUrl,
                CreatedAt = x.CreatedAt,
                TgLogin = x.TgLink,
                PracticeVacancy = practiceVacancies.First(v => v.Name.Equals(x.DirectionName)),
                PracticeVacancyId = practiceVacancies.First(v => v.Name.Equals(x.DirectionName)).Id
            })
            .ToList();
    }
}