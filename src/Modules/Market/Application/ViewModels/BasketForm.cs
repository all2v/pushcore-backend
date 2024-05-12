namespace Astrum.Market.ViewModels;

public class BasketForm
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? Owner { get; set; }
    public List<BasketProductForm>? Products { get; set; }
}