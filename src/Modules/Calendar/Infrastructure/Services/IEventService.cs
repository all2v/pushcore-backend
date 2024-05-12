using Astrum.Calendar.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Calendar.Services;

public interface IEventService : IGoogleCalendar
{
    Task<IEnumerable<EventForm>> GetEventsAsync(string calendarId);
    Task<EventForm> GetEventAsync(string calendarId, string eventId);
    Task<Result<EventForm>> Create(EventForm form);
    Task<Result<EventForm>> Update(EventForm form);
    Task<Result<EventForm>> Delete(string calendarId, string id);
}