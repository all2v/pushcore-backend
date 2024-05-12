using Astrum.Storage.Aggregates;
using Astrum.Storage.Application.ViewModels;
using Astrum.Storage.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Astrum.Storage.Mappings;

public class StorageProfile : Profile
{
    public StorageProfile()
    {
        CreateMap<StorageFile, FileForm>();
        CreateMap<FileForm, StorageFile>();

        CreateMap<StorageFile, MinimizedFileForm>();
        CreateMap<MinimizedFileForm, StorageFile>();

        CreateMap<IFormFile, FileForm>();

        CreateMap<FileForm, MinimizedFileForm>().ForMember(dest => dest.FileBytes, opts => opts.Ignore());
        CreateMap<MinimizedFileForm, FileForm>().ForMember(dest => dest.FileBytes, opts => opts.Ignore());
            //.ForMember(dest => dest.FileBytes, opts => opts.MapFrom(src => {
            //    byte[] imageData = null;
            //    using (var binaryReader = new BinaryReader(src.File.OpenReadStream()))
            //    {
            //        imageData = binaryReader.ReadBytes((int)src.File.Length);
            //    }
            //    return imageData;
            //}));
    }
}