using Astrum.SampleData.Aggregates;
using Astrum.SampleData.Models;
using AutoMapper;

namespace Astrum.SampleData.Mappings;

internal class SampleContentMapping : Profile
{
    public SampleContentMapping()
    {
        CreateMap<SampleContentDTO, SampleContentFile>();
        CreateMap<SampleContentFile, SampleContentView>();
        
    }
}
