using Astrum.Infrastructure.Services;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.Market.Aggregates;
using Astrum.Market.Repositories;
using Astrum.Market.Specifications.MarketProduct;
using Astrum.Market.ViewModels;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using Astrum.Storage.ViewModels;
using AutoMapper;

namespace Astrum.Market.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IMarketProductRepository _marketProductRepository;
    private readonly IFileStorage _fileStorage;
    private readonly SentryService _sentryService;

    private readonly string MarketStorageBucketName = "market";

    public ProductService(IMarketProductRepository marketProductRepository, IMapper mapper, 
        IFileStorage fileStorage, SentryService sentryService)
    {
        _marketProductRepository = marketProductRepository;
        _mapper = mapper;
        _fileStorage = fileStorage;
        _sentryService = sentryService;
    }

    #region IProductService Members

    public async Task<SharedLib.Common.Results.Result<List<MarketProductFormResponse>>> GetAllProducts()
    {
        var products = await _marketProductRepository.ListAsync();
        var forms = _mapper.Map<List<MarketProductFormResponse>>(products);
        await SetProductImageUrls(forms);
        return Result.Success(forms);
    }

    public async Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> GetProduct(Guid productId)
    {
        var specification = new GetMarketProductByIdSpec(productId);
        var marketProduct = await _marketProductRepository.FirstOrDefaultAsync(specification);
        //await _sentryService.SendEvent(new Exception("Test"));
        if (marketProduct is null)
            throw new Exception("Market product not found");
        var productForm = _mapper.Map<MarketProductFormResponse>(marketProduct);
        await SetProductImageUrls(new List<MarketProductFormResponse>(new[] {productForm}));
        return Result.Success(productForm);
    }

    public async Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> Create(MarketProductFormRequest marketProduct, FileForm? image = null)
    {
        var newProduct = _mapper.Map<MarketProduct>(marketProduct);
        if (image != null)
        {
            try
            {
                var res = await _fileStorage.UploadFile(image);
                if (res is { Success: true })
                    newProduct.CoverImageId = res.UploadedFileId;
            }
            catch (Exception ex)
            {
                return SharedLib.Common.Results.Result.Error($"Не удалось сохранить изображение. {ex}");
            }
        }
        await _marketProductRepository.AddAsync(newProduct);
        try
        {
            await _marketProductRepository.UnitOfWork.SaveChangesAsync();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(marketProduct);
        }
        catch
        {
            return SharedLib.Common.Results.Result.Error($"Не удалось сохранить изменения.");
        }

        var marketProductFormResponse = _mapper.Map<MarketProductFormResponse>(newProduct);
        return SharedLib.Common.Results.Result.Success(marketProductFormResponse);
    }

    public async Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> Delete(Guid id)
    {
        var spec = new GetMarketProductByIdSpec(id);
        var product = await _marketProductRepository.FirstOrDefaultAsync(spec);
        await _marketProductRepository.DeleteAsync(product);
        await _marketProductRepository.UnitOfWork.SaveChangesAsync();
        var marketProductFormResponse = _mapper.Map<MarketProductFormResponse>(product);
        return SharedLib.Common.Results.Result.Success(marketProductFormResponse);
    }

    public async Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> Update(Guid productId, MarketProductFormRequest marketProduct)
    {
        var specification = new GetMarketProductByIdSpec(productId);
        var product = await _marketProductRepository.FirstOrDefaultAsync(specification);
        if (product is null)
            return SharedLib.Common.Results.Result.NotFound("Продукт по id не найден");
        _mapper.Map(marketProduct, product);
        try
        {
            await _marketProductRepository.UnitOfWork.SaveChangesAsync();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(marketProduct);
        }
        catch
        {
            return SharedLib.Common.Results.Result.Error($"Не удалось сохранить изменения.");
        }

        var marketProductFormResponse = _mapper.Map<MarketProductFormResponse>(marketProduct);
        return SharedLib.Common.Results.Result.Success(marketProductFormResponse);
    }

    public async Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> Replenish(Guid productId, int amount)
    {
        return await ChangeAmount(productId, amount);
    }

    public async Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> Reduce(Guid productId, int amount)
    {
        return await ChangeAmount(productId, -amount);
    }

    private async Task<SharedLib.Common.Results.Result<MarketProductFormResponse>> ChangeAmount(Guid productId, int amount)
    {
        var specification = new GetMarketProductByIdSpec(productId);
        var product = await _marketProductRepository.FirstOrDefaultAsync(specification);
        if (product is null)
            return SharedLib.Common.Results.Result.NotFound("Продукт по id не найден");
        product.Remain += amount;
        try
        {
            await _marketProductRepository.UnitOfWork.SaveChangesAsync();
        }
        catch
        {
            return SharedLib.Common.Results.Result.Error($"Не удалось сохранить изменения.");
        }

        var marketProductFormResponse = _mapper.Map<MarketProductFormResponse>(product);
        return SharedLib.Common.Results.Result.Success(marketProductFormResponse);
    }

    public async Task SetProductImageUrls(List<MarketProductFormResponse> products)
    {
        foreach (var form in products.Where(form => form.CoverImageId.HasValue))
            form.CoverUrl = await _fileStorage.GetFileUrl(form.CoverImageId.Value);
    }

    #endregion
}