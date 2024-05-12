using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.Market.GraphQL;
using Astrum.Market.Services;
using Astrum.Market.ViewModels;
using Astrum.Projects.Aggregates;
using Astrum.SharedLib.Common.Results;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Astrum.Market.Controllers;
//[Authorize(Roles = "admin")]

[Area("Market")]
[Route("[area]/[controller]")]
public class OrdersController : ApiBaseController
{
    private readonly IMarketOrderService _orderService;
    private readonly ITopicEventSender _sender;
    private readonly ILogHttpService _logger;

    public OrdersController(ITopicEventSender sender, IMarketOrderService orderService, ILogHttpService logger)
    {
        _sender = sender;
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    ///     Создание нового заказа
    /// </summary>
    /// <param name="order">Новый заказ</param>
    /// <returns></returns>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(MarketOrderFormResponse), StatusCodes.Status200OK)]
    public async Task<Result<MarketOrderFormResponse>> Create([FromBody] MarketOrderFormRequest order)
    {
        var newOrder = await _orderService.Add(order);
        _logger.Log(order, newOrder, HttpContext, "Создан заказ", TypeRequest.POST, ModuleAstrum.Market);
        return Result.Success(newOrder);
    }

    /// <summary>
    ///     Изменение заказа
    /// </summary>
    /// <param name="id">Id заказа</param>
    /// <param name="order">Изменённый заказ</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(MarketOrderFormResponse), StatusCodes.Status200OK)]
    public async Task<Result<MarketOrderFormResponse>> Update([FromRoute] Guid id, [FromBody] MarketOrderFormRequest order)
    {
        var updated = await _orderService.Update(id, order);
        _logger.Log(order, updated, HttpContext, "Заказ обновлён", TypeRequest.PUT, ModuleAstrum.Market);
        return Result.Success(updated);
    }

    /// <summary>
    ///     Удаляет заказ
    /// </summary>
    /// <param name="id">Id удаляемого заказа</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(MarketOrderFormResponse), StatusCodes.Status200OK)]
    public async Task<Result<MarketOrderFormResponse>> Delete([FromRoute] Guid id)
    {
        var order = await _orderService.Remove(id);
        _logger.Log(id, order, HttpContext, "Заказ удалён", TypeRequest.DELETE, ModuleAstrum.Market);
        return Result.Success(order);
    }
}