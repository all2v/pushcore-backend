using System.IO;
using Astrum.SampleData.Aggregates;
using Astrum.SampleData.Models;
using Astrum.SampleData.Repositories;
using Astrum.SampleData.Specifications;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel;

namespace Astrum.SampleData.Services;

public class SampleContentService : ISampleContentService
{
    private readonly ISampleContentRepository _sampleContentRepository;
    private readonly IFileStorage _fileStorage;
    private readonly IMapper _mapper;

    public SampleContentService(ISampleContentRepository sampleContentRepository,
        IFileStorage fileStorage, IMapper mapper)
    {
        _sampleContentRepository = sampleContentRepository;
        _fileStorage = fileStorage;
        _mapper = mapper;
    }
    public async Task<Result> Create(SampleContentDTO sampleContent)
    {
        var sampleContentFile = _mapper.Map<SampleContentFile>(sampleContent);
        var uploadResult = await _fileStorage.UploadFile(sampleContent.File);
        if (!uploadResult.Success)
            return Result.Error("Upload File Error");
        sampleContentFile.FileId = uploadResult.UploadedFileId.Value;
        await _sampleContentRepository.AddAsync(sampleContentFile);
        await _sampleContentRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> Delete(Guid id)
    {
        var spec = new GetSampleContentByIdSpec(id);

        var sampleContent = await _sampleContentRepository.FirstOrDefaultAsync(spec);
        if (sampleContent == null)
            return Result.NotFound("File was not found");

        await _sampleContentRepository.DeleteAsync(sampleContent);
        await _sampleContentRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task Update()
    {

    }

    public async Task<Result<List<SampleContentView>>> GetAll()
    {
        var spec =new SampleContentOrderByNameSpec();
        var result =  await _sampleContentRepository
            .GetBy(spec)
            .Select(e=>new SampleContentView() {Id = e.Id, ContextName = e.ContextName })
            .ToListAsync();
        return Result.Success(result);
    }

    public async IAsyncEnumerable<string> GetFiles()
    {
        var spec = new SampleContentOrderByNameSpec();
        var result = await _sampleContentRepository.ListAsync(spec);
        foreach (var e in result)
        {
            var text = string.Empty;
            var file = await _fileStorage.GetFile(e.FileId);
            using (var stream = new MemoryStream(file.FileBytes))
            using (var sr = new StreamReader(stream))
            {
                text = await sr.ReadToEndAsync();
            }
            yield return text;
        }
    }

}
