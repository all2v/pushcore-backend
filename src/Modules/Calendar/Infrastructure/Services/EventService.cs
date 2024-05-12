using Astrum.Calendar.ViewModels;
using Astrum.SharedLib.Common.Results;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

namespace Astrum.Calendar.Services;

public class EventService : IEventService
{
    private readonly IAccessorService _accessorService;
    private CalendarService service;

    public EventService(IAccessorService accessorService)
    {
        _accessorService = accessorService;
        GetAccess();
    }

    #region IEventService Members

    public async Task<IEnumerable<EventForm>> GetEventsAsync(string calendarId)
    {
        var events = await service.Events.List(calendarId).ExecuteAsync();
        var eventForms = events.Items.Select(ev => ToEventForm(ev, calendarId));
        return eventForms;
    }

    public async Task<EventForm> GetEventAsync(string calendarId, string eventId)
    {
        var ev = await service.Events.Get(calendarId, eventId).ExecuteAsync();
        var eventForm = ToEventForm(ev, calendarId);
        return eventForm;
    }

    public async Task<Result<EventForm>> Create(EventForm form)
    {
        try
        {
            var ev = CreateFromForm(form);
            if (ev.Failed)
                return Result<EventForm>.Error(ev.MessageWithErrors);
            var newEvent = await service.Events.Insert(ev, form.CalendarId).ExecuteAsync();
            newEvent.ICalUID = form.CalendarId;
            var eventForm = ToEventForm(newEvent, form.CalendarId);
            return Result.Success(eventForm);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании события.");
        }
        
    }

    public async Task<Result<EventForm>> Update(EventForm form)
    {
        try
        {
            var ev = await service.Events.Get(form.CalendarId, form.Id).ExecuteAsync();
            ev = CreateFromForm(form, ev);
            var newEvent = await service.Events.Update(ev, form.CalendarId, form.Id).ExecuteAsync();
            newEvent.ICalUID = form.CalendarId;
            var eventForm = ToEventForm(newEvent, form.CalendarId);
            return Result.Success(eventForm);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении события.");
        }
    }

    public async Task<Result<EventForm>> Delete(string calendarId, string id)
    {
        try
        {
            var ev = await service.Events.Get(calendarId, id).ExecuteAsync();
            var eventForm = ToEventForm(ev, calendarId);
            await service.Events.Delete(calendarId, id).ExecuteAsync();
            return Result.Success(eventForm);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении события.");
        }

    }

    public async void GetAccess(CancellationToken cancellationToken = default)
    {
        service = await _accessorService.GetAccess(cancellationToken);
    }

    #endregion

    private static Result<Event> CreateFromForm(EventForm form, Event? ev = null)
    {
        if(form.Start?.Day != form.End?.Day)
            return Result<Event>.Error("Даты начала и конца события должны совпадать");
        ev ??= new Event();
        ev.Summary = form.Summary ?? ev.Summary;
        ev.Description = form.Description ?? ev.Description;
        ev.Created = DateTime.Now;
        ev.Start ??= new EventDateTime();
        ev.Start.DateTime = form.Start ?? ev.Start.DateTime;
        ev.End ??= new EventDateTime();
        ev.End.DateTime = form.End ?? ev.End.DateTime;
        return ev;
    }

    private EventForm ToEventForm(Event ev, string calendarId)
    {
        return new EventForm
        {
            Id = ev.Id,
            CalendarId = calendarId,
            Summary = ev.Summary,
            Description = ev.Description,
            Created = ev.Created,
            Start = ev.Start.DateTime,
            End = ev.End.DateTime
        };
    }
}