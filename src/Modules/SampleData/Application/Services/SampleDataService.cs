using Astrum.Infrastructure;
using Astrum.SampleData.Repositories;
using Astrum.Storage.Services;

namespace Astrum.SampleData.Services;

public class SampleDataService : ISampleDataService
{
    private readonly ISampleDataRepository _repository;
    private readonly ISampleContentService _sampleContentService;

    public SampleDataService(ISampleDataRepository repository,
        ISampleContentService sampleContentService)
    {
        _repository = repository;
        _sampleContentService = sampleContentService;
    }
    public async Task ResetToSampleData()
    {
        await foreach(var file in _sampleContentService.GetFiles())
        //var srcPath = Path.Combine(GlobalConfiguration.ContentRootPath, @"..\..\");
        //var filePath = Path.Combine(srcPath, "Modules", "SampleData", "Domain", "SampleContent","Articles", "ResetToSampleData.sql");
        //var lines = await File.ReadAllLinesAsync(filePath);
        //var commands = _repository.PostgresCommands(lines);
        _repository.RunCommand(file);
    }
}
