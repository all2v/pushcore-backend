using Astrum.Market.Mappings;
using Astrum.Market.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Astrum.Market.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(MarketProfile).Assembly;

        services.AddMediatR(currentAssembly);
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(currentAssembly);
        });

        //services.AddScoped(provider => new MapperConfiguration(cfg =>
        //{
        //    cfg.AddProfile(new MarketProfile(provider.GetService<IProductService>()));
        //}).CreateMapper());
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IMarketOrderService, MarketOrderService>();
        services.AddScoped<IProductService, ProductService>();
    }
}