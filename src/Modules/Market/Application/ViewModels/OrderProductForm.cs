namespace Astrum.Market.ViewModels;

public class OrderProductFormRequest
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Amount { get; set; }
    public OrderStatus Status { get; set; }
}

public class OrderProductFormResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Amount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Ordered;
    public MarketProductFormResponse? Product { get; set; }
}