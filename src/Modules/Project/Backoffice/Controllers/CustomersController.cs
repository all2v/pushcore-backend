using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.Projects.Services;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Common.Results;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Projects.Controllers;

[Area("Projects")]
[Route("[area]/[controller]")]
public class CustomersController : ApiBaseController
{
    private readonly ICustomerService _customerService;
    private readonly ILogHttpService _logger;
    private readonly ITopicEventSender _sender;

    public CustomersController(ITopicEventSender sender, ICustomerService customerService, ILogHttpService logger)
    {
        _sender = sender;
        _customerService = customerService;
        _logger = logger;
    }

    /// <summary>
    ///     Список заказчиков
    /// </summary>
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(List<CustomerView>), StatusCodes.Status200OK)]
    public async Task<Result<List<CustomerView>>> GetAll()
    {
        var response = await _customerService.GetAll();
        return response;
    }

    /// <summary>
    ///     Создать заказчика
    /// </summary>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(CustomerView), StatusCodes.Status200OK)]
    public async Task<Result<CustomerView>> Create([FromBody] CustomerRequest customer)
    {
        var response = await _customerService.Create(customer);
        _logger.Log(customer, response, HttpContext, "Покупатель создан.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Project);
        return response;
    }

    /// <summary>
    ///     Удалить заказчика по id
    /// </summary>
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result<CustomerView>> Delete([FromRoute] Guid id)
    {
        var response = await _customerService.Delete(id);
        _logger.Log(id, response, HttpContext, "Покупатель удалён.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Project);
        return response;
    }
}