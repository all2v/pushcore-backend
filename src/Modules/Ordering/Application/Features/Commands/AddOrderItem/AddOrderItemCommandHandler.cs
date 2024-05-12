using Astrum.Ordering.Repositories;
using Astrum.Ordering.Specifications.Order;
using Astrum.SharedLib.Common.CQS.Implementations;
using Result = Astrum.SharedLib.Common.Results.Result;

namespace Astrum.Ordering.Features.Commands.AddOrderItem;

/// <summary>
///     Add Order Item Command Handler
/// </summary>
public class AddOrderItemCommandHandler : CommandResultHandler<AddOrderItemCommand>
{
    private readonly IOrderRepository _orderRepository;

    public AddOrderItemCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }


    public override async Task<Result> Handle(AddOrderItemCommand command,
        CancellationToken cancellationToken = default)
    {
        var spec = new GetOrderByIdSpec(command.OrderId);
        var order = await _orderRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (order is null)
            return Result.NotFound();
        order.AddOrderItem(command.ProductName, command.ProductPrice, command.Quantity);
        await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}