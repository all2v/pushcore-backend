using Astrum.Storage.Aggregates;
using Astrum.Storage.Entities;
using AutoMapper;

namespace Astrum.Storage.Mappings;

public class StorageProfile : Profile
{
    public StorageProfile()
    {
        CreateMap<FileEntity, StorageFile>();
        CreateMap<StorageFile, FileEntity>();
    }
}