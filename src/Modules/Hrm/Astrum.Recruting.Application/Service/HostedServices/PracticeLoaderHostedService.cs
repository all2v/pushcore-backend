using Astrum.Recruting.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Astrum.Recruting.Application.Service.HostedServices;

public class PracticeLoaderHostedService : IHostedService
{
    private readonly RecrutingDbContext _context;
    private readonly PracticeLoaderService _practiceLoaderService;
    private readonly IKanbanService _kanbanService;

    public PracticeLoaderHostedService(IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope().ServiceProvider;
        _practiceLoaderService = scope.GetRequiredService<PracticeLoaderService>();
        _kanbanService = scope.GetRequiredService<IKanbanService>();
        _context = scope.GetRequiredService<RecrutingDbContext>();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = CollectPracticeVacanciesAndRequests(cancellationToken);
        return Task.CompletedTask;
    }

    //TODO ДОБАВИТЬ СОЗДАНИЕ КАНБАН ДОСКИ ДЛЯ КАЖДОГО НАПРАВЛЕНИЯ ПРАКТИКИ
    private async Task CollectPracticeVacanciesAndRequests(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var vacancies = await _practiceLoaderService.CollectNewPracticeDirections();
            await _context.SaveChangesAsync(cancellationToken);

            var personsRequests = await _practiceLoaderService.GetGroupedPersonsRequests();
            await _practiceLoaderService.ProcessPersonRequests(personsRequests);

            await _context.SaveChangesAsync(cancellationToken);
            foreach (var vacancy in vacancies)
            {
                await _kanbanService.CreateDefaultBoardForVacancy(vacancy.Id);
            }

            await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}