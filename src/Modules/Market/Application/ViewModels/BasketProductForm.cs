namespace Astrum.Market.ViewModels;

public class BasketProductForm
{
    public Guid Id { get; set; }
    public Guid BasketId { get; set; }
    public Guid ProductId { get; set; }
    public int Amount { get; set; }
    public MarketProductFormResponse? Product { get; set; }
}