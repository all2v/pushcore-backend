using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.Market.Aggregates;
using Astrum.Market.Repositories;
using Astrum.Market.Specifications.MarketBasket;
using Astrum.Market.Specifications.MarketProduct;
using Astrum.Market.ViewModels;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Astrum.Market.Services;

public class BasketService : IBasketService
{
    private readonly IMapper _mapper;
    private readonly IMarketBasketRepository _marketBasketRepository;
    private readonly IMarketProductRepository _marketProductsRepository;
    private readonly IProductService _productService;

    public BasketService(IMarketBasketRepository marketBasketRepository, IMapper mapper,
        IMarketProductRepository marketProductsRepository, IProductService productService)
    {
        _marketBasketRepository = marketBasketRepository;
        _mapper = mapper;
        _marketProductsRepository = marketProductsRepository;
        _productService = productService;
    }

    #region IBasketService Members
    public async Task<SharedLib.Common.Results.Result<BasketForm>> GetBasket(Guid owner)
    {
        var basketSpec = new GetMarketBasketByOwnerIdSpec(owner);
        var basket = await _marketBasketRepository.FirstOrDefaultAsync(basketSpec);
        if (basket is null)
        {
            Result.Error("Корзина не найдена");
        }
        var basketForm = _mapper.Map<BasketForm>(basket);
        var products = basketForm.Products.Select(p => p.Product).ToList();
        await _productService.SetProductImageUrls(products);
        return Result.Success(basketForm);
    }

    public async Task<SharedLib.Common.Results.Result<BasketForm>> CreateBasket(Guid userId)
    {
        var aggregate = new MarketBasket(userId);
        await _marketBasketRepository.AddAsync(aggregate);
        try
        {
            await _marketBasketRepository.UnitOfWork.SaveChangesAsync();
        }
        catch
        {
            Result.Error("Не удалось сохранить изменения");
        }

        var basketForm = _mapper.Map<BasketForm>(aggregate);
        return Result.Success(basketForm);
    }
    
    //TODO: извлечь общую логику с методом remove
    public async Task<SharedLib.Common.Results.Result<BasketForm>> AddProductAsync(Guid basketId, Guid productId, int amount = 1)
    {
        var marketBasket = await GetMarketBasket(basketId);

        await CheckMarketProduct(productId);

        var basketProduct = marketBasket.Products.FirstOrDefault(prod => prod.Product.Id == productId);
        if (basketProduct != null)
            basketProduct.Amount += amount;
        else
        {
            var newProduct = new BasketProduct
            {
                Amount = amount,
                BasketId = basketId,
                ProductId = productId
            };
            marketBasket.AddProducts(newProduct);
        }

        await _marketBasketRepository.UpdateAsync(marketBasket);
        try
        {
            await _marketBasketRepository.UnitOfWork.SaveChangesAsync();
        }
        catch
        {
            Result.Error("Не удалось сохранить изменения");
        }

        var basketForm = _mapper.Map<BasketForm>(marketBasket);
        var products = basketForm.Products.Select(p => p.Product).ToList();
        await _productService.SetProductImageUrls(products);
        return Result.Success(basketForm);
    }
    
    public async Task<SharedLib.Common.Results.Result<BasketForm>> RemoveProduct(Guid basketId, Guid productId, int amount = 1)
    {
        var marketBasket = await GetMarketBasket(basketId);

        await CheckMarketProduct(productId);

        var basketProduct = marketBasket.Products.FirstOrDefault(prod => prod.Product.Id == productId);
        if (basketProduct is null)
        {
            Result.Error("Продукт в корзине не найден");
        }
        if (basketProduct.Amount > 1 && amount < basketProduct.Amount)
            basketProduct.Amount -= amount;
        else
            marketBasket.RemoveProducts(basketProduct);

        try
        {
            await _marketBasketRepository.UnitOfWork.SaveChangesAsync();
        }
        catch
        {
            Result.Error("Не удалось сохранить изменения");
        }

        var basketForm = _mapper.Map<BasketForm>(marketBasket);
        var products = basketForm.Products.Select(p => p.Product).ToList();
        await _productService.SetProductImageUrls(products);
        return Result.Success(basketForm);
    }

    private async Task CheckMarketProduct(Guid productId)
    {
        var getMarketProductByIdSpec = new GetMarketProductByIdSpec(productId);
        var marketProduct = await _marketProductsRepository.FirstOrDefaultAsync(getMarketProductByIdSpec);
        if (marketProduct is null)
        {
            Result.Error("Продукт не найден");
        }
    }

    private async Task<MarketBasket?> GetMarketBasket(Guid basketId)
    {
        var getMarketBasketByIdSpec = new GetMarketBasketByIdSpec(basketId);
        var marketBasket = await _marketBasketRepository.FirstOrDefaultAsync(getMarketBasketByIdSpec);
        if (marketBasket is null)
        {
            Result.Error("Корзина не найдена");
        }
        return marketBasket;
    }

    #endregion
}