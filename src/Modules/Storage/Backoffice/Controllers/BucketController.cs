using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Storage.Controllers;

/// <summary>
///     Storage endpoints
/// </summary>
//[Authorize(Roles = "admin")]
[Area("StorageTest")]
[Route("[controller]")]
public class BucketController : ApiBaseController
{
    private readonly IFileStorage _fileStorage;

    /// <summary>
    ///     BucketController initializer
    /// </summary>
    public BucketController(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }

    /// <summary>
    ///     Get buckets
    /// </summary>
    [HttpGet]
    public async Task<Result<List<string>>> GetBuckets()
    {
        var buckets = await _fileStorage.GetBucketNames();
        return Result.Success(buckets);
    }

    /// <summary>
    ///     Add bucket to storage
    /// </summary>
    [HttpPost]
    public async Task<Result> CreateBucket(string bucket)
    {
        await _fileStorage.CreateBucket(bucket);
        return Result.Success();
    }
}