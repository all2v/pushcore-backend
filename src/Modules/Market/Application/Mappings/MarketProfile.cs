using Astrum.Market.Aggregates;
using Astrum.Market.ViewModels;
using AutoMapper;

namespace Astrum.Market.Mappings;

public class MarketProfile : Profile
{
    public MarketProfile()
    {
        CreateMap<MarketProductFormRequest, MarketProduct>().ReverseMap();
        CreateMap<MarketProduct, MarketProductFormResponse>().ReverseMap();
        CreateMap<MarketOrder, MarketOrderFormRequest>().ReverseMap();
        CreateMap<MarketBasket, BasketForm>();
        CreateMap<MarketOrderFormResponse, MarketOrder>().ReverseMap();
        CreateMap<BasketForm, MarketBasket>();
        CreateMap<OrderProductFormRequest, OrderProduct>().ReverseMap();
        CreateMap<BasketProductForm, BasketProduct>();
        CreateMap<OrderProduct, OrderProductFormResponse>().ReverseMap();
        CreateMap<BasketProduct, BasketProductForm>();
        CreateMap<MarketProductFormRequest, MarketProductFormResponse>().ReverseMap();
        CreateMap<MarketOrderFormResponse, MarketOrderFormRequest>().ReverseMap();
        CreateMap<OrderProductFormRequest, OrderProductFormResponse>().ReverseMap();
    }
}