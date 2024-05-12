using Astrum.Account.Application.Services;
using Astrum.Account.Services;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.Market.Aggregates;
using Astrum.Market.Application.ViewModels;
using Astrum.Market.Domain.Enums;
using Astrum.Market.Repositories;
using Astrum.Market.Specifications.MarketOrder;
using Astrum.Market.Specifications.MarketProduct;
using Astrum.Market.ViewModels;
using Astrum.Projects.Aggregates;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetBox.Extensions;

namespace Astrum.Market.Services;

public class MarketOrderService : IMarketOrderService
{
    private readonly IMapper _mapper;
    private readonly IMarketOrderRepository _marketOrderRepository;
    private readonly IOrderProductRepository _orderProductRepository;
    private readonly IUserProfileService _userProfileService;
    private readonly IProductService _productService;
    private readonly IBasketService _basketService;
    private readonly ITransactionService _transactionService;

    public MarketOrderService(IMarketOrderRepository marketOrderRepository, IMapper mapper,
        IProductService productService, IUserProfileService userProfileService, 
        IBasketService basketService, IOrderProductRepository orderProductRepository, ITransactionService transactionService)
    {
        _marketOrderRepository = marketOrderRepository;
        _mapper = mapper;
        _userProfileService = userProfileService;
        _productService = productService;
        _basketService = basketService;
        _orderProductRepository = orderProductRepository;
        _transactionService = transactionService;
    }

    #region IMarketOrderService Members

    public async Task<SharedLib.Common.Results.Result<List<MarketOrderFormResponse>>> GetAllOrders()
    {
        var spec = new GetMarketOrderWithProductSpec();
        var orders = await _marketOrderRepository.ListAsync(spec);
        var marketOrderForms = _mapper.Map<List<MarketOrder>, List<MarketOrderFormResponse>>(orders);
        foreach (var form in marketOrderForms)
        {
            form.User = await _userProfileService.GetUserProfileAsync(form.UserId);
            var products = form.OrderProducts.Select(p => p.Product).ToList();
            await _productService.SetProductImageUrls(products);
        }
        return Result.Success(marketOrderForms);
    }

    public async Task<SharedLib.Common.Results.Result<MarketOrderFormResponse>> GetOrder(Guid id)
    {
        var getMarketOrderByIdSpec = new GetMarketOrderByIdSpec(id);
        var order = await _marketOrderRepository.FirstOrDefaultAsync(getMarketOrderByIdSpec);
        var marketOrderForm = _mapper.Map<MarketOrderFormResponse>(order);
        marketOrderForm.User = await _userProfileService.GetUserProfileAsync(marketOrderForm.UserId);
        return Result.Success(marketOrderForm);
    }

    public async Task<SharedLib.Common.Results.Result<MarketOrderFormResponse>> Add(MarketOrderFormRequest order)
    {
        var products = _mapper.Map<List<OrderProduct>>(order.OrderProducts);
        var newOrder = new MarketOrder
        {
            Comment = order.Comment ?? "",
            UserId = (Guid)order.UserId,
            OrderProducts = products
        };
        var price = (int) await GetTotalPrice(newOrder);
        var productNames = string.Join(',', newOrder.OrderProducts.Select(x => x.Product.Name));
        var description = $"Совершена покупка в магазине: {productNames}";
        var request = new TransactionRequest(-price, order.UserId, TransactionType.Market, description);
        var spendResult = await _transactionService.AddTransaction(request);
        if (spendResult.Failed)
        {
            return SharedLib.Common.Results.Result<MarketOrderFormResponse>.Error(spendResult.MessageWithErrors);
        }
        var basket = await _basketService.GetBasket(order.UserId);
        foreach (var product in newOrder.OrderProducts)
        {
            var reduceResult = await _productService.Reduce(product.ProductId, product.Amount);
            if (reduceResult.Failed) 
                return SharedLib.Common.Results.Result<MarketOrderFormResponse>.Error(reduceResult.MessageWithErrors);
            var removeResult = await _basketService.RemoveProduct(basket.Data.Id, product.ProductId, product.Amount);
            if (removeResult.Failed) 
                return SharedLib.Common.Results.Result<MarketOrderFormResponse>.Error(reduceResult.MessageWithErrors);
        }
        newOrder = await _marketOrderRepository.AddAsync(newOrder);
        newOrder.OrderProducts.ForEach(prod => prod.OrderId = newOrder.Id);
        try
        {
            await _marketOrderRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Result.Error($"Не удалось сохранить изменения. {ex.Message}");
        }

        var marketOrderFormResponse = _mapper.Map<MarketOrderFormResponse>(newOrder);
        return Result.Success(marketOrderFormResponse);
    }

    public async Task<SharedLib.Common.Results.Result<MarketOrderFormResponse>> Remove(Guid id)
    {
        var spec = new GetMarketOrderByIdSpec(id);
        var order = await _marketOrderRepository.FirstOrDefaultAsync(spec);
        await _marketOrderRepository.DeleteAsync(order);
        try
        {
            await _marketOrderRepository.UnitOfWork.SaveChangesAsync();
        }
        catch
        {
            Result.Error("Не удалось сохранить изменения");
        }

        var marketOrderFormResponse = _mapper.Map<MarketOrderFormResponse>(order);
        return Result.Success(marketOrderFormResponse);
    }

    public async Task<SharedLib.Common.Results.Result<MarketOrderFormResponse>> Update(Guid id, MarketOrderFormRequest order)
    {
        var marketOrder = await _marketOrderRepository.FirstOrDefaultAsync(new GetMarketOrderByIdAsNoTrackingSpec(id));
        if (marketOrder is null)
            return Result.Error("Заказ по заданному id не найден");

        var orderedProducts = order.OrderProducts.Where(product => product.Status == OrderStatus.Ordered);
        foreach (var product in orderedProducts) 
            product.Status = (OrderStatus)order.Status;
        _mapper.Map(order, marketOrder);
        var alreadyRefund = false;
        if (order.Status is not null)
        {
            var updateResult = await UpdateOrderStatus(marketOrder);
            alreadyRefund = true;
            if(updateResult.Failed)
                return Result.Error(updateResult.MessageWithErrors);
        }
        if (order.OrderProducts != null)
        {
            var updateResult = await UpdateOrderProducts(marketOrder, alreadyRefund);
            if (updateResult.Failed)
                return Result.Error(updateResult.MessageWithErrors);
        }

        if (order.OrderProducts.Count > 0)
            await _marketOrderRepository.UpdateAsync(marketOrder);
        else
            await _marketOrderRepository.DeleteAsync(marketOrder);

        try
        {
            await _marketOrderRepository.UnitOfWork.SaveChangesAsync();
        }
        catch
        {
            Result.Error("Не удалось сохранить изменения");
        }

        var marketOrderFormResponse = _mapper.Map<MarketOrderFormResponse>(order);
        return Result.Success(marketOrderFormResponse);
    }
    #endregion

    private async Task<Result> UpdateOrderProducts(MarketOrder marketOrder, bool alreadyRefund = false)
    {
        var removeId = marketOrder.OrderProducts.Select(p => p.ProductId)
            .Except(marketOrder.OrderProducts.Select(p => p.ProductId)).FirstOrDefault();
        var removingProduct = marketOrder.OrderProducts.FirstOrDefault(p => p.ProductId == removeId);
        if(removingProduct != null)
        {
            marketOrder.OrderProducts.Remove(removingProduct);
            await _orderProductRepository.DeleteAsync(removingProduct);
        }
        else
            removingProduct =
                marketOrder.OrderProducts.FirstOrDefault(product => product.Status == (int)OrderStatus.Cancelled);

        if (removingProduct != null)
        {
            var replenishResult = await _productService.Replenish(removingProduct.ProductId, removingProduct.Amount);
            if (replenishResult.Failed)
                return Result.Error(replenishResult.MessageWithErrors);
            var productTotalPrice = (int) await GetProductTotalPrice(removingProduct);
            if(!alreadyRefund)
            {
                var description = "Возвращены деньги из-за удаления продукта из заказа";
                var request = new TransactionRequest(productTotalPrice, marketOrder.UserId, TransactionType.Market,
                    description);
                var spendResult = await _transactionService.AddTransaction(request);
            }
        }

        return Result.Success();
    }

    private async Task<Result> UpdateOrderStatus(MarketOrder marketOrder)
    {
        if (marketOrder.Status == (int)OrderStatus.Cancelled)
        {
            var price = await GetTotalPrice(marketOrder);
            var description = "Возвращены деньги из-за отмены заказа";
            var request = new TransactionRequest((int)price, marketOrder.UserId, TransactionType.Market,
                description);
            var spendResult = await _transactionService.AddTransaction(request);
            foreach (var product in marketOrder.OrderProducts)
            {
                var replenishResult = await _productService.Replenish(product.ProductId, product.Amount);
                if (replenishResult.Failed)
                    return Result.Error(replenishResult.MessageWithErrors);
            }
        }
        return Result.Success();
    }

    private async Task<double> GetTotalPrice(MarketOrder order)
    {
        double result = 0;
        foreach (var product in order.OrderProducts) 
            result += await GetProductTotalPrice(product);
        return result;
    }

    private async Task<double> GetProductTotalPrice(OrderProduct product)
    {
        var marketProduct = await _productService.GetProduct(product.ProductId);
        return (double) (marketProduct.Data.Price * product.Amount);
    }

    /*private async Task SetProducts(MarketOrderForm order, MarketOrderAggregate newOrder)
    {
        var productIds = order.Products.Select(prod => prod.Id);
        var products = _productRepository.Items.AsNoTracking().AsEnumerable().Where(prod => productIds.Contains(prod.Id))
            .ToList();
        newOrder.Products = products;
        _marketOrderRepository.Update(newOrder);
        await _marketOrderRepository.UnitOfWork.SaveChangesAsync();
    }*/
}