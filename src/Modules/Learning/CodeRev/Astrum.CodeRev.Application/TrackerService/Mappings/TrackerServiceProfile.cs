using Astrum.CodeRev.Application.TrackerService.ViewModels.DTO;
using Astrum.CodeRev.Domain.Actions;
using Astrum.CodeRev.Domain.Primitives;
using Astrum.CodeRev.Domain.Records;
using AutoMapper;

namespace Astrum.CodeRev.Application.TrackerService.Mappings;

public class TrackerServiceProfile : Profile
{
    public TrackerServiceProfile()
    {
        CreateMap<RecordChunkDto, RecordChunk>().ReverseMap();
        CreateMap<TaskRecord, TaskRecordDto>().ReverseMap();
        CreateMap<Record, RecordDto>().ReverseMap();
        CreateMap<TimelinePrimitive, TimelineDto>().ReverseMap();
        CreateMap<Operation, OperationDto>().ReverseMap();
        CreateMap<OperationTypePrimitive, OperationTypeDto>().ReverseMap();
        CreateMap<PeriodPrimitive, PeriodDto>().ReverseMap();
        CreateMap<IndexPrimitive, IndexDto>().ReverseMap();
        CreateMap<ValuePrimitive, ValueDto>().ReverseMap();
        CreateMap<RemoveAction, RemoveDto>().ReverseMap();
        CreateMap<SelectAction, SelectDto>().ReverseMap();
        
    }
}