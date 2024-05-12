using System.ComponentModel.DataAnnotations;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.Projects.Services;
using Astrum.Projects.ViewModels.DTO;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Common.Results;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Projects.Controllers;

[Route("[controller]")]
public class ProductController : ApiBaseController
{
    private readonly IProductService _productService;
    private readonly ILogHttpService _logger;
    private readonly ITopicEventSender _sender;

    public ProductController(ITopicEventSender sender, IProductService productService, ILogHttpService logger)
    {
        _sender = sender;
        _productService = productService;
        _logger = logger;
    }


    /// <summary>
    ///     Список продуктов
    /// </summary>
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(ProductPaginationView), StatusCodes.Status200OK)]
    public async Task<Result<ProductPaginationView>> GetProducts([Required][FromQuery] int count,
        [Required][FromQuery] int startIndex, [FromQuery] string? predicate)
    {
        var response = await _productService.GetAllPagination(count, startIndex, predicate);
        return response;
    }

    /// <summary>
    ///     Создать продукт
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Manager,Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(ProductView), StatusCodes.Status200OK)]
    public async Task<Result<ProductView>> Create([FromForm] ProductRequest product)
    {
        var response = await _productService.Create(product);
        _logger.Log(product, response, HttpContext, "Создан продукт.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Project);
        return response;
    }

    /// <summary>
    ///     Удалить продукт по id
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager,Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result<ProductView>> Delete([FromRoute] Guid id)
    {
        var response = await _productService.Delete(id);
        _logger.Log(id, response, HttpContext, "Продукт удалён", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Project);
        return response;
    }

    /// <summary>
    ///     Получить продукт по id
    /// </summary>
    [HttpGet("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(ProductView), StatusCodes.Status200OK)]
    public async Task<Result<ProductView>> Get([FromRoute] Guid id)
    {
        var response = await _productService.Get(id);
        if (!response.IsSuccess)
        {
            _logger.Log(id, response, HttpContext, "Продукт получен.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Project);
        }
        return response;
    }

    /// <summary>
    ///     Обновить продукт
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Manager,Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(ProductView), StatusCodes.Status200OK)]
    public async Task<Result<ProductView>> Update(Guid id, [FromForm] ProductUpdateDto product)
    {
        var response = await _productService.Update(id, product);
        _logger.Log(product, response, HttpContext, "Продукт обновлён.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Project);
        return response;
    }
}