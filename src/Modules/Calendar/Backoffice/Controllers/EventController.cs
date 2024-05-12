using Astrum.Calendar.GraphQL;
using Astrum.Calendar.Services;
using Astrum.Calendar.ViewModels;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Calendar.Controllers;

//[Authorize(Roles = "admin")]
[Area("Calendar")]
[Route("[area]/[controller]")]
public class EventController : ApiBaseController
{
    private readonly IEventService _eventService;
    private readonly ILogHttpService _logger;
    private readonly ITopicEventSender _sender;

    public EventController(ITopicEventSender sender, IEventService eventService, ILogHttpService logger)
    {
        _sender = sender;
        _eventService = eventService;
        _logger = logger;
    }

    /// <summary>
    ///     Создаёт новое событие в одном из календарей
    /// </summary>
    /// <param name="ev">Новое событие</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(SharedLib.Common.Results.Result<EventForm>), StatusCodes.Status200OK)]
    public async Task<SharedLib.Common.Results.Result<EventForm>> Create([FromBody] EventForm ev)
    {
        var response = await _eventService.Create(ev);
        _logger.Log(ev, response, HttpContext, "Создано событие.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Calendar);
        return response;
        //return Json(newEvent);
    }

    /// <summary>
    ///     Изменяет существующее событие
    /// </summary>
    /// <param name="ev">Изменённое событие</param>
    /// <returns></returns>
    [HttpPut]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(SharedLib.Common.Results.Result<EventForm>), StatusCodes.Status200OK)]
    public async Task<SharedLib.Common.Results.Result<EventForm>> Update([FromBody] EventForm ev)
    {
        var response = await _eventService.Update(ev);
        _logger.Log(ev, response, HttpContext, "Обновлено событие.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Calendar);
        return response;
    }

    /// <summary>
    ///     Удаляет событие из определённого календаря
    /// </summary>
    /// <param name="calendarId">id календаря</param>
    /// <param name="eventId">id события</param>
    /// <returns></returns>
    [HttpDelete("{calendarId}/{eventId}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(SharedLib.Common.Results.Result<EventForm>), StatusCodes.Status200OK)]
    public async Task<SharedLib.Common.Results.Result<EventForm>> Delete([FromRoute] string calendarId,
        [FromRoute] string eventId)
    {
        var response = await _eventService.Delete(calendarId, eventId);
        _logger.Log(eventId, response, HttpContext, "Событие удалено.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Calendar);
        return response;
    }
}