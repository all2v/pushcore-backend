using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using Astrum.Storage.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Storage.Controllers;

/// <summary>
///     Storage endpoints
/// </summary>
//[Authorize(Roles = "admin")]
[Area("StorageTest")]
[Route("[controller]")]
public class StorageController : ApiBaseController
{
    private readonly IFileStorage _fileStorage;

    /// <summary>
    ///     StorageConroller initializer
    /// </summary>
    public StorageController(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }

    /// <summary>
    ///     Add file to storage
    /// </summary>
    //[HttpGet("/api/storage/file")]
    //public async Task<Result<Stream>> GetFile(Guid id)
    //{
    //    var file = await _fileStorage.GetFile(id);
    //    if(file != null)
    //        return Result.Success(file.FileStream);
    //    return Result.Error(new string[] {"Unable to get file!"});
    //}

    /// <summary>
    ///     Get file link
    /// </summary>
    [HttpGet("/api/storage/url")]
    public async Task<Result<string>> GetFileLink(Guid id)
    {
        var fileUrl = await _fileStorage.GetFileUrl(id);
        if (fileUrl != null)
            return Result.Success(fileUrl);
        return Result.Error(new string[] { "Unable to get file!" });
    }

    /// <summary>
    ///     Add file to storage
    /// </summary>
    [HttpPost]
    public async Task<Result> UploadFile(FileForm file)
    {

        await _fileStorage.UploadFile(file);
        return Result.Success();
    }
}