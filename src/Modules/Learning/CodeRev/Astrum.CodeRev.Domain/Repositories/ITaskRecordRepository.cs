using Astrum.CodeRev.Domain.Records;

namespace Astrum.CodeRev.Domain.Repositories;

public interface ITaskRecordRepository
{
    public Task<TaskRecord?> Get(Guid taskSolutionId);
    public Task Save(TaskRecord request);
   // Task<List<RecordChunk>> GetRecords(Guid taskSolutionId, int offset, int limit);
}