using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Ordering.Features.Commands.AddOrderItem;

public class AddOrderItemCommand : CommandResult
{
    public AddOrderItemCommand(Guid OrderId, string ProductName, decimal ProductPrice, int Quantity)
    {
        this.OrderId = OrderId;
        this.ProductName = ProductName;
        this.ProductPrice = ProductPrice;
        this.Quantity = Quantity;
    }

    public Guid OrderId { get; init; }
    public string ProductName { get; init; }
    public decimal ProductPrice { get; init; }
    public int Quantity { get; init; }

    public void Deconstruct(out Guid OrderId, out string ProductName, out decimal ProductPrice, out int Quantity)
    {
        OrderId = this.OrderId;
        ProductName = this.ProductName;
        ProductPrice = this.ProductPrice;
        Quantity = this.Quantity;
    }
}