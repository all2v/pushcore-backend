using Astrum.Appeal.Aggregates;
using Astrum.Appeal.ViewModels;
using AutoMapper;

namespace Astrum.Appeal.Mappings;

public class AppealAppealFormConverter : ITypeConverter<Aggregates.Appeal, AppealForm>
{
    private readonly IMapper _mapper;

    public AppealAppealFormConverter(IMapper mapper)
    {
        _mapper = mapper;
    }

    #region ITypeConverter<Appeal,AppealForm> Members

    public AppealForm Convert(Aggregates.Appeal source, AppealForm destination, ResolutionContext context)
    {
        destination ??= new AppealForm();
        var body = _mapper.Map<AppealFormData>(source);
        destination.Body = body;
        destination.Id = source.Id;
        return destination;
    }

    #endregion
}

public class AppealFormAppealConverter : ITypeConverter<AppealForm, Aggregates.Appeal>
{
    private readonly IMapper _mapper;

    public AppealFormAppealConverter(IMapper mapper)
    {
        _mapper = mapper;
    }

    #region ITypeConverter<AppealForm,Appeal> Members

    public Aggregates.Appeal Convert(AppealForm source, Aggregates.Appeal destination,
        ResolutionContext context)
    {
        destination ??= new Aggregates.Appeal();
        _mapper.Map(source.Body, destination);
        destination.Id = source.Id;
        return destination;
    }

    #endregion
}

public class AppealFormAppealFormResponseConverter : ITypeConverter<AppealForm, AppealFormResponse>
{
    private readonly IMapper _mapper;

    public AppealFormAppealFormResponseConverter(IMapper mapper)
    {
        _mapper = mapper;
    }

    #region ITypeConverter<AppealForm,AppealFormResponse> Members

    public AppealFormResponse Convert(AppealForm source, AppealFormResponse destination, ResolutionContext context)
    {
        destination ??= new AppealFormResponse();
        _mapper.Map(source.Body, destination);
        destination.Id = source.Id;
        return destination;
    }

    #endregion
}

public class AppealConverter : ITypeConverter<Aggregates.Appeal, AppealForm>
{
    private readonly IMapper _mapper;

    public AppealConverter(IMapper mapper)
    {
        _mapper = mapper;
    }

    #region ITypeConverter<Appeal,AppealForm> Members

    public AppealForm Convert(Aggregates.Appeal source, AppealForm destination, ResolutionContext context)
    {
        destination ??= new AppealForm();
        _mapper.Map(source, destination.Body);
        destination.Id = source.Id;
        return destination;
    }

    #endregion
}

public class AppealProfile : Profile
{
    public AppealProfile()
    {
        CreateMap<AppealCategory, AppealCategoryForm>().ReverseMap();
        CreateMap<AppealFormResponse, Aggregates.Appeal>().ReverseMap();

        CreateMap<Aggregates.Appeal, AppealForm>()
            .ConstructUsingServiceLocator()
            .ConvertUsing(typeof(AppealAppealFormConverter));
        CreateMap<AppealForm, Aggregates.Appeal>()
            .ConstructUsingServiceLocator()
            .ConvertUsing<AppealFormAppealConverter>();
        CreateMap<AppealFormData, Aggregates.Appeal>()
            .ReverseMap();

        CreateMap<AppealFormData, AppealFormResponse>();
        CreateMap<AppealForm, AppealFormResponse>()
            .ConstructUsingServiceLocator()
            .ConvertUsing<AppealFormAppealFormResponseConverter>();
    }
}