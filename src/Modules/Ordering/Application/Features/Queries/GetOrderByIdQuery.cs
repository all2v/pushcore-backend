using Astrum.Ordering.ReadModels;
using Astrum.Ordering.Repositories;
using Astrum.Ordering.Specifications.Order;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Ordering.Features.Queries;

public class GetOrderByIdQuery : QueryResult<OrderReadModel>
{
    public GetOrderByIdQuery(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }
}

public class GetOrderByIdQueryHandler : QueryResultHandler<GetOrderByIdQuery, OrderReadModel>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IMapper mapper, IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public override async Task<Result<OrderReadModel>> Handle(GetOrderByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var spec = new GetOrderByIdSpec(query.OrderId);
        var order = await _orderRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (order is null)
            return Result.NotFound();

        return Result.Success(_mapper.Map<OrderReadModel>(order));
    }
}