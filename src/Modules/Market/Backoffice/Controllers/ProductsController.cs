using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.Market.GraphQL;
using Astrum.Market.Services;
using Astrum.Market.ViewModels;
using Astrum.SharedLib.Common.Results;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Astrum.Market.Controllers;

[Area("Market")]
[Route("[area]/[controller]")]
public class ProductsController : ApiBaseController
{
    private readonly IBasketService _basketService;
    private readonly IProductService _productService;
    private readonly ITopicEventSender _sender;
    private readonly ILogHttpService _logger;

    public ProductsController(ITopicEventSender sender, IProductService productService, ILogHttpService logger)
    {
        _sender = sender;
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    ///     Создаёт новый продукт
    /// </summary>
    /// <param name="product">Новый продукт</param>
    /// <returns></returns>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(MarketProductFormResponse), StatusCodes.Status200OK)]
    public async Task<Result<MarketProductFormResponse>> Create([FromBody] MarketProductFormRequest product)
    {
        var newProduct = await _productService.Create(product);
        _logger.Log(product, newProduct, HttpContext, "Создан продукт", TypeRequest.POST, ModuleAstrum.Market);
        return Result.Success(newProduct);

    }

    /// <summary>
    ///     Изменяет продукт
    /// </summary>
    /// <param name="id">Id продукта</param>
    /// <param name="product">Изменённый продукт</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(MarketProductFormResponse), StatusCodes.Status200OK)]
    public async Task<Result<MarketProductFormResponse>> Update([FromRoute] Guid id, [FromBody] MarketProductFormRequest product)
    {
        var updated = await _productService.Update(id, product);
        _logger.Log(product, updated, HttpContext, "Обновлён продукт", TypeRequest.PUT, ModuleAstrum.Market);
        return Result.Success(updated);
    }

    ///// <summary>
    /////     Увеличивает количество товара
    ///// </summary>
    ///// <param name="id">Id товара</param>
    ///// <param name="amount">Новое количество товара</param> 
    ///// <returns></returns>
    //[HttpPut("replenish/{id}")]
    //[TranslateResultToActionResult]
    //[ProducesDefaultResponseType(typeof(Result))]
    //[ProducesResponseType(typeof(MarketProductFormResponse), StatusCodes.Status200OK)]
    //public async Task<Result<MarketProductFormResponse>> Replenish([FromRoute] Guid id, [FromBody] int amount)
    //{
    //    var updated = await _productService.Replenish(id, amount);
    //    return Result.Success(updated);
    //}

    /// <summary>
    ///     Удаляет товар
    /// </summary>
    /// <param name="id">Id товара</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(MarketProductFormResponse), StatusCodes.Status200OK)]
    public async Task<Result<MarketProductFormResponse>> Delete([FromRoute] Guid id)
    {
        var product = await _productService.Delete(id);
        _logger.Log(id, product, HttpContext, "Удалён продукт", TypeRequest.DELETE, ModuleAstrum.Market);
        return Result.Success(product);
    }
}