using Astrum.Calendar.Infrastructure.Services;
using Astrum.Calendar.ViewModels;
using Astrum.SharedLib.Common.Results;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using MassTransit;

namespace Astrum.Calendar.Services;

public class CalendarListService : ICalendarListService
{
    private readonly IAccessorService _accessorService;
    private readonly IEventService _eventService;
    private CalendarService service;

    public CalendarListService(IAccessorService accessorService, IEventService eventService)
    {
        _accessorService = accessorService;
        _eventService = eventService;
        GetAccess();
    }

    #region ICalendarListService Members

    public async Task<IEnumerable<CalendarForm>> GetCalendarListAsync()
    {
        var calendarList = new CalendarList();
        try
        {
            calendarList = await service.CalendarList.List().ExecuteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"Не удалось получить календарь. Ошибка: {e.Message}");
        }
        var calendarForms = calendarList.Items
            .Select(async c => await ToCalendarForm(c))
            .Select(t => t.Result);
        return calendarForms;
    }

    public async Task<CalendarForm> GetCalendarAsync(string id)
    {
        var calendar = await service.CalendarList.Get(id).ExecuteAsync();
        var calendarForm = await ToCalendarForm(calendar);
        return calendarForm;
    }

    public async Task<Result<CalendarForm>> CreateCalendarAsync(CalendarForm form)
    {
        try
        {
            var calendar = new Google.Apis.Calendar.v3.Data.Calendar
            {
                Summary = form.Summary
            };
            var newCalendar = await service.Calendars.Insert(calendar).ExecuteAsync();
            var entry = await service.CalendarList.Get(newCalendar.Id).ExecuteAsync();
            var calendarForm = await ToCalendarForm(entry);
            return Result.Success(calendarForm);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании календаря.");
        }
        
    }

    public async Task<Result<CalendarForm>> UpdateCalendarAsync(CalendarForm form)
    {
        try
        {
            var entry = await service.CalendarList.Get(form.CalendarId).ExecuteAsync();
            entry = await service.CalendarList.Update(entry, form.CalendarId).ExecuteAsync();
            var calendarForm = await ToCalendarForm(entry);
            return Result.Success(calendarForm);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении календаря.");
        }
    }

    public async Task<Result<CalendarForm>> DeleteCalendarAsync(string id)
    {
        try
        {
            var entry = await service.CalendarList.Get(id).ExecuteAsync();
            var calendarForm = await ToCalendarForm(entry);
            await service.CalendarList.Delete(id).ExecuteAsync();
            return Result.Success(calendarForm);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении календаря.");
        }
    }

    public async void GetAccess(CancellationToken cancellationToken = default)
    {
        try
        {
            service = await _accessorService.GetAccess(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Не удалось получить доступ к календарю. {ex}");
        }
    }

    #endregion

    private async Task<CalendarForm> ToCalendarForm(CalendarListEntry cal)
    {
        var events = await _eventService.GetEventsAsync(cal.Id);
        return new CalendarForm
        {
            CalendarId = cal.Id,
            Summary = cal.Summary,
            AccessRole = cal.AccessRole,
            BackgroundColor = cal.BackgroundColor,
            ForegroundColor = cal.ForegroundColor,
            Events = events.ToList()
        };
    }
}