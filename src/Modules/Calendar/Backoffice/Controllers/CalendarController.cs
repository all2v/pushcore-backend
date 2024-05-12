using Astrum.Calendar.GraphQL;
using Astrum.Calendar.Infrastructure.Services;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Astrum.Calendar.Controllers;

//[Authorize(Roles = "admin")]
[Area("Calendar")]
[Route("[controller]")]
public class CalendarController : ApiBaseController
{
    private readonly ICalendarListService _calendarListService;
    private readonly ITopicEventSender _sender;
    private readonly ILogHttpService _logger;

    public CalendarController(ICalendarListService calendarListService, ITopicEventSender sender, ILogHttpService logger)
    {
        _calendarListService = calendarListService;
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    /// Создаёт новый календарь
    /// </summary>
    /// <param name="calendar">Новый календарь</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(SharedLib.Common.Results.Result<CalendarForm>), StatusCodes.Status200OK)]
    public async Task<SharedLib.Common.Results.Result<CalendarForm>> Create([FromBody] CalendarForm calendar)
    {
        var newCalendar = await _calendarListService.CreateCalendarAsync(calendar);
        _logger.Log(calendar, newCalendar, HttpContext, "Создан календарь.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Calendar);
        return Result.Success(newCalendar);
    }

    /// <summary>
    /// Изменяет выбранный календарь
    /// </summary>
    /// <param name="calendar">Изменённый календарь</param>
    /// <returns></returns>
    [HttpPut("{calendarId}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(SharedLib.Common.Results.Result<CalendarForm>), StatusCodes.Status200OK)]
    public async Task<SharedLib.Common.Results.Result<CalendarForm>> Update([FromBody] CalendarForm calendar)
    {
        var updated = await _calendarListService.UpdateCalendarAsync(calendar);
        _logger.Log(calendar, updated, HttpContext, "Обовлён календарь.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Calendar);
        return Result.Success(updated);
    }

    /// <summary>
    /// Удаляет календарь по его id
    /// </summary>
    /// <param name="id">id календаря</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(SharedLib.Common.Results.Result<CalendarForm>), StatusCodes.Status200OK)]
    public async Task<SharedLib.Common.Results.Result<CalendarForm>> Delete([FromRoute] string id)
    {
        var deleted = await _calendarListService.DeleteCalendarAsync(id);
        _logger.Log(id, deleted, HttpContext, "Удалён календарь.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Calendar);
        return Result.Success(deleted);
    }
}