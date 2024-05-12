using Astrum.Ordering.Aggregates;
using Astrum.Ordering.Repositories;
using Astrum.SharedLib.Common.CQS.Implementations;
using Result = Astrum.SharedLib.Common.Results.Result;

namespace Astrum.Ordering.Features.Commands;

public class CreateNewOrderCommand : CommandResult
{
    public CreateNewOrderCommand(string TrackingNumber)
    {
        this.TrackingNumber = TrackingNumber;
    }

    public string TrackingNumber { get; init; }

    public void Deconstruct(out string TrackingNumber)
    {
        TrackingNumber = this.TrackingNumber;
    }
}

/// <summary>
///     Create New Order Command Handler
/// </summary>
public class CreateNewOrderCommandHandler : CommandResultHandler<CreateNewOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public CreateNewOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public override async Task<Result> Handle(CreateNewOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var order = new Order(command.TrackingNumber);
        await _orderRepository.AddAsync(order);
        await _orderRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}