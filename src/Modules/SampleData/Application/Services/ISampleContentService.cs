using Astrum.SampleData.Aggregates;
using Astrum.SampleData.Models;
using Astrum.SampleData.Specifications;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;

namespace Astrum.SampleData.Services;

public interface ISampleContentService
{
    public Task<Result> Create(SampleContentDTO sampleContent);

    public Task<Result> Delete(Guid id);

    public Task Update();

    public Task<Result<List<SampleContentView>>> GetAll();
    public IAsyncEnumerable<string> GetFiles();
}
