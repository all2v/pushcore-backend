using System.Globalization;
using Astrum.CodeRev.Domain.Records;
using Astrum.CodeRev.Domain.Repositories;
using Astrum.CodeRev.Persistence.MongoSettings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Astrum.CodeRev.Persistence.Repositories;

public class TaskRecordRepository : ITaskRecordRepository
{
    private readonly IMongoCollection<TaskRecord> _taskRecords;

    public TaskRecordRepository(TaskRecordsTrackerDataBaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DataBaseName);
        _taskRecords = database.GetCollection<TaskRecord>(settings.TaskRecordsCollectionName);
    }

    public async Task<TaskRecord?> Get(Guid taskSolutionId)
    {
        var records = await _taskRecords.FindAsync(
            record =>
                record.TaskSolutionId == taskSolutionId);
        
        return records.FirstOrDefault();
    }
    
    public async Task Save(TaskRecord request)
    {
        var record = await Get(request.TaskSolutionId);
        if (record == null)
            await _taskRecords.InsertOneAsync(request);
        else
            await Update(new TaskRecord
            {
                TaskSolutionId = record.TaskSolutionId,
                Id = record.Id,
                Code = request.Code,
                RecordChunks = record.RecordChunks.Concat(request.RecordChunks).ToList()
            });
    }

    private async Task Update(TaskRecord request)
    {
        var filter = Builders<TaskRecord>.Filter.Eq(x => x.TaskSolutionId, request.TaskSolutionId);
        var update = Builders<TaskRecord>.Update.Set(x => x.RecordChunks, request.RecordChunks)
            .Set(x => x.Code, request.Code);
        await _taskRecords.UpdateOneAsync(filter, update);
    }
}