using MongoDB.Bson;

namespace Astrum.CodeRev.Domain.Records;


public class TaskRecord 
{
    public ObjectId Id { get; set; }
    public Guid TaskSolutionId { get; set; }

    public string Code { get; set; }

   public List<RecordChunk> RecordChunks { get; set; }
}