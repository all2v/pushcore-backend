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

/// <summary>
///     Basket endpoints
/// </summary>
//[Authorize(Roles = "admin")]
[Area("Market")]
[Route("[area]/[controller]")]
public class BasketController : ApiBaseController
{
    private readonly IBasketService _basketService;
    private readonly ITopicEventSender _sender;
    private readonly ILogHttpService _logger;

    public BasketController(ITopicEventSender sender, IBasketService basketService, ILogHttpService logger)
    {
        _sender = sender;
        _basketService = basketService;
        _logger = logger;
    }

    /// <summary>
    ///     Получение корзины
    /// </summary>
    /// <param name="userId">Id нового пользователя</param>
    /// <returns></returns>
    [HttpGet("get/{userId}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(BasketForm), StatusCodes.Status200OK)]
    public async Task<Result<BasketForm>> GetBasket([FromRoute] Guid userId)
    {
        var basket = await _basketService.GetBasket(userId);
        //_logger.Log(userId, newBasket, HttpContext, "Корзина для пользователя создана", TypeRequest.POST, ModuleAstrum.Market);
        return Result.Success(basket);
    }

    /// <summary>
    ///     Создание пустой корзины для нового пользователя
    ///     TODO ну если создается при создании пользователя, логично же что это на бэке и не нужен для этого эндпоинт новый, не?
    ///     Создаётся при создании нового пользователя
    /// </summary>
    /// <param name="userId">Id нового пользователя</param>
    /// <returns></returns>
    [HttpPost("{userId}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(BasketForm), StatusCodes.Status200OK)]
    public async Task<Result<BasketForm>> Create([FromRoute] Guid userId)
    {
        var newBasket = await _basketService.CreateBasket(userId);
        _logger.Log(userId, newBasket, HttpContext, "Корзина для пользователя создана", TypeRequest.POST, ModuleAstrum.Market);
        return Result.Success(newBasket);
    }

    /// <summary>
    ///     Добавляет в корзину продукт из списка продуктов
    /// </summary>
    /// <param name="basketId">Id корзины</param>
    /// <param name="productId">Id продукта</param>
    /// <returns></returns>
    [HttpPost("product/{basketId}/{productId}/{amount}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(BasketForm), StatusCodes.Status200OK)]
    public async Task<Result<BasketForm>> AddProduct([FromRoute] Guid basketId, [FromRoute] Guid productId, [FromRoute] int amount = 1)
    {
        var basket = await _basketService.AddProductAsync(basketId, productId, amount);
        _logger.Log(basketId, basket, HttpContext, "Добавлен продукт в корзину", TypeRequest.POST, ModuleAstrum.Market);
        return Result.Success(basket);
    }

    /// <summary>
    ///     Удаляет из корзины продукт
    /// </summary>
    /// <param name="basketId">Id корзины</param>
    /// <param name="productId">Id продукта</param>
    /// <returns></returns>
    [HttpDelete("product/{basketId}/{productId}/{amount}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(BasketForm), StatusCodes.Status200OK)]
    public async Task<Result<BasketForm>> Delete([FromRoute] Guid basketId, [FromRoute] Guid productId, [FromRoute] int amount = 1)
    {
        var basket = await _basketService.RemoveProduct(basketId, productId, amount);
        _logger.Log(basketId, basket, HttpContext, "Продукт удалён из корзины", TypeRequest.DELETE, ModuleAstrum.Market);
        return Result.Success(basket);
    }
}