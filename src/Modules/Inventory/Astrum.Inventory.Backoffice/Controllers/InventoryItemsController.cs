using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Inventory.Application.Commands;
using Astrum.Inventory.Application.Models;
using Astrum.Inventory.Application.Models.ViewModels;
using Astrum.Inventory.Application.Services;
using Astrum.Inventory.Domain.Aggregates;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Inventory.Backoffice.Controllers
{
    [Route("[controller]")]
    public class InventoryItemsController : ApiBaseController
    {
        private readonly IInventoryItemsService _service;
        private readonly ILogHttpService _logger;
        public InventoryItemsController(IInventoryItemsService service, ILogHttpService logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("/api/inventory/get-inventoryitems")]
        [ProducesDefaultResponseType(typeof(Result))]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(List<InventoryItemView>), StatusCodes.Status200OK)]
        public async Task<Result<List<InventoryItemView>>> Get()
        {
            var inventoryItems = await _service.GetInventoryItems();
            return Result.Success(inventoryItems);
        }

        [HttpGet("/api/inventory/get-filtering-inventoryitems")]
        [ProducesDefaultResponseType(typeof(Result))]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(InventoryItemsPaginationView), StatusCodes.Status200OK)]
        public async Task<Result<InventoryItemsPaginationView>> GetFilteringItems(string[]? templates, string? predicate,
            Status[]? statuses, Guid? userId, int? startIndex = 1, int? count = 15)
        {
            var inventoryItems = await _service.GetFilteringInventoryItems(templates, predicate, statuses, userId, startIndex, count);
            return Result.Success(inventoryItems.Data);
        }

        [HttpGet("{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(InventoryItemView), StatusCodes.Status200OK)]
        public async Task<Result<InventoryItemView>> GetById([FromRoute] Guid id)
        {
            var inventoryItem = await _service.GetInventoryItemById(id);
            if (!inventoryItem.IsSuccess)
            {
                _logger.Log(id, inventoryItem, HttpContext, "Товар получен.", TypeRequest.GET, ModuleAstrum.Inventory);
            }
            return Result.Success(inventoryItem);
        }

        [HttpPost]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(InventoryItemView), StatusCodes.Status200OK)]
        public async Task<Result<InventoryItemView>> Create([FromBody] InventoryItemCreateRequest inventoryItemCreate)
        {
            var inventoryItem = await _service.Create(inventoryItemCreate);
            _logger.Log(inventoryItemCreate, inventoryItem, HttpContext, "Товар создан.", TypeRequest.POST, ModuleAstrum.Inventory);
            return Result.Success(inventoryItem);
        }

        [HttpPut("{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(InventoryItemView), StatusCodes.Status200OK)]
        public async Task<Result<InventoryItemView>> Update([FromRoute] Guid id, [FromBody] InventoryItemUpdateRequest inventoryItemUpdate)
        {
            var command = new UpdateInventoryItemCommand(id, inventoryItemUpdate);
            var response = await Mediator.Send(command);
            _logger.Log(inventoryItemUpdate, response, HttpContext, "Товар обновлён.", TypeRequest.PUT, ModuleAstrum.Inventory);
            return response;
        }

        [HttpDelete("{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(InventoryItemView), StatusCodes.Status200OK)]
        public async Task<Result<InventoryItemView>> Delete([FromRoute] Guid id)
        {
            var command = new DeleteInventoryItemCommand(id);
            var response = await Mediator.Send(command);
            _logger.Log(id, response, HttpContext, "Товар удалён.", TypeRequest.DELETE, ModuleAstrum.Inventory);
            return response;
        }
    }
}
