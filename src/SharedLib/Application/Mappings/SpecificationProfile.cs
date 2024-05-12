using System.Linq.Expressions;
using Astrum.Core.Domain.Specifications;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;

namespace Astrum.Core.Application.Mappings;

public class SpecificationProfile : Profile
{
    public SpecificationProfile()
    {
        CreateMap(typeof(Specification<>), typeof(Specification<>))
            .ConstructUsingServiceLocator()
            .ConvertUsing(typeof(SpecificationTypeConverter<,>));

        CreateMap(typeof(ISpecification<>), typeof(ISpecification<>))
            .ConstructUsingServiceLocator()
            .ConvertUsing(typeof(SpecificationInterfaceTypeConverter<,>));
    }
}

public class SpecificationTypeConverter<TSource, TDestination> : ITypeConverter<Specification<TSource>,
    Specification<TDestination>>
{
    private readonly IMapper _mapper;

    public SpecificationTypeConverter(IMapper mapper)
    {
        _mapper = mapper;
    }

    #region ITypeConverter<Specification<TSource>,Specification<TDestination>> Members

    public Specification<TDestination> Convert(Specification<TSource> source, Specification<TDestination> destination,
        ResolutionContext context)
    {
        var expression = source.IsSatisfiedBy();
        var mappedExpression = _mapper.MapExpression<Expression<Func<TDestination, bool>>>(expression);
        return new Specification<TDestination>(mappedExpression);
    }

    #endregion
}

public class SpecificationInterfaceTypeConverter<TSource, TDestination> : ITypeConverter<ISpecification<TSource>,
    ISpecification<TDestination>>
{
    private readonly IMapper _mapper;

    public SpecificationInterfaceTypeConverter(IMapper mapper)
    {
        _mapper = mapper;
    }

    #region ITypeConverter<ISpecification<TSource>,ISpecification<TDestination>> Members

    public ISpecification<TDestination> Convert(ISpecification<TSource> source,
        ISpecification<TDestination> destination, ResolutionContext context)
    {
        var expression = source.IsSatisfiedBy();
        var mappedExpression = _mapper.MapExpression<Expression<Func<TDestination, bool>>>(expression);
        var spec = new Specification<TDestination>(mappedExpression);
        spec.IncludeStrings.AddRange(source.IncludeStrings);
        spec.Includes.AddRange(source.Includes.Select(e => _mapper.MapExpression<Expression<Func<TDestination, object>>>(e)));
        return spec;
    }

    #endregion
}