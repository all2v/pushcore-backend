using Astrum.Storage.Application.ViewModels;

namespace Astrum.Market.ViewModels;

//TODO: разбить на request и response
public class MarketProductFormRequest
{
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
    public int? Remain { get; set; }
    public bool? IsAvailable { get; set; }
    public bool? IsInfinite { get; set; }
    public MinimizedFileForm? Image { get; set; }
}

public class MarketProductFormResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
    public int? Remain { get; set; }
    public bool? IsAvailable { get; set; }
    public bool? IsInfinite { get; set; }
    public string? CoverUrl { get; set; }
    public Guid? CoverImageId { get; set; }
}